using System.Collections;
using System.Linq;
using NUnit.Framework;
using SortingGame.Core;
using SortingGame.Data;
using SortingGame.Section;
using UnityEngine;
using UnityEngine.TestTools;

namespace SortingGame.Tests
{
    /// <summary>
    /// M6 end to end (GDD 10.3): a hired helper works in the room the player is in, shelves common items (also the
    /// ones that turn up later), leaves boxes, dirt, rare items and Chubby alone, carries more per trip at a higher
    /// level, strolls around a finished room and likes being petted. Its Shop slot follows venue progress.
    /// </summary>
    public class HelperTests
    {
        GameBootstrap Boot => TestGame.Boot;
        SectionController Section => Boot.Section;
        int Placed => Section.Shelves.Sum(s => s.FilledCount);

        [SetUp]
        public void SetUp() => TestGame.UseTestSave();

        [TearDown]
        public void TearDown()
        {
            Time.timeScale = 1f;
            TestGame.RestoreSave();
        }

        HelperDefinition Definition(string id) => Boot.Context.Database.Helpers.First(h => h.Id == id);

        /// <summary>Hires past the venue rule: these tests jump straight into rooms.</summary>
        HelperView Hire(string id, int level = 1)
        {
            var definition = Definition(id);
            Boot.Wallet.Add(10000);
            while (Boot.Context.Helpers.LevelOf(definition) < level)
                Assert.IsTrue(Boot.Context.Helpers.TryUpgrade(definition, Boot.Wallet, true));
            return Boot.Crew.Of(definition);
        }

        /// <summary>Waits in game time (follows Time.timeScale) until enough items are on shelves.</summary>
        IEnumerator WaitForPlaced(int target, float timeout)
        {
            var waited = 0f;
            while (Placed < target && waited < timeout)
            {
                waited += Time.deltaTime;
                yield return null;
            }
        }

        IEnumerator WaitGameSeconds(float seconds)
        {
            var waited = 0f;
            while (waited < seconds)
            {
                waited += Time.deltaTime;
                yield return null;
            }
        }

        int ClosedBoxes => Section.Containers.Count(c => c != null && !c.IsOpened);

        [UnityTest, Timeout(300000)]
        public IEnumerator Helper_ShelvesCommonItems_AlsoLaterOnes_AndLeavesTheRestAlone()
        {
            yield return TestGame.LoadIntoSection("garage", "garage");
            Assert.AreEqual(0, Boot.Crew.Helpers.Count, "Nobody is hired at the start.");
            var boxes = ClosedBoxes;
            var dirt = Section.DirtCleaned;
            var buried = Section.Items.Count(i => i.State == ItemState.Buried);

            var pip = Hire("pip");
            Assert.IsNotNull(pip, "Hired in this room: it shows up right away.");
            yield return new WaitForSeconds(0.7f);
            yield return TestSnapshots.Capture("m6_01_helper_in_the_room");

            // It works by itself, slowly, one item per trip.
            Time.timeScale = 6f;
            var coins = Boot.Wallet.Coins;
            var most = 0;
            var waited = 0f;
            while (Placed < 3 && waited < 150f)
            {
                most = Mathf.Max(most, pip.CarriedCount);
                waited += Time.deltaTime;
                yield return null;
            }
            Assert.GreaterOrEqual(Placed, 3, "The helper shelved items on its own.");
            Assert.AreEqual(1, most, "Level 1 carries one item at a time.");
            Assert.Greater(Boot.Wallet.Coins, coins, "What a helper shelves pays like the player's own work.");

            // What it never does (GDD 10.3).
            Assert.AreEqual(boxes, ClosedBoxes, "Helpers do not open boxes.");
            Assert.AreEqual(dirt, Section.DirtCleaned, 0.0001f, "Helpers do not sweep.");
            Assert.AreEqual(buried, Section.Items.Count(i => i.State == ItemState.Buried));
            Assert.That(Section.SortableItems.Where(i => i.IsRare).All(i => i.State != ItemState.Placed), "Rare items wait for the player.");
            Assert.That(Section.Collectibles.All(c => c.State != ItemState.Found), "Chubby waits for the player.");

            // The player clears the floor: nothing left to do, the helper strolls.
            foreach (var item in Section.SortableItems.Where(i => !i.IsRare && i.State == ItemState.Resting).ToList())
            {
                var shelf = Section.ShelfFor(item.Definition.Category);
                Section.TryPlace(item, shelf, shelf.transform.position);
            }
            yield return WaitGameSeconds(25f);
            Assert.AreEqual(0, pip.CarriedCount);
            Assert.AreEqual(HelperView.Activity.Wandering, pip.Current, "No loose common item: it wanders.");
            var before = Placed;

            // Items that turn up later are found too: boxes opened by the player...
            Time.timeScale = 1f;
            foreach (var container in Section.Containers.ToList()) Section.OpenContainer(container);
            yield return new WaitForSeconds(3f);
            Time.timeScale = 6f;
            yield return WaitForPlaced(before + 3, 150f);
            Assert.GreaterOrEqual(Placed, before + 3, "Items spilled later were picked up.");
            Time.timeScale = 1f;
            yield return TestSnapshots.Capture("m6_02_helper_working");

            // ...and items swept free.
            foreach (var item in Section.SortableItems.Where(i => !i.IsRare && i.State == ItemState.Resting).ToList())
            {
                var shelf = Section.ShelfFor(item.Definition.Category);
                Section.TryPlace(item, shelf, shelf.transform.position);
            }
            yield return new WaitForSeconds(1f);
            var bounds = Section.ViewBounds;
            for (var pass = 0; pass < 2; pass++)
            for (var z = bounds.min.z; z <= bounds.max.z; z += 0.25f)
            for (var x = bounds.min.x; x <= bounds.max.x; x += 0.2f)
                Section.Sweep(new Vector3(x, 0f, z), 0.2f);
            yield return new WaitForSeconds(1f);
            before = Placed;
            Assert.That(Section.SortableItems.Any(i => !i.IsRare && i.State == ItemState.Resting), "Sweeping revealed common items.");
            Time.timeScale = 6f;
            yield return WaitForPlaced(before + 3, 150f);
            Assert.GreaterOrEqual(Placed, before + 3, "Items swept free later were picked up.");
        }

