using System.Collections;
using System.Linq;
using NUnit.Framework;
using SortingGame.Core;
using SortingGame.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SortingGame.Tests
{
    /// <summary>Shared PlayMode helpers: isolated save file, loading Main, jumping into a section.</summary>
    public static class TestGame
    {
        public const string SaveFile = "test_save.json";

        public static void UseTestSave()
        {
            SaveSystem.FileName = SaveFile;
            Loc.OverrideLanguage = Loc.DefaultLanguage; // never the language picked on this machine
            new FileSaveStorage(SaveFile).Delete();
        }

        public static void RestoreSave()
        {
            new FileSaveStorage(SaveFile).Delete();
            SaveSystem.FileName = "save.json";
            Loc.OverrideLanguage = null;
        }

        public static GameBootstrap Boot { get; private set; }

        public static IEnumerator LoadMain()
        {
            SceneManager.LoadScene("Main");
            yield return null;
            yield return null;
            Boot = Object.FindFirstObjectByType<GameBootstrap>();
            Assert.IsNotNull(Boot, "GameBootstrap missing from Main scene.");
        }

        /// <summary>The game reloaded its scene by itself (language switch): pick up the new bootstrap.</summary>
        public static IEnumerator AfterReload()
        {
            yield return null;
            yield return null;
            Boot = Object.FindFirstObjectByType<GameBootstrap>();
            Assert.IsNotNull(Boot, "GameBootstrap missing after the reload.");
            yield return null;
        }

        public static VenueDefinition Venue(string id) => Boot.Context.Database.Venues.First(v => v.Id == id);

        /// <summary>Loads Main and opens a section directly (owning its venue), skipping the overview.</summary>
        public static IEnumerator LoadIntoSection(string venueId, string sectionId)
        {
            yield return LoadMain();
            var venue = Venue(venueId);
            Boot.Flow.OpenSectionImmediately(venue, venue.Sections.First(s => s.Id == sectionId));
            yield return null;
        }

        /// <summary>Open boxes, sweep everything, find collectibles, shelve everything.</summary>
        public static IEnumerator FinishSection(bool pickUpCollectibles = true, string showcaseShot = null)
        {
            foreach (var container in Boot.Section.Containers.ToList()) Boot.Section.OpenContainer(container);
            yield return new WaitForSeconds(2.5f);
            var bounds = Boot.Section.ViewBounds;
            for (var pass = 0; pass < 2; pass++)
            for (var z = bounds.min.z; z <= bounds.max.z; z += 0.25f)
            for (var x = bounds.min.x; x <= bounds.max.x; x += 0.2f)
                Boot.Section.Sweep(new Vector3(x, 0f, z), 0.2f);
            yield return new WaitForSeconds(1f);

            foreach (var collectible in pickUpCollectibles ? Boot.Section.Collectibles.ToList() : new System.Collections.Generic.List<SortingGame.Section.ItemView>())
            {
                Boot.Section.FindCollectible(collectible);
                yield return new WaitForSeconds(1f);
                Boot.RareFind.Dismiss();
                yield return new WaitForSeconds(0.6f);
            }

            foreach (var item in Boot.Section.SortableItems.Where(i => i.State is SortingGame.Section.ItemState.Resting or SortingGame.Section.ItemState.Physics).ToList())
            {
                // The list was taken a moment ago: a helper may have picked this one up since.
                if (!item.CanPick) continue;
                var shelf = Boot.Section.ShelfFor(item.Definition.Category);
                Boot.Section.TryPlace(item, shelf, shelf.transform.position);
                yield return null;
            }
            if (showcaseShot != null)
            {
                yield return new WaitForSeconds(1.3f); // mid-glide
                Assert.IsTrue(Boot.Showcase.IsPlaying, "A full shelf gets its camera showcase.");
                yield return TestSnapshots.Capture(showcaseShot);
            }
            // Every finished shelf gets its showcase; wait for the queue to drain.
            var waited = 0f;
            while ((Boot.Showcase.IsPlaying || waited < 1f) && waited < 15f)
            {
                waited += Time.deltaTime;
                yield return null;
            }
            Assert.IsFalse(Boot.Showcase.IsPlaying);
        }
    }
}
