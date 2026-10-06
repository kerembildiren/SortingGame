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
            new FileSaveStorage(SaveFile).Delete();
        }

        public static void RestoreSave()
        {
            new FileSaveStorage(SaveFile).Delete();
            SaveSystem.FileName = "save.json";
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

        public static VenueDefinition Venue(string id) => Boot.Context.Database.Venues.First(v => v.Id == id);

        /// <summary>Loads Main and opens a section directly (owning its venue), skipping the overview.</summary>
        public static IEnumerator LoadIntoSection(string venueId, string sectionId)
        {
            yield return LoadMain();
            var venue = Venue(venueId);
            Boot.Flow.OpenSectionImmediately(venue, venue.Sections.First(s => s.Id == sectionId));
            yield return null;
        }
    }
}
