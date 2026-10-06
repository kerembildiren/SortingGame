using SortingGame.Data;
using UnityEngine;

namespace SortingGame.Section
{
    /// <summary>
    /// GDD 6.2: 3/4 top-down view that frames the whole section between the top bar and the tool bar,
    /// for any portrait aspect ratio.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class CameraFitter : MonoBehaviour
    {
        Camera _camera;
        Bounds _target;
        FeelConfig _feel;
        Vector2Int _lastScreen;
        bool _hasTarget;

        void Awake() => _camera = GetComponent<Camera>();

        public void Frame(Bounds target, FeelConfig feel)
        {
            _target = target;
            _feel = feel;
            _hasTarget = true;
            Fit();
        }

        void LateUpdate()
        {
            if (!_hasTarget) return;
            var screen = new Vector2Int(Screen.width, Screen.height);
            if (screen != _lastScreen) Fit();
        }

        public void Fit()
        {
            _lastScreen = new Vector2Int(Screen.width, Screen.height);
            _camera.fieldOfView = _feel.CameraFov;
            var rotation = Quaternion.Euler(_feel.CameraPitch, 0f, 0f);
            transform.rotation = rotation;

            var safeMin = _feel.BottomSafeArea;
            var safeMax = 1f - _feel.TopSafeArea;
            var focus = _target.center;

            // Binary search the distance at which all corners fit, then re-centre vertically. Two passes is plenty.
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
                var distance = far;
                var worldPerViewport = 2f * distance * Mathf.Tan(_camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
                focus += rotation * Vector3.up * (offset * worldPerViewport);
            }
        }

        bool Fits(float safeMin, float safeMax, out float minY, out float maxY)
        {
            minY = float.MaxValue;
            maxY = float.MinValue;
            var fits = true;
            var e = _target.extents;
            for (var i = 0; i < 8; i++)
            {
                var corner = _target.center + new Vector3((i & 1) == 0 ? -e.x : e.x, (i & 2) == 0 ? -e.y : e.y, (i & 4) == 0 ? -e.z : e.z);
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
