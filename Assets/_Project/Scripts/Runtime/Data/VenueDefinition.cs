using System.Collections.Generic;
using UnityEngine;

namespace SortingGame.Data
{
    /// <summary>GDD 5. A place made of one or more sections. Opens for free when the previous venue is complete.</summary>
    [CreateAssetMenu(menuName = "Sorting Game/Venue", fileName = "Venue_")]
    public class VenueDefinition : ScriptableObject
    {
        public string Id;
        public string DisplayNameKey;
        public string ThemeId;

        public List<SectionDefinition> Sections = new();

        [Tooltip("This venue's entries in the Collection Book album (GDD 9.2: one costumed Chubby).")]
        public List<CollectibleDefinition> CollectionPage = new();
    }
}
