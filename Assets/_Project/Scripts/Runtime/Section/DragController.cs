using System;
using SortingGame.Core;
using SortingGame.Data;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SortingGame.Section
{
    /// <summary>
    /// GDD 7.1 Hand tool: press on an item to pick it up, drag, release over a shelf.
    /// Tapping a container opens it. Pointer.current makes mouse and touch behave the same.
    /// </summary>
    public class DragController : MonoBehaviour
    {
        const float TapMaxMovePixels = 30f;

        readonly RaycastHit[] _hits = new RaycastHit[32];

        Camera _camera;
        SectionController _section;
        FeelConfig _feel;
        Func<Vector2, bool> _isOverUi;
        Func<float> _uiScale;

        ItemView _dragged;
        ShelfView _hoveredShelf;
        Vector3 _shelfHitPoint;
        ContainerView _tapCandidate;
        Vector2 _pressPosition;
        bool _pressStartedOnUi;

        public bool InputEnabled { get; set; } = true;

        public void Init(Camera cam, SectionController section, FeelConfig feel, Func<Vector2, bool> isOverUi, Func<float> uiScale)
        {
            _camera = cam;
            _section = section;
            _feel = feel;
            _isOverUi = isOverUi;
            _uiScale = uiScale;
        }

        void Update()
        {
            var pointer = Pointer.current;
            if (pointer == null || _section == null) return;
            var position = pointer.position.ReadValue();

            if (pointer.press.wasPressedThisFrame) OnPress(position);
            else if (pointer.press.isPressed) OnHold(position);
            if (pointer.press.wasReleasedThisFrame) OnRelease(position);
        }

        void OnPress(Vector2 screen)
        {
            _pressStartedOnUi = _isOverUi != null && _isOverUi(screen);
            if (_pressStartedOnUi || !InputEnabled) return;

            _pressPosition = screen;
            var ray = _camera.ScreenPointToRay(screen);
            var count = Physics.SphereCastNonAlloc(ray, _feel.PickRadius, _hits, 100f, ~0, QueryTriggerInteraction.Ignore);

            ItemView bestItem = null;
            ContainerView bestContainer = null;
            float bestItemDistance = float.MaxValue, bestContainerDistance = float.MaxValue;
            for (var i = 0; i < count; i++)
            {
                var hit = _hits[i];
                if (hit.distance <= 0f) continue; // overlap at cast start
                var item = hit.collider.GetComponentInParent<ItemView>();
                if (item != null && item.CanPick && hit.distance < bestItemDistance)
                {
                    bestItem = item;
                    bestItemDistance = hit.distance;
                    continue;
                }
                var container = hit.collider.GetComponentInParent<ContainerView>();
                if (container != null && !container.IsOpened && hit.distance < bestContainerDistance)
                {
                    bestContainer = container;
                    bestContainerDistance = hit.distance;
                }
            }

            // A container in front of an item wins only if it is clearly closer.
            if (bestItem != null && (bestContainer == null || bestItemDistance <= bestContainerDistance + 0.05f))
            {
                _dragged = bestItem;
                _dragged.BeginDrag();
                SfxPlayer.Instance?.Play(Sfx.Pickup);
                OnHold(screen);
            }
            else
            {
                _tapCandidate = bestContainer;
            }
        }

        void OnHold(Vector2 screen)
        {
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

        void OnRelease(Vector2 screen)
        {
            if (_pressStartedOnUi)
            {
                _pressStartedOnUi = false;
                return;
            }

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
                return;
            }

            if (_tapCandidate != null)
            {
                if ((screen - _pressPosition).magnitude <= TapMaxMovePixels) _section.OpenContainer(_tapCandidate);
                _tapCandidate = null;
            }
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

        /// <summary>Called when the section is rebuilt or input is interrupted.</summary>
        public void CancelDrag()
        {
            if (_dragged != null && _dragged) _dragged.Drop();
            _dragged = null;
            if (_hoveredShelf != null && _hoveredShelf) _hoveredShelf.SetHovered(false);
            _hoveredShelf = null;
            _tapCandidate = null;
        }
    }
}