        [UnityTest, Timeout(300000)]
        public IEnumerator UpgradedHelper_CarriesSeveralItemsPerTrip_AndTwoHelpersShareTheWork()
        {
            yield return TestGame.LoadIntoSection("garage", "garage");
            foreach (var container in Section.Containers.ToList()) Section.OpenContainer(container);
            yield return new WaitForSeconds(4f);

            var pip = Hire("pip", 3);
            Assert.AreEqual(3, pip.Capacity);
            var dot = Hire("dot");
            Assert.AreEqual(2, Boot.Crew.Helpers.Count);
            Assert.AreEqual(1, dot.Capacity);

            Time.timeScale = 6f;
            var most = 0;
            var sameTarget = false;
            var waited = 0f;
            var start = Placed;
            while ((most < 2 || Placed < start + 6) && waited < 200f)
            {
                most = Mathf.Max(most, pip.CarriedCount);
                // One claim per item: the set of taken items never holds fewer entries than the helpers hold in total.
                sameTarget |= Boot.Crew.Taken.Count < pip.CarriedCount + dot.CarriedCount;
                waited += Time.deltaTime;
                yield return null;
            }
            Time.timeScale = 1f;
            yield return TestSnapshots.Capture("m6_03_two_helpers");
            Assert.GreaterOrEqual(most, 2, "A level 3 helper gathers several items before going to the shelf.");
            Assert.GreaterOrEqual(Placed, start + 6);
            Assert.IsFalse(sameTarget, "Two helpers never hold the same item.");
        }

