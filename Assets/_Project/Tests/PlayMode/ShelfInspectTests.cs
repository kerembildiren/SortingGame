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
    /// Pre-M5 request: a finished room can be stayed in (banner choice), and a full shelf opens a close-up camera
    /// that pans and zooms along the shelf; Back returns to the room view exactly where it was.
    /// </summary>
    public class ShelfInspectTests
    {
        GameBootstrap Boot => TestGame.Boot;
        SectionController Section => Boot.Section;

        [SetUp]
        public void SetUp() => TestGame.UseTestSave();

        [TearDown]
        public void TearDown() => TestGame.RestoreSave();

        [UnityTest, Timeout(180000)]
        public IEnumerator FinishedRoom_StayAndInspectAShelf_ThenBack()
        {
            yield return TestGame.LoadMain();
            Boot.Flow.OpenSectionImmediately(Boot.Flow.Venue, Boot.Flow.Venue.Sections[0]);
            yield return null;
            yield return TestGame.FinishSection();
            Assert.IsTrue(Section.IsComplete);

            var waited = 0f;
            while (!Boot.Hud.IsBannerVisible && waited < 8f)
            {
                // Finding every Comic Box collectible opens the book; the banner waits until it is closed.
                if (Boot.Hud.IsBookOpen) Boot.Hud.CloseBook();
                waited += Time.deltaTime;
                yield return null;
            }
            Assert.IsTrue(Boot.Hud.IsBannerVisible, "Finished room shows the banner.");
            yield return TestSnapshots.Capture("pre5_01_banner_with_stay");

            // Stay: banner goes away, the room is playable (pan, taps).
            Boot.Hud.StayInRoom();
            Assert.IsFalse(Boot.Hud.IsBannerVisible);
            Assert.AreEqual(GameFlow.Screen.Section, Boot.Flow.Current);
            Assert.IsTrue(Boot.Drag.InputEnabled);

            // A tap on a full shelf is recognised; a tap on the floor in front of it is not.
            var camera = Camera.main;
            var fitter = camera.GetComponent<CameraFitter>();
            var shelf = Section.Shelves.First();
            fitter.PanTo(shelf.transform.position.x);
            yield return null;
            var shelfMiddle = shelf.transform.position + new Vector3(0f, shelf.Size.y * 0.5f, -shelf.Size.z * 0.5f);
            Assert.AreEqual(shelf, Boot.Drag.FullShelfAt(camera.WorldToScreenPoint(shelfMiddle)));
            var floorInFront = shelf.transform.position + new Vector3(0f, 0f, -shelf.Size.z - 0.6f);
            Assert.IsNull(Boot.Drag.FullShelfAt(camera.WorldToScreenPoint(floorInFront)));

            var homePosition = camera.transform.position;
            var homeRotation = camera.transform.rotation;

            Boot.Inspector.Open(shelf);
            yield return new WaitForSeconds(Boot.Context.Feel.InspectFlyDuration + 0.2f);
            Assert.IsTrue(Boot.Inspector.IsOpen);
            Assert.IsTrue(Boot.Hud.IsInspecting);
            Assert.IsFalse(Boot.Drag.InputEnabled, "Room input is paused while looking at a shelf.");
            Assert.IsFalse(fitter.enabled);
            Assert.Less(Vector3.Distance(camera.transform.position, shelfMiddle), Vector3.Distance(homePosition, shelfMiddle),
                "Camera is closer to the shelf than the room view.");
            yield return TestSnapshots.Capture("pre5_02_shelf_closeup");

            // Zoom in and slide to a corner.
            Boot.Inspector.View.ZoomBy(3f);
            Boot.Inspector.View.PanBy(new Vector2(10f, 10f));
            yield return null;
            Assert.Greater(Boot.Inspector.View.Focus.y, 0f, "Zoomed in, the view can move up the shelf.");
            yield return TestSnapshots.Capture("pre5_03_shelf_zoomed_corner");

            Boot.Inspector.Close();
            Assert.IsFalse(Boot.Hud.IsInspecting);
            yield return new WaitForSeconds(Boot.Context.Feel.InspectFlyDuration + 0.2f);
            Assert.IsTrue(fitter.enabled);
            Assert.IsTrue(Boot.Drag.InputEnabled);
            Assert.Less(Vector3.Distance(camera.transform.position, homePosition), 0.01f, "Back exactly where the room view was.");
            Assert.Less(Quaternion.Angle(camera.transform.rotation, homeRotation), 0.1f);
            yield return TestSnapshots.Capture("pre5_04_back_in_room");

            // Leaving the section while a close-up is open hands the camera back cleanly.
            Boot.Inspector.Open(shelf);
            yield return new WaitForSeconds(0.2f);
            Boot.Flow.BackToOverview();
            yield return new WaitForSeconds(1.5f);
            Assert.AreEqual(GameFlow.Screen.Overview, Boot.Flow.Current);
            Assert.IsFalse(Boot.Inspector.IsOpen);
            Assert.IsFalse(Boot.Hud.IsInspecting);
        }
    }
}
