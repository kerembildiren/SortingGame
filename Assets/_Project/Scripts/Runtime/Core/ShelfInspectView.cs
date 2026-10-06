using UnityEngine;

namespace SortingGame.Core
{
    /// <summary>
    /// Close-up look at a finished shelf: where the camera looks on the shelf face (Focus, metres from the shelf
    /// centre, x right / y up) and how far it stands (Distance). Panning stops at the shelf edges, zoom stays between
    /// a close-up and the whole shelf, flicks glide. Plain class (no scene access) so the feel can be unit tested.
    /// </summary>
    public class ShelfInspectView
    {
        public Vector2 Focus { get; private set; }
        public float Distance { get; private set; } = 1f;

        /// <summary>Half width / half height of the shelf face in metres.</summary>
        public Vector2 HalfSize;
        /// <summary>Visible half width / half height per metre of distance (tangents of the camera's half angles).</summary>
        public Vector2 ViewPerDistance = new(0.2f, 0.35f);
        public float MinDistance = 0.5f;
        public float MaxDistance = 3f;
        /// <summary>Share of the half view that must still show shelf at the pan limit (1 = shelf edge on the screen edge).</summary>
        public float EdgeShare = 0.85f;
        /// <summary>How fast a flick slows down (1/s).</summary>
        public float Damping = 5f;

        Vector2 _velocity;

        /// <summary>Furthest the focus may move from the centre at the current distance. Zero when the whole shelf fits.</summary>
        public Vector2 PanLimit
        {
            get
            {
                var visible = ViewPerDistance * (Distance * EdgeShare);
                return new Vector2(Mathf.Max(0f, HalfSize.x - visible.x), Mathf.Max(0f, HalfSize.y - visible.y));
            }
        }

        public bool IsGliding => _velocity.sqrMagnitude > 0.0001f;

        /// <summary>
        /// Distance at which the whole shelf face fits with the same air as at the pan limit (so nothing is left to pan),
        /// capped to MaxDistance.
        /// </summary>
        public float FitDistance
        {
            get
            {
                var air = 1f / Mathf.Max(0.1f, EdgeShare);
                var x = HalfSize.x * air / Mathf.Max(0.01f, ViewPerDistance.x);
                var y = HalfSize.y * air / Mathf.Max(0.01f, ViewPerDistance.y);
                return Mathf.Clamp(Mathf.Max(x, y), MinDistance, MaxDistance);
            }
        }

        /// <summary>Back to the start framing: centred, as much of the shelf as allowed.</summary>
        public void Reset()
        {
            Focus = Vector2.zero;
            Distance = FitDistance;
            _velocity = Vector2.zero;
        }

        /// <summary>Move the look point by <paramref name="delta"/> metres on the shelf face.</summary>
        public void PanBy(Vector2 delta, float deltaTime = 0f)
        {
            Focus = Clamp(Focus + delta);
            if (deltaTime > 0f) _velocity = Vector2.Lerp(_velocity, delta / deltaTime, 0.5f);
        }

        /// <summary>factor &gt; 1 moves closer.</summary>
        public void ZoomBy(float factor)
        {
            if (factor <= 0f) return;
            Distance = Mathf.Clamp(Distance / factor, MinDistance, MaxDistance);
            Focus = Clamp(Focus);
        }

        /// <summary>Finger is down without moving: stop gliding.</summary>
        public void Hold() => _velocity = Vector2.zero;

        /// <summary>Advance the flick glide. Call once per frame while nobody is dragging.</summary>
        public void Tick(float deltaTime)
        {
            if (!IsGliding)
            {
                _velocity = Vector2.zero;
                return;
            }
            var before = Focus;
            Focus = Clamp(Focus + _velocity * deltaTime);
            // Hitting an edge stops the glide on that axis.
            if (Mathf.Approximately(Focus.x, before.x)) _velocity.x = 0f;
            if (Mathf.Approximately(Focus.y, before.y)) _velocity.y = 0f;
            _velocity *= Mathf.Exp(-Damping * deltaTime);
        }

        Vector2 Clamp(Vector2 focus)
        {
            var limit = PanLimit;
            return new Vector2(Mathf.Clamp(focus.x, -limit.x, limit.x), Mathf.Clamp(focus.y, -limit.y, limit.y));
        }
    }
}
