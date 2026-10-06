# Milestones

Each milestone ends with a playtest by the user in the editor. Next one starts only after approval.

| # | Milestone | Status |
|---|---|---|
| M0 | Project skeleton: Unity 6 URP, portrait, Input System, folders, asmdefs, data layer, git, batchmode tooling | ✅ Approved 2026-10-06 |
| M1 | Core loop in one grey-box section: shelves with dashed slots, tap-to-tip containers, drag & drop (correct = snap + coin, wrong = soft return + hint), section %, basic juice/haptics. Android build at the end. | ✅ Approved 2026-10-06 (editor/Simulator; device test deferred, no device yet) |
| M2 | Broom + dirt layer, items under dirt, rare item glow, "Rare find!" moment, Collection Book data (first copy to book, duplicates sold), section 100% before/after | ⏳ |
| M3 | Tools (Magnet, Magnifier, Hand upgrade), Category Mastery auto-fly, coin upgrades, JSON save (sorted stays sorted) | ⏳ |
| M4 | Venue structure: isometric overview of 4-section warehouse, zoom transition, section locks, venue sale, 3 venues as data | ⏳ |
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
