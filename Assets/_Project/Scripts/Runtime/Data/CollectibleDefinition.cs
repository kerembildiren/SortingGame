using UnityEngine;

namespace SortingGame.Data
{
    /// <summary>
    /// GDD 9.2. A costumed Chubby, the only kind of collectible. Goes to the Collection Book when found
    /// and never appears again.
    /// </summary>
    [CreateAssetMenu(menuName = "Sorting Game/Collectible", fileName = "Collectible_")]
    public class CollectibleDefinition : ItemDefinition
    {
        public string DescriptionKey;
        [Tooltip("Costume accent colour of the placeholder figure (mask, cape).")]
        public Color CostumeColor = new(0.9f, 0.2f, 0.2f);

        public bool IsMascot => Rarity == ItemRarity.Mascot;

        void Reset()
        {
            Rarity = ItemRarity.Mascot;
        }
    }
}
