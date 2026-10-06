using System.Collections.Generic;

namespace SortingGame.Core
{
    /// <summary>
    /// GDD 15.6: no hard-coded UI text. Every string goes through Loc.Get(key).
    /// Temporary English table; M5 swaps this for the Unity Localization package without touching call sites.
    /// </summary>
    public static class Loc
    {
        static readonly Dictionary<string, string> English = new()
        {
            ["category.comics"] = "Comics",
            ["category.toys"] = "Toys",
            ["category.tools"] = "Tools",
            ["section.garage"] = "Garage",
            ["venue.garage"] = "Garage",
            ["container.cardboard_box"] = "Box",

            ["tool.hand"] = "Hand",
            ["tool.broom"] = "Broom",
            ["tool.magnet"] = "Magnet",
            ["tool.magnifier"] = "Magnifier",
            ["tool.coming_soon"] = "Coming soon",

            ["hud.section_complete"] = "Section complete!",
            ["hud.shelf_full"] = "{0} shelf full!",
            ["hud.play_again"] = "Play again",
            ["hud.overview_soon"] = "Overview arrives in M4",
            ["hud.settings"] = "Settings",
            ["hud.sound"] = "Sound",
            ["hud.haptics"] = "Haptics",
            ["hud.restart"] = "Restart section",
            ["hud.close"] = "Close",
            ["hud.on"] = "On",
            ["hud.off"] = "Off",
        };

        public static string Get(string key) =>
            !string.IsNullOrEmpty(key) && English.TryGetValue(key, out var value) ? value : key;

        public static string Format(string key, params object[] args) => string.Format(Get(key), args);
    }
}
