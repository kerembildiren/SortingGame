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
            ["tool.hand.effect"] = "carry {0:0} items at once (any kind)",
            ["tool.broom.effect"] = "brush {0:0.##} m wide",
            ["tool.magnet.effect"] = "pulls up to {1:0} same items within {0:0.##} m",

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
            ["hud.book"] = "Book",
            ["hud.collection_book"] = "Collection Book",
            ["hud.rare_find"] = "Rare find!",
            ["hud.continue"] = "Continue",
            ["hud.duplicate_sold"] = "Duplicate {0} sold +{1}",
            ["hud.shop"] = "Shop",
            ["hud.shop_title"] = "Tools",
            ["hud.locked"] = "locked",
            ["hud.unlock"] = "Unlock",
            ["hud.next"] = "Next",
            ["hud.max"] = "MAX",
            ["hud.bought"] = "{0} level {1}!",
            ["hud.not_enough"] = "Not enough coins yet",
            ["hud.mastered"] = "{0} mastered! They sort themselves now.",
            ["hud.reset_progress"] = "Reset all progress",
            ["hud.view_hint"] = "Drag to turn  ·  Pinch to zoom  ·  Two fingers to move  ·  Double-tap to reset",

            // Collectibles: original names only (GDD 13).
            ["collectible.captain_chubby"] = "Captain Chubby",
            ["collectible.captain_chubby.desc"] = "A tiny hero in a homemade cape. Always ready to save the day, right after a nap.",
            ["collectible.golden_robot"] = "Golden Robot",
            ["collectible.golden_robot.desc"] = "A wind-up tin robot with a golden shine. Still walks, a little sideways.",
            ["collectible.first_issue"] = "First Issue",
            ["collectible.first_issue.desc"] = "The very first issue of 'Moonlight Mice'. The corners are a bit chewed.",
            ["collectible.lucky_wrench"] = "Lucky Wrench",
            ["collectible.lucky_wrench.desc"] = "Someone engraved a tiny star on it. Fixes bolts, and maybe luck too.",
        };

        /// <summary>Number formatting follows the UI language, not the device region (English for now).</summary>
        public static readonly System.Globalization.CultureInfo Culture = System.Globalization.CultureInfo.InvariantCulture;

        public static string Get(string key) =>
            !string.IsNullOrEmpty(key) && English.TryGetValue(key, out var value) ? value : key;

        public static string Format(string key, params object[] args) => string.Format(Culture, Get(key), args);
    }
}
