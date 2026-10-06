using System.Collections;
using System.IO;
using System.Linq;
using NUnit.Framework;
using SortingGame.Core;
using SortingGame.Data;
using SortingGame.Section;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace SortingGame.Tests
{
    /// <summary>M3 end to end: save/reload keeps the sorted room, Magnet group placement, Category Mastery auto-sort.</summary>
    public class ProgressionPlayTests
    {
        const string TestSave = "test_save.json";
        GameBootstrap _boot;
        SectionController Section => _boot.Section;

        [SetUp]
        public void SetUp()
        {
            SaveSystem.FileName = TestSave;
            DeleteTestSave();
        }

        [TearDown]
        public void TearDown()
        {
            DeleteTestSave();
            SaveSystem.FileName = "save.json";
        }

        static void DeleteTestSave() => new FileSaveStorage(TestSave).Delete();

        IEnumerator LoadMain()
        {
            SceneManager.LoadScene("Main");
            yield return null;
            yield return null;
            _boot = Object.FindFirstObjectByType<GameBootstrap>();
            Assert.IsNotNull(_boot);
        }

        IEnumerator OpenAllBoxesAndSettle()
        {
            foreach (var container in Section.Containers.ToList()) Section.OpenContainer(container);
            var waited = 0f;
            while (waited < 12f && (Section.Containers.Any(c => c != null && c.Contents.Count > 0) ||
                                    Section.Items.Any(i => i.State is ItemState.Physics or ItemState.Flying)))
            {
                waited += Time.deltaTime;
                yield return null;
            }
        }

        [UnityTest, Timeout(120000)]
        public IEnumerator SaveAndReload_KeepsTheRoomAsLeft()
        {
            yield return LoadMain();
            yield return OpenAllBoxesAndSettle();

            // Sweep the front half, sort six items, find one collectible.
            var bounds = Section.ViewBounds;
            for (var z = bounds.min.z; z < bounds.center.z; z += 0.25f)
            for (var x = bounds.min.x; x <= bounds.max.x; x += 0.2f)
                Section.Sweep(new Vector3(x, 0f, z), 0.2f);

            foreach (var item in Section.CommonItems.Where(i => i.State == ItemState.Resting).Take(6).ToList())
            {
                var shelf = Section.ShelfFor(item.Definition.Category);
                Section.TryPlace(item, shelf, shelf.transform.position);
            }
            var collectible = Section.Collectibles.FirstOrDefault(c => c.CanTapToFind);
            if (collectible != null)
            {
                Section.FindCollectible(collectible);
                yield return new WaitForSeconds(1f);
                _boot.RareFind.Dismiss();
            }
            yield return new WaitForSeconds(1f);

            var coins = _boot.Wallet.Coins;
            var placed = Section.Shelves.Sum(s => s.FilledCount);
            var percent = Section.Progress.Percent;
            var dirt = Section.DirtCleaned;
            var found = _boot.Book.FoundIds.Count;
            var floorItems = Section.Items.Count(i => i.State is ItemState.Resting or ItemState.Physics);
            var buried = Section.Items.Count(i => i.State == ItemState.Buried);
            Assert.AreEqual(6, placed);
            _boot.SaveNow();
            Assert.IsTrue(File.Exists(Path.Combine(Application.persistentDataPath, TestSave)));

            // "Close the app and come back".
            yield return LoadMain();
            yield return new WaitForSeconds(0.5f);

            Assert.AreEqual(coins, _boot.Wallet.Coins);
            Assert.AreEqual(placed, Section.Shelves.Sum(s => s.FilledCount), "Sorted stays sorted.");
            Assert.AreEqual(percent, Section.Progress.Percent);
            Assert.AreEqual(dirt, Section.DirtCleaned, 0.001f);
            Assert.AreEqual(found, _boot.Book.FoundIds.Count);
            Assert.AreEqual(floorItems, Section.Items.Count(i => i.State is ItemState.Resting or ItemState.Physics));
            Assert.AreEqual(buried, Section.Items.Count(i => i.State == ItemState.Buried));
            Assert.IsEmpty(Section.Containers.Where(c => c != null), "Opened boxes do not come back.");
        }

        [UnityTest, Timeout(120000)]
        public IEnumerator Magnet_FollowersLandOnTheSameShelf()
        {
            yield return LoadMain();
            yield return OpenAllBoxesAndSettle();

            var magnet = _boot.Context.Database.ToolFor(ToolType.Magnet);
            Assert.IsFalse(_boot.Context.Tools.IsOwned(magnet), "Magnet starts locked.");
            Assert.IsFalse(_boot.Context.Tools.TryUpgrade(magnet, _boot.Wallet), "Cannot buy without coins.");
            _boot.Wallet.Add(1000);
            _boot.Hud.OpenShop();
            yield return TestSnapshots.Capture("m3_02_shop");
            _boot.Hud.CloseShop();
            Assert.IsTrue(_boot.Context.Tools.TryUpgrade(magnet, _boot.Wallet));

            // Magnet pulls same-category items that come within its radius (here: the nearest neighbour).
            var leader = Section.CommonItems.First(i => i.CanPick &&
                Section.FindAttachable(i.transform.position, 10f, i.Definition.Category, new[] { i }) != null);
            var stack = new System.Collections.Generic.List<ItemView> { leader };
            var radius = _boot.Context.ToolStats(ToolType.Magnet).Primary;
            var limit = Mathf.RoundToInt(_boot.Context.ToolStats(ToolType.Magnet).Secondary);
            Assert.AreEqual(2, limit, "Magnet level 1 pulls 2 extra items.");
            Assert.Less(radius, 0.5f, "Magnet radius is small; it works as the finger moves.");
            while (stack.Count < 1 + limit)
            {
                var next = Section.FindAttachable(leader.transform.position, 10f, leader.Definition.Category, stack);
                if (next == null) break;
                stack.Add(next);
            }
            Assert.That(stack.All(i => i.Definition.Category == leader.Definition.Category));
            Assert.IsNull(Section.FindAttachable(leader.transform.position, 0.01f, null, stack), "Nothing within a tiny radius.");

            var shelf = Section.ShelfFor(leader.Definition.Category);
            var before = shelf.FilledCount;
            foreach (var item in stack) item.BeginDrag();
            var delivered = Section.DeliverStack(stack, shelf, shelf.transform.position, true);
            Assert.AreEqual(stack.Count, delivered.Count);
            yield return new WaitForSeconds(1.2f);
            Assert.AreEqual(before + stack.Count, shelf.FilledCount);
        }

        [UnityTest, Timeout(120000)]
        public IEnumerator HandStack_DeliversMixedItemsShelfByShelf()
        {
            yield return LoadMain();
            yield return OpenAllBoxesAndSettle();

            var hand = _boot.Context.Database.ToolFor(ToolType.Hand);
            Assert.AreEqual(1, Mathf.RoundToInt(_boot.Context.ToolStats(ToolType.Hand).Primary), "Hand starts with one item.");
            _boot.Wallet.Add(1000);
            Assert.IsTrue(_boot.Context.Tools.TryUpgrade(hand, _boot.Wallet));
            Assert.IsTrue(_boot.Context.Tools.TryUpgrade(hand, _boot.Wallet));
            Assert.AreEqual(3, Mathf.RoundToInt(_boot.Context.ToolStats(ToolType.Hand).Primary));

            // Carry one comic, one toy and one tool together.
            var categories = _boot.Context.Database.Categories;
            var stack = categories.Select(c => Section.CommonItems.First(i => i.CanPick && i.Definition.Category == c)).ToList();
            foreach (var item in stack) item.BeginDrag();

            // Rest over the toys shelf (not releasing): only the toy jumps in, the others stay in hand.
            var toys = Section.ShelfFor(categories.First(c => c.Id == "toys"));
            var delivered = Section.DeliverStack(stack, toys, toys.transform.position, false);
            Assert.AreEqual(1, delivered.Count);
            Assert.AreEqual("toys", delivered[0].Definition.Category.Id);
            stack.RemoveAll(delivered.Contains);
            Assert.That(stack.All(i => i.State == ItemState.Dragging), "The rest are still carried.");

            // Let go over the comics shelf: the comic lands, the tool goes back to where it was.
            var comics = Section.ShelfFor(categories.First(c => c.Id == "comics"));
            delivered = Section.DeliverStack(stack, comics, comics.transform.position, true);
            Assert.AreEqual(1, delivered.Count);
            yield return new WaitForSeconds(1f);
            Assert.AreEqual(1, toys.FilledCount);
            Assert.AreEqual(1, comics.FilledCount);
            var tool = stack.First(i => i.Definition.Category.Id == "tools");
            Assert.AreEqual(ItemState.Resting, tool.State);
        }

        [UnityTest, Timeout(120000)]
        public IEnumerator Mastery_SortsLooseItemsAndSpills_ByThemselves()
        {
            yield return LoadMain();
            var toys = _boot.Context.Database.CategoryById("toys");
            _boot.Context.Mastery.Restore(new[] { new System.Collections.Generic.KeyValuePair<string, int>("toys", toys.MasteryThreshold - 1) });

            // Mastering moment: one more toy placed -> loose toys fly to the shelf.
            var looseToy = Section.CommonItems.First(i => i.Definition.Category == toys && i.State == ItemState.Resting);
            var mastered = false;
            Section.CategoryMastered += _ => mastered = true;
            var shelf = Section.ShelfFor(toys);
            Section.TryPlace(looseToy, shelf, shelf.transform.position);
            yield return new WaitForSeconds(3f);

            Assert.IsTrue(mastered);
            yield return TestSnapshots.Capture("m3_03_mastered");
            Assert.IsTrue(_boot.Context.Mastery.IsMastered(toys));
            Assert.That(Section.CommonItems.Where(i => i.Definition.Category == toys).All(i => i.State is ItemState.Placed or ItemState.Buried),
                "Every visible toy sorted itself.");

            // Boxes opened after mastery: toys skip the floor.
            yield return OpenAllBoxesAndSettle();
            yield return new WaitForSeconds(1f);
            Assert.That(Section.CommonItems.Where(i => i.Definition.Category == toys).All(i => i.State is ItemState.Placed or ItemState.Buried));
            Assert.That(Section.CommonItems.Any(i => i.Definition.Category != toys && i.State == ItemState.Resting),
                "Other categories still need the player.");
            Assert.That(Section.Collectibles.All(c => c.State != ItemState.Placed), "Collectibles are never auto-sorted.");
        }

        [UnityTest, Timeout(120000)]
        public IEnumerator Mastery_AlsoSortsSweptItems_AndFloorItemsOfANewGame()
        {
            yield return LoadMain();
            _boot.Context.Mastery.Restore(_boot.Context.Database.Categories
                .Select(c => new System.Collections.Generic.KeyValuePair<string, int>(c.Id, c.Id == "comics" ? c.MasteryThreshold : 0)));
            var comics = _boot.Context.Database.CategoryById("comics");

            // New game with comics mastered: loose comics on the floor sort themselves after a moment.
            _boot.Restart();
            yield return null;
            Assert.That(Section.CommonItems.Any(i => i.Definition.Category == comics && i.State == ItemState.Resting), "Expected loose comics at start.");
            yield return new WaitForSeconds(3f);
            Assert.That(Section.CommonItems.Where(i => i.Definition.Category == comics).All(i => i.State is ItemState.Placed or ItemState.Buried),
                "Loose comics of a new game sorted themselves.");

            // Buried comics revealed by sweeping fly to the shelf too.
            var buriedComics = Section.CommonItems.Count(i => i.Definition.Category == comics && i.State == ItemState.Buried);
            var bounds = Section.ViewBounds;
            for (var pass = 0; pass < 2; pass++)
            for (var z = bounds.min.z; z <= bounds.max.z; z += 0.25f)
            for (var x = bounds.min.x; x <= bounds.max.x; x += 0.2f)
                Section.Sweep(new Vector3(x, 0f, z), 0.2f);
            yield return new WaitForSeconds(3f);
            Assert.AreEqual(0, Section.CommonItems.Count(i => i.Definition.Category == comics && i.State is ItemState.Resting or ItemState.Buried),
                $"No comics left on the floor (had {buriedComics} buried).");
        }
    }
}
