# Milestones

Each milestone ends with a playtest by the user in the editor. Next one starts only after approval.

| # | Milestone | Status |
|---|---|---|
| M0 | Project skeleton: Unity 6 URP, portrait, Input System, folders, asmdefs, data layer, git, batchmode tooling | ✅ Approved 2026-10-06 |
| M1 | Core loop in one grey-box section: shelves with dashed slots, tap-to-tip containers, drag & drop (correct = snap + coin, wrong = soft return + hint), section %, basic juice/haptics. Android build at the end. | ✅ Approved 2026-10-06 (editor/Simulator; device test deferred, no device yet) |
| M2 | Broom + dirt layer, items under dirt, rare item glow, "Rare find!" moment, Collection Book data (first copy to book, duplicates sold), section 100% before/after | ✅ Approved 2026-10-06 |
| M2+ | Collection viewer (GDD 9.1.1): tap a found item in the book -> 3D view on a dimmed screen, rotate / zoom / pan, double-tap reset | ✅ Approved 2026-10-06 |
| M3 | Tools (Magnet, Hand capacity; Magnifier dropped), Category Mastery auto-fly, coin upgrades, JSON save (sorted stays sorted) | ✅ Approved 2026-10-06 (Hand stack + Magnet pull playtested and approved later the same day; mastery is replaced in M5) |
| M4 | Venue structure: isometric overview of 4-section warehouse, zoom transition, section locks, venue sale, 3 venues as data | ✅ Approved 2026-10-06 |
| M4.1 | Playtest follow-ups: section only completes when every collectible in it is picked up; a found collectible never appears again; no venue selling/buying: all rooms 100% -> next venue opens for free; shelf-complete camera showcase (zoom + top-to-bottom pan); book flies in and opens when a venue's collection is complete | ✅ Approved 2026-10-06 |
| M4.2 | Big rooms: ~60 items (Comic Box), ~200 (Garage), ~300 per Warehouse room; horizontal camera pan in wide sections; item variety grows along the ladder | ✅ Approved 2026-10-06 (length and pace are good) |
| M4.3 | Pre-M5 requests: stay in a finished room (banner choice); tap a full shelf for a close-up camera (pan, zoom, back button) | ✅ Approved 2026-10-06 (known issue: mouse-wheel zoom in the close-up does not work in the editor; check together with pinch on a device) |
| M5 | **New progression rules** (GDD 0.2). (a) Category Mastery removed; **Auto Sort boost** instead: pick one shelf per room, its category sorts itself until the room is finished (floor items, later spills, swept-out items); obtained by rewarded ad or IAP charges, never coins; ad + IAP interfaces with fake providers. (b) **Magnet gate**: needs max Hand level, clearly higher price, Shop shows why it is locked. (c) **Collectibles = Chubby only**: one per venue, book becomes a single Chubby album; the six former gold rares become blue-glowing **rare items** that go on their category shelf for a bit more coins. Save migration for all of it. | ✅ Approved 2026-10-06 |
| M6 | **Helpers**: Shop purchase with coins, slots unlock with venue progress (at most 2 in the MVP); spawn in the room the player is in; pick loose common items one by one and shelve them, including items that appear later (box spills, swept-out); never touch boxes, dirt, rare items, Chubby; upgrades (carry capacity, speed); no offline work; after the room is finished they wander and can be petted (hearts, cute sounds) | ✅ Implemented 2026-10-06 (tests green; user playtest pending) |
| M7 | Localisation infrastructure, economy / balance pass over tools, helpers and rare items, on-device test (when a device is available) | ⏳ |

