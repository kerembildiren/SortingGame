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
        public List<ToolDefinition> Tools = new();

        public ToolDefinition ToolFor(ToolType type) => Tools.Find(t => t != null && t.Type == type);

        public ItemDefinition ItemById(string id) => Items.Find(i => i != null && i.Id == id);
        public CategoryDefinition CategoryById(string id) => Categories.Find(c => c != null && c.Id == id);
        public ContainerDefinition ContainerById(string id) => Containers.Find(c => c != null && c.Id == id);

        public IEnumerable<ItemDefinition> CommonItemsOf(CategoryDefinition category)
        {
            foreach (var item in Items)
                if (item.Rarity == ItemRarity.Common && item.Category == category)
                    yield return item;
        }

        /// <summary>Every collectible in album order: venue by venue along the ladder (GDD 9.1).</summary>
        public IEnumerable<CollectibleDefinition> Album
        {
            get
            {
                foreach (var venue in Venues)
                {
                    if (venue == null) continue;
                    foreach (var collectible in venue.CollectionPage)
                        if (collectible != null)
                            yield return collectible;
                }
            }
        }

        /// <summary>The venue whose album entry this collectible is.</summary>
        public VenueDefinition VenueOf(CollectibleDefinition collectible) =>
            Venues.Find(v => v != null && v.CollectionPage.Contains(collectible));

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
            CheckUniqueIds(Tools, t => t.Id, "Tool", errors);
            foreach (var tool in Tools)
                if (tool != null && tool.Levels.Count == 0) errors.Add($"Tool '{tool.Id}' has no levels.");

            foreach (var item in Items)
                if (item != null && !item.IsCollectible && item.Category == null)
                    errors.Add($"Item '{item.Id}' has no category.");

            foreach (var venue in Venues)
            {
                if (venue == null) continue;
                foreach (var section in venue.Sections)
                {
                    if (section == null) { errors.Add($"Venue '{venue.Id}' has an empty section slot."); continue; }
                    foreach (var rare in section.RareItems)
                    {
                        if (rare == null || !rare.IsRare)
                            errors.Add($"Section '{section.Id}' lists a rare item that is not marked Rare.");
                        else if (!section.Shelves.Exists(s => s.Category == rare.Category))
                            errors.Add($"Section '{section.Id}': rare item '{rare.Id}' has no shelf for its category.");
                    }
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
