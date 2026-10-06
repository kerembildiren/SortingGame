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
    /// the Garage opens for free; a room needs its collectibles to finish; Warehouse locked room opens by coins and by progress.
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
        public IEnumerator FinishVenue_OpensTheNextPlace()
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

            // Back out: the room is at 100%, which completes the venue and opens the next place for free.
            Flow.BackToOverview();
            yield return new WaitForSeconds(1.5f);
            Assert.AreEqual(GameFlow.Screen.Overview, Flow.Current);
            Assert.IsFalse(Section.IsLoaded, "Only the active section is loaded.");
            Assert.IsTrue(Boot.Overview.Rooms[0].Status.Completed);
            var ladder = Boot.Context.Database.Venues;
            Assert.AreEqual(VenueProgress.VenueState.Completed, VenueProgress.State(ladder, 0, Boot.Data));
            Assert.AreEqual(VenueProgress.VenueState.Open, VenueProgress.State(ladder, 1, Boot.Data));
            Assert.AreEqual(VenueProgress.VenueState.Locked, VenueProgress.State(ladder, 2, Boot.Data));
            yield return TestSnapshots.Capture("m4_02_venue_complete");

            var coins = Boot.Wallet.Coins;
            Flow.GoToNextVenue();
            yield return new WaitForSeconds(1f);
            Assert.AreEqual("garage", Flow.Venue.Id);
            Assert.AreEqual(GameFlow.Screen.Overview, Flow.Current);
            Assert.AreEqual(coins, Boot.Wallet.Coins, "No purchase: moving on is free.");

            Flow.ShowMap();
            yield return TestSnapshots.Capture("m4_03_map");
        }

        [UnityTest, Timeout(120000)]
        public IEnumerator Section_DoesNotFinish_WhileACollectibleIsLeft()
        {
            yield return TestGame.LoadMain();
            Flow.OpenSectionImmediately(Flow.Venue, Flow.Venue.Sections[0]);
            yield return null;
            var shiny = false;
            Section.OnlyCollectiblesLeft += () => shiny = true;

            yield return FinishSection(pickUpCollectibles: false, showcaseShot: "m4_05_shelf_showcase");
            Assert.IsFalse(Section.IsComplete, "A collectible is still on the floor.");
            Assert.IsTrue(shiny, "Player is told something shiny is left.");
            Assert.AreEqual(99, Section.Progress.Percent);
            yield return TestSnapshots.Capture("m4_06_shiny_left");

            foreach (var collectible in Section.Collectibles.ToList())
            {
                Section.FindCollectible(collectible);
                yield return new WaitForSeconds(1f);
                Boot.RareFind.Dismiss();
                yield return new WaitForSeconds(0.8f);
            }
            Assert.IsTrue(Section.IsComplete);
            yield return new WaitForSeconds(1.2f);
            yield return TestSnapshots.Capture("m4_07_collection_complete");
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
                Boot.Data.SetSection(new SectionSave { SectionId = section.Id, PlacedItems = (int)(section.TotalSlotCount * 0.62f), TotalItems = section.TotalSlotCount, Fraction = 0.62f });
            Flow.ShowOverviewImmediately(warehouse);
            yield return new WaitForSeconds(0.3f);
            Assert.IsTrue(Boot.Overview.RoomOf(basement).Status.Unlocked);
            yield return TestSnapshots.Capture("m4_04_warehouse_overview");

            // Coins: a fresh lock opened by paying.
            Boot.Data.UnlockedSections.Clear();
            foreach (var section in warehouse.Sections.Where(s => s != basement))
                Boot.Data.SetSection(new SectionSave { SectionId = section.Id, PlacedItems = 0, TotalItems = section.TotalSlotCount, Fraction = 0f });
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
        IEnumerator FinishSection(bool pickUpCollectibles = true, string showcaseShot = null)
        {
            foreach (var container in Section.Containers.ToList()) Section.OpenContainer(container);
            yield return new WaitForSeconds(2.5f);
            var bounds = Section.ViewBounds;
            for (var pass = 0; pass < 2; pass++)
            for (var z = bounds.min.z; z <= bounds.max.z; z += 0.25f)
            for (var x = bounds.min.x; x <= bounds.max.x; x += 0.2f)
                Section.Sweep(new Vector3(x, 0f, z), 0.2f);
            yield return new WaitForSeconds(1f);

            foreach (var collectible in pickUpCollectibles ? Section.Collectibles.ToList() : new System.Collections.Generic.List<ItemView>())
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
            if (showcaseShot != null)
            {
                yield return new WaitForSeconds(1.3f); // mid-glide
                Assert.IsTrue(Boot.Showcase.IsPlaying, "A full shelf gets its camera showcase.");
                yield return TestSnapshots.Capture(showcaseShot);
            }
            // Every finished shelf gets its showcase; wait for the queue to drain.
            var waited = 0f;
            while ((Boot.Showcase.IsPlaying || waited < 1f) && waited < 15f)
            {
                waited += Time.deltaTime;
                yield return null;
            }
            Assert.IsFalse(Boot.Showcase.IsPlaying);
        }
    }
}
