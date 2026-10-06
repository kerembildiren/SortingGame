using System.Collections;
using System.IO;
using SortingGame.Section;
using UnityEngine;
using UnityEngine.UIElements;

namespace SortingGame.Tests
{
    /// <summary>Saves camera + HUD renders to Logs/batch/*.png so changes can be reviewed visually.</summary>
    public static class TestSnapshots
    {
        /// <summary>Renders the camera and the HUD at phone resolution and saves a PNG for review.</summary>
        public static IEnumerator Capture(string name)
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
