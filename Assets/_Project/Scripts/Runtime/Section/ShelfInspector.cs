using System;
using System.Collections;
using SortingGame.Core;
using SortingGame.Data;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace SortingGame.Section
{
    /// <summary>
    /// Tap a finished shelf: the camera flies in front of it and the player can look at the sorted items up close.
    /// One finger slides along the shelf (with flick glide), two fingers pinch-zoom and slide, double tap resets.
    /// In the editor: mouse wheel zooms. Back (HUD button, Escape / Android back) flies back to the room view.
    /// </summary>
    public class ShelfInspector : MonoBehaviour
    {
        const float DoubleTapSeconds = 0.3f;
        const float DoubleTapPixels = 60f;
        const float WheelStep = 0.12f;

        public event Action<ShelfView> Opened;
        public event Action Closed;

        public bool IsOpen { get; private set; }
        public ShelfView Shelf { get; private set; }
        public ShelfInspectView View { get; } = new();

        Camera _camera;
        FeelConfig _feel;
        Func<Vector2, bool> _isOverUi;
        Action<bool> _setInput;
        CameraFitter _fitter;
        Coroutine _fly;
        bool _interactive;
        Vector3 _homePosition;
        Quaternion _homeRotation;
        float _aspect;

        // input state
        bool _dragging;
        bool _pressOnUi;
        Vector2 _lastPointer;
        float _lastTapTime = -1f;
        Vector2 _lastTapPosition;
        bool _twoFinger;
        float _lastPinchDistance;
        Vector2 _lastPinchCentre;

        public void Init(Camera cam, FeelConfig feel, Func<Vector2, bool> isOverUi, Action<bool> setInput)
        {
            _camera = cam;
            _feel = feel;
            _isOverUi = isOverUi;
            _setInput = setInput;
        }

        public void Open(ShelfView shelf)
        {
            if (IsOpen || shelf == null) return;
            IsOpen = true;
            Shelf = shelf;
            _interactive = false;
            _dragging = false;
            _twoFinger = false;
            _setInput?.Invoke(false);
            _fitter = _camera.GetComponent<CameraFitter>();
            if (_fitter != null) _fitter.enabled = false;
            _homePosition = _camera.transform.position;
            _homeRotation = _camera.transform.rotation;

            Setup(shelf);
            View.Reset();
            SfxPlayer.Instance?.PlayPitched(Sfx.Magnet, 0.6f, 0.5f);
            Opened?.Invoke(shelf);
            FlyTo(PosePosition(), Rotation, () => _interactive = true);
        }

        /// <summary>Back to the room view, the way the player left it.</summary>
        public void Close()
        {
            if (!IsOpen) return;
            IsOpen = false;
            _interactive = false;
            Closed?.Invoke();
            FlyTo(_homePosition, _homeRotation, GiveBack);
        }

        /// <summary>Leaving the section: give the camera back immediately.</summary>
        public void StopNow()
        {
            if (!IsOpen && _fly == null) return;
            if (_fly != null) StopCoroutine(_fly);
            _fly = null;
            var wasOpen = IsOpen;
            IsOpen = false;
            _interactive = false;
            if (wasOpen) Closed?.Invoke();
            Shelf = null;
            if (_fitter != null) _fitter.enabled = true;
        }

        void GiveBack()
        {
            Shelf = null;
            if (_fitter != null)
            {
                _fitter.enabled = true;
                _fitter.Fit();
            }
            _setInput?.Invoke(true);
        }

        Quaternion Rotation => Quaternion.Euler(_feel.InspectPitch, 0f, 0f);

        void Setup(ShelfView shelf)
        {
            _aspect = _camera.aspect;
            var tanVertical = Mathf.Tan(_camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            View.ViewPerDistance = new Vector2(tanVertical * _camera.aspect, tanVertical);
            View.HalfSize = new Vector2(shelf.Size.x, shelf.Size.y) * 0.5f;
            View.MinDistance = _feel.InspectMinFrameWidth * 0.5f / View.ViewPerDistance.x;
            View.MaxDistance = Mathf.Max(View.MinDistance, _feel.InspectMaxFrameWidth * 0.5f / View.ViewPerDistance.x);
        }

        /// <summary>Camera position for the current focus and distance, looking at the shelf front.</summary>
        Vector3 PosePosition()
        {
            var size = Shelf.Size;
            var centre = Shelf.transform.position + new Vector3(0f, size.y * 0.5f, -size.z * 0.5f);
            var target = centre + new Vector3(View.Focus.x, View.Focus.y, 0f);
            return target - Rotation * Vector3.forward * View.Distance;
        }

        void FlyTo(Vector3 position, Quaternion rotation, Action done)
        {
            if (_fly != null) StopCoroutine(_fly);
            _fly = StartCoroutine(Fly(position, rotation, done));
        }

        IEnumerator Fly(Vector3 position, Quaternion rotation, Action done)
        {
            var fromPosition = _camera.transform.position;
            var fromRotation = _camera.transform.rotation;
            var duration = Mathf.Max(0.01f, _feel.InspectFlyDuration);
            for (var time = 0f; time < duration; time += Time.deltaTime)
            {
                var t = Ease.InOutQuad(Mathf.Clamp01(time / duration));
                _camera.transform.SetPositionAndRotation(Vector3.Lerp(fromPosition, position, t), Quaternion.Slerp(fromRotation, rotation, t));
                yield return null;
            }
            _camera.transform.SetPositionAndRotation(position, rotation);
            _fly = null;
            done?.Invoke();
        }

        void Update()
        {
            if (!IsOpen || !_interactive || Shelf == null) return;
            var keyboard = Keyboard.current;
            if (keyboard != null && keyboard.escapeKey.wasPressedThisFrame)
            {
                Close();
                return;
            }
            if (!Mathf.Approximately(_aspect, _camera.aspect))
            {
                // Screen shape changed: keep the same shelf width on screen.
                var oldWidth = View.ViewPerDistance.x;
                Setup(Shelf);
                View.ZoomBy(View.ViewPerDistance.x / oldWidth);
            }
            HandleInput();
            if (!_dragging && !_twoFinger) View.Tick(Time.deltaTime);
            _camera.transform.SetPositionAndRotation(PosePosition(), Rotation);
        }

        void HandleInput()
        {
            var touches = ActiveTouches(out var a, out var b);
            if (touches >= 2)
            {
                var centre = (a + b) * 0.5f;
                var distance = Vector2.Distance(a, b);
                if (_twoFinger && _lastPinchDistance > 1f)
                {
                    View.ZoomBy(distance / _lastPinchDistance);
                    View.PanBy(-(centre - _lastPinchCentre) * WorldPerPixel);
                }
                _twoFinger = true;
                _dragging = false;
                _lastPinchDistance = distance;
                _lastPinchCentre = centre;
                return;
            }
            if (_twoFinger && touches == 0) _twoFinger = false;
            if (_twoFinger) return; // wait until all fingers lift to avoid a jump

            var mouse = Mouse.current;
            if (mouse != null)
            {
                var scroll = mouse.scroll.ReadValue().y;
                if (Mathf.Abs(scroll) > 0.01f) View.ZoomBy(1f + Mathf.Sign(scroll) * WheelStep);
            }

            var pointer = Pointer.current;
            if (pointer == null) return;
            var position = pointer.position.ReadValue();
            if (pointer.press.wasPressedThisFrame)
            {
                _pressOnUi = _isOverUi != null && _isOverUi(position);
                if (_pressOnUi) return;

                if (Time.unscaledTime - _lastTapTime < DoubleTapSeconds && Vector2.Distance(position, _lastTapPosition) < DoubleTapPixels)
                {
                    View.Reset();
                    _lastTapTime = -1f;
                }
                else
                {
                    _lastTapTime = Time.unscaledTime;
                    _lastTapPosition = position;
                }
                _dragging = true;
                _lastPointer = position;
                View.Hold();
            }
            else if (pointer.press.isPressed && _dragging)
            {
                // The shelf follows the finger.
                var delta = position - _lastPointer;
                _lastPointer = position;
                View.PanBy(-delta * WorldPerPixel, Time.deltaTime);
            }
            if (pointer.press.wasReleasedThisFrame) _dragging = false;
        }

        /// <summary>Metres on the shelf face per screen pixel at the current distance.</summary>
        float WorldPerPixel => 2f * View.Distance * View.ViewPerDistance.y / Mathf.Max(1f, Screen.height);

        static int ActiveTouches(out Vector2 first, out Vector2 second)
        {
            first = second = default;
            var screen = Touchscreen.current;
            if (screen == null) return 0;
            var count = 0;
            foreach (TouchControl touch in screen.touches)
            {
                if (!touch.press.isPressed) continue;
                if (count == 0) first = touch.position.ReadValue();
                else if (count == 1) second = touch.position.ReadValue();
                count++;
            }
            return count;
        }
    }
}
