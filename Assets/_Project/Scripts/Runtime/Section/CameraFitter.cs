using SortingGame.Data;
using UnityEngine;

namespace SortingGame.Section
{
    /// <summary>
    /// GDD 6.2: 3/4 top-down view of a section between the top bar and the tool bar, for any portrait aspect.
    /// Big sections are wider than the screen: the camera frames a fixed-width window and pans sideways (GDD 6.2).
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class CameraFitter : MonoBehaviour
    {
        const float PanDamping = 5f;

        Camera _camera;
        Bounds _window;
        FeelConfig _feel;
        Vector2Int _lastScreen;
        bool _hasTarget;
        Vector3 _basePosition;
        float _minPan;
        float _maxPan;
        float _panVelocity;

        /// <summary>Horizontal camera offset from the window framed at x = 0.</summary>
        public float PanX { get; private set; }
        public bool CanPan => _maxPan > _minPan + 0.01f;
        public float PanMin => _minPan;
        public float PanMax => _maxPan;

        void Awake() => _camera = GetComponent<Camera>();

        /// <summary>
        /// <paramref name="room"/> is the whole section; the visible window is at most FeelConfig.SectionViewWidth wide
        /// and can be panned across the room.
        /// </summary>
        public void Frame(Bounds room, FeelConfig feel)
        {
            _feel = feel;
            var width = Mathf.Min(room.size.x, feel.SectionViewWidth);
            _window = new Bounds(new Vector3(0f, room.center.y, room.center.z), new Vector3(width, room.size.y, room.size.z));
            var half = (room.size.x - width) / 2f;
            _minPan = room.center.x - half;
            _maxPan = room.center.x + half;
            PanX = Mathf.Clamp(room.center.x, _minPan, _maxPan);
            _panVelocity = 0f;
            _hasTarget = true;
            Fit();
        }

        /// <summary>Move the view sideways by <paramref name="worldDelta"/> metres (positive = look further right).</summary>
        public void Pan(float worldDelta, float deltaTime = 0f)
        {
            if (!CanPan) return;
            PanX = Mathf.Clamp(PanX + worldDelta, _minPan, _maxPan);
            if (deltaTime > 0f) _panVelocity = Mathf.Lerp(_panVelocity, worldDelta / deltaTime, 0.5f);
            Apply();
        }

        bool _dragging;

        /// <summary>Finger down for a pan: stop any glide.</summary>
        public void BeginPan()
        {
            _dragging = true;
            _panVelocity = 0f;
        }

        /// <summary>Finger lifted after a pan drag: keep gliding a little (flick).</summary>
        public void EndPan() => _dragging = false;

        public void PanTo(float x)
        {
            PanX = Mathf.Clamp(x, _minPan, _maxPan);
            _panVelocity = 0f;
            Apply();
        }

        void LateUpdate()
        {
            if (!_hasTarget) return;
            var screen = new Vector2Int(Screen.width, Screen.height);
            if (screen != _lastScreen) Fit();

            if (!_dragging && Mathf.Abs(_panVelocity) > 0.01f)
            {
                PanX = Mathf.Clamp(PanX + _panVelocity * Time.deltaTime, _minPan, _maxPan);
                _panVelocity *= Mathf.Exp(-PanDamping * Time.deltaTime);
                Apply();
            }
        }

        void Apply() => transform.position = _basePosition + Vector3.right * PanX;

        public void Fit()
        {
            if (!_hasTarget) return;
            _lastScreen = new Vector2Int(Screen.width, Screen.height);
            _camera.orthographic = false;
            _camera.fieldOfView = _feel.CameraFov;
            var rotation = Quaternion.Euler(_feel.CameraPitch, 0f, 0f);
            transform.rotation = rotation;

            var safeMin = _feel.BottomSafeArea;
            var safeMax = 1f - _feel.TopSafeArea;
            var focus = _window.center;

            // Binary search the distance at which all corners fit, then re-centre vertically. A few passes is plenty.
            for (var pass = 0; pass < 3; pass++)
            {
                float near = 0.5f, far = 100f;
                for (var i = 0; i < 30; i++)
                {
                    var mid = (near + far) * 0.5f;
                    transform.position = focus - rotation * Vector3.forward * mid;
                    if (Fits(safeMin, safeMax, out _, out _)) far = mid; else near = mid;
                }
                transform.position = focus - rotation * Vector3.forward * far;

                Fits(safeMin, safeMax, out var minY, out var maxY);
                var offset = ((minY + maxY) - (safeMin + safeMax)) * 0.5f; // viewport units
                var worldPerViewport = 2f * far * Mathf.Tan(_camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
                focus += rotation * Vector3.up * (offset * worldPerViewport);
            }
            _basePosition = transform.position;
            Apply();
        }

        bool Fits(float safeMin, float safeMax, out float minY, out float maxY)
        {
            minY = float.MaxValue;
            maxY = float.MinValue;
            var fits = true;
            var e = _window.extents;
            for (var i = 0; i < 8; i++)
            {
                var corner = _window.center + new Vector3((i & 1) == 0 ? -e.x : e.x, (i & 2) == 0 ? -e.y : e.y, (i & 4) == 0 ? -e.z : e.z);
                var v = _camera.WorldToViewportPoint(corner);
                if (v.z <= 0f) return false;
                minY = Mathf.Min(minY, v.y);
                maxY = Mathf.Max(maxY, v.y);
                if (v.x < 0.02f || v.x > 0.98f || v.y < safeMin || v.y > safeMax) fits = false;
            }
            return fits;
        }
    }
}
