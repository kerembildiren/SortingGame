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
    /// M8: the Abandoned Circus, the fourth venue. Eight rooms of 400 items with eight shelves each, opened in
    /// stages (by progress only); a Ringmaster Chubby in the Main Tent; it opens when the Warehouse is fully restored.
    /// </summary>
    public class CircusTests
    {
        GameBootstrap Boot => TestGame.Boot;
        GameFlow Flow => Boot.Flow;
        SectionController Section => Boot.Section;

        [SetUp]
        public void SetUp() => TestGame.UseTestSave();

        [TearDown]
        public void TearDown() => TestGame.RestoreSave();

        void Finish(SectionDefinition section) =>
            Boot.Data.SetSection(new SectionSave
            {
                SectionId = section.Id,
                PlacedItems = section.TotalSlotCount,
                TotalItems = section.TotalSlotCount,
                Fraction = 1f,
                Completed = true
            });

        [UnityTest, Timeout(120000)]
        public IEnumerator Circus_OpensAfterTheWarehouse_AndItsRoomsOpenInStages()
        {
            yield return TestGame.LoadMain();
            var ladder = Boot.Context.Database.Venues;
            var circus = TestGame.Venue("circus");
            Assert.AreEqual(4, ladder.Count);
            Assert.AreSame(circus, ladder[3]);
            Assert.AreEqual(8, circus.Sections.Count);
            Assert.AreEqual(VenueProgress.VenueState.Locked, VenueProgress.State(ladder, 3, Boot.Data));

            // Everything before it restored: the Circus is open.
            foreach (var venue in ladder.Take(3))
            foreach (var section in venue.Sections)
                Finish(section);
            Assert.AreEqual(VenueProgress.VenueState.Open, VenueProgress.State(ladder, 3, Boot.Data));

            Flow.ShowOverviewImmediately(circus);
            yield return null;
            Assert.AreEqual(8, Boot.Overview.Rooms.Count);
            Assert.AreEqual(3, Boot.Overview.Rooms.Count(r => r.Status.Unlocked), "Three rooms to start with.");
            var tent = circus.Sections.Last();
            Assert.AreEqual("cs_main_tent", tent.Id);
            Assert.IsFalse(Boot.Overview.RoomOf(tent).Status.Unlocked, "The Main Tent comes last.");
            yield return TestSnapshots.Capture("m8_01_circus_overview");
            Flow.ShowMap();
            yield return TestSnapshots.Capture("m8_02_map_four_places");
            Boot.Hud.CloseMap();

            // The open rooms half done on average: the next room opens by itself, the later ones stay shut.
            foreach (var section in circus.Sections.Take(3))
                Boot.Data.SetSection(new SectionSave { SectionId = section.Id, PlacedItems = 200, TotalItems = section.TotalSlotCount, Fraction = 0.5f });
            Flow.ShowOverviewImmediately(circus);
            yield return new WaitForSeconds(0.3f);
            Assert.AreEqual(4, Boot.Overview.Rooms.Count(r => r.Status.Unlocked));
            Assert.IsTrue(Boot.Overview.RoomOf(circus.Sections[3]).Status.Unlocked);

            // The rest wait their turn: the freshly opened, empty room pulls the average down.
            Assert.IsFalse(Boot.Overview.RoomOf(circus.Sections[4]).Status.Unlocked);
            Assert.IsFalse(Boot.Overview.RoomOf(tent).Status.Unlocked);
        }

        [UnityTest, Timeout(180000)]
        public IEnumerator MainTent_Has400Items_EightShelves_AndTheRingmaster()
        {
            yield return TestGame.LoadIntoSection("circus", "cs_main_tent");
            Assert.AreEqual(400, Section.Progress.TotalItems, "About 400 items per Circus room (user: bigger than the Warehouse).");
            Assert.AreEqual(8, Section.Shelves.Count);
            Assert.AreEqual(8, Section.Shelves.Select(s => s.Category).Distinct().Count(), "Eight different categories.");
            Assert.That(Section.Definition.Collectibles.Any(c => c.Id == "ringmaster_chubby"), "The Circus Chubby hides in the Main Tent.");
            Assert.AreEqual(1, Section.Definition.RareItems.Count);

            var fitter = Camera.main.GetComponent<CameraFitter>();
            Assert.IsTrue(fitter.CanPan);
            fitter.PanTo(fitter.PanMin);
            yield return TestSnapshots.Capture("m8_03_main_tent_left");

            // Spill everything: the room copes with all 400 items at once.
            foreach (var container in Section.Containers.ToList()) Section.OpenContainer(container);
            var waited = 0f;
            while (waited < 15f && Section.Items.Any(i => i.State == ItemState.Physics))
            {
                waited += Time.deltaTime;
                yield return null;
            }
            Assert.That(Section.Items.All(i => i.transform.position.y > -0.5f), "Items fell out of the room.");
            fitter.PanTo((fitter.PanMin + fitter.PanMax) * 0.5f);
            yield return TestSnapshots.Capture("m8_04_main_tent_spilled");

            // Every category of the room can be shelved.
            foreach (var shelf in Section.Shelves)
            {
                var item = Section.SortableItems.First(i => i.CanPick && i.Definition.Category == shelf.Category);
                Assert.IsTrue(Section.TryPlace(item, shelf, shelf.transform.position), $"{item.Definition.Id} does not fit its shelf.");
            }
            yield return new WaitForSeconds(1f);
            Assert.AreEqual(8, Section.Shelves.Sum(s => s.FilledCount));
        }
    }
}
