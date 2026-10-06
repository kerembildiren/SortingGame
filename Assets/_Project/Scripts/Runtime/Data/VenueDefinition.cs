using System.Collections.Generic;
using UnityEngine;

namespace SortingGame.Data
{
    /// <summary>GDD 5. A purchasable place made of one or more sections.</summary>
    [CreateAssetMenu(menuName = "Sorting Game/Venue", fileName = "Venue_")]
    public class VenueDefinition : ScriptableObject
    {
        public string Id;
        public string DisplayNameKey;
        public string ThemeId;

        public int PurchasePrice;
        public int SellValue;

        public List<SectionDefinition> Sections = new();

        [Tooltip("This venue's page in the Collection Book.")]
        public List<CollectibleDefinition> CollectionPage = new();
    }
}
