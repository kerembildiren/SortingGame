using UnityEngine;

namespace SortingGame.Data
{
    /// <summary>GDD 14. Each category has its own placement sound family.</summary>
    public enum PlaceSoundType
    {
        Paper,
        Plastic,
        Metal
    }

    /// <summary>GDD 8.2. Categories are shared across venues, so what the player learned stays useful.</summary>
    [CreateAssetMenu(menuName = "Sorting Game/Category", fileName = "Category_")]
    public class CategoryDefinition : ScriptableObject
    {
        public string Id;
        public string DisplayNameKey;
        public Color LabelColor = Color.white;
        public Sprite Icon;

        [Tooltip("Coins for a common item of this category, unless the item overrides it.")]
        public int BaseCoinValue = 1;

        [Tooltip("Size of one shelf slot (x = width, y = height, z = depth). Must fit every item of this category upright.")]
        public Vector3 SlotSize = new(0.3f, 0.34f, 0.3f);

        public PlaceSoundType PlaceSound = PlaceSoundType.Plastic;
    }
}
