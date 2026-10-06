# Graph Report - SortingGame  (2026-10-06)

## Corpus Check
- 81 files · ~58,997 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 291 file(s) not represented in the graph (top: .meta 191, .asset 83, (none) 4)

## Summary
- 1470 nodes · 3849 edges · 74 communities (50 shown, 24 thin omitted)
- Extraction: 88% EXTRACTED · 12% INFERRED · 0% AMBIGUOUS · INFERRED: 466 edges (avg confidence: 0.89)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `2d458cee`
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
- .Sweep
- 5. Mekânlar ve bölümler
- CollectionViewer
- unity.sh
- Fx
- GameFlow
- 12. Görsel yön
- 4. Oyun döngüleri
- .ShelfFor
- .Awake
- SectionHud
- SectionController
- ToolDefinition
- SectionProgress
- .Capture
- 11. Ekonomi ve gelir modeli
- 7. Etkileşim ve kontroller
- 16. MVP kapsamı
- 6. Görünümler ve kamera
- OverviewController
- DirtMask
- VenueDefinition
- PlaceholderFactory
- GameDatabase
- FakeAdProvider
- GameBootstrap
- SaveData
- Session log
- .Run
- SectionDefinition
- HelperView
- ShelfView
- CollectibleDefinition
- PlaceholderVisual
- PlaceholderShape
- .CreateMainScene
- ProceduralTextures
- CategoryDefinition
- .Garage_IsBig_AndPansEndToEnd
- SectionSave
- CoreLoopTests
- ScriptableObject
- ShelfInspectTests
- ItemState
- VenueState
- Loc
- HelperDefinition
- .ShowOverviewNow
- M1 architecture (quick map)
- MonoBehaviour
- 9. Koleksiyon sistemi
- VenueFlowTests
- HelperTests
- AutoSortBoost
- .OnItemPlaced
- M5 architecture (quick map)
- .PlayAlbumCelebration
- ItemDefinition
- 8. Eşyalar ve kategoriler

## God Nodes (most connected - your core abstractions)
1. `SectionHud` - 116 edges
2. `SectionController` - 108 edges
3. `ItemView` - 68 edges
4. `GameBootstrap` - 61 edges
5. `SortingGame.Core` - 55 edges
6. `DragController` - 54 edges
7. `SortingGame.Data` - 52 edges
8. `HelperView` - 49 edges
9. `ShelfView` - 49 edges
10. `SectionDefinition` - 44 edges

## Surprising Connections (you probably didn't know these)
- `9.1 Koleksiyon Kitabı (`CollectionBook`) [KARAR]` --references--> `CollectionBook`  [INFERRED]
  Docs/GDD.md → Assets/_Project/Scripts/Runtime/Core/CollectionBook.cs
- `8.1 Eşya türleri` --references--> `ContainerDefinition`  [INFERRED]
  Docs/GDD.md → Assets/_Project/Scripts/Runtime/Data/ContainerDefinition.cs
- `9.1.1 Vitrin (`CollectionViewer`) [KARAR]` --references--> `CollectionViewer`  [INFERRED]
  Docs/GDD.md → Assets/_Project/Scripts/Runtime/Section/CollectionViewer.cs
- `M1 architecture (quick map)` --references--> `ContentBuilder`  [INFERRED]
  Docs/PROGRESS.md → Assets/_Project/Scripts/Editor/ContentBuilder.cs
- `M4 architecture (quick map)` --references--> `ContentBuilder`  [INFERRED]
  Docs/PROGRESS.md → Assets/_Project/Scripts/Editor/ContentBuilder.cs

## Import Cycles
- None detected.

## Communities (74 total, 24 thin omitted)

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
Cohesion: 0.13
Nodes (9): ItemView, CanPick, CanTapToFind, Definition, IsCollectible, IsRare, SavePose, State (+1 more)

### Community 6 - "Sfx"
Cohesion: 0.06
Nodes (29): Sfx, BookStamp, CleanAmbienceLoop, Coin, HelperChirp, Magnet, Mastery, Pickup (+21 more)