        [UnityTest, Timeout(300000)]
        public IEnumerator FinishedRoom_HelpersStroll_LikeBeingPetted_AndComeBackAfterReload()
        {
            yield return TestGame.LoadIntoSection("comic_box", "comic_box");
            var pip = Hire("pip");
            yield return TestGame.FinishSection();

            // The helper may still hold the very last item; the album opens after the Chubby find.
            var waited = 0f;
            while ((!Section.IsComplete || pip.CarriedCount > 0 || Boot.Showcase.IsPlaying) && waited < 30f)
            {
                if (Boot.Hud.IsBookOpen) Boot.Hud.CloseBook();
                waited += Time.deltaTime;
                yield return null;
            }
            Assert.IsTrue(Section.IsComplete);
            waited = 0f;
            while (!Boot.Hud.IsBannerVisible && waited < 8f)
            {
                if (Boot.Hud.IsBookOpen) Boot.Hud.CloseBook();
                waited += Time.deltaTime;
                yield return null;
            }
            Boot.Hud.StayInRoom(); // the player chooses to stay with the helper

            // Strolling around the tidy room.
            var from = pip.transform.position;
            var strolled = 0f;
            waited = 0f;
            while (strolled < 0.3f && waited < 20f)
            {
                strolled = Mathf.Max(strolled, Vector3.Distance(from, pip.transform.position));
                waited += Time.deltaTime;
                yield return null;
            }
            Assert.AreEqual(HelperView.Activity.Wandering, pip.Current);
            Assert.Greater(strolled, 0.3f, "It walks around instead of standing frozen.");

            // A tap on it is recognised, and petting makes it happy for a moment.
            var camera = Camera.main;
            var screen = camera.WorldToScreenPoint(pip.transform.position + Vector3.up * 0.12f);
            Assert.AreEqual(pip, Boot.Drag.HelperAt(screen));
            Assert.IsNull(Boot.Drag.HelperAt(camera.WorldToScreenPoint(pip.transform.position + new Vector3(1.2f, 0f, 0f))));

            pip.Pet();
            Assert.IsTrue(pip.IsHappy);
            Assert.AreEqual(1, pip.PetCount);
            yield return new WaitForSeconds(0.35f);
            yield return TestSnapshots.Capture("m6_04_petting");
            yield return new WaitForSeconds(1.2f);
            Assert.IsFalse(pip.IsHappy, "Then it goes back to strolling.");

            // Hired helpers are saved and show up again with the room.
            Boot.SaveNow();
            yield return TestGame.LoadMain();
            Assert.AreEqual("comic_box", Boot.Flow.ActiveSection.Id);
            Assert.IsTrue(Boot.Context.Helpers.IsHired(Definition("pip")));
            Assert.AreEqual(1, Boot.Crew.Helpers.Count(h => h != null));
        }

        [UnityTest, Timeout(120000)]
        public IEnumerator HelperSlots_OpenWithVenueProgress_AndHelpersLiveInRooms()
        {
            yield return TestGame.LoadMain();
            var pip = Definition("pip");
            var dot = Definition("dot");
            Boot.Wallet.Add(5000);

            // Fresh game: coins are there, the slot is not.
            Assert.IsFalse(Boot.Context.HelperSlotOpen(pip));
            Boot.Hud.OpenShop();
            yield return TestSnapshots.Capture("m6_05_shop_helpers_locked");
            Boot.Hud.BuyHelper(pip);
            Assert.IsFalse(Boot.Context.Helpers.IsHired(pip), "Coins alone do not hire.");
            Assert.AreEqual(5000, Boot.Wallet.Coins);
            Boot.Hud.CloseShop();

            // Garage fully restored: Pip's slot opens and is announced on the overview.
            var garage = TestGame.Venue("garage");
            foreach (var section in garage.Sections) Finish(section);
            Boot.Flow.ShowOverviewImmediately(garage);
            Assert.IsTrue(Boot.Context.HelperSlotOpen(pip));
            Assert.Contains("pip", Boot.Data.AnnouncedHelpers);
            Assert.IsFalse(Boot.Context.HelperSlotOpen(dot), "Not every step brings a new helper.");

            Boot.Hud.OpenShop();
            yield return TestSnapshots.Capture("m6_06_shop_helper_for_hire");
            Boot.Hud.BuyHelper(pip);
            Assert.IsTrue(Boot.Context.Helpers.IsHired(pip));
            Assert.AreEqual(5000 - pip.Levels[0].Cost, Boot.Wallet.Coins);
            Boot.Hud.CloseShop();
            Assert.AreEqual(0, Boot.Crew.Helpers.Count(h => h != null), "No helpers on the overview.");

            // Halfway through the Warehouse: Dot's slot.
            var warehouse = TestGame.Venue("warehouse");
            Finish(warehouse.Sections.First(s => s.Id == "wh_office"));
            Assert.IsFalse(Boot.Context.HelperSlotOpen(dot));
            Finish(warehouse.Sections.First(s => s.Id == "wh_dock"));
            Assert.IsTrue(Boot.Context.HelperSlotOpen(dot));

            // Helpers appear in whatever room the player enters; there is nothing to assign.
            Boot.Flow.OpenSectionImmediately(warehouse, warehouse.Sections.First(s => s.Id == "wh_aisle"));
            yield return null;
            Assert.AreEqual(1, Boot.Crew.Helpers.Count(h => h != null));
            Assert.AreEqual(pip, Boot.Crew.Helpers.First(h => h != null).Definition);
        }

        void Finish(SectionDefinition section) =>
            Boot.Data.SetSection(new SectionSave
            {
                SectionId = section.Id,
                PlacedItems = section.TotalSlotCount,
                TotalItems = section.TotalSlotCount,
                Fraction = 1f,
                Completed = true
            });
    }
}
