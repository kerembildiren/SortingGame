using System.Collections;
using System.Linq;
using NUnit.Framework;
using SortingGame.Core;
using SortingGame.Section;
using UnityEngine;
using UnityEngine.TestTools;

namespace SortingGame.Tests
{
    /// <summary>
    /// M4 end to end: a fresh game starts on the Comic Box overview; zoom into the room, finish it, come back,
    /// sell the venue, buy the Garage; Warehouse locked room opens by coins and by progress.
    /// </summary>
    public class VenueFlowTests
    {
        GameBootstrap Boot => TestGame.Boot;
        GameFlow Flow => Boot.Flow;
        SectionController Section => Boot.Section;

        [SetUp]
        public void SetUp() => TestGame.UseTestSave();

        [TearDown]
        public void TearDown() => TestGame.RestoreSave();

        [UnityTest, Timeout(180000)]
        public IEnumerator FinishSellAndBuy_WalksUpTheVenueLadder()
        {
            yield return TestGame.LoadMain();
            Assert.AreEqual(GameFlow.Screen.Overview, Flow.Current, "A fresh game starts on the overview.");
            Assert.AreEqual("comic_box", Flow.Venue.Id);
            Assert.AreEqual(1, Boot.Overview.Rooms.Count);
            yield return TestSnapshots.Capture("m4_01_comic_box_overview");

            // Zoom into the room.
            Flow.EnterSection(Flow.Venue.Sections[0]);
            yield return new WaitForSeconds(1.2f);
            Assert.AreEqual(GameFlow.Screen.Section, Flow.Current);
            Assert.IsTrue(Section.IsLoaded);

            yield return FinishSection();
            Assert.IsTrue(Section.IsComplete);

            // Back out: the overview now shows the room at 100% and offers the sale.
            Flow.BackToOverview();
            yield return new WaitForSeconds(1.5f);
            Assert.AreEqual(GameFlow.Screen.Overview, Flow.Current);
            Assert.IsFalse(Section.IsLoaded, "Only the active section is loaded.");
            Assert.IsTrue(Boot.Overview.Rooms[0].Status.Completed);
            Assert.IsTrue(VenueProgress.CanSell(Flow.Venue, Boot.Data));
            yield return TestSnapshots.Capture("m4_02_ready_to_sell");

            var coins = Boot.Wallet.Coins;
            var comicBox = Flow.Venue;
            Flow.SellVenue();
            yield return new WaitForSeconds(0.5f);
            Assert.AreEqual(coins + comicBox.SellValue, Boot.Wallet.Coins);
            Assert.IsTrue(Boot.Data.Venue("comic_box").Sold);
            yield return TestSnapshots.Capture("m4_03_map_after_sale");

            var garage = TestGame.Venue("garage");
            var ladder = Boot.Context.Database.Venues;
            Assert.AreEqual(VenueProgress.VenueState.ForSale, VenueProgress.State(ladder, 1, Boot.Data));
            Flow.BuyVenue(garage);
            yield return new WaitForSeconds(1f);
            Assert.AreEqual("garage", Flow.Venue.Id);
            Assert.AreEqual(GameFlow.Screen.Overview, Flow.Current);
            Assert.AreEqual(coins + comicBox.SellValue - garage.PurchasePrice, Boot.Wallet.Coins);
        }

        [UnityTest, Timeout(120000)]
        public IEnumerator Warehouse_LockedRoom_OpensByCoinsOrProgress()
        {
            yield return TestGame.LoadMain();
            var warehouse = TestGame.Venue("warehouse");
            var basement = warehouse.Sections.First(s => s.Id == "wh_basement");
            Flow.ShowOverviewImmediately(warehouse);
            yield return null;

            Assert.AreEqual(4, Boot.Overview.Rooms.Count);
            Assert.IsFalse(Boot.Overview.RoomOf(basement).Status.Unlocked);

            // Progress: other rooms at 60% on average -> opens by itself next time the overview shows.
            foreach (var section in warehouse.Sections.Where(s => s != basement))
                Boot.Data.SetSection(new SectionSave { SectionId = section.Id, PlacedItems = 13, TotalItems = 21, Fraction = 0.62f });
            Flow.ShowOverviewImmediately(warehouse);
            yield return new WaitForSeconds(0.3f);
            Assert.IsTrue(Boot.Overview.RoomOf(basement).Status.Unlocked);
            yield return TestSnapshots.Capture("m4_04_warehouse_overview");

            // Coins: a fresh lock opened by paying.
            Boot.Data.UnlockedSections.Clear();
            foreach (var section in warehouse.Sections.Where(s => s != basement))
                Boot.Data.SetSection(new SectionSave { SectionId = section.Id, PlacedItems = 0, TotalItems = 21, Fraction = 0f });
            Flow.ShowOverviewImmediately(warehouse);
            Assert.IsFalse(Boot.Overview.RoomOf(basement).Status.Unlocked);
            Boot.Hud.ShowUnlock(basement);
            yield return TestSnapshots.Capture("m4_05_unlock_card");
            Boot.Wallet.Add(basement.UnlockCoinCost);
            Flow.UnlockWithCoins(basement);
            Assert.IsTrue(Boot.Overview.RoomOf(basement).Status.Unlocked);
            Assert.AreEqual(0, Boot.Wallet.Coins);
        }

        /// <summary>Open boxes, sweep everything, find collectibles, shelve everything.</summary>
        IEnumerator FinishSection()
        {
            foreach (var container in Section.Containers.ToList()) Section.OpenContainer(container);
            yield return new WaitForSeconds(2.5f);
            var bounds = Section.ViewBounds;
            for (var pass = 0; pass < 2; pass++)
            for (var z = bounds.min.z; z <= bounds.max.z; z += 0.25f)
            for (var x = bounds.min.x; x <= bounds.max.x; x += 0.2f)
                Section.Sweep(new Vector3(x, 0f, z), 0.2f);
            yield return new WaitForSeconds(1f);

            foreach (var collectible in Section.Collectibles.ToList())
            {
                Section.FindCollectible(collectible);
                yield return new WaitForSeconds(1f);
                Boot.RareFind.Dismiss();
                yield return new WaitForSeconds(0.6f);
            }

            foreach (var item in Section.CommonItems.Where(i => i.State is ItemState.Resting or ItemState.Physics).ToList())
            {
                var shelf = Section.ShelfFor(item.Definition.Category);
                Section.TryPlace(item, shelf, shelf.transform.position);
                yield return null;
            }
            yield return new WaitForSeconds(3f);
        }
    }
}
