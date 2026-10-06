using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SortingGame.Core;
using SortingGame.Data;
using UnityEngine;

namespace SortingGame.Tests
{
    public class ProgressionTests
    {
        readonly List<Object> _created = new();

        [TearDown]
        public void TearDown()
        {
            foreach (var o in _created) Object.DestroyImmediate(o);
            _created.Clear();
        }

        ToolDefinition MakeTool(string id, params int[] costs)
        {
            var tool = ScriptableObject.CreateInstance<ToolDefinition>();
            _created.Add(tool);
            tool.Id = id;
            foreach (var cost in costs) tool.Levels.Add(new ToolDefinition.Level { Cost = cost, Primary = cost / 10f });
            return tool;
        }

        CategoryDefinition MakeCategory(string id, int threshold)
        {
            var category = ScriptableObject.CreateInstance<CategoryDefinition>();
            _created.Add(category);
            category.Id = id;
            category.MasteryThreshold = threshold;
            return category;
        }

        // ---------- Tools ----------

        [Test]
        public void FreeFirstLevel_IsOwnedFromStart_PaidOneIsLocked()
        {
            var progress = new ToolProgress();
            Assert.AreEqual(1, progress.LevelOf(MakeTool("hand", 0, 50)));
            Assert.AreEqual(0, progress.LevelOf(MakeTool("magnet", 40, 120)));
        }

        [Test]
        public void Upgrade_SpendsCoins_AndRaisesLevel()
        {
            var magnet = MakeTool("magnet", 40, 120);
            var progress = new ToolProgress();
            var wallet = new Wallet(100);

            Assert.IsTrue(progress.TryUpgrade(magnet, wallet));
            Assert.AreEqual(1, progress.LevelOf(magnet));
            Assert.AreEqual(60, wallet.Coins);
            Assert.AreEqual(120, progress.NextCost(magnet));
        }

        [Test]
        public void Upgrade_FailsWithoutCoins_AndAtMax()
        {
            var broom = MakeTool("broom", 0, 50);
            var progress = new ToolProgress();

            Assert.IsFalse(progress.TryUpgrade(broom, new Wallet(10)));
            Assert.IsTrue(progress.TryUpgrade(broom, new Wallet(50)));
            Assert.IsTrue(progress.IsMaxed(broom));
            Assert.AreEqual(-1, progress.NextCost(broom));
            Assert.IsFalse(progress.TryUpgrade(broom, new Wallet(999)));
        }

        // ---------- Mastery ----------

        [Test]
        public void Mastery_TriggersExactlyOnce_AtThreshold()
        {
            var comics = MakeCategory("comics", 3);
            var mastery = new CategoryMastery();
            var events = 0;
            mastery.Mastered += _ => events++;

            Assert.IsFalse(mastery.RecordPlacement(comics));
            Assert.IsFalse(mastery.RecordPlacement(comics));
            Assert.IsTrue(mastery.RecordPlacement(comics));
            Assert.IsFalse(mastery.RecordPlacement(comics));

            Assert.IsTrue(mastery.IsMastered(comics));
            Assert.AreEqual(1, events);
            Assert.AreEqual(1f, mastery.ProgressOf(comics));
        }

        [Test]
        public void Mastery_IsPerCategory_AndRestorable()
        {
            var comics = MakeCategory("comics", 2);
            var toys = MakeCategory("toys", 2);
            var mastery = new CategoryMastery();
            mastery.RecordPlacement(comics);
            mastery.RecordPlacement(comics);

            var restored = new CategoryMastery();
            restored.Restore(mastery.Export().ToList());

            Assert.IsTrue(restored.IsMastered(comics));
            Assert.IsFalse(restored.IsMastered(toys));
        }

        // ---------- Save ----------

        class MemoryStorage : ISaveStorage
        {
            public string Content;
            public string Read() => Content;
            public void Write(string content) => Content = content;
            public void Delete() => Content = null;
        }

        [Test]
        public void SaveData_RoundTrips()
        {
            var storage = new MemoryStorage();
            var system = new SaveSystem(storage);
            var data = new SaveData { Coins = 123 };
            data.Collection.Add("captain_chubby");
            data.Mastery.Add(new IdCount("comics", 7));
            var section = new SectionSave { SectionId = "garage", Seed = 42 };
            section.Items.Add(new ItemSave { Id = "comic_red", State = ItemSaveState.Placed, Shelf = 0, Slot = 3 });
            section.Containers.Add(new ContainerSave { Id = "box", Contents = { "toy_ball", "first_issue" } });
            data.SetSection(section);

            system.Save(data);
            var loaded = system.Load();

            Assert.AreEqual(123, loaded.Coins);
            CollectionAssert.AreEqual(new[] { "captain_chubby" }, loaded.Collection);
            Assert.AreEqual(7, loaded.Mastery[0].Count);
            var garage = loaded.SectionById("garage");
            Assert.AreEqual(42, garage.Seed);
            Assert.AreEqual(ItemSaveState.Placed, garage.Items[0].State);
            Assert.AreEqual(3, garage.Items[0].Slot);
            CollectionAssert.AreEqual(new[] { "toy_ball", "first_issue" }, garage.Containers[0].Contents);
        }

        [Test]
        public void CorruptSave_StartsFresh_InsteadOfCrashing()
        {
            var system = new SaveSystem(new MemoryStorage { Content = "{ this is not json" });
            Assert.IsNull(system.Load());
        }

        [Test]
        public void DirtMask_SurvivesPackAndImport()
        {
            var mask = new DirtMask(40, 30);
            mask.Generate(0.6f, 3);
            mask.Brush(0.5f, 0.5f, 0.2f, 1f);
            var cleaned = mask.CleanedFraction;

            var packed = SaveSystem.Pack(mask.Export());
            var restored = new DirtMask(40, 30);
            restored.Import(SaveSystem.Unpack(packed), mask.InitialTotal);

            Assert.AreEqual(cleaned, restored.CleanedFraction, 0.0001f);
            Assert.AreEqual(mask[20, 15], restored[20, 15]);
        }
    }
}