### Community 7 - "DragController"
Cohesion: 0.07
Nodes (12): CameraFitter, CanPan, PanMax, PanMin, PanX, DragController, Capacity, Carried (+4 more)

### Community 9 - "5. Mekânlar ve bölümler"
Cohesion: 0.29
Nodes (7): 5.1 Yapı [KARAR], 5.2 İlerleme sırası [VARSAYILAN], 5.3 Mekân merdiveni (taslak), 5.4 Bölüm kilitleri [VARSAYILAN], 5.5 Hafif renovasyon [KARAR], 5.6 Mekânın tamamlanması [KARAR], 5. Mekânlar ve bölümler

### Community 10 - "CollectionViewer"
Cohesion: 0.09
Nodes (12): OrbitView, IsIdleSpinning, Pan, Pitch, Yaw, Zoom, CollectionViewer, FrustumHeight (+4 more)

### Community 13 - "GameFlow"
Cohesion: 0.11
Nodes (11): GameFlow, ActiveSection, Busy, Current, Data, Venue, Screen, Overview (+3 more)

### Community 14 - "12. Görsel yön"
Cohesion: 0.40
Nodes (5): 12.1 Stil [KARAR], 12.2 Arayüz [KARAR], 12.3 Konsept görseller, 12.4 Konsept görsellerdeki bilinen sorunlar [KARAR: oyunda düzeltilecek], 12. Görsel yön

### Community 15 - "4. Oyun döngüleri"
Cohesion: 0.40
Nodes (5): 4.1 Anlık döngü (saniyeler), 4.2 Oturum döngüsü (1–5 dakika), 4.3 Meta döngü (günler/haftalar), 4.4 Kaynak akışı, 4. Oyun döngüleri

### Community 17 - ".Awake"
Cohesion: 0.06
Nodes (12): Haptics, Vibrator, Ease, FeelConfig, RareFindPresenter, DisplayHeight, DisplaySize, IsPresenting (+4 more)

### Community 18 - "SectionHud"
Cohesion: 0.09
Nodes (11): GameContext, Feel, SectionHud, CurrentMode, IsBannerVisible, IsBookOpen, IsBoostOpen, IsInspecting (+3 more)

### Community 19 - "SectionController"
Cohesion: 0.07
Nodes (17): SectionController, AutoSortCategory, CanStartAutoSort, Collectibles, Containers, Definition, DirtCleaned, HasDirt (+9 more)

### Community 20 - "ToolDefinition"
Cohesion: 0.07
Nodes (14): FileSaveStorage, ISaveStorage, SaveSystem, ToolProgress, ToolType, Broom, Hand, Magnet (+6 more)

### Community 21 - "SectionProgress"
Cohesion: 0.17
Nodes (12): SectionProgress, CollectiblesRemaining, DirtCleaned, Fraction, HasDirt, IsComplete, OnlyCollectiblesLeft, Percent (+4 more)

### Community 22 - ".Capture"
Cohesion: 0.15
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

### Community 27 - "OverviewController"
Cohesion: 0.14
Nodes (5): OverviewController, BuildingBounds, InputEnabled, IsActive, Rooms

### Community 28 - "DirtMask"
Cohesion: 0.16
Nodes (7): DirtMask, CleanedFraction, HasDirt, Height, InitialTotal, Width, DirtMaskTests

### Community 29 - "VenueDefinition"
Cohesion: 0.23
Nodes (6): SectionStatus, Percent, VenueProgress, VenueStatus, Fraction, VenueDefinition

### Community 30 - "PlaceholderFactory"
Cohesion: 0.09
Nodes (9): SectionVisuals, RoomView, Centre, LabelAnchor, Section, Size, Status, PlaceholderFactory (+1 more)

### Community 31 - "GameDatabase"
Cohesion: 0.18
Nodes (3): GameDatabase, Album, GameDatabaseTests

### Community 32 - "FakeAdProvider"
Cohesion: 0.13
Nodes (14): FakeAdProvider, IsRewardedReady, ShownCount, FakeStoreProvider, LastProductId, IAdProvider, IsRewardedReady, IStoreProvider (+6 more)

