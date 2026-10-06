using System;
using SortingGame.Core;
using SortingGame.Data;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SortingGame.Section
{
    /// <summary>GDD 10.1 tools. Magnet and Magnifier arrive in M3.</summary>
    public enum ToolType
    {
        Hand,
        Broom
    }

    /// <summary>
    /// GDD 7.1 gestures, one finger: Hand drags items to shelves, Broom sweeps the dirt layer.
    /// With any tool, a tap opens a box or picks up a glowing collectible.
    /// Pointer.current makes mouse and touch behave the same.
    /// </summary>
    public class DragController : MonoBehaviour
    {
        const float TapMaxMovePixels = 30f;
        const float DustInterval = 0.06f;

        readonly RaycastHit[] _hits = new RaycastHit[32];

        Camera _camera;
        SectionController _section;
        FeelConfig _feel;
        Func<Vector2, bool> _isOverUi;
        Func<float> _uiScale;
        Fx _fx;
        SectionVisuals _visuals;

        ItemView _dragged;
        ShelfView _hoveredShelf;
        Vector3 _shelfHitPoint;
        ContainerView _tapContainer;
        ItemView _tapCollectible;
        Vector2 _pressPosition;
        Vector2 _lastPosition;
        bool _pressStartedOnUi;
        bool _sweeping;
        float _dustTimer;
        Transform _broomCursor;

        public bool InputEnabled { get; set; } = true;
        public ToolType Tool { get; private set; } = ToolType.Hand;

        public void Init(Camera cam, SectionController section, FeelConfig feel, SectionVisuals visuals, Func<Vector2, bool> isOverUi, Func<float> uiScale)
        {
            _camera = cam;
            _section = section;
            _feel = feel;
            _visuals = visuals;
            _isOverUi = isOverUi;
            _uiScale = uiScale;
            _fx = new Fx(visuals);

            var factory = new PlaceholderFactory(visuals);
            var cursor = factory.CreateSoftQuad("BroomCursor", null, new Color(1f, 0.95f, 0.8f, 0.35f), ProceduralTextures.SoftDot);
            cursor.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            _broomCursor = cursor.transform;
            _broomCursor.gameObject.SetActive(false);
        }

        public void SetTool(ToolType tool)
        {
            CancelDrag();
            Tool = tool;
        }

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
            if (Tool == ToolType.Hand)
            {
                if (item == null) return;
                _tapContainer = null;
                _dragged = item;
                _dragged.BeginDrag();
                SfxPlayer.Instance?.Play(Sfx.Pickup);
                OnHold(screen);
            }
            else
            {
                _sweeping = true;
                OnHold(screen);
            }
        }

        void OnHold(Vector2 screen)
        {
            if (!InputEnabled) return;
            if (_sweeping) Sweep(screen);
            if (_dragged == null) return;

            // Draw the item a bit above the finger so it stays visible.
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
                target = plane.Raycast(ray, out var enter) ? ray.GetPoint(enter) : _dragged.transform.position;
                target = _section.ClampToRoom(target);
            }

            if (_hoveredShelf != shelf)
            {
                if (_hoveredShelf != null) _hoveredShelf.SetHovered(false);
                if (shelf != null) shelf.SetHovered(true);
                _hoveredShelf = shelf;
            }

            _dragged.DragTowards(target, Time.deltaTime);
        }

        void Sweep(Vector2 screen)
        {
            var ray = _camera.ScreenPointToRay(screen);
            if (!new Plane(Vector3.up, Vector3.zero).Raycast(ray, out var enter))
                return;
            var point = _section.ClampToRoom(ray.GetPoint(enter));

            _broomCursor.gameObject.SetActive(true);
            _broomCursor.position = point + Vector3.up * 0.01f;
            _broomCursor.localScale = Vector3.one * (_feel.BroomRadius * 2.2f);

            var removed = _section.Sweep(point, Time.deltaTime);
            var speed = (screen - _lastPosition).magnitude / Mathf.Max(Time.deltaTime, 0.001f) / Mathf.Max(1f, Screen.height);
            SfxPlayer.Instance?.SetLoop(Sfx.SweepLoop, removed ? Mathf.Clamp01(0.25f + speed * 0.6f) * 0.7f : 0.08f, 0.9f + Mathf.Clamp01(speed) * 0.3f);

            _dustTimer -= Time.deltaTime;
            if (removed && _dustTimer <= 0f)
            {
                _dustTimer = DustInterval;
                _fx.Burst(point + Vector3.up * 0.05f, _visuals.DirtColor * 1.4f, 4, 0.8f, 0.12f, 0.6f, -0.05f);
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

            if (_dragged != null)
            {
                var item = _dragged;
                _dragged = null;
                if (_hoveredShelf != null)
                {
                    _hoveredShelf.SetHovered(false);
                    _section.TryPlace(item, _hoveredShelf, _shelfHitPoint);
                    _hoveredShelf = null;
                }
                else
                {
                    item.Drop();
                }
            }
            else if (isTap && InputEnabled)
            {
                if (_tapCollectible != null) _section.FindCollectible(_tapCollectible);
                else if (_tapContainer != null) _section.OpenContainer(_tapContainer);
            }

            _tapCollectible = null;
            _tapContainer = null;
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
            var count = Physics.SphereCastNonAlloc(ray, _feel.PickRadius, _hits, 100f, ~0, QueryTriggerInteraction.Ignore);

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
            if (_dragged != null && _dragged) _dragged.Drop();
            _dragged = null;
            if (_hoveredShelf != null && _hoveredShelf) _hoveredShelf.SetHovered(false);
            _hoveredShelf = null;
            _tapContainer = null;
            _tapCollectible = null;
            StopSweeping();
        }
    }
}
