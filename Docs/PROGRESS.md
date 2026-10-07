# Progress & Handoff

Living notes so any session (any machine, any Claude account) can pick up where the last one stopped.
Update this file at the end of every work chunk, before committing. Newest entry on top in the log.

## Current state
- **Active milestone:** none. M0 to M7 are implemented, playtested in the editor and approved (M7 on 2026-10-07). Last full run: EditMode 69/69, PlayMode 18/18, Android development APK builds (41 MB).
- **Open item: on-device test.** There is no Android device yet. When one is there: `tools/unity.sh android`, install `Builds/Android/SortingGame.apk`, and check
  - frame rate in a 300-item room (Warehouse), with both helpers and Auto Sort running,
  - touch sizes: picking small items, the tool bar, the Shop and Auto Sort rows,
  - pinch and two-finger gestures in the shelf close-up and the collection viewer (mouse-wheel zoom does not work in the editor; never checked on touch),
  - haptics, sound levels,
  - Turkish letters with the device font (fine in the editor), text that does not fit,
  - safe area on a notched screen.
- **Next concrete step:** nothing is planned. Ask the user what comes next. Candidates from the GDD: real models and sounds instead of placeholders (12, 14), more venues (5.3), real ad and store SDKs behind `IAdProvider` / `IStoreProvider` (15.5), final Turkish translation, open questions in GDD 18 (set bonus, premium currency, cloud save).
- **Blocking / waiting on user:** the next direction; an Android device for the device test.
- **Pushed:** `origin/main` has everything up to M7 (2026-10-07). Still: push only when the user asks.
- **User intent:** mechanics first, limits/numbers later. Helpers and upgrades should not be cheap; the game should stay playable for a long time.

## M7 architecture (quick map)
- **Texts:** `Assets/_Project/Localization/<code>.txt`, one "key = text" line each; `en.txt` is the reference and the fallback. Listed in `GameDatabase.Languages` (filled by `ContentBuilder`). `Loc.Load` + `Loc.SetLanguage` in `GameBootstrap.Awake`; `Loc.Get` / `Loc.Format` everywhere else; `Loc.Culture` for numbers.
- The language is a device setting (`PlayerPrefs`, key `language`), default English, not part of the save. Settings has a button that cycles the languages; `GameBootstrap.ChangeLanguage` saves and reloads the scene, and `GameFlow.Resume` brings the player back to the same place.
- `Loc.OverrideLanguage` pins the language in tests (`TestGame.UseTestSave`), so they never depend on what was picked on the machine.
- `LocTests` keep the tables honest: same keys and placeholders in every language, every key used in code (`"hud.x"` literals in `Scripts/Runtime`) and every content name exists, no dead keys. **Adding a text = add it to every `.txt`; adding a language = a new file + a line in `ContentBuilder.Build`, then `tools/unity.sh rebuild`.**
- `tr.txt` is a draft translation that proves the infrastructure; the user may reword it freely.
- **Economy:** `EconomyModel` (pure) computes room income, Shop totals and the reference player's purchase timeline from the content. `EconomyReport` writes `Docs/ECONOMY.md` (`tools/unity.sh economy`); regenerate after price or content changes. `EconomyTests` guard the pacing goals listed in GDD 11.5.
- Helper prices after the pass: Pip 400 / 800 / 1300, Dot 750 / 1200 / 1900.