### Community 33 - "GameBootstrap"
Cohesion: 0.08
Nodes (16): GameBootstrap, Book, Context, Crew, Data, Drag, Flow, Hud (+8 more)

### Community 35 - "Session log"
Cohesion: 0.17
Nodes (10): BuildTools, 2026-10-06 — M0 done, approved, 2026-10-06 — M1 implemented and approved (committed), 2026-10-06 — M2 approved; Collection viewer added, 2026-10-06 — M2 implemented, 2026-10-06 — M3 implemented, 2026-10-06 — M3 round 2 (user feedback), 2026-10-06 — M4.x approved; progression redesign written down (no code) (+2 more)

### Community 37 - "SectionDefinition"
Cohesion: 0.16
Nodes (3): ContainerEntry, SectionDefinition, TotalSlotCount

### Community 38 - "HelperView"
Cohesion: 0.06
Nodes (20): HelperCrew, Fx, Helpers, Activity, Idle, Picking, Placing, ToItem (+12 more)

### Community 39 - "ShelfView"
Cohesion: 0.11
Nodes (12): ShelfSlot, IsFree, IsReserved, Occupant, WorldBase, ShelfView, Category, FilledCount (+4 more)

### Community 40 - "CollectibleDefinition"
Cohesion: 0.08
Nodes (8): CollectionBook, FoundIds, ContainerContent, SectionLayoutGenerator, CollectibleDefinition, IsMascot, CollectionBookTests, SectionLayoutGeneratorTests

### Community 41 - "PlaceholderVisual"
Cohesion: 0.25
Nodes (3): ContainerDefinition, PlaceholderVisual, 17. Sözlük

### Community 42 - "PlaceholderShape"
Cohesion: 0.17
Nodes (11): ItemRarity, Common, Mascot, Rare, PlaceholderShape, Book, Capsule, Cube (+3 more)

### Community 44 - "ProceduralTextures"
Cohesion: 0.29
Nodes (5): ProceduralTextures, Heart, Rays, SoftDot, 2026-10-06 — Personal PC set up; glow fix

### Community 45 - "CategoryDefinition"
Cohesion: 0.17
Nodes (9): CategoryDefinition, ShelfEntry, 15.1 Genel, 15.2 Veri odaklı tasarım, 15.3 Performans, 15.4 Kayıt, 15.5 Entegrasyonlar [AÇIK], 15.6 Dil (+1 more)

### Community 48 - "SectionSave"
Cohesion: 0.21
Nodes (7): ContainerSave, ItemSave, ItemSaveState, Buried, Floor, Placed, SectionSave

### Community 52 - "ShelfInspectTests"
Cohesion: 0.14
Nodes (3): ShelfInspectTests, Boot, Section

### Community 53 - "ItemState"
Cohesion: 0.25
Nodes (8): ItemState, Buried, Dragging, Flying, Found, Physics, Placed, Resting

### Community 54 - "VenueState"
Cohesion: 0.50
Nodes (4): VenueState, Completed, Locked, Open

### Community 56 - "HelperDefinition"
Cohesion: 0.12
Nodes (8): HelperProgress, Wallet, Coins, HelperDefinition, MaxLevel, Level, HelperProgressTests, WalletTests

### Community 60 - "9. Koleksiyon sistemi"
Cohesion: 0.29
Nodes (7): 9.1.1 Vitrin (`CollectionViewer`) [KARAR], 9.1 Koleksiyon Kitabı (`CollectionBook`) [KARAR], 9.2 Maskot [KARAR], 9.3 Chubby bulma anı [KARAR], 9.4 Nadir (mavi) eşyalar [KARAR], 9.5 Set bonusları [AÇIK], 9. Koleksiyon sistemi

### Community 61 - "VenueFlowTests"
Cohesion: 0.11
Nodes (5): VenueFlowTests, Boot, Flow, Section, 2026-10-06 — M4 implemented

### Community 62 - "HelperTests"
Cohesion: 0.22
Nodes (6): HelperTests, Boot, ClosedBoxes, Placed, Section, 2026-10-06 — M5 approved; M6 implemented (helpers)

