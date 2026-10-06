using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace SortingGame.Core
{
    /// <summary>
    /// GDD 15.6: no hard-coded UI text. Every string goes through <see cref="Get"/> / <see cref="Format"/>.
    /// The texts live in one plain "key = text" file per language (Assets/_Project/Localization), listed in the
    /// GameDatabase. English is the reference table and the fallback for anything a language is missing.
    /// The chosen language is a device setting (PlayerPrefs), not part of the save.
    /// </summary>
    public static class Loc
    {
        public const string DefaultLanguage = "en";
        const string PrefKey = "language";

        public readonly struct LanguageInfo
        {
            public readonly string Code;
            public readonly string NativeName;

            public LanguageInfo(string code, string nativeName)
            {
                Code = code;
                NativeName = nativeName;
            }
        }

        static readonly List<LanguageInfo> _languages = new();
        static readonly Dictionary<string, Dictionary<string, string>> _tables = new();
        static Dictionary<string, string> _current = new();
        static Dictionary<string, string> _fallback = new();

        /// <summary>Tests pin the language here so they never depend on what the player chose on this machine.</summary>
        public static string OverrideLanguage;

        public static string Language { get; private set; } = DefaultLanguage;
        public static IReadOnlyList<LanguageInfo> Languages => _languages;

        /// <summary>Number formatting follows the UI language, not the device region.</summary>
        public static CultureInfo Culture { get; private set; } = CultureInfo.InvariantCulture;

        /// <summary>What the player picked in Settings; the default until they pick something.</summary>
        public static string SavedLanguage =>
            !string.IsNullOrEmpty(OverrideLanguage) ? OverrideLanguage : PlayerPrefs.GetString(PrefKey, DefaultLanguage);

        public static void SaveLanguage(string code)
        {
            if (!string.IsNullOrEmpty(OverrideLanguage))
            {
                OverrideLanguage = code; // tests: leave the player's own choice alone
                return;
            }
            PlayerPrefs.SetString(PrefKey, code);
            PlayerPrefs.Save();
        }

        /// <summary>Replaces all tables. Call once at startup, then <see cref="SetLanguage"/>.</summary>
        public static void Load(IEnumerable<(string code, string nativeName, string text)> tables)
        {
            _languages.Clear();
            _tables.Clear();
            foreach (var (code, nativeName, text) in tables)
            {
                if (string.IsNullOrEmpty(code) || _tables.ContainsKey(code)) continue;
                _tables[code] = Parse(text);
                _languages.Add(new LanguageInfo(code, nativeName));
            }
            _fallback = _tables.TryGetValue(DefaultLanguage, out var english) ? english : new Dictionary<string, string>();
            SetLanguage(Language);
        }

        /// <summary>Unknown codes fall back to English.</summary>
        public static void SetLanguage(string code)
        {
            if (string.IsNullOrEmpty(code) || !_tables.ContainsKey(code)) code = DefaultLanguage;
            Language = code;
            _current = _tables.TryGetValue(code, out var table) ? table : _fallback;
            Culture = CultureFor(code);
        }

        /// <summary>The language after the current one in the list (Settings cycles through them).</summary>
        public static string NextLanguage()
        {
            if (_languages.Count == 0) return Language;
            var index = _languages.FindIndex(l => l.Code == Language);
            return _languages[(index + 1) % _languages.Count].Code;
        }

        public static string NativeNameOf(string code)
        {
            var index = _languages.FindIndex(l => l.Code == code);
            return index >= 0 ? _languages[index].NativeName : code;
        }

        /// <summary>The text for a key; English when the language lacks it; the key itself when nobody has it.</summary>
        public static string Get(string key)
        {
            if (string.IsNullOrEmpty(key)) return key;
            if (_current.TryGetValue(key, out var value)) return value;
            return _fallback.TryGetValue(key, out value) ? value : key;
        }

        public static string Format(string key, params object[] args) => string.Format(Culture, Get(key), args);

        public static bool Has(string key, string language = null) =>
            _tables.TryGetValue(language ?? Language, out var table) && table.ContainsKey(key);

        /// <summary>
        /// "key = text" per line. Blank lines and lines starting with # are skipped; the text keeps inner spaces
        /// and may contain '='. Later duplicates of a key are ignored.
        /// </summary>
        public static Dictionary<string, string> Parse(string text)
        {
            var table = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(text)) return table;
            foreach (var raw in text.Split('\n'))
            {
                var line = raw.Trim('\r', ' ', '\t', '﻿');
                if (line.Length == 0 || line[0] == '#') continue;
                var split = line.IndexOf('=');
                if (split <= 0) continue;
                var key = line.Substring(0, split).Trim();
                var value = line.Substring(split + 1).Trim();
                if (key.Length > 0 && !table.ContainsKey(key)) table[key] = value;
            }
            return table;
        }

        static CultureInfo CultureFor(string code)
        {
            if (code == DefaultLanguage) return CultureInfo.InvariantCulture;
            try
            {
                return CultureInfo.GetCultureInfo(code);
            }
            catch (CultureNotFoundException)
            {
                return CultureInfo.InvariantCulture; // stripped culture data on a device: numbers stay readable
            }
        }
    }
}
