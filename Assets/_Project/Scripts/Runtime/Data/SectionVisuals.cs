using UnityEngine;

namespace SortingGame.Data
{
    /// <summary>Grey-box look of a section: materials, colours and mood lighting (GDD 12.1 dirty vs clean).</summary>
    [CreateAssetMenu(menuName = "Sorting Game/Section Visuals", fileName = "SectionVisuals")]
    public class SectionVisuals : ScriptableObject
    {
        [Header("Materials (shaders must be referenced by an asset to be included in builds)")]
        public Material LitMaterial;
        public Material GhostMaterial;
        [Tooltip("Lit + transparent, for the dirt layer.")]
        public Material DirtMaterial;
        [Tooltip("URP Particles/Unlit, for dust and sparkles.")]
        public Material ParticleMaterial;

        [Header("Room: dirty")]
        public Color FloorColor = new(0.42f, 0.36f, 0.30f);
        public Color WallColor = new(0.30f, 0.27f, 0.26f);
        public Color BackgroundColor = new(0.10f, 0.09f, 0.09f);

        [Header("Room: clean (after 100%)")]
        public Color FloorColorClean = new(0.78f, 0.62f, 0.44f);
        public Color WallColorClean = new(0.93f, 0.85f, 0.72f);
        public Color BackgroundColorClean = new(0.22f, 0.18f, 0.15f);

        [Header("Dirt")]
        public Color DirtColor = new(0.30f, 0.25f, 0.19f);
        public Color DirtSpeckColor = new(0.78f, 0.72f, 0.60f);

        [Header("Props")]
        public Color WoodColor = new(0.62f, 0.42f, 0.24f);
        public Color SignColor = new(0.96f, 0.89f, 0.76f);
        public Color CardboardColor = new(0.74f, 0.56f, 0.36f);
        public Color SlotGhostColor = new(1f, 0.97f, 0.9f, 0.55f);

        [Header("Glow")]
        [Tooltip("Gold: Chubby figures, the only collectibles (GDD 9.3).")]
        public Color RareGlowColor = new(1f, 0.82f, 0.3f, 0.75f);
        [Tooltip("Blue: rare items that go on a shelf (GDD 9.4).")]
        public Color RareItemGlowColor = new(0.35f, 0.7f, 1f, 0.7f);

        [Header("Mood: dirty -> clean")]
        public Color LightColor = new(1f, 0.86f, 0.68f);
        public float LightIntensityDirty = 0.9f;
        public float LightIntensityClean = 1.5f;
        public Color AmbientDirty = new(0.30f, 0.27f, 0.25f);
        public Color AmbientClean = new(0.55f, 0.50f, 0.44f);
    }
}
