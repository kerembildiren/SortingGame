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

        [Header("Room")]
        public Color FloorColor = new(0.42f, 0.36f, 0.30f);
        public Color WallColor = new(0.30f, 0.27f, 0.26f);
        public Color BackgroundColor = new(0.10f, 0.09f, 0.09f);

        [Header("Props")]
        public Color WoodColor = new(0.62f, 0.42f, 0.24f);
        public Color SignColor = new(0.96f, 0.89f, 0.76f);
        public Color CardboardColor = new(0.74f, 0.56f, 0.36f);
        public Color SlotGhostColor = new(1f, 0.97f, 0.9f, 0.55f);

        [Header("Mood: dirty -> clean")]
        public Color LightColor = new(1f, 0.86f, 0.68f);
        public float LightIntensityDirty = 0.9f;
        public float LightIntensityClean = 1.5f;
        public Color AmbientDirty = new(0.30f, 0.27f, 0.25f);
        public Color AmbientClean = new(0.55f, 0.50f, 0.44f);
    }
}
