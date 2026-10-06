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
    /// <summary>
    /// M3 + M5 end to end: save/reload keeps the sorted room, Hand stack, Magnet (gated behind max Hand),
    /// Auto Sort boost paid by ad or by a store charge.
    /// </summary>
    public class ProgressionPlayTests
    {
        const string TestSave = TestGame.SaveFile;
        GameBootstrap _boot;
        SectionController Section => _boot.Section;

        [SetUp]
        public void SetUp() => TestGame.UseTestSave();

        [TearDown]
        public void TearDown() => TestGame.RestoreSave();

        IEnumerator LoadMain()
        {
            yield return TestGame.LoadIntoSection("garage", "garage");
            _boot = TestGame.Boot;
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
            // The Comic Box is open in a fresh game, so the game may resume straight into it.
            yield return TestGame.LoadIntoSection("comic_box", "comic_box");
            _boot = TestGame.Boot;
            yield return OpenAllBoxesAndSettle();

            // Sweep the front half, sort six items, find one collectible.
            var bounds = Section.ViewBounds;
            for (var z = bounds.min.z; z < bounds.center.z; z += 0.25f)
            for (var x = bounds.min.x; x <= bounds.max.x; x += 0.2f)
                Section.Sweep(new Vector3(x, 0f, z), 0.2f);

            foreach (var item in Section.SortableItems.Where(i => i.State == ItemState.Resting).Take(6).ToList())
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

            // "Close the app and come back": the game resumes in the same section by itself.
            yield return TestGame.LoadMain();
            _boot = TestGame.Boot;
            Assert.AreEqual(GameFlow.Screen.Section, _boot.Flow.Current);
            Assert.AreEqual("comic_box", _boot.Flow.ActiveSection.Id);
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
            _boot.Wallet.Add(2000);
            Assert.IsFalse(_boot.Context.Tools.TryUpgrade(magnet, _boot.Wallet), "Coins are not enough: Hand has to be maxed first (GDD 10.1).");
            _boot.Hud.OpenShop();
            yield return TestSnapshots.Capture("m5_01_shop_magnet_gated");
            var hand = _boot.Context.Database.ToolFor(ToolType.Hand);
            while (!_boot.Context.Tools.IsMaxed(hand)) Assert.IsTrue(_boot.Context.Tools.TryUpgrade(hand, _boot.Wallet));
            _boot.Hud.OpenShop();
            yield return TestSnapshots.Capture("m5_02_shop_magnet_open");
            _boot.Hud.CloseShop();
            Assert.IsTrue(_boot.Context.Tools.TryUpgrade(magnet, _boot.Wallet));

            // Magnet pulls same-category items that come within its radius (here: the nearest neighbour).
            var leader = Section.SortableItems.First(i => i.CanPick &&
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
            var categories = Section.Shelves.Select(shelf => shelf.Category).ToList();
            var stack = categories.Select(c => Section.SortableItems.First(i => i.CanPick && i.Definition.Category == c)).ToList();
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

        System.Collections.Generic.List<ItemView> CommonOf(CategoryDefinition category) =>
            Section.SortableItems.Where(i => !i.IsRare && i.Definition.Category == category).ToList();

        void SweepWholeFloor()
        {
            var bounds = Section.ViewBounds;
            for (var pass = 0; pass < 2; pass++)
            for (var z = bounds.min.z; z <= bounds.max.z; z += 0.25f)
            for (var x = bounds.min.x; x <= bounds.max.x; x += 0.2f)
                Section.Sweep(new Vector3(x, 0f, z), 0.2f);
        }

        [UnityTest, Timeout(180000)]
        public IEnumerator AutoSort_PaidByAd_SortsOneCategoryUntilTheRoomIsDone()
        {
            yield return LoadMain();
            var toys = _boot.Context.Database.CategoryById("toys");
            var shelf = Section.ShelfFor(toys);
            var ads = (FakeAdProvider)_boot.Context.Ads;
            Assert.That(CommonOf(toys).Any(i => i.State == ItemState.Resting), "Expected loose toys at start.");

            // Tool bar button -> card -> pick a shelf -> watch an ad. An ad that does not finish starts nothing.
            _boot.Hud.OpenBoost();
            Assert.IsTrue(_boot.Hud.IsBoostOpen);
            _boot.Hud.PickBoostShelf(shelf);
            yield return TestSnapshots.Capture("m5_03_auto_sort_card");
            ads.NextResult = false;
            _boot.Hud.BoostWithAd();
            Assert.IsNull(Section.AutoSortCategory, "No reward, no boost.");

            ads.NextResult = true;
            _boot.Hud.BoostWithAd();
            Assert.AreEqual(toys, Section.AutoSortCategory);
            Assert.IsFalse(_boot.Hud.IsBoostOpen);
            yield return new WaitForSeconds(1.2f);
            yield return TestSnapshots.Capture("m5_04_auto_sort_running");
            yield return new WaitForSeconds(3f);
            Assert.That(CommonOf(toys).All(i => i.State is ItemState.Placed or ItemState.Buried), "Every loose toy sorted itself.");

            // Boxes opened later: toys skip the floor.
            yield return OpenAllBoxesAndSettle();
            yield return new WaitForSeconds(1.5f);
            Assert.That(CommonOf(toys).All(i => i.State is ItemState.Placed or ItemState.Buried), "Toys from boxes went straight to the shelf.");
            Assert.That(Section.SortableItems.Any(i => i.Definition.Category != toys && i.State == ItemState.Resting),
                "Other categories still need the player.");

            // Swept free later: they fly too.
            SweepWholeFloor();
            yield return new WaitForSeconds(4f);
            Assert.That(CommonOf(toys).All(i => i.State == ItemState.Placed), "No common toy is left anywhere.");

            // Whatever comes loose again later is caught as well (the bug the first mastery version had).
            var rareToy = Section.SortableItems.First(i => i.IsRare && i.Definition.Category == toys);
            Assert.AreNotEqual(ItemState.Placed, rareToy.State, "Rare items are the player's to shelve (GDD 9.4).");
            Assert.That(Section.Collectibles.All(c => c.State != ItemState.Placed), "Collectibles are never auto-sorted.");
            Assert.IsFalse(shelf.IsFull, "The rare toy's slot is still waiting for the player.");

            // One shelf per room.
            Assert.IsFalse(Section.CanStartAutoSort);
            Assert.IsFalse(Section.StartAutoSort(Section.Shelves.First(s => s.Category != toys)));
            _boot.Hud.OpenBoost();
            Assert.IsFalse(_boot.Hud.IsBoostOpen);
            Assert.AreEqual(toys, Section.AutoSortCategory);
        }

        [UnityTest, Timeout(180000)]
        public IEnumerator AutoSort_DroppedItems_AreCaughtByTheNextTick()
        {
            yield return LoadMain();
            var tools = _boot.Context.Database.CategoryById("tools");
            var carried = CommonOf(tools).First(i => i.State == ItemState.Resting);

            // The player is holding a tool while the boost starts, then lets go over the floor.
            carried.BeginDrag();
            Assert.IsTrue(Section.StartAutoSort(Section.ShelfFor(tools)));
            yield return new WaitForSeconds(1f);
            Assert.AreEqual(ItemState.Dragging, carried.State, "An item in the hand is left alone.");
            carried.Drop();
            yield return new WaitForSeconds(3f);
            Assert.AreEqual(ItemState.Placed, carried.State, "Dropped later, sorted anyway.");
        }

        [UnityTest, Timeout(180000)]
        public IEnumerator AutoSort_ChargeFromTheStore_SurvivesReload()
        {
            yield return TestGame.LoadIntoSection("comic_box", "comic_box");
            _boot = TestGame.Boot;
            var comics = _boot.Context.Database.CategoryById("comics");
            var coins = _boot.Wallet.Coins;

            // Charges come from the (fake) store, never from coins.
            var pack = _boot.Context.Database.Balance.AutoSortPacks.First(p => p.Charges > 1);
            _boot.Hud.OpenBoost();
            _boot.Hud.OpenStore();
            yield return TestSnapshots.Capture("m5_05_store");
            _boot.Hud.BuyPack(pack);
            Assert.AreEqual(pack.Charges, _boot.Context.AutoSort.Charges);
            Assert.AreEqual(coins, _boot.Wallet.Coins, "Auto Sort is never paid with coins.");
            _boot.Hud.CloseStore();

            _boot.Hud.PickBoostShelf(Section.ShelfFor(comics));
            _boot.Hud.BoostWithCharge();
            Assert.AreEqual(comics, Section.AutoSortCategory);
            Assert.AreEqual(pack.Charges - 1, _boot.Context.AutoSort.Charges);
            Assert.AreEqual(0, ((FakeAdProvider)_boot.Context.Ads).ShownCount);
            yield return new WaitForSeconds(2.5f);
            _boot.SaveNow();

            // "Close the app and come back": the room remembers its boosted shelf, the charges are kept.
            yield return TestGame.LoadMain();
            _boot = TestGame.Boot;
            Assert.AreEqual("comic_box", _boot.Flow.ActiveSection.Id);
            Assert.AreEqual(comics, Section.AutoSortCategory);
            Assert.AreEqual(pack.Charges - 1, _boot.Context.AutoSort.Charges);
            Assert.IsFalse(Section.CanStartAutoSort);

            // And it keeps working: comics from the boxes go straight to the shelf.
            yield return OpenAllBoxesAndSettle();
            yield return new WaitForSeconds(2f);
            Assert.That(CommonOf(comics).All(i => i.State is ItemState.Placed or ItemState.Buried));
        }
    }
}
