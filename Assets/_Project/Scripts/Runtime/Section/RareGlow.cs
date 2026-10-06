using SortingGame.Core;
using UnityEngine;

namespace SortingGame.Section
{
    /// <summary>GDD 9.3 / 12.1: soft golden glow + twinkles that make a rare item stand out in the clutter.</summary>
    public class RareGlow : MonoBehaviour
    {
        const float ShimmerInterval = 3.5f;

        Transform _halo;
        ParticleSystem _sparkles;
        Camera _camera;
        float _baseSize;
        float _shimmerTimer;
        bool _glowing = true;

        public static RareGlow Attach(ItemView item, PlaceholderFactory factory, Fx fx, Color color)
        {
            var glow = item.gameObject.AddComponent<RareGlow>();
            glow._baseSize = item.VisualSize * 2.4f;

            var halo = factory.CreateSoftQuad("Halo", item.transform, color, ProceduralTextures.SoftDot);
            halo.transform.localScale = Vector3.one * glow._baseSize;
            glow._halo = halo.transform;

            var sparkleColor = color;
            sparkleColor.a = 1f;
            glow._sparkles = fx.Sparkles(item.transform, sparkleColor, item.VisualSize * 0.6f);
            glow._shimmerTimer = Random.Range(0.5f, ShimmerInterval);
            return glow;
        }

        public void SetGlowing(bool glowing)
        {
            _glowing = glowing;
            if (_halo != null) _halo.gameObject.SetActive(glowing);
            if (_sparkles == null) return;
            if (glowing) _sparkles.Play();
            else _sparkles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        void LateUpdate()
        {
            if (!_glowing || _halo == null) return;
            if (_camera == null) _camera = Camera.main;
            if (_camera == null) return;

            // Billboard: halo always faces the camera, independent of how the item lies.
            _halo.rotation = _camera.transform.rotation;
            var pulse = 1f + 0.12f * Mathf.Sin(Time.time * 3.2f);
            _halo.localScale = Vector3.one * (_baseSize * pulse);

            _shimmerTimer -= Time.deltaTime;
            if (_shimmerTimer <= 0f)
            {
                _shimmerTimer = ShimmerInterval;
                SfxPlayer.Instance?.Play(Sfx.RareShimmer, 0.05f, 0.35f);
            }
        }
    }
}
