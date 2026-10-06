using System;
using SortingGame.Core;
using SortingGame.Data;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace SortingGame.Section
{
    /// <summary>
    /// GDD 9.1.1 "Vitrin": a found collectible on display in the middle of a dimmed screen.
    /// One finger turns it (with inertia), two fingers pinch-zoom and pan, double tap resets.
    /// In the editor: mouse wheel zooms, right/middle drag pans.
    /// </summary>
    public class CollectionViewer : MonoBehaviour
    {
        const float DoubleTapSeconds = 0.3f;
        const float DoubleTapPixels = 60f;
        const float DisplayDistance = 2.4f;

        public bool IsOpen { get; private set; }
        public OrbitView View { get; } = new();

        Camera _camera;
        FeelConfig _feel;
        PlaceholderFactory _factory;
        Func<Vector2, bool> _isOverUi;
        Action<bool> _setGameInput;

        Transform _stage;
        Transform _pivot;
        Transform _model;
        MeshRenderer _dim;
        MeshRenderer _halo;
        Light _light;
        float _baseScale;
        float _openAmount;
        Coroutine _fade;

        // input state
        bool _rotating;
        bool _pressOnUi;
        Vector2 _lastPointer;
        float _lastTapTime = -1f;
        Vector2 _lastTapPosition;
        bool _twoFinger;
        float _lastPinchDistance;
        Vector2 _lastPinchCentre;

        public void Init(Camera cam, FeelConfig feel, SectionVisuals visuals, Func<Vector2, bool> isOverUi, Action<bool> setGameInput)
        {
            _camera = cam;
            _feel = feel;
            _factory = new PlaceholderFactory(visuals);
            _isOverUi = isOverUi;
            _setGameInput = setGameInput;

            _stage = new GameObject("ViewerStage").transform;
            _stage.SetParent(cam.transform, false);
            _dim = _factory.CreateSoftQuad("Dim", _stage, new Color(0.04f, 0.03f, 0.02f, 0f), null);
            _dim.transform.localPosition = new Vector3(0f, 0f, DisplayDistance + 1.2f);
            _halo = _factory.CreateSoftQuad("Halo", _stage, new Color(1f, 0.85f, 0.45f, 0f), ProceduralTextures.SoftDot);
            _halo.transform.localPosition = new Vector3(0f, 0f, DisplayDistance + 0.8f);
            _pivot = new GameObject("Pivot").transform;
            _pivot.SetParent(_stage, false);

            // Own key light so items look good whatever the room mood is.
            var lightObject = new GameObject("ViewerLight");
            lightObject.transform.SetParent(_stage, false);
            lightObject.transform.localPosition = new Vector3(0.8f, 1.2f, DisplayDistance - 1.6f);
            _light = lightObject.AddComponent<Light>();
            _light.type = LightType.Point;
            _light.range = 6f;
            _light.color = new Color(1f, 0.95f, 0.88f);
            _light.intensity = 0f;

            _stage.gameObject.SetActive(false);
        }

        public void Open(CollectibleDefinition collectible)
        {
            if (_model != null) Destroy(_model.gameObject);
            _model = new GameObject($"View_{collectible.Id}").transform;
            _model.SetParent(_pivot, false);
            _factory.CreateItemVisual(collectible, _model, false);

            var size = collectible.Placeholder.Size;
            var largest = Mathf.Max(size.x, Mathf.Max(size.y, size.z));
            _baseScale = 1f / Mathf.Max(0.05f, largest);

            View.Reset();
            View.MinZoom = _feel.ViewerMinZoom;
            View.MaxZoom = _feel.ViewerMaxZoom;
            View.IdleSpinSpeed = _feel.ViewerIdleSpinSpeed;
            View.Rotate(new Vector2(-25f, 10f)); // start at a flattering three-quarter angle

            IsOpen = true;
            _rotating = false;
            _twoFinger = false;
            _setGameInput?.Invoke(false);
            _stage.gameObject.SetActive(true);
            SfxPlayer.Instance?.Play(Sfx.BookStamp, 0.02f, 0.7f);
            FadeTo(1f, 0.3f, null);
        }

        public void Close()
        {
            if (!IsOpen) return;
            IsOpen = false;
            FadeTo(0f, 0.2f, () =>
            {
                _stage.gameObject.SetActive(false);
                if (_model != null) Destroy(_model.gameObject);
                _setGameInput?.Invoke(true);
            });
        }

        void FadeTo(float target, float duration, Action done)
        {
            Tween.Stop(_fade);
            var start = _openAmount;
            _fade = Tween.Run(this, duration, t => _openAmount = Mathf.Lerp(start, target, t), Ease.OutCubic, done);
        }

        void Update()
        {
            if (!_stage.gameObject.activeSelf) return;
            if (IsOpen) HandleInput();
            if (!_rotating && !_twoFinger) View.Tick(Time.deltaTime);
            Apply();
        }

        void Apply()
        {
            var frustumHeight = 2f * DisplayDistance * Mathf.Tan(_camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            var frustumWidth = frustumHeight * _camera.aspect;
            var screenSize = Mathf.Min(frustumWidth, frustumHeight) * _feel.ViewerScreenShare;
            View.PanLimit = new Vector2(frustumWidth * 0.4f, frustumHeight * 0.3f);

            // Slightly above centre: the name and close button sit below.
            var centre = new Vector3(0f, frustumHeight * 0.06f, DisplayDistance);
            _pivot.localPosition = centre + (Vector3)View.Pan;
            // Turntable feel: yaw around the item's own vertical, then tilt towards/away from the viewer.
            _pivot.localRotation = Quaternion.AngleAxis(View.Pitch, Vector3.right) * Quaternion.AngleAxis(View.Yaw, Vector3.up);
            var pop = Ease.OutBack(Mathf.Clamp01(_openAmount));
            _pivot.localScale = Vector3.one * (screenSize * _baseScale * View.Zoom * pop);

            SetAlpha(_dim, _feel.ViewerDimAlpha * _openAmount);
            SetAlpha(_halo, 0.45f * _openAmount);
            _dim.transform.localScale = new Vector3(frustumWidth * 2f, frustumHeight * 2f, 1f);
            _halo.transform.localPosition = new Vector3(_pivot.localPosition.x, _pivot.localPosition.y, DisplayDistance + 0.8f);
            _halo.transform.localScale = Vector3.one * (screenSize * 1.8f * View.Zoom);
            _light.intensity = 2.5f * _openAmount;
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
                    View.PanBy((centre - _lastPinchCentre) * WorldPerPixel);
                }
                _twoFinger = true;
                _rotating = false;
                _lastPinchDistance = distance;
                _lastPinchCentre = centre;
                return;
            }
            if (_twoFinger && touches == 0) _twoFinger = false;
            if (_twoFinger) return; // wait until all fingers lift to avoid a jump

            // Editor helpers: wheel zoom, right/middle drag pan.
            var mouse = Mouse.current;
            if (mouse != null)
            {
                var scroll = mouse.scroll.ReadValue().y;
                if (Mathf.Abs(scroll) > 0.01f) View.ZoomBy(1f + Mathf.Sign(scroll) * 0.1f);
                if (mouse.rightButton.isPressed || mouse.middleButton.isPressed)
                {
                    View.PanBy(mouse.delta.ReadValue() * WorldPerPixel);
                    return;
                }
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
                _rotating = true;
                _lastPointer = position;
                View.Hold();
            }
            else if (pointer.press.isPressed && _rotating)
            {
                var delta = position - _lastPointer;
                _lastPointer = position;
                var degreesPerPixel = _feel.ViewerRotateDegreesPerScreen / Mathf.Max(1f, Screen.width);
                View.Rotate(new Vector2(-delta.x, delta.y) * degreesPerPixel, Time.deltaTime);
            }
            if (pointer.press.wasReleasedThisFrame) _rotating = false;
        }

        float WorldPerPixel => 2f * DisplayDistance * Mathf.Tan(_camera.fieldOfView * 0.5f * Mathf.Deg2Rad) / Mathf.Max(1f, Screen.height);

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

        static void SetAlpha(MeshRenderer renderer, float alpha)
        {
            var material = renderer.sharedMaterial;
            var c = material.GetColor("_BaseColor");
            c.a = alpha;
            material.SetColor("_BaseColor", c);
        }
    }
}
