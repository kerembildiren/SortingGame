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
            public readonly List<CollectibleDefinition> Collectibles = new();
        }

        public readonly List<ContainerContent> Containers = new();
        public readonly List<ItemDefinition> LooseItems = new();
        public readonly List<ItemDefinition> BuriedItems = new();
        public readonly List<CollectibleDefinition> BuriedCollectibles = new();
        public readonly List<CollectibleDefinition> LooseCollectibles = new();

        /// <summary>Common items only: these are what the section % counts (collectibles are a bonus, GDD 5.6).</summary>
        public int TotalItems
        {
            get
            {
                var total = LooseItems.Count + BuriedItems.Count;
                foreach (var c in Containers) total += c.Items.Count;
                return total;
            }
        }
    }

    /// <summary>
    /// Creates exactly one item per shelf slot (so a section can always reach 100%),
    /// then spreads them over containers, the floor and under the dirt. Same seed = same layout.
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

            var hasDirt = section.DirtCoverage > 0f;
            var buriedCount = hasDirt ? (int)Math.Round(all.Count * section.BuriedItemRatio) : 0;
            var looseCount = (int)Math.Round(all.Count * section.LooseItemRatio);
            if (layout.Containers.Count == 0) looseCount = all.Count - buriedCount;

            var index = 0;
            for (var n = 0; n < buriedCount && index < all.Count; n++, index++)
                layout.BuriedItems.Add(all[index]);
            for (var n = 0; n < looseCount && index < all.Count; n++, index++)
                layout.LooseItems.Add(all[index]);

            // Round-robin into containers, respecting capacity. Overflow lands on the floor.
            var containerIndex = 0;
            for (; index < all.Count; index++)
            {
                var target = NextWithSpace(layout.Containers, ref containerIndex);
                if (target != null) target.Items.Add(all[index]);
                else layout.LooseItems.Add(all[index]);
            }

            // Collectibles: hidden in a box or under the dirt, never just lying in plain sight.
            foreach (var collectible in section.Collectibles)
            {
                if (collectible == null) continue;
                var canBury = hasDirt;
                var canBox = layout.Containers.Count > 0;
                if (canBury && (!canBox || random.NextDouble() < 0.5))
                    layout.BuriedCollectibles.Add(collectible);
                else if (canBox)
                    layout.Containers[random.Next(layout.Containers.Count)].Collectibles.Add(collectible);
                else
                    layout.LooseCollectibles.Add(collectible);
            }

            return layout;
        }

        static SectionLayout.ContainerContent NextWithSpace(List<SectionLayout.ContainerContent> containers, ref int start)
        {
            for (var attempt = 0; attempt < containers.Count; attempt++)
            {
                var candidate = containers[(start + attempt) % containers.Count];
                if (candidate.Items.Count >= candidate.Container.Capacity) continue;
                start = (start + attempt + 1) % containers.Count;
                return candidate;
            }
            return null;
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
