using UnityEngine;

namespace SortingGame.Data
{
    /// <summary>GDD 8.2. Categories are shared across venues so Category Mastery carries over.</summary>
    [CreateAssetMenu(menuName = "Sorting Game/Category", fileName = "Category_")]
    public class CategoryDefinition : ScriptableObject
    {
        public string Id;
        public string DisplayNameKey;
        public Color LabelColor = Color.white;
        public Sprite Icon;

        [Tooltip("Coins for a common item of this category, unless the item overrides it.")]
        public int BaseCoinValue = 1;

        [Tooltip("Correct placements needed to master this category (GDD 10.2).")]
        public int MasteryThreshold = 100;
    }
}
