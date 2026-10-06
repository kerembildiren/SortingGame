using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;
using SortingGame.Core;
using SortingGame.Data;
using UnityEditor;

namespace SortingGame.Tests
{
    /// <summary>
    /// GDD 15.6: texts live in string tables, one file per language. These tests keep the tables honest:
    /// same keys and placeholders in every language, nothing the code or the content asks for is missing,
    /// nothing in the table is dead.
    /// </summary>
    public class LocTests
    {
        const string DatabasePath = "Assets/_Project/Data/GameDatabase.asset";
        const string ScriptsFolder = "Assets/_Project/Scripts/Runtime";
        static readonly Regex KeyLiteral = new("\"((?:hud|tool|helper|category|section|venue|collectible|item)\\.[a-z0-9_.]+)\"");
        static readonly Regex Placeholder = new(@"\{(\d+)[^}]*\}");

        static GameDatabase Database => AssetDatabase.LoadAssetAtPath<GameDatabase>(DatabasePath);

        static Dictionary<string, Dictionary<string, string>> Tables() =>
            Database.Languages.ToDictionary(l => l.Code, l => Loc.Parse(l.Table.text));

        [TearDown]
        public void TearDown() => Loc.Load(new (string, string, string)[0]);

        [Test]
        public void Parse_ReadsKeys_SkipsComments_KeepsEqualsSignsInTheText()
        {
            var table = Loc.Parse("# comment\n\nhud.a = Hello\r\n  hud.b=x = y  \nnot a pair\nhud.a = second copy is ignored\n");

            Assert.AreEqual(2, table.Count);
            Assert.AreEqual("Hello", table["hud.a"]);
            Assert.AreEqual("x = y", table["hud.b"]);
        }

        [Test]
        public void MissingText_FallsBackToEnglish_ThenToTheKey()
        {
            Loc.Load(new[] { ("en", "English", "hud.a = Apple\nhud.b = Banana"), ("tr", "Türkçe", "hud.a = Elma") });

            Loc.SetLanguage("tr");
            Assert.AreEqual("Elma", Loc.Get("hud.a"));
            Assert.AreEqual("Banana", Loc.Get("hud.b"), "Not translated yet: English.");
            Assert.AreEqual("hud.c", Loc.Get("hud.c"), "Nobody has it: the key shows, so it gets noticed.");

            Loc.SetLanguage("xx");
            Assert.AreEqual("en", Loc.Language, "Unknown language: English.");
            Assert.AreEqual("tr", Loc.NextLanguage());
            Assert.AreEqual("Türkçe", Loc.NativeNameOf("tr"));
        }

        [Test]
        public void Numbers_FollowTheLanguage()
        {
            Loc.Load(new[] { ("en", "English", "hud.n = {0:N0} items"), ("tr", "Türkçe", "hud.n = {0:N0} eşya") });

            Loc.SetLanguage("en");
            Assert.AreEqual("1,234 items", Loc.Format("hud.n", 1234));
            Loc.SetLanguage("tr");
            Assert.AreEqual("1.234 eşya", Loc.Format("hud.n", 1234));
        }

        [Test]
        public void EveryLanguage_HasExactlyTheEnglishKeys_AndTheSamePlaceholders()
        {
            var tables = Tables();
            Assert.That(tables.Keys, Has.Member("en"));
            Assert.Greater(tables.Count, 1, "The infrastructure is proven by a second language.");
            var english = tables["en"];

            foreach (var (code, table) in tables.Where(t => t.Key != "en"))
            {
                CollectionAssert.IsEmpty(english.Keys.Except(table.Keys), $"'{code}' is missing texts.");
                CollectionAssert.IsEmpty(table.Keys.Except(english.Keys), $"'{code}' has texts English does not have.");
                foreach (var key in english.Keys)
                    CollectionAssert.AreEquivalent(Slots(english[key]), Slots(table[key]), $"'{code}' / {key}: different placeholders.");
            }
        }

        static List<string> Slots(string text) =>
            Placeholder.Matches(text).Select(m => m.Groups[1].Value).Distinct().OrderBy(s => s).ToList();

        static HashSet<string> KeysInCode() =>
            new(Directory.GetFiles(ScriptsFolder, "*.cs", SearchOption.AllDirectories)
                .SelectMany(file => KeyLiteral.Matches(File.ReadAllText(file)).Select(m => m.Groups[1].Value)));

        /// <summary>Names and descriptions the content shows to the player.</summary>
        static HashSet<string> KeysInContent()
        {
            var database = Database;
            var keys = new HashSet<string>();
            foreach (var category in database.Categories) keys.Add(category.DisplayNameKey);
            foreach (var venue in database.Venues)
            {
                keys.Add(venue.DisplayNameKey);
                foreach (var section in venue.Sections) keys.Add(section.DisplayNameKey);
            }
            foreach (var tool in database.Tools)
            {
                keys.Add(tool.DisplayNameKey);
                keys.Add(tool.EffectKey);
            }
            foreach (var helper in database.Helpers) keys.Add(helper.DisplayNameKey);
            foreach (var collectible in database.Album)
            {
                keys.Add(collectible.DisplayNameKey);
                keys.Add(collectible.DescriptionKey);
            }
            foreach (var item in database.Items.Where(i => i.IsRare)) keys.Add(item.DisplayNameKey);
            return keys;
        }

        [Test]
        public void EveryTextTheGameAsksFor_Exists()
        {
            var english = Tables()["en"];
            CollectionAssert.IsEmpty(KeysInCode().Where(k => !english.ContainsKey(k)), "Used in code, missing in en.txt.");
            CollectionAssert.IsEmpty(KeysInContent().Where(k => !english.ContainsKey(k)), "Shown by content, missing in en.txt.");
        }

        [Test]
        public void NoDeadTexts_InTheTable()
        {
            var used = KeysInCode();
            used.UnionWith(KeysInContent());
            CollectionAssert.IsEmpty(Tables()["en"].Keys.Where(k => !used.Contains(k)), "In en.txt but used nowhere.");
        }
    }
}
