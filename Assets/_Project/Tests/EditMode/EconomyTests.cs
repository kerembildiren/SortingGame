using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SortingGame.Core;
using SortingGame.Data;
using UnityEditor;
using UnityEngine;

namespace SortingGame.Tests
{
    /// <summary>
    /// GDD 11.5 balance guard rails, checked against the real content. They do not fix numbers; they fail when a
    /// price or content change breaks the pacing the design asks for (see Docs/ECONOMY.md for the full picture).
    /// </summary>
    public class EconomyTests
    {
        const string DatabasePath = "Assets/_Project/Data/GameDatabase.asset";
        readonly List<Object> _created = new();

        static GameDatabase Database => AssetDatabase.LoadAssetAtPath<GameDatabase>(DatabasePath);

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

        [Test]
        public void RoomIncome_CountsCommonItemsAtCategoryValue_AndRareItemsAtTheirOwn()
        {
            var comics = Make<CategoryDefinition>();
            comics.BaseCoinValue = 1;
            var toys = Make<CategoryDefinition>();
            toys.BaseCoinValue = 2;
            var rare = Make<ItemDefinition>();
            rare.Rarity = ItemRarity.Rare;
            rare.Category = comics;
            rare.CoinValueOverride = 5;
            var section = Make<SectionDefinition>();
            section.Shelves.Add(new SectionDefinition.ShelfEntry { Category = comics, SlotCount = 10 });
            section.Shelves.Add(new SectionDefinition.ShelfEntry { Category = toys, SlotCount = 4 });
            section.RareItems.Add(rare);

            Assert.AreEqual(9 * 1 + 5 + 4 * 2, EconomyModel.CoinsIn(section));
        }

        [Test]
        public void Content_MeetsEveryToolAndHelper_ButDoesNotPayForEverything()
        {
            var database = Database;
            var timeline = EconomyModel.ReferenceTimeline(database);

            foreach (var tool in database.Tools.Where(t => !t.StartsOwned))
                Assert.IsTrue(timeline.Any(p => p.Id == $"tool.{tool.Id}" && p.Level == 1 && p.Reached), $"{tool.Id} is never unlocked.");
            foreach (var helper in database.Helpers)
                Assert.IsTrue(timeline.Any(p => p.Id == $"helper.{helper.Id}" && p.Level == 1 && p.Reached), $"{helper.Id} is never hired.");

            // User: helpers and upgrades must not be cheap, the game should stay playable for a long time.
            Assert.GreaterOrEqual(EconomyModel.TotalSinks(database), 2 * EconomyModel.TotalIncome(database));
            Assert.IsTrue(timeline.Any(p => !p.Reached), "Something should be left to save for after the last room.");
        }

        [Test]
        public void FirstUpgrade_IsWithinReach_InTheTutorialVenue()
        {
            var database = Database;
            var first = EconomyModel.ReferenceTimeline(database).First();

            Assert.IsTrue(first.Reached);
            CollectionAssert.Contains(database.Venues[0].Sections, first.Room, "The first venue should already pay for something.");
        }

        [Test]
        public void Magnet_ComesLate_AfterHandIsMaxed_AndCostsMoreThanAllOfHand()
        {
            var database = Database;
            var hand = database.ToolFor(ToolType.Hand);
            var magnet = database.ToolFor(ToolType.Magnet);
            var timeline = EconomyModel.ReferenceTimeline(database);
            var handMaxed = timeline.FindIndex(p => p.Id == $"tool.{hand.Id}" && p.Level == hand.MaxLevel);
            var magnetBought = timeline.FindIndex(p => p.Id == $"tool.{magnet.Id}" && p.Level == 1);

            Assert.AreSame(hand, magnet.RequiresMaxed);
            Assert.Greater(magnetBought, handMaxed);
            Assert.Greater(magnet.Levels[0].Cost, EconomyModel.TotalCost(hand));
            CollectionAssert.Contains(database.Venues.Last().Sections, timeline[magnetBought].Room, "Magnet belongs to the last venue of the MVP, not earlier.");
        }

        [Test]
        public void Helpers_ComeOneAtATime_TheSecondBeforeTheFirstIsUpgraded()
        {
            var database = Database;
            Assert.GreaterOrEqual(database.Helpers.Count, 2);
            var timeline = EconomyModel.ReferenceTimeline(database);
            var hires = timeline.Where(p => p.Id.StartsWith("helper.") && p.Level == 1).ToList();
            var firstUpgrade = timeline.FindIndex(p => p.Id.StartsWith("helper.") && p.Level == 2);

            Assert.AreNotSame(hires[0].Room, hires[1].Room, "Not every step brings a new helper.");
            Assert.Greater(firstUpgrade, timeline.IndexOf(hires[1]), "Meeting a new helper comes before upgrading the old one.");
            Assert.That(database.Helpers.All(h => h.Levels[0].Cost > EconomyModel.TotalCost(database.ToolFor(ToolType.Hand))), "Helpers are not cheap.");
        }

        [Test]
        public void RareItems_PayABitMore_NotAFortune()
        {
            var rares = Database.Items.Where(i => i.IsRare).ToList();
            Assert.IsNotEmpty(rares);
            foreach (var rare in rares)
            {
                Assert.Greater(rare.CoinValue, rare.Category.BaseCoinValue, rare.Id);
                Assert.LessOrEqual(rare.CoinValue, rare.Category.BaseCoinValue * 6, $"{rare.Id}: 'a bit more', not a jackpot.");
            }
        }
    }
}
