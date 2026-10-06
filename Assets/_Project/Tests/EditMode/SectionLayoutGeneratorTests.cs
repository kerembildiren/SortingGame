using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SortingGame.Core;
using SortingGame.Data;
using UnityEngine;

namespace SortingGame.Tests
{
    public class SectionLayoutGeneratorTests
    {
        readonly List<Object> _created = new();

        [TearDown]
        public void TearDown()
        {
            foreach (var o in _created) Object.DestroyImmediate(o);
            _created.Clear();
        }

        T Make<T>() where T : ScriptableObject
        {
            var instance = ScriptableObject.CreateInstance<T>();
            _created.Add(instance);
            return instance;
        }

        (SectionDefinition section, Dictionary<CategoryDefinition, List<ItemDefinition>> items) MakeSection(int slotsA, int slotsB, int boxes, int capacity, float looseRatio)
        {
            var a = Make<CategoryDefinition>(); a.Id = "a";
            var b = Make<CategoryDefinition>(); b.Id = "b";
            var items = new Dictionary<CategoryDefinition, List<ItemDefinition>>
            {
                [a] = Enumerable.Range(0, 3).Select(i => { var it = Make<ItemDefinition>(); it.Id = $"a{i}"; it.Category = a; return it; }).ToList(),
                [b] = Enumerable.Range(0, 2).Select(i => { var it = Make<ItemDefinition>(); it.Id = $"b{i}"; it.Category = b; return it; }).ToList(),
            };
            var box = Make<ContainerDefinition>(); box.Capacity = capacity;
            var section = Make<SectionDefinition>();
            section.Shelves.Add(new SectionDefinition.ShelfEntry { Category = a, SlotCount = slotsA });
            section.Shelves.Add(new SectionDefinition.ShelfEntry { Category = b, SlotCount = slotsB });
            if (boxes > 0) section.Containers.Add(new SectionDefinition.ContainerEntry { Container = box, Count = boxes });
            section.LooseItemRatio = looseRatio;
            section.DirtCoverage = 0f; // individual tests opt in to dirt
            return (section, items);
        }

        static IEnumerable<ItemDefinition> AllItems(SectionLayout layout) =>
            layout.LooseItems.Concat(layout.BuriedItems).Concat(layout.Containers.SelectMany(c => c.Items));

        CollectibleDefinition MakeCollectible(string id)
        {
            var c = Make<CollectibleDefinition>();
            c.Id = id;
            c.Rarity = ItemRarity.Rare;
            return c;
        }

        [Test]
        public void ItemCountPerCategory_EqualsShelfSlots()
        {
            var (section, items) = MakeSection(10, 7, 2, 20, 0.2f);

            var layout = SectionLayoutGenerator.Generate(section, c => items[c], 42);

            var byCategory = AllItems(layout).GroupBy(i => i.Category.Id).ToDictionary(g => g.Key, g => g.Count());
            Assert.AreEqual(10, byCategory["a"]);
            Assert.AreEqual(7, byCategory["b"]);
            Assert.AreEqual(17, layout.TotalItems);
        }

        [Test]
        public void EveryVariantAppears_BeforeAnyRepeats()
        {
            var (section, items) = MakeSection(3, 2, 0, 0, 0f);

            var layout = SectionLayoutGenerator.Generate(section, c => items[c], 1);

            Assert.AreEqual(5, AllItems(layout).Distinct().Count());
        }

        [Test]
        public void ContainerCapacity_IsRespected_OverflowGoesToFloor()
        {
            var (section, items) = MakeSection(10, 10, 2, 4, 0f);

            var layout = SectionLayoutGenerator.Generate(section, c => items[c], 7);

            Assert.That(layout.Containers.All(c => c.Items.Count <= 4));
            Assert.AreEqual(12, layout.LooseItems.Count);
            Assert.AreEqual(20, layout.TotalItems);
        }

        [Test]
        public void SameSeed_SameLayout()
        {
            var (section, items) = MakeSection(8, 8, 2, 10, 0.25f);

            var first = SectionLayoutGenerator.Generate(section, c => items[c], 99);
            var second = SectionLayoutGenerator.Generate(section, c => items[c], 99);

            CollectionAssert.AreEqual(AllItems(first).Select(i => i.Id).ToList(), AllItems(second).Select(i => i.Id).ToList());
        }
    

        [Test]
        public void WithDirt_SomeItemsAreBuried_AndStillCounted()
        {
            var (section, items) = MakeSection(10, 10, 2, 20, 0.2f);
            section.DirtCoverage = 0.5f;
            section.BuriedItemRatio = 0.25f;

            var layout = SectionLayoutGenerator.Generate(section, c => items[c], 3);

            Assert.AreEqual(5, layout.BuriedItems.Count);
            Assert.AreEqual(20, layout.TotalItems);
        }

        [Test]
        public void WithoutDirt_NothingIsBuried()
        {
            var (section, items) = MakeSection(10, 10, 2, 20, 0.2f);
            section.BuriedItemRatio = 0.5f;

            var layout = SectionLayoutGenerator.Generate(section, c => items[c], 3);

            Assert.IsEmpty(layout.BuriedItems);
            Assert.IsEmpty(layout.BuriedCollectibles);
        }

        [Test]
        public void FoundCollectibles_AreLeftOut()
        {
            var (section, items) = MakeSection(6, 6, 2, 20, 0.2f);
            var found = MakeCollectible("found");
            var fresh = MakeCollectible("fresh");
            section.Collectibles.AddRange(new[] { found, fresh });

            var layout = SectionLayoutGenerator.Generate(section, c => items[c], 5, c => c != found);

            var all = layout.BuriedCollectibles.Concat(layout.LooseCollectibles).Concat(layout.Containers.SelectMany(c => c.Collectibles)).ToList();
            CollectionAssert.AreEqual(new[] { fresh }, all);
        }

        [Test]
        public void Collectibles_AreHidden_AndNotCountedInTotal()
        {
            var (section, items) = MakeSection(6, 6, 2, 20, 0.2f);
            section.DirtCoverage = 0.5f;
            section.Collectibles.AddRange(new[] { MakeCollectible("r1"), MakeCollectible("r2"), MakeCollectible("r3"), MakeCollectible("r4") });

            var layout = SectionLayoutGenerator.Generate(section, c => items[c], 11);

            var hidden = layout.BuriedCollectibles.Count + layout.Containers.Sum(c => c.Collectibles.Count);
            Assert.AreEqual(4, hidden, "Collectibles go into boxes or under the dirt, never in plain sight.");
            Assert.IsEmpty(layout.LooseCollectibles);
            Assert.AreEqual(12, layout.TotalItems);
        }
    }
}
