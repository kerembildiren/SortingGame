using System.Collections;
using System.IO;
using System.Linq;
using NUnit.Framework;
using SortingGame.Core;
using SortingGame.Section;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace SortingGame.Tests
{
    /// <summary>
    /// Runs the loop end to end on the real Main scene: tip every box, sweep the floor, find every
    /// collectible, try a wrong shelf, sort everything, expect 100%; then replay and check duplicates sell.
    /// Saves screenshots to Logs/batch for review.
    /// </summary>
    public class CoreLoopTests
    {
        GameBootstrap _boot;
        SectionController Section => _boot.Section;

        [UnityTest, Timeout(180000)]
        public IEnumerator FullLoop_SweepFindSort_ReachesHundredPercent()
        {
            yield return LoadMain();
            var section = Section;
            var expectedTotal = section.Progress.TotalItems;
            Assert.Greater(expectedTotal, 0);
            Assert.IsTrue(section.HasDirt, "Sample section should have a dirt layer.");
            Assert.That(section.Items.Any(i => i.State == ItemState.Buried), "Some items should start buried.");

            yield return Snapshot("m2_01_start");

            yield return OpenAllBoxes();
            yield return Snapshot("m2_02_spilled");

            // Sweep half the floor and look at it, then the rest.
            yield return SweepFloor(0.5f);
            yield return Snapshot("m2_03_half_swept");
            yield return SweepFloor(1f);
            Assert.AreEqual(1f, section.DirtCleaned, 0.001f);
            Assert.That(section.Items.All(i => i.State != ItemState.Buried), "Sweeping everything reveals every buried item.");

            // Find all collectibles. First one: snapshot the moment.
            var collectibles = section.Collectibles.ToList();
            Assert.AreEqual(4, collectibles.Count);
            var coinsBeforeFinds = _boot.Wallet.Coins;
            for (var i = 0; i < collectibles.Count; i++)
            {
                section.FindCollectible(collectibles[i]);
                yield return new WaitForSeconds(_boot.RareFind != null ? 1.0f : 0f);
                Assert.IsTrue(_boot.RareFind.IsPresenting);
                if (i == 0) yield return Snapshot("m2_04_rare_find");
                _boot.RareFind.Dismiss();
                yield return new WaitForSeconds(0.6f);
                Assert.IsFalse(_boot.RareFind.IsPresenting);
            }
            Assert.AreEqual(4, _boot.Book.FoundIds.Count);
            Assert.AreEqual(coinsBeforeFinds, _boot.Wallet.Coins, "First copies give no coins, they go to the book.");
            Assert.IsTrue(_boot.Drag.InputEnabled, "Input must come back after the moment.");

            _boot.Hud.OpenBook();
            yield return Snapshot("m2_05_book");
            _boot.Hud.CloseBook();

            // Wrong shelf: item comes back, no coins lost or gained (GDD 7.2).
            var item = section.CommonItems.First(i => i.State == ItemState.Resting);
            var wrong = section.Shelves.First(s => s.Category != item.Definition.Category);
            var coinsBefore = _boot.Wallet.Coins;
            Assert.IsFalse(section.TryPlace(item, wrong, wrong.transform.position));
            yield return new WaitForSeconds(0.6f);
            Assert.AreEqual(coinsBefore, _boot.Wallet.Coins);
            Assert.AreEqual(ItemState.Resting, item.State);

            // Sort everything.
            var expectedCoins = coinsBefore;
            foreach (var each in section.CommonItems.ToList())
            {
                var shelf = section.ShelfFor(each.Definition.Category);
                Assert.IsTrue(section.TryPlace(each, shelf, shelf.transform.position), $"{each.Definition.Id} could not be placed.");
                expectedCoins += each.Definition.CoinValue;
                yield return null;
            }
            yield return new WaitForSeconds(1f);

            Assert.IsTrue(section.Progress.IsComplete);
            Assert.AreEqual(100, section.Progress.Percent);
            Assert.AreEqual(expectedCoins, _boot.Wallet.Coins);
            Assert.That(section.Shelves.All(s => s.IsFull));

            yield return new WaitForSeconds(1.0f);
            yield return Snapshot("m2_06_renovated");
            yield return new WaitForSeconds(1.2f);
            yield return Snapshot("m2_07_complete");

            // Replay: the same collectibles are duplicates now and sell automatically (GDD 9.3).
            _boot.Restart();
            yield return null;
            yield return OpenAllBoxes();
            yield return SweepFloor(1f);
            var duplicate = Section.Collectibles.First();
            var value = ((SortingGame.Data.CollectibleDefinition)duplicate.Definition).DuplicateSellValue;
            var coinsBeforeDuplicate = _boot.Wallet.Coins;
            Section.FindCollectible(duplicate);
            yield return new WaitForSeconds(0.6f);
            Assert.IsFalse(_boot.RareFind.IsPresenting, "Duplicates do not open the full moment.");
            Assert.AreEqual(coinsBeforeDuplicate + value, _boot.Wallet.Coins);
            Assert.AreEqual(4, _boot.Book.FoundIds.Count);
        }

        IEnumerator LoadMain()
        {
            SceneManager.LoadScene("Main");
            yield return null;
            yield return null;
            _boot = Object.FindFirstObjectByType<GameBootstrap>();
            Assert.IsNotNull(_boot, "GameBootstrap missing from Main scene.");
        }

        IEnumerator OpenAllBoxes()
        {
            foreach (var container in Section.Containers.ToList()) Section.OpenContainer(container);
            var waited = 0f;
            while (waited < 12f && (Section.Containers.Any(c => c != null && c.Contents.Count > 0) ||
                                    Section.Items.Any(i => i.State == ItemState.Physics)))
            {
                waited += Time.deltaTime;
                yield return null;
            }
            Assert.That(Section.Items.All(i => i.transform.position.y > -0.5f), "Items fell out of the room.");
        }

        /// <summary>Simulates broom strokes in rows across the floor, front to back, up to <paramref name="share"/> of the depth.</summary>
        IEnumerator SweepFloor(float share)
        {
            var bounds = Section.ViewBounds;
            var minZ = bounds.min.z;
            var maxZ = Mathf.Lerp(bounds.min.z, bounds.max.z, share);
            for (var pass = 0; pass < 2; pass++)
            {
                for (var z = minZ; z <= maxZ; z += 0.25f)
                {
                    for (var x = bounds.min.x; x <= bounds.max.x; x += 0.2f)
                        Section.Sweep(new Vector3(x, 0f, z), 0.2f);
                    yield return null;
                }
            }
            yield return new WaitForSeconds(0.7f);
        }

        /// <summary>Renders the camera and the HUD at phone resolution and saves a PNG for review.</summary>
        static IEnumerator Snapshot(string name)
        {
            const int width = 720, height = 1280;
            var camera = Camera.main;
            var fitter = camera.GetComponent<CameraFitter>();
            var document = Object.FindFirstObjectByType<UIDocument>();
            var panel = document.panelSettings;

            var sceneTexture = new RenderTexture(width, height, 24);
            var uiTexture = new RenderTexture(width, height, 24);
            camera.targetTexture = sceneTexture;
            if (fitter != null) fitter.Fit();
            panel.targetTexture = uiTexture;
            panel.clearColor = true;
            panel.colorClearValue = Color.clear;

            yield return null;
            yield return null; // WaitForEndOfFrame never fires in batchmode
            camera.Render();

            var scene = Read(sceneTexture, width, height);
            var ui = Read(uiTexture, width, height);
            var pixels = scene.GetPixels();
            var overlay = ui.GetPixels();
            for (var i = 0; i < pixels.Length; i++)
                pixels[i] = Color.Lerp(pixels[i], overlay[i], overlay[i].a);
            scene.SetPixels(pixels);
            scene.Apply();

            var folder = Path.Combine(Application.dataPath, "..", "Logs", "batch");
            Directory.CreateDirectory(folder);
            File.WriteAllBytes(Path.Combine(folder, name + ".png"), scene.EncodeToPNG());

            camera.targetTexture = null;
            panel.targetTexture = null;
            panel.clearColor = false;
            if (fitter != null) fitter.Fit();
            Object.Destroy(sceneTexture);
            Object.Destroy(uiTexture);
        }

        static Texture2D Read(RenderTexture source, int width, int height)
        {
            var previous = RenderTexture.active;
            RenderTexture.active = source;
            var texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            texture.Apply();
            RenderTexture.active = previous;
            return texture;
        }
    }
}
