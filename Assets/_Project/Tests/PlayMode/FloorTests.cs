using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SortingGame.Core;
using SortingGame.Section;
using UnityEngine;
using UnityEngine.TestTools;

namespace SortingGame.Tests
{
    /// <summary>
    /// Playtest feedback after M8: items must never pile up in the middle of the room. Everything is spread evenly
    /// along the room, an item that gets lost comes back where it left, and an item let go (wrong shelf included)
    /// falls to the floor right there and stays.
    /// </summary>
    public class FloorTests
    {
        GameBootstrap Boot => TestGame.Boot;
        SectionController Section => Boot.Section;

        [SetUp]
        public void SetUp() => TestGame.UseTestSave();

        [TearDown]
        public void TearDown() => TestGame.RestoreSave();

        static float Flat(Vector3 a, Vector3 b) => Vector2.Distance(new Vector2(a.x, a.z), new Vector2(b.x, b.z));

        /// <summary>Share of the given positions in each quarter of the room's width, left to right.</summary>
        float[] QuarterShares(IEnumerable<Vector3> positions)
        {
            var bounds = Section.ViewBounds;
            var counts = new float[4];
            var total = 0;
            foreach (var position in positions)
            {
                var t = Mathf.InverseLerp(bounds.min.x, bounds.max.x, position.x);
                counts[Mathf.Clamp((int)(t * 4f), 0, 3)]++;
                total++;
            }
            for (var i = 0; i < 4; i++) counts[i] /= Mathf.Max(1, total);
            return counts;
        }

        IEnumerator WaitUntilStill(float timeout)
        {
            var waited = 0f;
            while (waited < timeout && (Section.Containers.Any(c => c != null && c.Contents.Count > 0) ||
                                        Section.Items.Any(i => i.State == ItemState.Physics)))
            {
                waited += Time.deltaTime;
                yield return null;
            }
        }

        [UnityTest, Timeout(180000)]
        public IEnumerator FreshRoom_IsSpreadEvenly_AndSpillsDoNotPileUpInTheMiddle()
        {
            yield return TestGame.LoadIntoSection("garage", "garage");
            var centre = Section.ViewBounds.center;
            Assert.Greater(Section.ViewBounds.size.x, 12f, "A long room, so uneven spread would show.");

            // Boxes and loose items: every quarter of the room gets its share.
            var boxes = QuarterShares(Section.Containers.Select(c => c.transform.position));
            var loose = QuarterShares(Section.Items.Where(i => i.State == ItemState.Resting).Select(i => i.transform.position));
            var buried = QuarterShares(Section.Items.Where(i => i.State == ItemState.Buried).Select(i => i.transform.position));
            Assert.That(boxes, Is.All.InRange(0.15f, 0.35f), "Boxes: " + string.Join(" / ", boxes.Select(s => s.ToString("P0"))));
            Assert.That(loose, Is.All.InRange(0.18f, 0.32f), "Loose items: " + string.Join(" / ", loose.Select(s => s.ToString("P0"))));
            Assert.That(buried, Is.All.GreaterThan(0.08f), "Buried items: " + string.Join(" / ", buried.Select(s => s.ToString("P0"))));

            // Open everything at once: the worst case for items being pushed around.
            foreach (var container in Section.Containers.ToList()) Section.OpenContainer(container);
            yield return WaitUntilStill(20f);
            yield return new WaitForSeconds(0.5f);

            var states = string.Join(", ", Section.Items.GroupBy(i => i.State).Select(g => $"{g.Key} {g.Count()}"));
            Assert.That(Section.Items.All(i => i.State is ItemState.Resting or ItemState.Buried), $"Everything has come to rest ({states}).");
            var onFloor = Section.Items.Where(i => i.State == ItemState.Resting).ToList();
            Assert.That(onFloor.All(i => i.transform.position.y > -0.2f), "Nothing under the floor.");
            Assert.That(onFloor.All(i => i.transform.position.y < 1.0f), "Nothing on top of the shelves or hanging in the air.");
            var shelfFront = Section.Shelves.Min(s => s.transform.position.z - s.Size.z / 2f);
            Assert.That(onFloor.All(i => i.transform.position.z < shelfFront), "Nothing lying among the shelves.");

            var spilled = QuarterShares(onFloor.Select(i => i.transform.position));
            Assert.That(spilled, Is.All.InRange(0.15f, 0.35f), "After spilling: " + string.Join(" / ", spilled.Select(s => s.ToString("P0"))));
            var inTheMiddle = onFloor.Count(i => Flat(i.transform.position, centre) < 1f);
            Assert.Less(inTheMiddle, onFloor.Count * 0.12f, $"{inTheMiddle} of {onFloor.Count} items lie within a metre of the room's centre.");

            var fitter = Camera.main.GetComponent<CameraFitter>();
            fitter.PanTo(centre.x);
            yield return TestSnapshots.Capture("m9_01_middle_after_spill");
            fitter.PanTo(fitter.PanMin);
            yield return TestSnapshots.Capture("m9_02_left_end_after_spill");
        }

