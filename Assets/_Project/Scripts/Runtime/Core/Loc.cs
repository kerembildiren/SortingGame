using System.Collections.Generic;

namespace SortingGame.Core
{
    /// <summary>
    /// GDD 15.6: no hard-coded UI text. Every string goes through Loc.Get(key).
    /// Temporary English table; M7 swaps this for the Unity Localization package without touching call sites.
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
            ["hud.rare_find"] = "You found a Chubby!",
            ["hud.continue"] = "Continue",
            ["hud.rare_item"] = "Rare: {0}  +{1}",
            ["hud.shop"] = "Shop",
            ["hud.shop_title"] = "Shop",
            ["hud.shop_tools"] = "Tools",
            ["hud.shop_helpers"] = "Helpers",
            ["hud.helper_for_hire"] = "for hire",
            ["hud.helper_hire"] = "Hire",
            ["hud.helper_effect"] = "carries {0:0} at a time, speed {1:0.0}",
            ["hud.helper_needs_venue"] = "Opens when {0} is fully restored",
            ["hud.helper_needs_percent"] = "Opens at {0}% of {1}",
            ["hud.helper_hired"] = "{0} joins you! Helpers work in whatever room you are in.",
            ["hud.helper_available"] = "A helper is waiting in the Shop: {0}",
            ["helper.pip"] = "Pip",
            ["helper.dot"] = "Dot",
            ["hud.locked"] = "locked",
            ["hud.unlock"] = "Unlock",
            ["hud.next"] = "Next",
            ["hud.max"] = "MAX",
            ["hud.bought"] = "{0} level {1}!",
            ["hud.not_enough"] = "Not enough coins yet",
            ["hud.needs_max"] = "Needs {0} at max level",

            ["hud.auto_sort"] = "Auto Sort",
            ["hud.auto_ad"] = "AD",
            ["hud.auto_charges"] = "x{0}",
            ["hud.auto_used"] = "used",
            ["hud.auto_tag"] = "{0}  AUTO",
            ["hud.auto_title"] = "Auto Sort",
            ["hud.auto_text"] = "Pick one shelf. Its items sort themselves until this room is finished. One shelf per room.",
            ["hud.auto_left"] = "{0:N0} to go",
            ["hud.auto_watch_ad"] = "Watch an ad",
            ["hud.auto_use_charge"] = "Use a charge  (you have {0:N0})",
            ["hud.auto_get_charges"] = "Get charges",
            ["hud.auto_sort_on"] = "{0} sort themselves in this room now!",
            ["hud.auto_used_toast"] = "Auto Sort is already on in this room",
            ["hud.auto_room_done"] = "This room is already finished",
            ["hud.ad_failed"] = "The ad did not finish, nothing was used",
            ["hud.store_title"] = "Auto Sort charges",
            ["hud.store_owned"] = "You have {0:N0}",
            ["hud.store_pack"] = "{0:N0} x Auto Sort",
            ["hud.store_bought"] = "+{0:N0} Auto Sort",
            ["hud.store_test_note"] = "Test store: nothing is charged.",
            ["hud.purchase_failed"] = "Purchase did not go through",
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
            ["hud.album_title"] = "Chubby album",
            ["hud.album_progress"] = "{0} joins the album!  {1} / {2}",
            ["hud.album_complete"] = "Every Chubby found! The album is complete.",
            ["hud.venue_opened"] = "All rooms restored! {0} is now open.",
            ["hud.venue_restored"] = "Fully restored",
            ["hud.restored_tag"] = "100%",
            ["hud.shiny_left"] = "Almost done! Something shiny is still waiting to be picked up.",
            ["hud.unlock_rule"] = "Opens when the other rooms reach {0}% on average. Or open it now:",
            ["hud.unlock_coins_only"] = "Open it now:",
            ["hud.unlock_now"] = "Unlock  {0:N0} coins",
            ["hud.room_unlocked"] = "{0} is open!",
            ["hud.view_hint"] = "Drag to turn  ·  Pinch to zoom  ·  Two fingers to move  ·  Double-tap to reset",

            // Collectibles: original names only (GDD 13). One costumed Chubby per venue.
            ["collectible.captain_chubby"] = "Captain Chubby",
            ["collectible.captain_chubby.desc"] = "A tiny hero in a homemade cape. Always ready to save the day, right after a nap.",
            ["collectible.mechanic_chubby"] = "Mechanic Chubby",
            ["collectible.mechanic_chubby.desc"] = "Overalls, a smudge of oil and a lot of confidence. Fixes nothing, cheers everyone up.",
            ["collectible.night_guard_chubby"] = "Night Guard Chubby",
            ["collectible.night_guard_chubby.desc"] = "Keeps watch over the warehouse. Mostly with his eyes closed.",

            // Rare items (GDD 9.4): shelved like the rest of their category, worth a bit more.
            ["item.rare_first_issue"] = "Sealed First Issue",
            ["item.rare_golden_robot"] = "Golden Robot",
            ["item.rare_lucky_wrench"] = "Lucky Wrench",
            ["item.rare_brass_stapler"] = "Brass Stapler",
            ["item.rare_chrome_hubcap"] = "Chrome Hubcap",
            ["item.rare_message_bottle"] = "Message in a Bottle",
        };

        /// <summary>Number formatting follows the UI language, not the device region (English for now).</summary>
        public static readonly System.Globalization.CultureInfo Culture = System.Globalization.CultureInfo.InvariantCulture;

        public static string Get(string key) =>
            !string.IsNullOrEmpty(key) && English.TryGetValue(key, out var value) ? value : key;

        public static string Format(string key, params object[] args) => string.Format(Culture, Get(key), args);
    }
}
