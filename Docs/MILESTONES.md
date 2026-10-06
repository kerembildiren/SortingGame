# Milestones

Each milestone ends with a playtest by the user in the editor. Next one starts only after approval.

| # | Milestone | Status |
|---|---|---|
| M0 | Project skeleton: Unity 6 URP, portrait, Input System, folders, asmdefs, data layer, git, batchmode tooling | ✅ Done, waiting for check |
| M1 | Core loop in one grey-box section: shelves with dashed slots, tap-to-tip containers, drag & drop (correct = snap + coin, wrong = soft return + hint), section %, basic juice/haptics. Android build at the end. | ⏳ |
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
