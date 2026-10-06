# Graph Report - SortingGame  (2026-10-07)

## Corpus Check
- 87 files · ~64,887 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 299 file(s) not represented in the graph (top: .meta 199, .asset 83, (none) 4)

## Summary
- 1573 nodes · 4100 edges · 88 communities (57 shown, 31 thin omitted)
- Extraction: 88% EXTRACTED · 12% INFERRED · 0% AMBIGUOUS · INFERRED: 490 edges (avg confidence: 0.89)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `54a699c8`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- .PlaceBuried
- GDD — Chubby's Clutter (çalışma adı)
- ShelfInspector
- unityengine
- ContentBuilder
- ItemView
- Sfx
- DragController
- SectionProgress
- 5. Mekânlar ve bölümler
- CollectionViewer
- unity.sh
- Fx
- GameFlow
- 12. Görsel yön
- 4. Oyun döngüleri
- .ShelfFor
- ShelfShowcase
- SectionHud
- SectionController
- ToolDefinition
- GameContext
- .Capture
- 11. Ekonomi ve gelir modeli
- 7. Etkileşim ve kontroller
- 16. MVP kapsamı
- 6. Görünümler ve kamera
- .Awake
- DirtMask
- OverviewController
- PlaceholderFactory
- GameDatabase
- M5 architecture (quick map)
- GameBootstrap
- SaveData
- Session log
- .Run
- SectionDefinition
- HelperView
- ShelfView
- ItemDefinition
- RareFindPresenter
- PlaceholderVisual
- .ShowOverviewNow
- ProceduralTextures
- 15. Teknik notlar (Unity)
- EconomyTests
- .LoadIntoSection
- SectionSave
- CoreLoopTests
- CategoryDefinition
- VenueFlowTests
- ItemState
- VenueState
- DirtLayerView
- HelperDefinition
- .Get
- M1 architecture (quick map)
- MonoBehaviour
- CameraFitter
- ShelfInspectTests
- HelperTests
- ToolType
- RoomView
- Haptics
- BalanceConfig
- Sorting Game (working title: Chubby's Clutter)
- Mode
- .SwitchingLanguage_ReloadsInPlace_WithEveryTextTranslated
- Progress & Handoff
- .Bind
- SectionVisuals
- Activity
- FeelConfig
- 8. Eşyalar ve kategoriler
- ShelfSlot
- 10. Otomasyon ve ilerleme

## God Nodes (most connected - your core abstractions)
1. `SectionHud` - 119 edges
2. `SectionController` - 108 edges
3. `ItemView` - 68 edges
4. `GameBootstrap` - 64 edges
5. `SortingGame.Core` - 60 edges
6. `SortingGame.Data` - 56 edges
7. `DragController` - 54 edges
8. `HelperView` - 49 edges
9. `ShelfView` - 49 edges
10. `SectionDefinition` - 48 edges

## Surprising Connections (you probably didn't know these)
- `10.2 Oto Sort güçlendirmesi (`AutoSortBoost`) [KARAR]` --references--> `AutoSortBoost`  [INFERRED]
  Docs/GDD.md → Assets/_Project/Scripts/Runtime/Core/AutoSortBoost.cs
- `9.1 Koleksiyon Kitabı (`CollectionBook`) [KARAR]` --references--> `CollectionBook`  [INFERRED]
  Docs/GDD.md → Assets/_Project/Scripts/Runtime/Core/CollectionBook.cs
- `8.1 Eşya türleri` --references--> `ContainerDefinition`  [INFERRED]
  Docs/GDD.md → Assets/_Project/Scripts/Runtime/Data/ContainerDefinition.cs
- `Economy report` --references--> `ContentBuilder`  [INFERRED]
  Docs/ECONOMY.md → Assets/_Project/Scripts/Editor/ContentBuilder.cs
- `M1 architecture (quick map)` --references--> `ContentBuilder`  [INFERRED]
  Docs/PROGRESS.md → Assets/_Project/Scripts/Editor/ContentBuilder.cs

## Import Cycles
- None detected.

## Communities (88 total, 31 thin omitted)

### Community 1 - "GDD — Chubby's Clutter (çalışma adı)"
Cohesion: 0.20
Nodes (9): 0. Bu belge nasıl okunmalı, 13. Fikri mülkiyet ve özgünlük kuralları [KARAR], 14. Ses ve haptik, 18. Açık sorular, 19. Karar günlüğü, 1. Özet, 2. Değişmez prensipler, 3. Referanslar ve farklılaşma (+1 more)

### Community 2 - "ShelfInspector"
Cohesion: 0.09
Nodes (13): ShelfInspectView, Distance, FitDistance, Focus, IsGliding, PanLimit, ShelfInspector, IsOpen (+5 more)

### Community 3 - "unityengine"
Cohesion: 0.07
Nodes (7): SortingGame.Tests, SortingGame.Data, SortingGame.UI, SortingGame.Core, SortingGame.Overview, SortingGame.EditorTools, SortingGame.Section

### Community 5 - "ItemView"
Cohesion: 0.14
Nodes (9): ItemView, CanPick, CanTapToFind, Definition, IsCollectible, IsRare, SavePose, State (+1 more)

### Community 6 - "Sfx"
Cohesion: 0.06
Nodes (29): Sfx, BookStamp, CleanAmbienceLoop, Coin, HelperChirp, Magnet, Mastery, Pickup (+21 more)

### Community 7 - "DragController"
Cohesion: 0.12
Nodes (6): DragController, Capacity, Carried, Fitter, InputEnabled, Tool

### Community 8 - "SectionProgress"
Cohesion: 0.14
Nodes (12): SectionProgress, CollectiblesRemaining, DirtCleaned, Fraction, HasDirt, IsComplete, OnlyCollectiblesLeft, Percent (+4 more)

### Community 9 - "5. Mekânlar ve bölümler"
Cohesion: 0.29
Nodes (7): 5.1 Yapı [KARAR], 5.2 İlerleme sırası [VARSAYILAN], 5.3 Mekân merdiveni (taslak), 5.4 Bölüm kilitleri [VARSAYILAN], 5.5 Hafif renovasyon [KARAR], 5.6 Mekânın tamamlanması [KARAR], 5. Mekânlar ve bölümler

### Community 10 - "CollectionViewer"
Cohesion: 0.08
Nodes (20): OrbitView, IsIdleSpinning, Pan, Pitch, Yaw, Zoom, CollectionViewer, FrustumHeight (+12 more)

### Community 13 - "GameFlow"
Cohesion: 0.12
Nodes (9): GameFlow, ActiveSection, Busy, Current, Data, Venue, Screen, Overview (+1 more)

### Community 14 - "12. Görsel yön"
Cohesion: 0.40
Nodes (5): 12.1 Stil [KARAR], 12.2 Arayüz [KARAR], 12.3 Konsept görseller, 12.4 Konsept görsellerdeki bilinen sorunlar [KARAR: oyunda düzeltilecek], 12. Görsel yön

### Community 15 - "4. Oyun döngüleri"
Cohesion: 0.40
Nodes (5): 4.1 Anlık döngü (saniyeler), 4.2 Oturum döngüsü (1–5 dakika), 4.3 Meta döngü (günler/haftalar), 4.4 Kaynak akışı, 4. Oyun döngüleri

### Community 18 - "SectionHud"
Cohesion: 0.08
Nodes (9): SectionHud, CurrentMode, IsBannerVisible, IsBookOpen, IsBoostOpen, IsInspecting, IsMapOpen, TitleText (+1 more)

### Community 19 - "SectionController"
Cohesion: 0.06
Nodes (17): SectionController, AutoSortCategory, CanStartAutoSort, Collectibles, Containers, Definition, DirtCleaned, HasDirt (+9 more)

### Community 20 - "ToolDefinition"
Cohesion: 0.07
Nodes (13): AutoSortBoost, Charges, FileSaveStorage, ISaveStorage, SaveSystem, ToolProgress, Level, ToolDefinition (+5 more)

### Community 21 - "GameContext"
Cohesion: 0.16
Nodes (5): GameContext, Feel, HelperCrew, Fx, Helpers

### Community 22 - ".Capture"
Cohesion: 0.13
Nodes (4): TestGame, Boot, TestSnapshots, 2026-10-06 — M4.3 implemented (pre-M5 user requests)

### Community 23 - "11. Ekonomi ve gelir modeli"
Cohesion: 0.33
Nodes (6): 11.1 Para birimleri, 11.2 Ödüllü reklamlar [KARAR], 11.3 Uygulama içi satın almalar [KARAR], 11.4 Gelir modeli kuralları [KARAR], 11.5 Dengeleme, 11. Ekonomi ve gelir modeli

### Community 24 - "7. Etkileşim ve kontroller"
Cohesion: 0.40
Nodes (5): 7.1 Temel hareketler [KARAR], 7.2 Yanlış yerleştirme [VARSAYILAN], 7.3 Çöp ve kategorisiz eşyalar [VARSAYILAN], 7.4 Geri bildirim [KARAR], 7. Etkileşim ve kontroller

### Community 25 - "16. MVP kapsamı"
Cohesion: 0.50
Nodes (4): 16.1 Dahil, 16.2 Dahil değil, 16.3 MVP başarı kriteri [VARSAYILAN], 16. MVP kapsamı

### Community 26 - "6. Görünümler ve kamera"
Cohesion: 0.50
Nodes (4): 6.1 Genel bakış (`OverviewView`) [KARAR], 6.2 Bölüm görünümü (`SectionView`) [KARAR], 6.3 Ekran yönü [KARAR], 6. Görünümler ve kamera

### Community 27 - ".Awake"
Cohesion: 0.07
Nodes (14): EconomyReport, LanguageInfo, Loc, Culture, Language, Languages, SavedLanguage, LocTests (+6 more)

### Community 28 - "DirtMask"
Cohesion: 0.15
Nodes (7): DirtMask, CleanedFraction, HasDirt, Height, InitialTotal, Width, DirtMaskTests

### Community 29 - "OverviewController"
Cohesion: 0.11
Nodes (6): OverviewController, BuildingBounds, InputEnabled, IsActive, Rooms, M4 architecture (quick map)

### Community 31 - "GameDatabase"
Cohesion: 0.18
Nodes (4): GameDatabase, Album, LanguageEntry, GameDatabaseTests

### Community 32 - "M5 architecture (quick map)"
Cohesion: 0.14
Nodes (10): FakeAdProvider, IsRewardedReady, ShownCount, FakeStoreProvider, LastProductId, IAdProvider, IsRewardedReady, IStoreProvider (+2 more)

### Community 33 - "GameBootstrap"
Cohesion: 0.09
Nodes (15): GameBootstrap, Book, Context, Crew, Data, Drag, Flow, Hud (+7 more)

### Community 34 - "SaveData"
Cohesion: 0.17
Nodes (7): SaveData, SectionStatus, Percent, VenueProgress, VenueStatus, Fraction, VenueProgressTests

### Community 35 - "Session log"
Cohesion: 0.15
Nodes (11): BuildTools, 2026-10-06 — M0 done, approved, 2026-10-06 — M1 implemented and approved (committed), 2026-10-06 — M2 implemented, 2026-10-06 — M3 round 2 (user feedback), 2026-10-06 — M4.1 implemented (playtest follow-ups), 2026-10-06 — M4.x approved; progression redesign written down (no code), 2026-10-06 — M5 approved; M6 implemented (helpers) (+3 more)

### Community 37 - "SectionDefinition"
Cohesion: 0.19
Nodes (5): RoomIncome, SectionDefinition, TotalSlotCount, VenueDefinition, 15.2 Veri odaklı tasarım

### Community 38 - "HelperView"
Cohesion: 0.13
Nodes (10): HelperView, Capacity, CarriedCount, Current, Definition, IsHappy, PetCount, RoomNeedsWork (+2 more)

### Community 39 - "ShelfView"
Cohesion: 0.15
Nodes (7): ShelfView, Category, FilledCount, HasFreeSlot, IsFull, LabelAnchor, Size

### Community 40 - "ItemDefinition"
Cohesion: 0.06
Nodes (17): CollectionBook, FoundIds, ContainerContent, SectionLayoutGenerator, CollectibleDefinition, IsMascot, ItemRarity, Common (+9 more)

### Community 41 - "RareFindPresenter"
Cohesion: 0.17
Nodes (5): RareFindPresenter, DisplayHeight, DisplaySize, IsPresenting, PresentedItem

### Community 42 - "PlaceholderVisual"
Cohesion: 0.15
Nodes (10): ContainerDefinition, PlaceholderShape, Book, Capsule, Cube, Cylinder, Rod, Sphere (+2 more)

### Community 44 - "ProceduralTextures"
Cohesion: 0.29
Nodes (5): ProceduralTextures, Heart, Rays, SoftDot, 2026-10-06 — Personal PC set up; glow fix

### Community 45 - "15. Teknik notlar (Unity)"
Cohesion: 0.40
Nodes (5): 15.1 Genel, 15.3 Performans, 15.4 Kayıt, 15.5 Entegrasyonlar [AÇIK], 15. Teknik notlar (Unity)

### Community 46 - "EconomyTests"
Cohesion: 0.15
Nodes (4): EconomyModel, Purchase, EconomyTests, Database

### Community 48 - "SectionSave"
Cohesion: 0.21
Nodes (8): ContainerSave, IdCount, ItemSave, ItemSaveState, Buried, Floor, Placed, SectionSave

### Community 52 - "VenueFlowTests"
Cohesion: 0.11
Nodes (5): VenueFlowTests, Boot, Flow, Section, 2026-10-06 — M4 implemented

### Community 53 - "ItemState"
Cohesion: 0.25
Nodes (8): ItemState, Buried, Dragging, Flying, Found, Physics, Placed, Resting

### Community 54 - "VenueState"
Cohesion: 0.50
Nodes (4): VenueState, Completed, Locked, Open

### Community 56 - "HelperDefinition"
Cohesion: 0.12
Nodes (8): HelperProgress, Wallet, Coins, HelperDefinition, MaxLevel, Level, HelperProgressTests, WalletTests

### Community 60 - "CameraFitter"
Cohesion: 0.18
Nodes (6): CameraFitter, CanPan, PanMax, PanMin, PanX, 2026-10-06 — M4.2 implemented (big rooms)

### Community 61 - "ShelfInspectTests"
Cohesion: 0.12
Nodes (3): ShelfInspectTests, Boot, Section

### Community 62 - "HelperTests"
Cohesion: 0.25
Nodes (5): HelperTests, Boot, ClosedBoxes, Placed, Section

### Community 63 - "ToolType"
Cohesion: 0.29
Nodes (4): ToolType, Broom, Hand, Magnet

### Community 64 - "RoomView"
Cohesion: 0.18
Nodes (6): RoomView, Centre, LabelAnchor, Section, Size, Status

### Community 66 - "BalanceConfig"
Cohesion: 0.20
Nodes (3): ProjectSetup, AutoSortPack, BalanceConfig

### Community 67 - "Sorting Game (working title: Chubby's Clutter)"
Cohesion: 0.33
Nodes (5): Git rules, graphify, Session routine, Sorting Game (working title: Chubby's Clutter), Verifying (editor must be closed for batchmode)

### Community 69 - "Mode"
Cohesion: 0.67
Nodes (3): Mode, Overview, Section

### Community 72 - "Progress & Handoff"
Cohesion: 0.25
Nodes (6): Continuing on a new machine (checklist), Current state, Environment notes, M4.2 plan + task list (tick as done), M4.3 architecture (quick map), Progress & Handoff

### Community 75 - "Activity"
Cohesion: 0.29
Nodes (7): Activity, Idle, Picking, Placing, ToItem, ToShelf, Wandering

### Community 76 - "FeelConfig"
Cohesion: 0.11
Nodes (5): Ease, FeelConfig, ContainerView, Definition, IsOpened

### Community 78 - "8. Eşyalar ve kategoriler"
Cohesion: 0.50
Nodes (4): 8.1 Eşya türleri, 8.2 Kategoriler [KARAR], 8.3 Eşya çeşitliliği, 8. Eşyalar ve kategoriler

### Community 79 - "ShelfSlot"
Cohesion: 0.33
Nodes (5): ShelfSlot, IsFree, IsReserved, Occupant, WorldBase

### Community 83 - "10. Otomasyon ve ilerleme"
Cohesion: 0.40
Nodes (5): 10.1 Aletler (`Tool`) [KARAR], 10.2 Oto Sort güçlendirmesi (`AutoSortBoost`) [KARAR], 10.3 Yardımcılar (`Helper`) [KARAR], 10.4 Ölçek büyüdükçe oyuncunun rolü, 10. Otomasyon ve ilerleme

## Knowledge Gaps
- **301 isolated node(s):** `Charges`, `FoundIds`, `Width`, `Height`, `HasDirt` (+296 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 534 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **31 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `SectionController` connect `SectionController` to `.PlaceBuried`, `unityengine`, `ItemView`, `DragController`, `SectionProgress`, `Fx`, `GameFlow`, `.ShelfFor`, `SectionHud`, `GameContext`, `.Awake`, `DirtMask`, `PlaceholderFactory`, `GameDatabase`, `GameBootstrap`, `.Run`, `SectionDefinition`, `HelperView`, `ShelfView`, `ItemDefinition`, `SectionSave`, `CoreLoopTests`, `CategoryDefinition`, `VenueFlowTests`, `DirtLayerView`, `HelperDefinition`, `M1 architecture (quick map)`, `MonoBehaviour`, `ShelfInspectTests`, `HelperTests`, `.Bind`, `SectionVisuals`, `FeelConfig`, `.Restore`?**
  _High betweenness centrality (0.174) - this node is a cross-community bridge._
- **Are the 2 inferred relationships involving `SectionHud` (e.g. with `M1 architecture (quick map)` and `M4 architecture (quick map)`) actually correct?**
  _`SectionHud` has 2 INFERRED edges - model-reasoned connections that need verification._
- **What connects `Charges`, `FoundIds`, `Width` to the rest of the system?**
  _301 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `ShelfInspector` be split into smaller, more focused modules?**
  _Cohesion score 0.08502415458937199 - nodes in this community are weakly interconnected._
- **Why does `M1 architecture (quick map)` connect `M1 architecture (quick map)` to `ContentBuilder`, `ItemView`, `Sfx`, `DragController`, `SectionProgress`, `CollectionViewer`, `.ShelfFor`, `SectionHud`, `SectionController`, `ToolDefinition`, `GameContext`, `DirtMask`, `PlaceholderFactory`, `GameBootstrap`, `.Run`, `SectionDefinition`, `ShelfView`, `ItemDefinition`, `RareFindPresenter`, `CoreLoopTests`, `DirtLayerView`, `HelperDefinition`, `.Get`, `MonoBehaviour`, `CameraFitter`, `ToolType`, `BalanceConfig`, `Progress & Handoff`, `SectionVisuals`, `FeelConfig`?**
  _High betweenness centrality (0.118) - this node is a cross-community bridge._
- **Are the 3 inferred relationships involving `GameBootstrap` (e.g. with `2026-10-06 — M3 implemented` and `M1 architecture (quick map)`) actually correct?**
  _`GameBootstrap` has 3 INFERRED edges - model-reasoned connections that need verification._
- **Should `unityengine` be split into smaller, more focused modules?**
  _Cohesion score 0.06795786612300374 - nodes in this community are weakly interconnected._