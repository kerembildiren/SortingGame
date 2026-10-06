# Milestones

Each milestone ends with a playtest by the user in the editor. Next one starts only after approval.

| # | Milestone | Status |
|---|---|---|
| M0 | Project skeleton: Unity 6 URP, portrait, Input System, folders, asmdefs, data layer, git, batchmode tooling | ✅ Approved 2026-10-06 |
| M1 | Core loop in one grey-box section: shelves with dashed slots, tap-to-tip containers, drag & drop (correct = snap + coin, wrong = soft return + hint), section %, basic juice/haptics. Android build at the end. | ✅ Approved 2026-10-06 (editor/Simulator; device test deferred, no device yet) |
| M2 | Broom + dirt layer, items under dirt, rare item glow, "Rare find!" moment, Collection Book data (first copy to book, duplicates sold), section 100% before/after | ✅ Approved 2026-10-06 |
| M2+ | Collection viewer (GDD 9.1.1): tap a found item in the book -> 3D view on a dimmed screen, rotate / zoom / pan, double-tap reset | ✅ Approved 2026-10-06 |
| M3 | Tools (Magnet, Hand capacity; Magnifier dropped), Category Mastery auto-fly, coin upgrades, JSON save (sorted stays sorted) | ✅ Approved 2026-10-06 (mastery playtested; Hand stack + Magnet pull not yet playtested by user) |
| M4 | Venue structure: isometric overview of 4-section warehouse, zoom transition, section locks, venue sale, 3 venues as data | ✅ Playtested 2026-10-06; follow-up changes in M4.1 / M4.2 |
| M5 | Helpers, offline progress, ad/IAP interfaces with fake providers, localisation infrastructure | ⏳ |

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
