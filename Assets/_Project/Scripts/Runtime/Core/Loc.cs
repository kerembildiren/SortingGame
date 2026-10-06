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
            ["category.stationery"] = "Stationery",
            ["category.mugs"] = "Mugs",
            ["category.tyres"] = "Tyres",
            ["category.bottles"] = "Bottles",

            ["section.comic_box"] = "Comic Box",
            ["section.garage"] = "Garage",
            ["section.wh_office"] = "Office",
            ["section.wh_dock"] = "Loading Dock",
            ["section.wh_aisle"] = "Aisle",
            ["section.wh_basement"] = "Basement",
            ["venue.comic_box"] = "Comic Box",
            ["venue.garage"] = "Garage",
            ["venue.warehouse"] = "Warehouse",
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
            ["hud.back_to_overview"] = "Back to overview",
            ["hud.stay_in_room"] = "Stay and look around",
            ["hud.inspect_tip"] = "Tap a full shelf to see it up close",
            ["hud.inspect_hint"] = "Drag to look around  ·  Pinch to zoom  ·  Double-tap to reset",
            ["hud.back"] = "Back",
            ["hud.items_count"] = "{0:N0} / {1:N0} items",
            ["hud.locked_room"] = "Locked",
            ["hud.map_title"] = "Places",
            ["hud.venue_rooms"] = "Rooms: {0}",
            ["hud.venue_locked"] = "Restore {0} first",
            ["hud.open"] = "Open",
            ["hud.go_to_venue"] = "Next place: {0}",
            ["hud.collection_complete"] = "{0} collection complete!",
            ["hud.venue_opened"] = "All rooms restored! {0} is now open.",
            ["hud.venue_restored"] = "Fully restored",
            ["hud.restored_tag"] = "100%",
            ["hud.shiny_left"] = "Almost done! Something shiny is still waiting to be picked up.",
            ["hud.unlock_rule"] = "Opens when the other rooms reach {0}% on average. Or open it now:",
            ["hud.unlock_coins_only"] = "Open it now:",
            ["hud.unlock_now"] = "Unlock  {0:N0} coins",
            ["hud.room_unlocked"] = "{0} is open!",
            ["hud.view_hint"] = "Drag to turn  ·  Pinch to zoom  ·  Two fingers to move  ·  Double-tap to reset",

            // Collectibles: original names only (GDD 13).
            ["collectible.captain_chubby"] = "Captain Chubby",
            ["collectible.captain_chubby.desc"] = "A tiny hero in a homemade cape. Always ready to save the day, right after a nap.",
            ["collectible.golden_robot"] = "Golden Robot",
            ["collectible.golden_robot.desc"] = "A wind-up tin robot with a golden shine. Still walks, a little sideways.",
            ["collectible.first_issue"] = "First Issue",
            ["collectible.first_issue.desc"] = "The very first issue of 'Moonlight Mice'. The corners are a bit chewed.",
            ["collectible.lucky_wrench"] = "Lucky Wrench",
            ["collectible.mechanic_chubby"] = "Mechanic Chubby",
            ["collectible.mechanic_chubby.desc"] = "Overalls, a smudge of oil and a lot of confidence. Fixes nothing, cheers everyone up.",
            ["collectible.night_guard_chubby"] = "Night Guard Chubby",
            ["collectible.night_guard_chubby.desc"] = "Keeps watch over the warehouse. Mostly with his eyes closed.",
            ["collectible.brass_stapler"] = "Brass Stapler",
            ["collectible.brass_stapler.desc"] = "Heavy, shiny and older than the building. Still staples like new.",
            ["collectible.chrome_hubcap"] = "Chrome Hubcap",
            ["collectible.chrome_hubcap.desc"] = "So polished you can check your hair in it.",
            ["collectible.message_bottle"] = "Message in a Bottle",
            ["collectible.message_bottle.desc"] = "The note inside says: 'Please tidy the basement.' Done!",
            ["collectible.lucky_wrench.desc"] = "Someone engraved a tiny star on it. Fixes bolts, and maybe luck too.",
        };

        /// <summary>Number formatting follows the UI language, not the device region (English for now).</summary>
        public static readonly System.Globalization.CultureInfo Culture = System.Globalization.CultureInfo.InvariantCulture;

        public static string Get(string key) =>
            !string.IsNullOrEmpty(key) && English.TryGetValue(key, out var value) ? value : key;

        public static string Format(string key, params object[] args) => string.Format(Culture, Get(key), args);
    }
}
