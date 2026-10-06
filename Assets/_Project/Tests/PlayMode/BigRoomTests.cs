using System.Collections;
using System.Linq;
using NUnit.Framework;
using SortingGame.Core;
using SortingGame.Section;
using UnityEngine;
using UnityEngine.TestTools;

namespace SortingGame.Tests
{
    /// <summary>M4.2: big rooms are wider than the screen; the camera pans across them within limits.</summary>
    public class BigRoomTests
    {
        [SetUp]
        public void SetUp() => TestGame.UseTestSave();

        [TearDown]
        public void TearDown() => TestGame.RestoreSave();

        [UnityTest, Timeout(120000)]
        public IEnumerator Garage_IsBig_AndPansEndToEnd()
        {
            yield return TestGame.LoadIntoSection("garage", "garage");
            var boot = TestGame.Boot;
            var section = boot.Section;
            var feel = boot.Context.Feel;

            Assert.AreEqual(200, section.Progress.TotalItems, "Garage holds ~200 items (M4.2 'big').");
            Assert.Greater(section.ViewBounds.size.x, feel.SectionViewWidth, "Room is wider than one screen.");

            var fitter = Camera.main.GetComponent<CameraFitter>();
            Assert.IsTrue(fitter.CanPan);

            fitter.PanTo(fitter.PanMin);
            var left = Camera.main.transform.position.x;
            yield return TestSnapshots.Capture("m42_01_garage_left");

            fitter.Pan(1000f); // clamped to the right end
            var right = Camera.main.transform.position.x;
            Assert.AreEqual(fitter.PanMax - fitter.PanMin, right - left, 0.01f);
            yield return TestSnapshots.Capture("m42_02_garage_right");

            // Shelves at the far ends are reachable by panning (labels drawn for every shelf).
            Assert.AreEqual(4, section.Shelves.Count);
            Assert.That(section.Shelves.First().transform.position.x, Is.LessThan(left + feel.SectionViewWidth / 2f));
            Assert.That(section.Shelves.Last().transform.position.x, Is.GreaterThan(right - feel.SectionViewWidth / 2f));
        }

        [UnityTest, Timeout(120000)]
        public IEnumerator Warehouse_Room_Has300Items_AndManyCategories()
        {
            yield return TestGame.LoadIntoSection("warehouse", "wh_aisle");
            var section = TestGame.Boot.Section;
            Assert.AreEqual(300, section.Progress.TotalItems);
            Assert.AreEqual(6, section.Shelves.Count, "Variety grows along the ladder.");
            yield return TestSnapshots.Capture("m42_03_aisle");
        }
    }
}
