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
    /// the Garage opens for free; a room needs its collectibles to finish; the Warehouse's locked room opens by progress only.
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

            yield return TestGame.FinishSection();
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

            yield return TestGame.FinishSection(pickUpCollectibles: false, showcaseShot: "m4_05_shelf_showcase");
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
        public IEnumerator Warehouse_LockedRoom_OpensByProgressOnly()
        {
            yield return TestGame.LoadMain();
            var warehouse = TestGame.Venue("warehouse");
            var basement = warehouse.Sections.First(s => s.Id == "wh_basement");
            Flow.ShowOverviewImmediately(warehouse);
            yield return null;

            Assert.AreEqual(4, Boot.Overview.Rooms.Count);
            Assert.IsFalse(Boot.Overview.RoomOf(basement).Status.Unlocked);

            // Tapping it explains what opens it; there is nothing to buy, however many coins the player has.
            Boot.Wallet.Add(5000);
            Boot.Hud.ShowUnlock(basement);
            Assert.IsTrue(Boot.Hud.IsUnlockCardOpen);
            yield return TestSnapshots.Capture("m4_05_unlock_card");
            Flow.ShowOverviewImmediately(warehouse);
            Assert.IsFalse(Boot.Overview.RoomOf(basement).Status.Unlocked, "Coins do not open rooms.");
            Assert.AreEqual(5000, Boot.Wallet.Coins);

            // Progress: other rooms at 60% on average -> opens by itself next time the overview shows.
            foreach (var section in warehouse.Sections.Where(s => s != basement))
                Boot.Data.SetSection(new SectionSave { SectionId = section.Id, PlacedItems = (int)(section.TotalSlotCount * 0.62f), TotalItems = section.TotalSlotCount, Fraction = 0.62f });
            Flow.ShowOverviewImmediately(warehouse);
            yield return new WaitForSeconds(0.3f);
            Assert.IsTrue(Boot.Overview.RoomOf(basement).Status.Unlocked);
            Assert.AreEqual(5000, Boot.Wallet.Coins);
            yield return TestSnapshots.Capture("m4_04_warehouse_overview");
        }
    }
}
