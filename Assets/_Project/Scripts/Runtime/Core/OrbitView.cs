using UnityEngine;

namespace SortingGame.Core
{
    /// <summary>
    /// GDD 9.1.1 viewer state: orbit angles, zoom and pan with limits, spin inertia and idle auto-turn.
    /// Plain class (no scene access) so the feel can be unit tested.
    /// </summary>
    public class OrbitView
    {
        public float Yaw { get; private set; }
        public float Pitch { get; private set; }
        public float Zoom { get; private set; } = 1f;
        public Vector2 Pan { get; private set; }

        public float MinPitch = -75f;
        public float MaxPitch = 75f;
        public float MinZoom = 0.6f;
        public float MaxZoom = 2.5f;
        public Vector2 PanLimit = new(0.4f, 0.4f);
        /// <summary>How fast a flick slows down (1/s).</summary>
        public float Damping = 4f;
        /// <summary>Degrees per second when nobody touches it.</summary>
        public float IdleSpinSpeed = 18f;
        public float IdleDelay = 2f;

        Vector2 _velocity; // degrees per second (yaw, pitch)
        float _idleTimer;

        public bool IsIdleSpinning => _idleTimer >= IdleDelay && _velocity.sqrMagnitude < 1f;

        /// <summary>Direct drag. <paramref name="deltaDegrees"/> = (yaw, pitch) change this frame.</summary>
        public void Rotate(Vector2 deltaDegrees, float deltaTime = 0f)
        {
            Yaw = Mathf.Repeat(Yaw + deltaDegrees.x, 360f);
            Pitch = Mathf.Clamp(Pitch + deltaDegrees.y, MinPitch, MaxPitch);
            if (deltaTime > 0f) _velocity = Vector2.Lerp(_velocity, deltaDegrees / deltaTime, 0.5f);
            _idleTimer = 0f;
        }

        public void ZoomBy(float factor)
        {
            if (factor <= 0f) return;
            Zoom = Mathf.Clamp(Zoom * factor, MinZoom, MaxZoom);
            _idleTimer = 0f;
        }

        public void PanBy(Vector2 delta)
        {
            Pan = new Vector2(Mathf.Clamp(Pan.x + delta.x, -PanLimit.x, PanLimit.x), Mathf.Clamp(Pan.y + delta.y, -PanLimit.y, PanLimit.y));
            _idleTimer = 0f;
        }

        /// <summary>Finger is down without moving: stop the spin.</summary>
        public void Hold()
        {
            _velocity = Vector2.zero;
            _idleTimer = 0f;
        }

        /// <summary>Advance inertia and idle spin. Call once per frame while nobody is dragging.</summary>
        public void Tick(float deltaTime)
        {
            if (_velocity.sqrMagnitude > 1f)
            {
                Yaw = Mathf.Repeat(Yaw + _velocity.x * deltaTime, 360f);
                Pitch = Mathf.Clamp(Pitch + _velocity.y * deltaTime, MinPitch, MaxPitch);
                _velocity *= Mathf.Exp(-Damping * deltaTime);
                return;
            }
            _velocity = Vector2.zero;
            _idleTimer += deltaTime;
            if (_idleTimer >= IdleDelay) Yaw = Mathf.Repeat(Yaw + IdleSpinSpeed * deltaTime, 360f);
        }

        public void Reset()
        {
            Yaw = 0f;
            Pitch = 0f;
            Zoom = 1f;
            Pan = Vector2.zero;
            _velocity = Vector2.zero;
            _idleTimer = 0f;
        }
    }
}
