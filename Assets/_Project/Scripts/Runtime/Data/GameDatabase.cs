using System.Collections.Generic;
using UnityEngine;

namespace SortingGame.Data
{
    /// <summary>Single entry point to all content. Lookups by id are used by the save system.</summary>
    [CreateAssetMenu(menuName = "Sorting Game/Game Database", fileName = "GameDatabase")]
    public class GameDatabase : ScriptableObject
    {
        public BalanceConfig Balance;
        public FeelConfig Feel;
        public List<VenueDefinition> Venues = new();
        public List<CategoryDefinition> Categories = new();
        public List<ItemDefinition> Items = new();
        public List<ContainerDefinition> Containers = new();

        public IEnumerable<ItemDefinition> CommonItemsOf(CategoryDefinition category)
        {
            foreach (var item in Items)
                if (!item.IsCollectible && item.Category == category)
                    yield return item;
        }

        /// <summary>Returns a list of human-readable problems. Empty list = content is valid.</summary>
        public List<string> Validate()
        {
            var errors = new List<string>();
            if (Balance == null) errors.Add("No BalanceConfig assigned.");
            if (Feel == null) errors.Add("No FeelConfig assigned.");
            CheckUniqueIds(Categories, c => c.Id, "Category", errors);
            CheckUniqueIds(Items, i => i.Id, "Item", errors);
            CheckUniqueIds(Containers, c => c.Id, "Container", errors);
            CheckUniqueIds(Venues, v => v.Id, "Venue", errors);

            foreach (var item in Items)
                if (item != null && !item.IsCollectible && item.Category == null)
                    errors.Add($"Common item '{item.Id}' has no category.");

            foreach (var venue in Venues)
            {
                if (venue == null) continue;
                foreach (var section in venue.Sections)
                {
                    if (section == null) { errors.Add($"Venue '{venue.Id}' has an empty section slot."); continue; }
                    foreach (var shelf in section.Shelves)
                    {
                        if (shelf.Category == null) { errors.Add($"Section '{section.Id}' has a shelf without category."); continue; }
                        using var e = CommonItemsOf(shelf.Category).GetEnumerator();
                        if (!e.MoveNext()) errors.Add($"Section '{section.Id}': category '{shelf.Category.Id}' has no items.");
                    }
                }
            }
            return errors;
        }

        static void CheckUniqueIds<T>(List<T> list, System.Func<T, string> id, string label, List<string> errors) where T : Object
        {
            var seen = new HashSet<string>();
            foreach (var entry in list)
            {
                if (entry == null) { errors.Add($"{label} list has an empty entry."); continue; }
                var value = id(entry);
                if (string.IsNullOrEmpty(value)) errors.Add($"{label} '{entry.name}' has no id.");
                else if (!seen.Add(value)) errors.Add($"Duplicate {label} id '{value}'.");
            }
        }
    }
}
