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
    /// Runs the M1 loop end to end on the real Main scene: tip every box, try a wrong shelf,
    /// sort everything, expect 100% and the right coin total. Also saves screenshots to Logs/batch.
    /// </summary>
    public class CoreLoopTests
    {
        [UnityTest, Timeout(120000)]
        public IEnumerator SortEverything_ReachesHundredPercent()
        {
            SceneManager.LoadScene("Main");
            yield return null;
            yield return null;

            var boot = Object.FindFirstObjectByType<GameBootstrap>();
            Assert.IsNotNull(boot, "GameBootstrap missing from Main scene.");
            var section = boot.Section;
            var expectedTotal = section.Progress.TotalItems;
            Assert.Greater(expectedTotal, 0);
            Assert.AreEqual(0, section.Progress.Percent);

            yield return Snapshot("m1_01_start");

            foreach (var container in section.Containers.ToList()) section.OpenContainer(container);
            var waited = 0f;
            while (waited < 12f && (section.Containers.Any(c => c != null && c.Contents.Count > 0) ||
                                    section.Items.Any(i => i.State == ItemState.Physics)))
            {
                waited += Time.deltaTime;
                yield return null;
            }
            Assert.AreEqual(expectedTotal, section.Items.Count, "Every item should be spawned after opening all boxes.");
            Assert.That(section.Items.All(i => i.transform.position.y > -0.5f), "Items fell out of the room.");

            yield return Snapshot("m1_02_spilled");

            // Wrong shelf: item comes back, no coins lost or gained (GDD 7.2).
            var item = section.Items.First();
            var wrong = section.Shelves.First(s => s.Category != item.Definition.Category);
            var coinsBefore = boot.Wallet.Coins;
            Assert.IsFalse(section.TryPlace(item, wrong, wrong.transform.position));
            yield return new WaitForSeconds(0.6f);
            Assert.AreEqual(coinsBefore, boot.Wallet.Coins);
            Assert.AreEqual(ItemState.Resting, item.State);

            // Sort everything.
            var expectedCoins = coinsBefore;
            foreach (var each in section.Items.ToList())
            {
                var shelf = section.ShelfFor(each.Definition.Category);
                Assert.IsTrue(section.TryPlace(each, shelf, shelf.transform.position), $"{each.Definition.Id} could not be placed.");
                expectedCoins += each.Definition.CoinValue;
                yield return null;
            }
            yield return new WaitForSeconds(1f);

            Assert.IsTrue(section.Progress.IsComplete);
            Assert.AreEqual(100, section.Progress.Percent);
            Assert.AreEqual(expectedCoins, boot.Wallet.Coins);
            Assert.That(section.Shelves.All(s => s.IsFull));

            yield return new WaitForSeconds(1.8f);
            yield return Snapshot("m1_03_complete");
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