### Community 65 - "AutoSortBoost"
Cohesion: 0.24
Nodes (7): AutoSortBoost, Charges, 10.1 Aletler (`Tool`) [KARAR], 10.2 Oto Sort güçlendirmesi (`AutoSortBoost`) [KARAR], 10.3 Yardımcılar (`Helper`) [KARAR], 10.4 Ölçek büyüdükçe oyuncunun rolü, 10. Otomasyon ve ilerleme

### Community 69 - ".OnItemPlaced"
Cohesion: 0.18
Nodes (3): Mode, Overview, Section

### Community 71 - "M5 architecture (quick map)"
Cohesion: 0.18
Nodes (6): Continuing on a new machine (checklist), Current state, Environment notes, M4.2 plan + task list (tick as done), M5 architecture (quick map), Progress & Handoff

### Community 76 - "ItemDefinition"
Cohesion: 0.15
Nodes (7): ItemDefinition, CoinValue, IsCollectible, IsRare, ContainerView, Definition, IsOpened

### Community 78 - "8. Eşyalar ve kategoriler"
Cohesion: 0.50
Nodes (4): 8.1 Eşya türleri, 8.2 Kategoriler [KARAR], 8.3 Eşya çeşitliliği, 8. Eşyalar ve kategoriler

## Knowledge Gaps
- **290 isolated node(s):** `Charges`, `FoundIds`, `Width`, `Height`, `HasDirt` (+285 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 497 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **24 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `SectionController` connect `SectionController` to `.PlaceBuried`, `unityengine`, `ItemView`, `DragController`, `.Sweep`, `Fx`, `GameFlow`, `.ShelfFor`, `.Awake`, `SectionHud`, `SectionProgress`, `DirtMask`, `PlaceholderFactory`, `GameDatabase`, `GameBootstrap`, `SectionDefinition`, `HelperView`, `ShelfView`, `CollectibleDefinition`, `CategoryDefinition`, `Vector3`, `SectionSave`, `CoreLoopTests`, `ShelfInspectTests`, `HelperDefinition`, `M1 architecture (quick map)`, `MonoBehaviour`, `VenueFlowTests`, `HelperTests`, `ItemDefinition`?**
  _High betweenness centrality (0.171) - this node is a cross-community bridge._
- **Are the 2 inferred relationships involving `SectionHud` (e.g. with `M1 architecture (quick map)` and `M4 architecture (quick map)`) actually correct?**
  _`SectionHud` has 2 INFERRED edges - model-reasoned connections that need verification._
- **What connects `Charges`, `FoundIds`, `Width` to the rest of the system?**
  _290 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `ShelfInspector` be split into smaller, more focused modules?**
  _Cohesion score 0.09090909090909091 - nodes in this community are weakly interconnected._
- **Why does `M1 architecture (quick map)` connect `M1 architecture (quick map)` to `ContentBuilder`, `ItemView`, `Sfx`, `DragController`, `.Sweep`, `CollectionViewer`, `.Awake`, `SectionHud`, `SectionController`, `ToolDefinition`, `DirtMask`, `PlaceholderFactory`, `GameBootstrap`, `.Run`, `SectionDefinition`, `ShelfView`, `CollectibleDefinition`, `.CreateMainScene`, `.Get`, `CoreLoopTests`, `ScriptableObject`, `HelperDefinition`, `MonoBehaviour`, `M5 architecture (quick map)`, `.PlayAlbumCelebration`, `ItemDefinition`?**
  _High betweenness centrality (0.126) - this node is a cross-community bridge._
- **Are the 3 inferred relationships involving `GameBootstrap` (e.g. with `2026-10-06 — M3 implemented` and `M1 architecture (quick map)`) actually correct?**
  _`GameBootstrap` has 3 INFERRED edges - model-reasoned connections that need verification._
- **Should `unityengine` be split into smaller, more focused modules?**
  _Cohesion score 0.07105416423995341 - nodes in this community are weakly interconnected._