## Decisions taken during development
| Date | Decision | Why |
|---|---|---|
| 2026-10-06 | Placeholders: primitives with per-category shape + colour | Tests "recognisable at a glance" (GDD 8.2) without art cost |
| 2026-10-06 | Item count of a section = sum of shelf slots; generator distributes items into containers/floor | Shelves always fill exactly, section can always reach 100% |
| 2026-10-06 | Scatter uses light physics, bodies go kinematic once settled | GDD 15.3 |
| 2026-10-06 | Empty containers shrink away ~1 s after spilling | Kaos -> düzen: an empty tipped box is just more clutter. **Validate in playtest.** |
| 2026-10-06 | Hovering a shelf while dragging gives a neutral highlight; only a wrong drop flashes the correct shelf | Keeps sorting a small "knowledge" challenge (principle 8); showing the right shelf is the Magnifier's job (GDD 10.1) |
| 2026-10-06 | Shelves for portrait: up to 4 rows (12 slots = 4 x 3); items ~1.3x real scale | Readable + touchable items (~1 cm on a phone) |
| 2026-10-06 | Placeholder sounds are synthesised in code (`SfxPlayer`), category sets the placement sound | GDD 7.4 says feedback is not postponed; no audio assets needed yet |
| 2026-10-06 | HUD built with UI Toolkit in code + USS | Text-authorable, no scene editing needed |
| 2026-10-06 | "Play again" / Restart reshuffles the layout (new seed) | Prototype convenience only |
| 2026-10-06 | Android APK build check is part of every milestone (`tools/unity.sh android`); on-device test deferred until a device is available (before end of M3 at the latest) | No device yet; build problems still caught early |
| 2026-10-06 | Section % = items (80%) + dirt (20%); collectibles are NOT counted | GDD 5.6: finding every rare item is not required to finish |
| 2026-10-06 | Collectibles are hidden in boxes or under dirt, never lying in plain sight; tapping (any tool) finds them, they cannot be dragged | Keeps the "treasure" feeling (principle 5) |
| 2026-10-06 | Dirt auto-finishes at 93% cleaned | Nobody should hunt the last speck (principle 2) |
| 2026-10-06 | Mood brightens a little with progress (up to 35% of the clean look), full renovation at 100% | Principle 1: every action visibly improves the place |
| 2026-10-06 | 100% "before/after" = dust fades, floor/walls recolour, lights + background warm up, sparkle wave, warm music pad; no split-screen wipe yet | Cheap and clear; a real before/after wipe can come with art |
| 2026-10-06 | Rare find moment: no blur, a 3D dimmer behind the item instead | Blur needs post-processing on mobile; dim reads the same |
| 2026-10-06 | Collection Book is in-memory until M3 (save) | Save system is M3 scope |
| 2026-10-06 | Collection viewer added to M2 (user request): own camera-parented stage, own key light, game input paused while open, returns to the book on close | Builds on the rare-find stage; lighting independent of room mood |
| 2026-10-06 | ~~ Hand upgrade = faster placement + bigger grab radius; carrying several items is the Magnet's job | GDD 10.1 lists "carry more at once" for Hand, but with one finger that would duplicate Magnet~~ (superseded after playtest) |
| 2026-10-06 | ~~ Magnet = a tool: carried item pulls up to N same-category items within R; they trail behind and land one after another (wrong shelf: all go back) | GDD 10.1~~ (superseded after playtest) |
| 2026-10-06 | ~~ Magnifier = a tool: on the floor it is a lens that marks buried items (rares in gold); when carrying an item the right shelf lights up. Upgrade = lens size. No timer/cooldown | Principle 2 (no waiting); keeps the treasure hunt active~~ (superseded after playtest) |
| 2026-10-06 | Magnet starts locked, bought in the Shop (Tools panel); tapping a locked tool opens the Shop | GDD 5.3: first tool upgrades in the Garage |
| 2026-10-06 | Category Mastery: single tier, threshold 20 per category (prototype; GDD example is 100). On mastery: loose items of that category fly to the shelf; later box spills of it skip the floor. Shelf label shows progress bar, "*" when mastered | GDD 10.2 tiers stay [AÇIK] |
| 2026-10-06 | Save: local JSON (`persistentDataPath/save.json`), autosave 1.5 s after any change + on pause/quit. Stores coins, book, mastery, tool levels and the full section (shelf slots, floor poses, buried items, unopened boxes, gzip dirt mask). Opened boxes do not come back | GDD 15.4, principle 1 |
| 2026-10-06 | "Play again" keeps coins/book/tools/mastery and reshuffles the section; Settings has "Reset all progress" for testing | Prototype convenience |
| 2026-10-06 | UI numbers use invariant (English) formatting regardless of device region | UI is English (GDD 15.6); Turkish formatting comes with localisation |
| 2026-10-06 | Invisible guard in front of the shelves stops spilled items from landing on shelf boards | Items on a board looked sorted but were not counted |
| 2026-10-06 | After M3 playtest: Magnifier removed; Hand = carry capacity 1/2/3 (any kinds, pass over items to pick up); Magnet = continuous pull of same-category items within a small radius (0.35/0.45/0.55 m) while carrying, limit 2/4/6; rest over a shelf `ShelfDepositDwell` (0.3 s) to drop in the matching carried items, the rest stay in hand | User feedback |
| 2026-10-06 | Mastery also auto-sorts items revealed by sweeping and floor items at the start of a new game / after loading | User feedback (bug) |
| 2026-10-06 | M4 screens: Map (venue ladder) is a modal over the overview; Overview = isometric orthographic cutaway, one room per section (back/left walls tall, front/right cut away); Section unchanged. Back button goes one level up | GDD 6.1; fewest screens to build and test |
| 2026-10-06 | Overview rooms are stand-ins, not the real items: clutter boxes/debris shrink and shelf blocks fill with progress, colours go dirty -> clean, locked rooms are dark with a padlock, finished rooms get a plant | GDD 15.3 (no thousands of items on the overview) |
| 2026-10-06 | Zoom transition = orthographic camera tween into the room + fade to black, then the section loads; the reverse when coming back | GDD 6.1 |
| 2026-10-06 | Venues (prototype counts): Comic Box (1 room, 12 comics, free), Garage (1 room, 36 items, 100 coins), Warehouse (Office, Loading Dock, Aisle, Basement; 84 items, 300 coins). Sell values 150 / 350 / 1500. New categories: Stationery, Mugs, Tyres, Bottles. One mascot costume per venue: Captain / Mechanic / Night Guard Chubby | GDD 5.3 ladder, scaled down to be testable |
| 2026-10-06 | Basement starts locked: opens when the other rooms average 60%, or for 150 coins | GDD 5.4 |
| 2026-10-06 | Selling a venue requires every room at 100% (collectibles not required); sold venues stay on the map as "SOLD" and cannot be re-entered | GDD 5.6 |
| 2026-10-06 | Section 100% banner button is now "Back to overview"; "Restart section" (reshuffle) stays in Settings as a dev helper | Fits the venue flow |
| 2026-10-06 | Game resumes where it was left: last venue, and the section if the player was inside one | GDD 15.4 |
| 2026-10-06 | Collection Book has one page per venue with < > paging | GDD 9.1 |
| 2026-10-06 | (user) A section is complete only when every collectible in it has been picked up as well | Auto-sort could finish a room while a collectible was still lying on the floor |
| 2026-10-06 | (user) A collectible already in the book never spawns again; no duplicates, no duplicate sales | Replaying to farm duplicates was not wanted |
| 2026-10-06 | (user) Cleaned rooms stay clean and are not replayable ("Restart section" stays as a dev tool only) | GDD principle 1 |
| 2026-10-06 | (user) No venue sale / purchase: when every room of a venue is 100%, the next venue opens for free. Coins are for tools and room unlocks for now; economy redesign later | Progress through rooms, not through a coin wall |
| 2026-10-06 | (user) Room sizes "Big": ~60 / ~200 / ~300 per Warehouse room | Rooms should take much longer to clean |
| 2026-10-06 | Big rooms are one long strip of bookcases (up to 5 rows) along the back wall; the camera shows a 4.6 m window and pans. Pan by dragging empty floor (Hand/Magnet), two fingers or right mouse; edges auto-scroll while carrying/sweeping | GDD 6.2 "large sections pan sideways"; keeps items readable |
| 2026-10-06 | Category variety grows along the ladder: Comic Box 2 categories, Garage 4, Warehouse rooms 5-6. Mastery threshold raised to 50 to match bigger rooms | User request (more different items as you progress) |
| 2026-10-06 | Changed content invalidates the old section save (item count mismatch -> regenerate) | Content rebalances must not leave rooms in a broken state |
| 2026-10-06 | (user) Finishing a room offers "Stay and look around" next to "Back to overview" | Player should be free to enjoy the finished room |
| 2026-10-06 | (user) Tapping a full shelf (any time, any tool) opens a close-up: camera in front of the shelf, slide along it, zoom between ~0.8 m and 2.6 m of shelf width, Back returns to the room view. Pan stops at the shelf edges (no wandering to the next shelf); room input is paused meanwhile | Look at the sorted result up close if the showcase was skipped or after finishing |
| 2026-10-06 | (user) Permanent Category Mastery is removed. Auto sort becomes a boost: rewarded ad or real money (IAP packs), never coins; one shelf per room; that shelf's category sorts itself until the room is finished, including items that fall or are found later | Permanent auto sort made the game too easy after a while |
| 2026-10-06 | (user) Helpers replace mastery as the lasting progression: bought in the Shop, slots tied to venue progress (not a new helper at every venue), slow, one item at a time, upgrades such as carrying several items; helpers and upgrades are not cheap | Without auto sort progress would be too slow; with cheap helpers the game would be over too fast |
| 2026-10-06 | (user) Helpers must also pick up items that appear after they started (box spills, swept-out items) | The first mastery version missed items that appeared later; same bug must not come back |
| 2026-10-06 | (user) No AFK / offline progress at all. Helpers work only in the room the player is in; no assignment screen, they spawn on entering the room | The player should always be encouraged to play |
| 2026-10-06 | (user) Helpers are small, cute, bubbly / squishy creatures. In a finished room they wander around and react to taps like pets (hearts, cute sounds) | Charm; a reason to stay in a finished room |
| 2026-10-06 | (user) Magnet: requires max Hand level and costs clearly more (it was 40 coins, cheaper than Hand level 2 at 60) | Skipping Hand and buying Magnet first was the best move |
| 2026-10-06 | (user) Only themed Chubby figures are collectibles, one per venue; the book is a single Chubby album. Some rooms have a few blue-glowing rare items: a special member of a category (e.g. a sealed first issue), shelved like a normal item, worth a bit more | Finding a collectible should be rarer and more valuable |
| 2026-10-06 | (user) Locks follow venue progress; there is no separate player level / XP system | "Level" in the user's wording means how far along the venue ladder the player is |
| 2026-10-06 | Defaults chosen while writing this down, to be confirmed in playtests: helpers earn normal coins for what they shelve; Auto Sort leaves blue rare items to the player; set bonuses are open again (single album has no pages) | Not specified by the user; GDD marks them [VARSAYILAN] / [AÇIK] |
| 2026-10-06 | Auto Sort is started from a fourth tool bar button: a card lists the room's unfinished shelves, the player picks one and then pays (ad or charge). Charges are bought in a small store card reached from there | Thumb reach (GDD 6.3); picking in a list needs no new world gesture and is easy to test |
| 2026-10-06 | Besides the spill / sweep hooks, a 0.4 s tick sorts any loose item of the boosted category (dropped by the player, returned from a wrong shelf) | User: items that appear later must never be missed |
| 2026-10-06 | The fake ad finishes at once and the fake store charges nothing (the store card says so); pack sizes 1 / 5 / 15 with stand-in prices | Real ad network and store are still [AÇIK] (GDD 15.5) |
| 2026-10-06 | Magnet prices 500 / 900 / 1600 (Hand costs 240 in total, a Warehouse room pays about 550) | First guess for "clearly more expensive"; real balance pass is M7 |
| 2026-10-06 | Rare items: 5 to 12 coins (commons pay 1 to 3). Comic Box 1, Garage 2, Office 1, Loading Dock 1, Basement 1, Aisle none. They lie around like common items (floor, boxes, under dirt) | "A bit more" money; not every room needs one |
| 2026-10-06 | Every Chubby find opens the album (book flies out, shows n / 3) | One per venue makes each find the big moment; the old trigger was "page complete" |
| 2026-10-06 | Old saves are kept: rare items got new ids, so the former collectibles simply drop out of saved rooms; a room saved before M5 has no rare items until it is regenerated | Players (and the user's own test save) keep their progress |
| 2026-10-06 | Two helpers in the MVP: Pip (slot opens when the Garage is fully restored) and Dot (slot opens at 50% of the Warehouse). Each has three levels: hire, then two upgrades that raise speed and items per trip together | User: slots follow venue progress, not every venue brings one; one ladder per helper keeps the Shop short |
| 2026-10-06 | Helper prices: Pip 400 / 700 / 1200, Dot 900 / 1300 / 2000 coins; speed 0.9 / 1.1 / 1.3 m/s, 1 / 2 / 3 items per trip (about 8 items a minute at level 1) | User: slow, and not cheap; first guess, balance pass is M7 |
| 2026-10-06 | A helper only takes items that lie still; it re-plans when the player (or Auto Sort) takes its target first, and the player cannot take an item out of its arms | The player always has priority on the floor (GDD 10.4) |
| 2026-10-06 | Helpers walk in straight lines and only steer around closed boxes; they hop over loose items | No pathfinding needed in a room without walls; cheap and readable |
| 2026-10-06 | Petting works only in a finished room; during play a tap near a helper still goes to the item or box under the finger | User asked for it "once everything is clean"; avoids missed taps while sorting |
| 2026-10-06 | The shelf showcase waits for the finger to lift before it takes the camera | Helpers and Auto Sort can fill a shelf while the player is dragging |
| 2026-10-06 | Milestone order: M5 reworks existing systems (mastery -> boost, Magnet gate, collectibles), M6 adds helpers, M7 is localisation + balance + device test. Offline progress is dropped from the plan | Between M5 and M6 the big rooms are expected to feel slow (no mastery, no helpers yet); judge pacing only after M6 |
