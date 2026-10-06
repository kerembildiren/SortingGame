using System;
using System.Collections.Generic;
using SortingGame.Core;
using SortingGame.Data;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SortingGame.Section
{
    /// <summary>
    /// GDD 7.1 gestures with the GDD 10.1 tools, one finger:
    /// Hand: press on an item to carry it; pass over more items to pick them up too (any kind, up to the Hand capacity).
    /// Magnet: like Hand, but same-category items within the magnet radius are pulled in automatically (up to its limit).
    /// While carrying, rest over a shelf for a moment (or let go) and the items that belong there jump in; the rest stay
    /// in hand for the next shelf. Broom sweeps the dirt. With any tool, a tap opens a box or picks up a glowing collectible.
    /// </summary>
    public class DragController : MonoBehaviour
    {
        const float TapMaxMovePixels = 30f;
        const float DustInterval = 0.06f;
        const float StackSpacing = 0.2f;

        readonly RaycastHit[] _hits = new RaycastHit[32];
        /// <summary>Carried items; [0] is under the finger, the rest trail behind.</summary>
        readonly List<ItemView> _stack = new();

        Camera _camera;
        SectionController _section;
        GameContext _ctx;
        FeelConfig _feel;
        Func<Vector2, bool> _isOverUi;
        Func<float> _uiScale;
        Fx _fx;

        CategoryDefinition _magnetCategory;
        ShelfView _hoveredShelf;
        float _hoverTime;
        Vector3 _shelfHitPoint;
        ContainerView _tapContainer;
        ItemView _tapCollectible;
        Vector2 _pressPosition;
        Vector2 _lastPosition;
        bool _pressStartedOnUi;
        bool _carrying;
        bool _sweeping;
        float _dustTimer;
        Transform _broomCursor;

        public bool InputEnabled { get; set; } = true;
        public ToolType Tool { get; private set; } = ToolType.Hand;
        public IReadOnlyList<ItemView> Carried => _stack;

        public void Init(Camera cam, SectionController section, GameContext context, Func<Vector2, bool> isOverUi, Func<float> uiScale)
        {
            _camera = cam;
            _section = section;
            _ctx = context;
            _feel = context.Feel;
            _isOverUi = isOverUi;
            _uiScale = uiScale;
            _fx = new Fx(context.Visuals);

            var factory = new PlaceholderFactory(context.Visuals);
            var cursor = factory.CreateSoftQuad("BroomCursor", null, new Color(1f, 0.95f, 0.8f, 0.35f), ProceduralTextures.SoftDot);
            cursor.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            cursor.gameObject.SetActive(false);
            _broomCursor = cursor.transform;
        }

        public void SetTool(ToolType tool)
        {
            CancelDrag();
            Tool = tool;
        }

        /// <summary>How many items may be carried at once with the current tool.</summary>
        int Capacity => Tool == ToolType.Magnet
            ? 1 + Mathf.RoundToInt(_ctx.ToolStats(ToolType.Magnet).Secondary)
            : Mathf.Max(1, Mathf.RoundToInt(_ctx.ToolStats(ToolType.Hand).Primary));

        void Update()
        {
            var pointer = Pointer.current;
            if (pointer == null || _section == null) return;
            var position = pointer.position.ReadValue();

            if (pointer.press.wasPressedThisFrame) OnPress(position);
            else if (pointer.press.isPressed) OnHold(position);
            if (pointer.press.wasReleasedThisFrame) OnRelease(position);
            _lastPosition = position;
        }

        void OnPress(Vector2 screen)
        {
            _pressStartedOnUi = _isOverUi != null && _isOverUi(screen);
            if (_pressStartedOnUi || !InputEnabled) return;

            _pressPosition = screen;
            _lastPosition = screen;
            var (item, collectible, container) = Pick(screen);

            if (collectible != null)
            {
                _tapCollectible = collectible; // never dragged, only tapped
                return;
            }

            _tapContainer = container;
            if (Tool == ToolType.Broom)
            {
                _sweeping = true;
                OnHold(screen);
                return;
            }

            if (item == null) return;
            _tapContainer = null;
            _carrying = true;
            _magnetCategory = item.Definition.Category;
            Attach(item, Sfx.Pickup);
            OnHold(screen);
        }

        void Attach(ItemView item, Sfx sound)
        {
            item.BeginDrag();
            _stack.Add(item);
            SfxPlayer.Instance?.Play(sound, 0.05f, 0.8f);
        }

        void OnHold(Vector2 screen)
        {
            if (!InputEnabled) return;
            if (_sweeping) Sweep(screen);
            if (_stack.Count == 0) return;

            // Draw the lead item a bit above the finger so it stays visible.
            var offset = _feel.FingerOffsetPixels * (_uiScale?.Invoke() ?? 1f);
            var aim = screen + new Vector2(0f, offset);
            var ray = _camera.ScreenPointToRay(aim);

            var shelf = RaycastShelf(ray, out var hitPoint);
            Vector3 target;
            if (shelf != null)
            {
                target = hitPoint - ray.direction * 0.25f; // hover in front of the shelf
                _shelfHitPoint = hitPoint;
            }
            else
            {
                var plane = new Plane(Vector3.up, new Vector3(0f, _feel.DragLiftHeight, 0f));
                target = plane.Raycast(ray, out var enter) ? ray.GetPoint(enter) : _stack[0].transform.position;
                target = _section.ClampToRoom(target);
            }

            if (_hoveredShelf != shelf)
            {
                if (_hoveredShelf != null) _hoveredShelf.SetHovered(false);
                if (shelf != null) shelf.SetHovered(true);
                _hoveredShelf = shelf;
                _hoverTime = 0f;
            }

            _stack[0].DragTowards(target, Time.deltaTime);
            TrailStack(ray);

            if (shelf == null) CollectMore();
            else DepositAfterDwell(shelf);
        }

        /// <summary>Carried items trail behind the lead one like beads on a string.</summary>
        void TrailStack(Ray ray)
        {
            var previous = _stack[0].transform.position;
            for (var i = 1; i < _stack.Count; i++)
            {
                var follower = _stack[i];
                var away = follower.transform.position - previous;
                away = away.sqrMagnitude < 0.0001f ? -ray.direction : away.normalized;
                follower.DragTowards(previous + away * StackSpacing, Time.deltaTime);
                previous = follower.transform.position;
            }
        }

        /// <summary>
        /// Hand: anything the carried item passes over is picked up. Magnet: same-category items inside the
        /// magnet radius are pulled in. Both stop at the capacity.
        /// </summary>
        void CollectMore()
        {
            if (_stack.Count >= Capacity) return;
            var hover = HoverPoint(_stack[0].transform.position);
            var magnet = Tool == ToolType.Magnet;
            var radius = magnet ? _ctx.ToolStats(ToolType.Magnet).Primary : _ctx.ToolStats(ToolType.Hand).Secondary * 1.5f;
            var next = _section.FindAttachable(hover, radius, magnet ? _magnetCategory : null, _stack);
            if (next != null) Attach(next, magnet ? Sfx.Magnet : Sfx.Pickup);
        }

        /// <summary>Resting over a shelf briefly drops in what belongs there; the rest stays in hand.</summary>
        void DepositAfterDwell(ShelfView shelf)
        {
            _hoverTime += Time.deltaTime;
            if (_hoverTime < _feel.ShelfDepositDwell) return;
            _hoverTime = float.MinValue; // once per visit
            var delivered = _section.DeliverStack(_stack, shelf, _shelfHitPoint, false);
            foreach (var item in delivered) _stack.Remove(item);
            if (_stack.Count == 0) EndCarry();
        }

        /// <summary>Floor point the carried item visually floats over (along the camera ray).</summary>
        Vector3 HoverPoint(Vector3 carried)
        {
            var ray = new Ray(_camera.transform.position, carried - _camera.transform.position);
            return new Plane(Vector3.up, Vector3.zero).Raycast(ray, out var enter) ? ray.GetPoint(enter) : carried;
        }

        void Sweep(Vector2 screen)
        {
            var ray = _camera.ScreenPointToRay(screen);
            if (!new Plane(Vector3.up, Vector3.zero).Raycast(ray, out var enter)) return;
            var point = _section.ClampToRoom(ray.GetPoint(enter));
            var radius = _ctx.ToolStats(ToolType.Broom).Primary;

            _broomCursor.gameObject.SetActive(true);
            _broomCursor.position = point + Vector3.up * 0.01f;
            _broomCursor.localScale = Vector3.one * (radius * 2.2f);

            var removed = _section.Sweep(point, Time.deltaTime, radius);
            var speed = (screen - _lastPosition).magnitude / Mathf.Max(Time.deltaTime, 0.001f) / Mathf.Max(1f, Screen.height);
            SfxPlayer.Instance?.SetLoop(Sfx.SweepLoop, removed ? Mathf.Clamp01(0.25f + speed * 0.6f) * 0.7f : 0.08f, 0.9f + Mathf.Clamp01(speed) * 0.3f);

            _dustTimer -= Time.deltaTime;
            if (removed && _dustTimer <= 0f)
            {
                _dustTimer = DustInterval;
                _fx.Burst(point + Vector3.up * 0.05f, _ctx.Visuals.DirtColor * 1.4f, 4, 0.8f, 0.12f, 0.6f, -0.05f);
            }
        }

        void OnRelease(Vector2 screen)
        {
            if (_pressStartedOnUi)
            {
                _pressStartedOnUi = false;
                return;
            }

            var isTap = (screen - _pressPosition).magnitude <= TapMaxMovePixels;
            StopSweeping();

            if (_carrying)
            {
                if (_stack.Count > 0)
                {
                    if (_hoveredShelf != null) _section.DeliverStack(_stack, _hoveredShelf, _shelfHitPoint, true);
                    else foreach (var item in _stack) item.Drop();
                }
                EndCarry();
            }
            else if (isTap && InputEnabled)
            {
                if (_tapCollectible != null) _section.FindCollectible(_tapCollectible);
                else if (_tapContainer != null) _section.OpenContainer(_tapContainer);
            }

            _tapCollectible = null;
            _tapContainer = null;
        }

        void EndCarry()
        {
            _stack.Clear();
            _carrying = false;
            _magnetCategory = null;
            if (_hoveredShelf != null && _hoveredShelf) _hoveredShelf.SetHovered(false);
            _hoveredShelf = null;
        }

        void StopSweeping()
        {
            if (!_sweeping) return;
            _sweeping = false;
            _broomCursor.gameObject.SetActive(false);
            SfxPlayer.Instance?.SetLoop(Sfx.SweepLoop, 0f);
        }

        /// <summary>Closest pickable item, tappable collectible and closed container under the finger.</summary>
        (ItemView item, ItemView collectible, ContainerView container) Pick(Vector2 screen)
        {
            var ray = _camera.ScreenPointToRay(screen);
            var radius = Mathf.Max(0.02f, _ctx.ToolStats(ToolType.Hand).Secondary);
            var count = Physics.SphereCastNonAlloc(ray, radius, _hits, 100f, ~0, QueryTriggerInteraction.Ignore);

            ItemView bestItem = null, bestCollectible = null;
            ContainerView bestContainer = null;
            float itemDistance = float.MaxValue, collectibleDistance = float.MaxValue, containerDistance = float.MaxValue;
            for (var i = 0; i < count; i++)
            {
                var hit = _hits[i];
                if (hit.distance <= 0f) continue; // overlap at cast start
                var item = hit.collider.GetComponentInParent<ItemView>();
                if (item != null)
                {
                    if (item.CanTapToFind && hit.distance < collectibleDistance) { bestCollectible = item; collectibleDistance = hit.distance; }
                    else if (item.CanPick && hit.distance < itemDistance) { bestItem = item; itemDistance = hit.distance; }
                    continue;
                }
                var container = hit.collider.GetComponentInParent<ContainerView>();
                if (container != null && !container.IsOpened && hit.distance < containerDistance)
                {
                    bestContainer = container;
                    containerDistance = hit.distance;
                }
            }

            // A glowing collectible always wins when it is under the finger: finding it is the best moment.
            if (bestCollectible != null) return (null, bestCollectible, null);
            // A container in front of an item wins only if it is clearly closer.
            if (bestItem != null && (bestContainer == null || itemDistance <= containerDistance + 0.05f)) return (bestItem, null, null);
            return (null, null, bestContainer);
        }

        ShelfView RaycastShelf(Ray ray, out Vector3 point)
        {
            point = default;
            var count = Physics.RaycastNonAlloc(ray, _hits, 100f, ~0, QueryTriggerInteraction.Collide);
            ShelfView best = null;
            var bestDistance = float.MaxValue;
            for (var i = 0; i < count; i++)
            {
                var shelf = _hits[i].collider.GetComponentInParent<ShelfView>();
                if (shelf == null || _hits[i].distance >= bestDistance) continue;
                best = shelf;
                bestDistance = _hits[i].distance;
                point = _hits[i].point;
            }
            return best;
        }

        /// <summary>Called when the section is rebuilt, the tool changes or input is interrupted.</summary>
        public void CancelDrag()
        {
            foreach (var item in _stack)
                if (item != null) item.Drop();
            EndCarry();
            _tapContainer = null;
            _tapCollectible = null;
            StopSweeping();
        }
    }
}