        [UnityTest, Timeout(120000)]
        public IEnumerator LostItem_ComesBackWhereItLeft_NeverInTheMiddle()
        {
            yield return TestGame.LoadIntoSection("garage", "garage");
            var bounds = Section.ViewBounds;
            var items = Section.SortableItems.Where(i => i.State == ItemState.Resting).Take(2).ToList();

            // Pushed through the floor near the right end of the room.
            var under = new Vector3(bounds.max.x - 1.5f, -3f, -1f);
            items[0].transform.position = under;
            items[0].Launch(Vector3.zero, Vector3.zero);

            // Thrown out over the left wall.
            var outside = new Vector3(bounds.min.x - 4f, -3f, 0.5f);
            items[1].transform.position = outside;
            items[1].Launch(Vector3.zero, Vector3.zero);

            yield return null;
            yield return WaitUntilStill(8f);

            Assert.Less(Flat(items[0].transform.position, under), 0.6f, "Back on the floor right above where it fell through.");
            Assert.Greater(items[0].transform.position.y, -0.2f);
            Assert.Less(items[1].transform.position.x, bounds.min.x + 1f, "Back just inside the wall it left by.");
            Assert.Greater(items[1].transform.position.x, bounds.min.x);
            Assert.Greater(Flat(items[1].transform.position, bounds.center), 5f, "Not in the middle of the room.");
            Assert.That(items.All(i => i.State == ItemState.Resting));
        }

        [UnityTest, Timeout(120000)]
        public IEnumerator ItemLetGo_FallsWhereItIs_WrongShelfIncluded()
        {
            yield return TestGame.LoadIntoSection("garage", "garage");

            // Carry an item a long way to a shelf it does not belong on and let go.
            var item = Section.SortableItems.First(i => i.State == ItemState.Resting);
            var pickup = item.transform.position;
            var wrong = Section.Shelves.Where(s => s.Category != item.Definition.Category)
                .OrderByDescending(s => Mathf.Abs(s.transform.position.x - pickup.x)).First();
            var shelfFront = wrong.transform.position + new Vector3(0f, 1.2f, -wrong.Size.z / 2f - 0.25f);
            Assert.Greater(Flat(pickup, shelfFront), 3f, "Far enough that flying back would show.");

            item.BeginDrag();
            item.transform.position = shelfFront;
            var coins = Boot.Wallet.Coins;
            var delivered = Section.DeliverStack(new[] { item }, wrong, shelfFront, true);
            Assert.IsEmpty(delivered);
            yield return new WaitForSeconds(0.4f);
            Assert.Less(Flat(item.transform.position, shelfFront), 1f, "It drops here; it does not fly back to where it was picked up.");
            yield return WaitUntilStill(8f);

            Assert.AreEqual(ItemState.Resting, item.State);
            Assert.Less(Flat(item.transform.position, shelfFront), 1.2f, "It lies on the floor in front of that shelf.");
            Assert.Greater(Flat(item.transform.position, pickup), 2f);
            Assert.Less(item.transform.position.y, 0.5f, "On the floor, not on the shelf.");
            Assert.Less(item.transform.position.z, wrong.transform.position.z - wrong.Size.z / 2f, "In front of the shelf.");
            Assert.AreEqual(coins, Boot.Wallet.Coins, "No penalty, no reward (GDD 7.2).");
            Assert.AreEqual(0, wrong.FilledCount);

            // Let go over open floor after carrying it somewhere else: same rule.
            var second = Section.SortableItems.First(i => i.State == ItemState.Resting && i != item);
            var from = second.transform.position;
            var there = Section.FloorPointUnder(from + new Vector3(from.x < Section.ViewBounds.center.x ? 4f : -4f, 0f, 0f)) + Vector3.up * 0.6f;
            second.BeginDrag();
            second.transform.position = there;
            Section.DropOnFloor(second);
            yield return null;
            yield return WaitUntilStill(8f);
            Assert.AreEqual(ItemState.Resting, second.State);
            Assert.Less(Flat(second.transform.position, there), 1f, "It stays where it was dropped.");
        }
    }
}
