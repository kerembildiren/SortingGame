using SortingGame.Data;
using UnityEngine;

namespace SortingGame.Section
{
    /// <summary>Textures generated at runtime so placeholder effects need no art files.</summary>
    public static class ProceduralTextures
    {
        static Texture2D _softDot;
        static Texture2D _rays;

        // Statics survive Play sessions (domain reload is off) but the textures are destroyed on exit,
        // so these need Unity's null check: "??=" would hand back the destroyed texture.

        /// <summary>Round soft blob: particles, halos, broom cursor.</summary>
        public static Texture2D SoftDot
        {
            get
            {
                if (_softDot == null)
                    _softDot = Make("SoftDot", 64, (u, v) =>
                    {
                        var d = Mathf.Clamp01(Vector2.Distance(new Vector2(u, v), new Vector2(0.5f, 0.5f)) * 2f);
                        return Mathf.Pow(1f - d, 2f);
                    });
                return _softDot;
            }
        }

        /// <summary>Sun-ray burst behind a rare find (concept 03_rare_find_popup).</summary>
        public static Texture2D Rays
        {
            get
            {
                if (_rays == null)
                    _rays = Make("Rays", 256, (u, v) =>
                    {
                        var p = new Vector2(u - 0.5f, v - 0.5f);
                        var d = Mathf.Clamp01(p.magnitude * 2f);
                        var angle = Mathf.Atan2(p.y, p.x);
                        var ray = Mathf.SmoothStep(0.25f, 0.75f, 0.5f + 0.5f * Mathf.Sin(angle * 14f));
                        var centre = Mathf.Pow(1f - d, 4f);
                        return Mathf.Clamp01(ray * (1f - d) * 0.8f + centre);
                    });
                return _rays;
            }
        }

        static Texture2D Make(string name, int size, System.Func<float, float, float> alpha)
        {
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, true) { name = name, wrapMode = TextureWrapMode.Clamp };
            var pixels = new Color32[size * size];
            for (var y = 0; y < size; y++)
            for (var x = 0; x < size; x++)
                pixels[y * size + x] = new Color32(255, 255, 255, (byte)(alpha((x + 0.5f) / size, (y + 0.5f) / size) * 255f));
            texture.SetPixels32(pixels);
            texture.Apply(true);
            return texture;
        }
    }

    /// <summary>Small particle effects built in code: dust puffs, sparkles, reveal pops.</summary>
    public class Fx
    {
        readonly Material _particle;

        public Fx(SectionVisuals visuals)
        {
            _particle = new Material(visuals.ParticleMaterial);
            _particle.SetTexture("_BaseMap", ProceduralTextures.SoftDot);
        }

        public ParticleSystem Burst(Vector3 position, Color color, int count, float speed, float size, float lifetime, float gravity = 0f)
        {
            var ps = Create("FxBurst", position, null);
            var main = ps.main;
            main.duration = 0.2f;
            main.loop = false;
            main.startLifetime = new ParticleSystem.MinMaxCurve(lifetime * 0.6f, lifetime);
            main.startSpeed = new ParticleSystem.MinMaxCurve(speed * 0.4f, speed);
            main.startSize = new ParticleSystem.MinMaxCurve(size * 0.5f, size);
            main.startColor = color;
            main.gravityModifier = gravity;
            main.maxParticles = count;
            main.stopAction = ParticleSystemStopAction.Destroy;

            var emission = ps.emission;
            emission.rateOverTime = 0f;
            emission.SetBursts(new[] { new ParticleSystem.Burst(0f, (short)count) });

            var shape = ps.shape;
            shape.shapeType = ParticleSystemShapeType.Sphere;
            shape.radius = 0.05f;

            FadeOut(ps);
            ps.Play();
            return ps;
        }

        /// <summary>Looping twinkle that follows a rare item.</summary>
        public ParticleSystem Sparkles(Transform parent, Color color, float radius)
        {
            var ps = Create("FxSparkles", parent.position, parent);
            var main = ps.main;
            main.loop = true;
            main.startLifetime = new ParticleSystem.MinMaxCurve(0.5f, 1.0f);
            main.startSpeed = new ParticleSystem.MinMaxCurve(0.05f, 0.25f);
            main.startSize = new ParticleSystem.MinMaxCurve(0.04f, 0.09f);
            main.startColor = color;
            main.maxParticles = 40;
            main.simulationSpace = ParticleSystemSimulationSpace.Local;

            var emission = ps.emission;
            emission.rateOverTime = 9f;

            var shape = ps.shape;
            shape.shapeType = ParticleSystemShapeType.Sphere;
            shape.radius = radius;

            var size = ps.sizeOverLifetime;
            size.enabled = true;
            size.size = new ParticleSystem.MinMaxCurve(1f, new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.3f, 1f), new Keyframe(1f, 0f)));

            ps.Play();
            return ps;
        }

        ParticleSystem Create(string name, Vector3 position, Transform parent)
        {
            var go = new GameObject(name);
            if (parent != null) go.transform.SetParent(parent, false);
            go.transform.position = position;
            var ps = go.AddComponent<ParticleSystem>();
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            var main = ps.main;
            main.playOnAwake = false;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            var renderer = go.GetComponent<ParticleSystemRenderer>();
            renderer.sharedMaterial = _particle;
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            return ps;
        }

        static void FadeOut(ParticleSystem ps)
        {
            var colour = ps.colorOverLifetime;
            colour.enabled = true;
            var gradient = new Gradient();
            gradient.SetKeys(
                new[] { new GradientColorKey(Color.white, 0f), new GradientColorKey(Color.white, 1f) },
                new[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 0.5f), new GradientAlphaKey(0f, 1f) });
            colour.color = gradient;
        }
    }
}
