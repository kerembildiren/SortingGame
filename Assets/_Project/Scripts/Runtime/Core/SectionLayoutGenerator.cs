using System;
using System.Collections.Generic;
using SortingGame.Data;

namespace SortingGame.Core
{
    /// <summary>What goes where in a freshly generated section. Positions are decided by the view.</summary>
    public class SectionLayout
    {
        public class ContainerContent
        {
            public ContainerDefinition Container;
            public readonly List<ItemDefinition> Items = new();
        }

        public readonly List<ContainerContent> Containers = new();
        public readonly List<ItemDefinition> LooseItems = new();

        public int TotalItems
        {
            get
            {
                var total = LooseItems.Count;
                foreach (var c in Containers) total += c.Items.Count;
                return total;
            }
        }
    }

    /// <summary>
    /// Creates exactly one item per shelf slot (so a section can always reach 100%),
    /// then spreads them over containers and the floor. Same seed = same layout.
    /// </summary>
    public static class SectionLayoutGenerator
    {
        public static SectionLayout Generate(SectionDefinition section, Func<CategoryDefinition, IReadOnlyList<ItemDefinition>> itemsOf, int seed)
        {
            var random = new Random(seed);
            var all = new List<ItemDefinition>();

            foreach (var shelf in section.Shelves)
            {
                var variants = itemsOf(shelf.Category);
                if (variants == null || variants.Count == 0)
                    throw new InvalidOperationException($"Category '{shelf.Category?.Id}' has no items.");

                // Walk through shuffled variants so every variant appears before any repeats.
                var bag = new List<ItemDefinition>();
                for (var i = 0; i < shelf.SlotCount; i++)
                {
                    if (bag.Count == 0)
                    {
                        bag.AddRange(variants);
                        Shuffle(bag, random);
                    }
                    all.Add(bag[^1]);
                    bag.RemoveAt(bag.Count - 1);
                }
            }

            Shuffle(all, random);

            var layout = new SectionLayout();
            foreach (var entry in section.Containers)
                for (var i = 0; i < entry.Count; i++)
                    layout.Containers.Add(new SectionLayout.ContainerContent { Container = entry.Container });

            var looseCount = (int)Math.Round(all.Count * section.LooseItemRatio);
            if (layout.Containers.Count == 0) looseCount = all.Count;

            var index = 0;
            for (; index < looseCount && index < all.Count; index++)
                layout.LooseItems.Add(all[index]);

            // Round-robin into containers, respecting capacity. Overflow lands on the floor.
            var containerIndex = 0;
            for (; index < all.Count; index++)
            {
                var placed = false;
                for (var attempt = 0; attempt < layout.Containers.Count; attempt++)
                {
                    var target = layout.Containers[(containerIndex + attempt) % layout.Containers.Count];
                    if (target.Items.Count >= target.Container.Capacity) continue;
                    target.Items.Add(all[index]);
                    containerIndex = (containerIndex + attempt + 1) % layout.Containers.Count;
                    placed = true;
                    break;
                }
                if (!placed) layout.LooseItems.Add(all[index]);
            }

            return layout;
        }

        static void Shuffle<T>(List<T> list, Random random)
        {
            for (var i = list.Count - 1; i > 0; i--)
            {
                var j = random.Next(i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}
