using UnityEngine;

namespace SortingGame.Data
{
    /// <summary>
    /// GDD 9. Rare item or mascot. First copy goes to the Collection Book automatically,
    /// duplicates are sold automatically for <see cref="DuplicateSellValue"/>.
    /// </summary>
    [CreateAssetMenu(menuName = "Sorting Game/Collectible", fileName = "Collectible_")]
    public class CollectibleDefinition : ItemDefinition
    {
        public string DescriptionKey;
        public int DuplicateSellValue = 50;

        [Tooltip("Mascot only: costume accent colour of the placeholder figure (mask, cape).")]
        public Color CostumeColor = new(0.9f, 0.2f, 0.2f);

        public bool IsMascot => Rarity == ItemRarity.Mascot;

        void Reset()
        {
            Rarity = ItemRarity.Rare;
        }
    }
}
