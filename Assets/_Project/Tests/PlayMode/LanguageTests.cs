using System.Collections;
using NUnit.Framework;
using SortingGame.Core;
using UnityEngine.TestTools;

namespace SortingGame.Tests
{
    /// <summary>
    /// M7 (GDD 15.6): the language is switched in Settings; the game reloads in place and every text, number
    /// format included, follows. Screenshots show whether the font has the glyphs.
    /// </summary>
    public class LanguageTests
    {
        GameBootstrap Boot => TestGame.Boot;

        [SetUp]
        public void SetUp() => TestGame.UseTestSave();

        [TearDown]
        public void TearDown() => TestGame.RestoreSave();

        [UnityTest, Timeout(120000)]
        public IEnumerator SwitchingLanguage_ReloadsInPlace_WithEveryTextTranslated()
        {
            // The Comic Box is open in a fresh game, so the reload resumes straight into it.
            yield return TestGame.LoadIntoSection("comic_box", "comic_box");
            Assert.AreEqual("en", Loc.Language);
            Assert.AreEqual("Comic Box", Boot.Hud.TitleText);
            Assert.GreaterOrEqual(Loc.Languages.Count, 2);
            Boot.Wallet.Add(1234);
            Boot.Hud.OpenSettings();
            yield return TestSnapshots.Capture("m7_01_settings_en");
            Boot.Hud.CloseSettings();

            Boot.ChangeLanguage("tr");
            yield return TestGame.AfterReload();
            Assert.AreEqual("tr", Loc.Language);
            Assert.AreEqual(GameFlow.Screen.Section, Boot.Flow.Current, "Back in the same room.");
            Assert.AreEqual("comic_box", Boot.Flow.ActiveSection.Id);
            Assert.AreEqual(1234, Boot.Wallet.Coins, "Nothing is lost by switching.");
            Assert.AreEqual("Çizgi Roman Kutusu", Boot.Hud.TitleText);
            Assert.AreEqual("1.234", 1234.ToString("N0", Loc.Culture), "Numbers follow the language.");
            yield return TestSnapshots.Capture("m7_02_section_tr");

            Boot.Hud.OpenShop();
            yield return TestSnapshots.Capture("m7_03_shop_tr");
            Boot.Hud.CloseShop();
            Boot.Hud.OpenBoost();
            yield return TestSnapshots.Capture("m7_04_auto_sort_tr");
            Boot.Hud.CloseBoost();
            Boot.Hud.OpenSettings();
            yield return TestSnapshots.Capture("m7_05_settings_tr");
            Boot.Hud.CloseSettings();

            Boot.ChangeLanguage("en");
            yield return TestGame.AfterReload();
            Assert.AreEqual("en", Loc.Language);
            Assert.AreEqual("Comic Box", Boot.Hud.TitleText);
        }
    }
}
