using UnityEngine;

namespace SortingGame.Data
{
    /// <summary>A single sortable item type. Variants of the same category are separate definitions.</summary>
    [CreateAssetMenu(menuName = "Sorting Game/Item", fileName = "Item_")]
    public class ItemDefinition : ScriptableObject
    {
        public string Id;
        public string DisplayNameKey;
        public ItemRarity Rarity = ItemRarity.Common;

        [Tooltip("Required for common items. Collectibles have no category.")]
        public CategoryDefinition Category;

        [Tooltip("0 = use the category's base value.")]
        public int CoinValueOverride;

        [Tooltip("Optional real model. When empty the placeholder is used.")]
        public GameObject Prefab;
        public PlaceholderVisual Placeholder = new(PlaceholderShape.Cube, Color.gray, new Vector3(0.2f, 0.2f, 0.2f));

        public bool IsCollectible => Rarity != ItemRarity.Common;

        public int CoinValue => CoinValueOverride > 0 ? CoinValueOverride : (Category != null ? Category.BaseCoinValue : 0);
    }
}