## M6 architecture (quick map)
- Data: `HelperDefinition` (colours, `RequiredVenue` + `RequiredVenuePercent` for the Shop slot, `Levels` = cost / speed / capacity; level 1 cost = hire price), listed in `GameDatabase.Helpers`. Content: Pip (slot: Garage 100%) and Dot (slot: Warehouse 50%).
- Rules (pure): `HelperProgress` (levels, `TryUpgrade(helper, wallet, slotOpen)`), `VenueProgress.HelperSlotOpen` (100 = every room finished, lower = share of the venue's items shelved). `GameContext.HelperSlotOpen` reads `GameContext.Save`. Saved in `SaveData.Helpers`; `AnnouncedHelpers` remembers which slots were announced (`GameFlow.ShowOverviewNow`).
- Room: `HelperCrew` (created by `GameBootstrap`, `Sync()` after a section is built and after hiring) spawns one `HelperView` per hired helper under `SectionController.Root`, so they leave with the room. No assignment, nothing on the overview, nothing offline.
- `HelperView` is a small state machine: Idle -> ToItem -> Picking -> (more items while capacity and `FeelConfig.HelperChainRadius` allow) -> ToShelf -> Placing -> Idle; Wandering when there is nothing to do. It asks the room every time (`SectionController.FindHelperTarget`: resting, common, not auto-sorted, not in `HelperCrew.Taken`), so late items are found like any other. Carried items use `ItemView.BeginCarry` (state `Dragging`, saved at their floor pose). `HelperStandPoint` / `HelperPlace` put them on the shelf; `OnLanded(byHelper: true)` pays the same coins with quieter feedback.
- Never touched: boxes, dirt, rare items, Chubby. Movement is a straight walk with a little steering around closed boxes; only a trigger collider (it must not push items).
- Petting: `HelperView.Pet()` (hearts via `Fx.Hearts`, `Sfx.HelperChirp`). `DragController.HelperAt` + tap, only when the room is complete.
- Shop: one list, headers "Tools" and "Helpers" (`SectionHud.RefreshShop`, `BuyHelper`).
- `ShelfShowcase` now waits until the finger is up before it takes the camera (a helper can fill a shelf while the player is dragging).
- Tests: time is sped up with `Time.timeScale` in `HelperTests`; reset in `TearDown`.

## M5 architecture (quick map)
- **Auto Sort boost** replaces Category Mastery (`CategoryMastery` is gone). `AutoSortBoost` (pure) only keeps the charges bought with real money. The room keeps the rest: `SectionController.AutoSortCategory`, `CanStartAutoSort` (one shelf per room, not after 100%), `StartAutoSort(shelf)`. Saved as `SectionSave.AutoSortCategoryId`, charges as `SaveData.AutoSortCharges`.
- Items of the boosted category fly from three places: the direct hooks (`SpillFromContainer`, `Reveal`) and a tick in `SectionController.Update` every 0.4 s that catches whatever is loose (dropped by the player, back from a wrong shelf). Only `ItemRarity.Common`; rare items and Chubby are never touched. **M6 helpers should use the same "scan what is loose now" idea, not one-off events.**
- Paying: `IAdProvider` / `IStoreProvider` in `Monetisation.cs`, `FakeAdProvider` / `FakeStoreProvider` in `GameContext.Ads` / `.Store` (instant result; tests flip `NextResult`). Never coins.
- HUD: fourth tool bar button (`tool--boost`) -> `OpenBoost` card with one row per unfinished shelf -> `PickBoostShelf` -> `BoostWithAd` / `BoostWithCharge`; `OpenStore` lists `BalanceConfig.AutoSortPacks` (`BuyPack`). The boosted shelf's label gets "AUTO".
- **Magnet gate:** `ToolDefinition.RequiresMaxed` (Magnet -> Hand), `ToolProgress.MeetsRequirement` / `CanBuy`; the Shop row shows "Needs Hand at max level". Prices 500 / 900 / 1600 (were 40 / 120 / 300).
- **Collectibles = Chubby only:** `ItemDefinition.IsCollectible` is true only for `ItemRarity.Mascot`. One per venue (`VenueDefinition.CollectionPage` has one entry; rooms: comic_box, garage, wh_aisle). The book is one album (`GameDatabase.Album`, `VenueOf`); every find opens it (`SectionHud.PlayAlbumCelebration`). No duplicates any more (`CollectionBook.Register` returns bool).
- **Rare items:** `ItemRarity.Rare` + `IsRare`, plain `ItemDefinition` with a category and `CoinValueOverride`, listed in `SectionDefinition.RareItems`. The generator gives each one slot of its category shelf (total stays = shelf slots). Blue glow (`SectionVisuals.RareItemGlowColor`), dragged like any item (Hand stack and Magnet pick them up too), small celebration on landing (`Sfx.RareItemPlaced`).
- `SectionController.CommonItems` was renamed `SortableItems` (common + rare).
- **Saves:** version 3. Older saves load as they are: unknown fields (`Mastery`) are ignored, the old gold rares have new ids (`rare_*`) so their entries in section saves are skipped on restore, and the book only keeps ids that are in the album.
- Content changes need `tools/unity.sh rebuild` (overwrites sample assets from `ContentBuilder`); `setup` only creates what is missing.

## M4.2 plan + task list (tick as done)
- Room = one long strip: bookcases along the back wall, floor width computed from shelf widths. Camera shows a fixed-width window (`FeelConfig.SectionViewWidth`) and pans sideways.
- Pan: one-finger drag on empty floor (Hand/Magnet), two-finger drag (any tool), right-mouse drag (editor), auto-scroll at screen edges while carrying or sweeping. Flick inertia.
- Big bookcases: up to 5 rows; walls taller. Container capacity 15; box count derived from item count.
- Content: Comic Box 60 (comics 40 + toys 20), Garage 200 (4 categories x 50), Warehouse rooms 300 each (5-6 categories x 50-60). More variants per new category. Mastery threshold 50.
- Dirt layer split into tiles so sweeping only re-uploads the touched tile (performance on long floors).
- Overview rooms clamp their drawn size so the building stays readable.
- [x] CameraFitter window + pan range + pan API; SectionController `RoomBounds` (full) vs `ViewBounds` (window)
- [x] Pan input + edge auto-scroll in DragController
- [x] Bigger bookcases (5 rows), taller walls
- [x] Tiled DirtLayerView
- [x] ContentBuilder: new sizes, categories per room, variants, auto box count, mastery 50
- [x] Overview RoomView size clamp
- [x] Tests (sweep RoomBounds, pan test) + screenshots + docs

## M4.3 architecture (quick map)
- Section banner has two buttons: "Back to overview" and "Stay and look around" (`SectionHud.StayInRoom`). Staying hides the banner; the top bar "<" leaves later.
- `DragController.FullShelfAt` (solid colliders only, must be in front of the floor) + `FullShelfTapped` event on a tap (any tool; Broom does not sweep when the press starts on a full shelf).
- `ShelfInspector` (camera component, like `ShelfShowcase`): `Open(shelf)` saves the room pose, disables `CameraFitter` + room input, flies in; one finger pans, pinch / wheel zooms, double tap resets, Escape / Back flies home. `StopNow()` on leaving the section. State is the pure `ShelfInspectView` (focus on the shelf face + distance, limits, flick glide; EditMode tested).
- HUD: `ShowInspect` / `HideInspect` (inspect layer: shelf name, hint, Back button; other HUD hidden via moment mode). Tunables: `FeelConfig.Inspect*`.

## M4 architecture (quick map)
- `GameFlow` (Core): screen state Overview/Section, transitions, buy/sell venue, unlock rooms, `Resume()` on start. Test helpers: `OpenSectionImmediately`, `ShowOverviewImmediately`.
- `VenueProgress` (pure): section/venue status from `SaveData`, ladder states, `CanSell`, auto-unlock rule.
- `OverviewController` + `RoomView` (namespace `SortingGame.Overview`): isometric building at world offset (500,0,0), orthographic camera, room tap -> `RoomTapped`, `Zoom()`, `Refit()` on resize.
- `SectionHud` has two modes (`Mode.Section` / `Mode.Overview`), map modal, unlock card, fade overlay, book paging.
- `SaveData` v2: `CurrentVenueId`, `CurrentSectionId`, `Venues` (owned/sold), `UnlockedSections`; `SectionSave` keeps Placed/Total/Fraction for the overview.
- Content: `ContentBuilder` makes 3 venues / 6 sections / 7 categories / 9 collectibles.

## M1 architecture (quick map)
- `GameBootstrap` (scene entry) creates: `SfxPlayer`, `Wallet`, `SectionController`, `SectionHud` (UI Toolkit), `DragController` + `CameraFitter` on the main camera.
- `SectionController.Build` turns a `SectionDefinition` into a room at runtime: `SectionLayoutGenerator` (pure, seeded) decides contents; `ShelfView` / `ContainerView` / `ItemView` are the views; `PlaceholderFactory` makes primitives.
- Rules live in `SectionController.TryPlace` (correct = `FlyToSlot` + coins, wrong = `ReturnToPickup` + `FlashHint`). HUD listens to its events.
- Tunables: `FeelConfig` (camera, drag, timings), `SectionVisuals` (colours, mood), `BalanceConfig` (economy). Text goes through `Loc.Get`.
- Sample content is created by `ContentBuilder` (menu `Sorting Game/Setup/...`). Main scene is generated by `ProjectSetup.CreateMainScene`; do not hand-edit it.
- M2 additions: `DirtMask` (pure, testable) + `DirtLayerView` (texture on the floor); `SectionController.Sweep` brushes and reveals `ItemState.Buried` items; `ToolType` Hand/Broom in `DragController`; collectibles get `RareGlow`, are tapped (`SectionController.FindCollectible`) -> `CollectionBook.Register` -> `RareFindPresenter` (3D stage parented to the camera) + HUD card; `PlayRenovation` at 100%.
- Viewer: `CollectionViewer` (stage under the camera, dim + halo + own point light) reads input itself; `OrbitView` holds yaw/pitch/zoom/pan/inertia/idle spin. HUD `OpenViewer`/`CloseViewer`.
- M3: `GameContext` bundles Database/Visuals/Wallet/Book/Mastery/Tools and is passed to Section/Drag/HUD. Saves: `SaveSystem.FileName` (tests switch it). Tool stats: `ctx.ToolStats(ToolType)`; meaning of Primary/Secondary documented on `ToolDefinition`.
- Gotcha: kinematic rigidbodies must have interpolation OFF while tweened/parented, or the rendered pose lags (`ItemView.SetPhysics` handles it).
- Tests: EditMode (logic) + PlayMode `CoreLoopTests` (whole loop on Main scene, saves screenshots to `Logs/batch/*.png`). `WaitForEndOfFrame` hangs in batchmode, never use it in tests.

## Environment notes
- Work PC: `C:\Users\keremb\Personal\SortingGame`, Unity 6000.6.3f1 via Unity Hub.
- 2026-10-06: the project moves to the user's **personal GitHub repo** and continues on their personal PC with their personal Claude account. Do not push this project from a company account.
- Personal PC (active since 2026-10-06): `C:\Users\4\Desktop\Claude_Projects\Mobile Games\SortingGame`, cloned from `github.com/kerembildiren/SortingGame` (`origin`). Unity 6000.6.3f1 + Android module, graphify 0.9.77 (the work PC had 0.9.73, so `graphify-out/cache` was rebuilt). Still commit locally only; push when the user asks.

### Continuing on a new machine (checklist)
1. Install Unity Hub + **Unity 6000.6.3f1** with the **Android Build Support** module (for `tools/unity.sh android`).
2. `git clone` the personal repo. Open the folder in Unity Hub ("Add project from disk"); the first open rebuilds `Library/` (takes a while).
3. Data and the Main scene are versioned. If anything looks missing, run menu `Sorting Game/Setup/Run Full Setup` (or `tools/unity.sh setup` with the editor closed).
4. `tools/unity.sh` expects Unity at `C:/Program Files/Unity/Hub/Editor/6000.6.3f1/Editor/Unity.exe`; elsewhere set the `UNITY` environment variable. Needs Git Bash on Windows.
5. Check: `tools/unity.sh test` (EditMode 48/48) and `tools/unity.sh playtest` (PlayMode 12/12) pass.
6. graphify (used by the session routine): install graphify, then the setup line below.
7. Claude Code: start in the project root; `CLAUDE.md` tells Claude to read this file first. Local Claude memory from the work PC does not travel; everything needed is in `CLAUDE.md`, `Docs/` and here. User preferences: talk in Turkish, intermediate Unity level (explain key points, no basics), milestone by milestone with a playtest before the next one, commit locally after each chunk, push only when asked.
- graphify setup on a new machine (once, in project root): `graphify hook install` (git hooks are not versioned) and `graphify claude install` (writes machine-local `.claude/settings.json`; rename it to `.claude/settings.local.json`, which is git-ignored, and revert any duplicate graphify section it adds to CLAUDE.md). `graphify-out/` itself is versioned.

## Session log
### 2026-10-07 — M7 approved
- User playtested M7 in the editor: approved as it is. Commits up to M7 were pushed to `origin/main` at the user's request.
- All planned milestones are done; the device test is the only open item.

### 2026-10-07 — M6 approved; M7 implemented (localisation, balance)
- User playtested M6: approved as it is.
- String tables in text files, language switch in Settings, Turkish draft table. Own small system instead of the Unity Localization package (decision in MILESTONES).
- Economy model + generated report + guard tests; helper prices adjusted so the reference player meets both helpers inside the MVP content.
- Android development APK build check: builds (41 MB, 0 errors). No device yet, so the on-device test stays open.
- EditMode 69/69, PlayMode 18/18.

### 2026-10-06 — M5 approved; M6 implemented (helpers)
- User playtested M5: approved as it is.
- Helpers: data, rules, room behaviour, petting, Shop rows, slot announcement. Placeholder look: mint / peach blob with big eyes, cheeks, feet and a sprout.
- Tests: `HelperProgressTests`, helper slot cases in `VenueProgressTests`, PlayMode `HelperTests` (works alone and finds late items, leaves boxes / dirt / rare / Chubby, level 3 carries several, two helpers, stroll + petting + reload, slots by venue progress). EditMode 57/57, PlayMode 17/17.

### 2026-10-06 — M5 implemented (new progression rules)
- Category Mastery removed; Auto Sort boost (ad or store charge, one shelf per room) with fake providers, HUD card + store.
- Magnet gated behind max Hand and repriced. Collectibles are the three Chubby figures; six former gold rares are blue rare items on shelves. Book is a single album.
- Tests: `ProgressionTests` (tool gate, charges, old save), `SectionLayoutGeneratorTests` (rare slot), `CollectionBookTests` rewritten, PlayMode `AutoSort_*` (ad, late items, dropped item, store charge + reload), Magnet gate in `Magnet_FollowersLandOnTheSameShelf`, rare items in `CoreLoopTests`. EditMode 51/51, PlayMode 13/13.
- `tools/unity.sh rebuild` added.

### 2026-10-06 — M4.x approved; progression redesign written down (no code)
- User playtested on the personal PC: M4.1, M4.2, M4.3 approved, Hand stack + Magnet pull approved. Glow fix confirmed.
- User asked for a change of direction before M5; clarified in six questions, then GDD (now 0.2), MILESTONES (new M5 / M6 / M7, decisions) and this file were updated. No game code changed for it.

### 2026-10-06 — Personal PC set up; glow fix
- Personal PC (`C:\Users\4\Desktop\Claude_Projects\Mobile Games\SortingGame`): Unity 6000.6.3f1 + Android module and graphify installed, EditMode 48/48 and PlayMode 12/12 pass here.
- Bug (user playtest): from the second Play press on, halos / rays / sparkles showed as solid yellow squares. Enter Play Mode has domain reload off, so the static texture caches in `ProceduralTextures` survived while the textures were destroyed; `??=` skips Unity's null check and returned the dead texture. Fixed with `== null` checks (`Fx.cs`, same pattern for the physics material in `ItemView.cs`). Confirmed by the user.
- Gotcha: never use `??` / `??=` / `?.` on cached `UnityEngine.Object`s.
- Editor note: no sound in the editor was the Game view "Mute Audio" toggle (machine-local editor pref), not the game.
### 2026-10-06 — M4.3 implemented (pre-M5 user requests)
- Finished room no longer forces the player out: banner choice "Stay and look around" (toast hints at the shelf close-up).
- Tap a full shelf (during play or after finishing) -> camera flies in front of it; drag to slide along it, pinch / mouse wheel to zoom, double tap resets, Back button returns to the exact room view.
- Tests: `ShelfInspectViewTests` (EditMode), `ShelfInspectTests` (PlayMode: stay, tap detection, close-up, zoom/pan, back, leaving mid close-up). `FinishSection` moved to `TestGame`.
- Fix found by screenshots: close-up kept the same distance when the screen shape changed; now it keeps the same shelf width on screen.

### 2026-10-06 — M4.2 implemented (big rooms)
- `CameraFitter` frames a `FeelConfig.SectionViewWidth` (4.6 m) window and pans (`Pan`, `PanTo`, flick glide, limits). `DragController`: empty-floor drag pan, two-finger / right-mouse pan, edge auto-scroll while carrying or sweeping.
- Content: Comic Box 60 (comics 40, toys 20), Garage 200 (4 x 50), Warehouse rooms 300 (5-6 categories). 7 more variants. Floor width and box count computed from shelves (`ContentBuilder.Section`). Box capacity 15. Mastery threshold 50.
- Bookcases up to 5 rows; back wall grows with them. Dirt layer split into 256-px tiles. Overview draws long rooms shortened (`RoomView.DrawnSize`).
- `VenueProgress.ValidSave`: a section save made for different content (item count changed) is ignored and the room regenerates. Shelf showcase frames at most 2.6 m and glides diagonally on wide shelves.
- Tests: `BigRoomTests` (200/300 items, pan end to end), showcase queue wait made robust.

### 2026-10-06 — M4.1 implemented (playtest follow-ups)
- `SectionProgress.CollectiblesRemaining`: room ends only when collectibles are picked up; "something shiny" hint at 99%.
- Generator `includeCollectible` filter: found collectibles never respawn (no duplicates).
- Venue ladder without money: `VenueProgress.State` Locked/Open/Completed; finishing every room opens the next venue; overview button "Next place"; map shows 100% / Open / "Restore X first".
- `ShelfShowcase` (camera component): full shelf -> fly in, glide top->bottom, return; tap skips; queued; stops when leaving the section.
- `SectionHud.PlayCollectionComplete`: book flies from its button, opens on the venue page, pieces pop in. Section banner waits for rare moment / showcase / book.

### 2026-10-06 — M4 implemented
- M3 approved and committed (af3fdca).
- Map / Overview / Section flow with zoom + fade transitions, room labels, sell button, venue purchase, locked Basement (60% rule or 150 coins), 3 venues of content, book paging, resume.
- Tests: `VenueProgressTests` (EditMode), `VenueFlowTests` (PlayMode: finish -> sell -> buy, unlock by progress and coins); older PlayMode tests jump into the garage via `TestGame.LoadIntoSection`.
- Fixes from screenshots: overview did not refit on screen-size change; big toast covered the map; snapshot helper reframing.

### 2026-10-06 — M3 round 2 (user feedback)
- Magnifier removed everywhere (code, content asset, GDD). Tool bar: Hand, Broom, Magnet.
- `DragController` now carries a stack: Hand picks up anything the carried item passes over (capacity 1/2/3), Magnet pulls same-category items in a small radius continuously (limit 2/4/6). Resting over a shelf for `FeelConfig.ShelfDepositDwell` drops matching items (`SectionController.DeliverStack`), others stay in hand.
- Mastery: revealed items and floor items at build time (new game / load) auto-sort.

### 2026-10-06 — M3 implemented
- `ToolDefinition` (levels, unlock cost) + `ToolProgress`, Shop panel, locked tools in the tool bar.
- Magnet (`GatherFollowers` / `TryPlaceGroup`), Magnifier (lens `UpdateLens` + right-shelf `SetHint`), Hand speed/grab, Broom radius from levels.
- `CategoryMastery` (threshold 20), mastery moment (loose items auto-sort, banner, fanfare), mastered spills fly straight to shelves, shelf label progress bars.
- `SaveSystem` + `SaveData`; `SectionController.Capture/Restore`; autosave in `GameBootstrap`; tests use `test_save.json`.
- Fixes from screenshots: hidden big toast was visible; region number format; spilled items landing on shelves (guard collider).

### 2026-10-06 — M2 approved; Collection viewer added
- User approved M2 ("her şey çok ayarında"), committed.
- User asked for a 3D display of found collectibles from the book. Not in the plan, fits M2: GDD 9.1.1 "Vitrin" added (+ glossary, decision log), `CollectionViewer` + `OrbitView` (pure, tested), book entries clickable, viewer UI layer.

### 2026-10-06 — M2 implemented
- Android APK build check passed (no device). Added `BuildTools` + `tools/unity.sh android`; `.utmp/` ignored.
- Dirt layer with blotchy noise coverage, broom tool (cursor, dust, sweep sound loop), buried items pop out when swept, auto-finish at 93%.
- 4 collectibles (Captain Chubby mascot + 3 rares), glow + sparkles, hidden in boxes or under dirt. Rare find moment (rise, rays, dim, fanfare, card with single Continue), duplicates auto-sold with toast. Collection Book page (found / ???).
- Progress mood brightening + 100% renovation (recolour, light, sparkles, warm ambience loop).
- Bugs found by tests: brush left 1/255 dirt at centre (added full-strength core); rare item rendered behind rays (rigidbody interpolation, fixed); rare item too large in portrait (size now a share of screen width).

### 2026-10-06 — M1 implemented and approved (committed)
- Core loop: data-driven garage section (Comics/Toys/Tools, 12 slots each, 3 boxes, 36 items), tap-to-tip boxes with physics spill, drag & drop with finger offset, correct = fly + punch + coins + rising-pitch sound + haptic, wrong = arc back + correct shelf flashes, shelf-full and section-complete celebrations, UI Toolkit HUD, settings (sound/haptics/restart).
- Fixes found by tests: stale DB reference in scene setup; `WaitForEndOfFrame` hangs in batchmode; shelf labels used screen-based projection (now viewport-based); items too small on phone (shelves 4x3, items 1.3x).
- Feel tunables in `Assets/_Project/Data/FeelConfig.asset`.

### 2026-10-06 — M0 done, approved
- Created URP project, portrait settings, asmdefs, data layer SOs, GameDatabase validation, Wallet + tests (6/6 pass), batchmode tooling `tools/unity.sh`.
- User check: project opens, Play runs, no errors (empty scene as expected).
- Committed M0 locally. Set up graphify (graph in `graphify-out/`, post-commit hook, CLAUDE.md section) and this handoff file.
