# Graph Report - SortingGame  (2026-10-07)

## Corpus Check
- 89 files · ~70,931 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 411 file(s) not represented in the graph (top: .meta 256, .asset 138, (none) 4)

## Summary
- 1625 nodes · 4273 edges · 90 communities (58 shown, 32 thin omitted)
- Extraction: 88% EXTRACTED · 12% INFERRED · 0% AMBIGUOUS · INFERRED: 519 edges (avg confidence: 0.89)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `b01eced0`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- GDD — Chubby's Clutter (çalışma adı)
- ShelfInspector
- unityengine
- ContentBuilder
- ItemView
- Sfx
- FloorTests
- .Build
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
- .Generate
- .ShowBookPage
- .Capture
- 11. Ekonomi ve gelir modeli
- 7. Etkileşim ve kontroller
- 16. MVP kapsamı
- 6. Görünümler ve kamera
- Loc
- DirtMask
- OverviewController
- PlaceholderFactory
- GameDatabase
- IAdProvider
- GameBootstrap
- SaveData
- Wallet
- .Run
- VenueDefinition
- HelperView
- ShelfView
- CollectibleDefinition
- FeelConfig
- PlaceholderShape
- M6 architecture (quick map)
- CircusTests
- 15.2 Veri odaklı tasarım
- EconomyTests
- .LoadIntoSection
- SectionSave
- CoreLoopTests
- .CreateMainScene
- ItemState
- ProceduralTextures
- Activity
- HelperDefinition
- .Format
- ToolDefinition
- M1 architecture (quick map)
- HelperTests
- SectionLayout
- .OnLanded
- Progress & Handoff
- Sorting Game (working title: Chubby's Clutter)
- Mode
- Session log
- DragController
- AutoSortBoost
- .Bind
- 10. Otomasyon ve ilerleme
- ItemDefinition
- .Awake
- 8. Eşyalar ve kategoriler
- Decisions taken during development
- SaveSystem
- 9. Koleksiyon sistemi
- SectionDefinition

## God Nodes (most connected - your core abstractions)
1. `SectionHud` - 119 edges
2. `SectionController` - 115 edges
3. `ItemView` - 71 edges
4. `GameBootstrap` - 66 edges
5. `SortingGame.Core` - 62 edges
6. `SortingGame.Data` - 57 edges
7. `DragController` - 55 edges
8. `SectionDefinition` - 51 edges
9. `HelperView` - 49 edges
10. `ShelfView` - 49 edges

## Surprising Connections (you probably didn't know these)
- `10.2 Oto Sort güçlendirmesi (`AutoSortBoost`) [KARAR]` --references--> `AutoSortBoost`  [INFERRED]
  Docs/GDD.md → Assets/_Project/Scripts/Runtime/Core/AutoSortBoost.cs
- `9.1 Koleksiyon Kitabı (`CollectionBook`) [KARAR]` --references--> `CollectionBook`  [INFERRED]
  Docs/GDD.md → Assets/_Project/Scripts/Runtime/Core/CollectionBook.cs
- `8.1 Eşya türleri` --references--> `ContainerDefinition`  [INFERRED]
  Docs/GDD.md → Assets/_Project/Scripts/Runtime/Data/ContainerDefinition.cs
- `9.1.1 Vitrin (`CollectionViewer`) [KARAR]` --references--> `CollectionViewer`  [INFERRED]
  Docs/GDD.md → Assets/_Project/Scripts/Runtime/Section/CollectionViewer.cs
- `Economy report` --references--> `ContentBuilder`  [INFERRED]
  Docs/ECONOMY.md → Assets/_Project/Scripts/Editor/ContentBuilder.cs

## Import Cycles
- None detected.

## Communities (90 total, 32 thin omitted)

### Community 1 - "GDD — Chubby's Clutter (çalışma adı)"
Cohesion: 0.20
Nodes (9): 0. Bu belge nasıl okunmalı, 13. Fikri mülkiyet ve özgünlük kuralları [KARAR], 14. Ses ve haptik, 18. Açık sorular, 19. Karar günlüğü, 1. Özet, 2. Değişmez prensipler, 3. Referanslar ve farklılaşma (+1 more)

### Community 2 - "ShelfInspector"
Cohesion: 0.09
Nodes (13): ShelfInspectView, Distance, FitDistance, Focus, IsGliding, PanLimit, ShelfInspector, IsOpen (+5 more)

### Community 3 - "unityengine"
Cohesion: 0.06
Nodes (9): BuildTools, SortingGame.Tests, SortingGame.Data, SortingGame.UI, SortingGame.Core, SortingGame.Overview, SortingGame.EditorTools, SortingGame.Section (+1 more)

### Community 4 - "ContentBuilder"
Cohesion: 0.15
Nodes (4): ContentBuilder, PlaceholderVisual, 17. Sözlük, M7 architecture (quick map)

### Community 5 - "ItemView"
Cohesion: 0.11
Nodes (14): ItemView, CanPick, CanTapToFind, Definition, IsCollectible, IsRare, SavePose, State (+6 more)

### Community 6 - "Sfx"
Cohesion: 0.06
Nodes (27): Sfx, BookStamp, CleanAmbienceLoop, Coin, HelperChirp, Magnet, Mastery, Pickup (+19 more)

### Community 7 - "FloorTests"
Cohesion: 0.31
Nodes (3): FloorTests, Boot, Section

### Community 8 - ".Build"
Cohesion: 0.22
Nodes (5): EconomyReport, Economy report, Income: coins a room pays when everything is shelved, Reference player, Shop prices

### Community 9 - "5. Mekânlar ve bölümler"
Cohesion: 0.29
Nodes (7): 5.1 Yapı [KARAR], 5.2 İlerleme sırası [VARSAYILAN], 5.3 Mekân merdiveni (taslak), 5.4 Bölüm kilitleri [VARSAYILAN], 5.5 Hafif renovasyon [KARAR], 5.6 Mekânın tamamlanması [KARAR], 5. Mekânlar ve bölümler

### Community 10 - "CollectionViewer"
Cohesion: 0.09
Nodes (13): OrbitView, IsIdleSpinning, Pan, Pitch, Yaw, Zoom, CollectionViewer, FrustumHeight (+5 more)

### Community 13 - "GameFlow"
Cohesion: 0.11
Nodes (10): GameFlow, ActiveSection, Busy, Current, Data, Venue, Screen, Overview (+2 more)

### Community 14 - "12. Görsel yön"
Cohesion: 0.40
Nodes (5): 12.1 Stil [KARAR], 12.2 Arayüz [KARAR], 12.3 Konsept görseller, 12.4 Konsept görsellerdeki bilinen sorunlar [KARAR: oyunda düzeltilecek], 12. Görsel yön

### Community 15 - "4. Oyun döngüleri"
Cohesion: 0.40
Nodes (5): 4.1 Anlık döngü (saniyeler), 4.2 Oturum döngüsü (1–5 dakika), 4.3 Meta döngü (günler/haftalar), 4.4 Kaynak akışı, 4. Oyun döngüleri

### Community 17 - "ShelfShowcase"
Cohesion: 0.13
Nodes (4): Ease, ShelfShowcase, IsPlaying, 2026-10-06 — M4.1 implemented (playtest follow-ups)

### Community 18 - "SectionHud"
Cohesion: 0.09
Nodes (9): SectionHud, CurrentMode, IsBannerVisible, IsBookOpen, IsBoostOpen, IsInspecting, IsMapOpen, TitleText (+1 more)

### Community 19 - "SectionController"
Cohesion: 0.07
Nodes (18): SectionController, AutoSortCategory, CanStartAutoSort, Collectibles, Containers, Definition, DirtCleaned, FloorBackLimit (+10 more)

### Community 22 - ".Capture"
Cohesion: 0.13
Nodes (7): ShelfInspectTests, Boot, Section, TestGame, Boot, TestSnapshots, 2026-10-06 — M4.3 implemented (pre-M5 user requests)

### Community 23 - "11. Ekonomi ve gelir modeli"
Cohesion: 0.33
Nodes (6): 11.1 Para birimleri, 11.2 Ödüllü reklamlar [KARAR], 11.3 Uygulama içi satın almalar [KARAR], 11.4 Gelir modeli kuralları [KARAR], 11.5 Dengeleme, 11. Ekonomi ve gelir modeli

### Community 24 - "7. Etkileşim ve kontroller"
Cohesion: 0.33
Nodes (6): 7.1 Temel hareketler [KARAR], 7.2.1 Eşyaların odaya dağılımı [KARAR], 7.2 Yanlış yerleştirme ve bırakma [KARAR], 7.3 Çöp ve kategorisiz eşyalar [VARSAYILAN], 7.4 Geri bildirim [KARAR], 7. Etkileşim ve kontroller

### Community 25 - "16. MVP kapsamı"
Cohesion: 0.50
Nodes (4): 16.1 Dahil, 16.2 Dahil değil, 16.3 MVP başarı kriteri [VARSAYILAN], 16. MVP kapsamı

### Community 26 - "6. Görünümler ve kamera"
Cohesion: 0.50
Nodes (4): 6.1 Genel bakış (`OverviewView`) [KARAR], 6.2 Bölüm görünümü (`SectionView`) [KARAR], 6.3 Ekran yönü [KARAR], 6. Görünümler ve kamera

### Community 27 - "Loc"
Cohesion: 0.09
Nodes (8): LanguageInfo, Loc, Culture, Language, Languages, SavedLanguage, LocTests, Database

### Community 28 - "DirtMask"
Cohesion: 0.05
Nodes (21): DirtMask, CleanedFraction, HasDirt, Height, InitialTotal, Width, SectionProgress, CollectiblesRemaining (+13 more)

### Community 29 - "OverviewController"
Cohesion: 0.13
Nodes (6): OverviewController, BuildingBounds, InputEnabled, IsActive, Rooms, M4 architecture (quick map)

### Community 30 - "PlaceholderFactory"
Cohesion: 0.09
Nodes (9): SectionVisuals, RoomView, Centre, LabelAnchor, Section, Size, Status, PlaceholderFactory (+1 more)

### Community 31 - "GameDatabase"
Cohesion: 0.11
Nodes (8): AutoSortPack, BalanceConfig, CategoryDefinition, GameDatabase, Album, LanguageEntry, ShelfEntry, GameDatabaseTests

### Community 32 - "IAdProvider"
Cohesion: 0.20
Nodes (10): FakeAdProvider, IsRewardedReady, ShownCount, FakeStoreProvider, LastProductId, IAdProvider, IsRewardedReady, IStoreProvider (+2 more)

### Community 33 - "GameBootstrap"
Cohesion: 0.07
Nodes (17): GameBootstrap, Book, Context, Crew, Data, Drag, Flow, Hud (+9 more)

### Community 35 - "Wallet"
Cohesion: 0.38
Nodes (3): Wallet, Coins, WalletTests

### Community 37 - "VenueDefinition"
Cohesion: 0.20
Nodes (10): SectionStatus, Percent, VenueProgress, VenueState, Completed, Locked, Open, VenueStatus (+2 more)

### Community 38 - "HelperView"
Cohesion: 0.13
Nodes (10): HelperView, Capacity, CarriedCount, Current, Definition, IsHappy, PetCount, RoomNeedsWork (+2 more)

### Community 39 - "ShelfView"
Cohesion: 0.12
Nodes (8): ShelfView, Category, FilledCount, HasFreeSlot, IsFull, LabelAnchor, Size, M8.1 notes (where items end up)

### Community 40 - "CollectibleDefinition"
Cohesion: 0.15
Nodes (5): CollectionBook, FoundIds, CollectibleDefinition, IsMascot, CollectionBookTests

### Community 41 - "FeelConfig"
Cohesion: 0.13
Nodes (6): FeelConfig, RareFindPresenter, DisplayHeight, DisplaySize, IsPresenting, PresentedItem

### Community 42 - "PlaceholderShape"
Cohesion: 0.29
Nodes (7): PlaceholderShape, Book, Capsule, Cube, Cylinder, Rod, Sphere

### Community 44 - "CircusTests"
Cohesion: 0.21
Nodes (5): CircusTests, Boot, Flow, Section, 2026-10-07 — M8 implemented (Abandoned Circus)

### Community 45 - "15.2 Veri odaklı tasarım"
Cohesion: 0.29
Nodes (7): 15.1 Genel, 15.2 Veri odaklı tasarım, 15.3 Performans, 15.4 Kayıt, 15.5 Entegrasyonlar [AÇIK], 15.6 Dil, 15. Teknik notlar (Unity)

### Community 46 - "EconomyTests"
Cohesion: 0.14
Nodes (5): EconomyModel, Purchase, RoomIncome, EconomyTests, Database

### Community 48 - "SectionSave"
Cohesion: 0.18
Nodes (8): ContainerSave, IdCount, ItemSave, ItemSaveState, Buried, Floor, Placed, SectionSave

### Community 53 - "ItemState"
Cohesion: 0.25
Nodes (8): ItemState, Buried, Dragging, Flying, Found, Physics, Placed, Resting

### Community 54 - "ProceduralTextures"
Cohesion: 0.29
Nodes (5): ProceduralTextures, Heart, Rays, SoftDot, 2026-10-06 — Personal PC set up; glow fix

### Community 55 - "Activity"
Cohesion: 0.29
Nodes (7): Activity, Idle, Picking, Placing, ToItem, ToShelf, Wandering

### Community 56 - "HelperDefinition"
Cohesion: 0.16
Nodes (5): HelperProgress, HelperDefinition, MaxLevel, Level, HelperProgressTests

### Community 58 - "ToolDefinition"
Cohesion: 0.18
Nodes (6): ToolProgress, Level, ToolDefinition, MaxLevel, StartsOwned, ProgressionTests

### Community 59 - "M1 architecture (quick map)"
Cohesion: 0.18
Nodes (3): TweenRunner, RareGlow, M1 architecture (quick map)

### Community 62 - "HelperTests"
Cohesion: 0.21
Nodes (6): HelperTests, Boot, ClosedBoxes, Placed, Section, 2026-10-06 — M5 approved; M6 implemented (helpers)

### Community 63 - "SectionLayout"
Cohesion: 0.31
Nodes (4): ContainerContent, SectionLayout, TotalItems, SectionLayoutGenerator

### Community 66 - "Progress & Handoff"
Cohesion: 0.33
Nodes (5): Continuing on a new machine (checklist), Environment notes, M4.2 plan + task list (tick as done), M8 notes (Abandoned Circus), Progress & Handoff

### Community 67 - "Sorting Game (working title: Chubby's Clutter)"
Cohesion: 0.33
Nodes (5): Git rules, graphify, Session routine, Sorting Game (working title: Chubby's Clutter), Verifying (editor must be closed for batchmode)

### Community 69 - "Mode"
Cohesion: 0.67
Nodes (3): Mode, Overview, Section

### Community 70 - "Session log"
Cohesion: 0.12
Nodes (14): VenueFlowTests, Boot, Flow, Section, 2026-10-06 — M0 done, approved, 2026-10-06 — M1 implemented and approved (committed), 2026-10-06 — M3 round 2 (user feedback), 2026-10-06 — M4 implemented (+6 more)

### Community 71 - "DragController"
Cohesion: 0.05
Nodes (20): ItemRarity, Common, Mascot, Rare, ToolType, Broom, Hand, Magnet (+12 more)

### Community 75 - "10. Otomasyon ve ilerleme"
Cohesion: 0.40
Nodes (5): 10.1 Aletler (`Tool`) [KARAR], 10.2 Oto Sort güçlendirmesi (`AutoSortBoost`) [KARAR], 10.3 Yardımcılar (`Helper`) [KARAR], 10.4 Ölçek büyüdükçe oyuncunun rolü, 10. Otomasyon ve ilerleme

### Community 76 - "ItemDefinition"
Cohesion: 0.11
Nodes (9): ContainerDefinition, ItemDefinition, CoinValue, IsCollectible, IsRare, ContainerEntry, ContainerView, Definition (+1 more)

### Community 77 - ".Awake"
Cohesion: 0.16
Nodes (5): GameContext, Feel, HelperCrew, Fx, Helpers

### Community 78 - "8. Eşyalar ve kategoriler"
Cohesion: 0.50
Nodes (4): 8.1 Eşya türleri, 8.2 Kategoriler [KARAR], 8.3 Eşya çeşitliliği, 8. Eşyalar ve kategoriler

### Community 81 - "SaveSystem"
Cohesion: 0.13
Nodes (5): FileSaveStorage, ISaveStorage, SaveSystem, MemoryStorage, 2026-10-06 — M3 implemented

### Community 87 - "9. Koleksiyon sistemi"
Cohesion: 0.29
Nodes (7): 9.1.1 Vitrin (`CollectionViewer`) [KARAR], 9.1 Koleksiyon Kitabı (`CollectionBook`) [KARAR], 9.2 Maskot [KARAR], 9.3 Chubby bulma anı [KARAR], 9.4 Nadir (mavi) eşyalar [KARAR], 9.5 Set bonusları [AÇIK], 9. Koleksiyon sistemi

## Knowledge Gaps
- **308 isolated node(s):** `Charges`, `FoundIds`, `Width`, `Height`, `HasDirt` (+303 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 547 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **32 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `SectionController` connect `SectionController` to `.PlaceBuried`, `unityengine`, `ItemView`, `FloorTests`, `Fx`, `GameFlow`, `.ShelfFor`, `SectionHud`, `.Capture`, `DirtMask`, `PlaceholderFactory`, `GameDatabase`, `GameBootstrap`, `Wallet`, `HelperView`, `ShelfView`, `CollectibleDefinition`, `FeelConfig`, `CircusTests`, `SectionSave`, `CoreLoopTests`, `M1 architecture (quick map)`, `HelperTests`, `.OnLanded`, `Session log`, `DragController`, `.Bind`, `ItemDefinition`, `.Awake`, `SectionDefinition`?**
  _High betweenness centrality (0.198) - this node is a cross-community bridge._
- **Are the 2 inferred relationships involving `SectionHud` (e.g. with `M1 architecture (quick map)` and `M4 architecture (quick map)`) actually correct?**
  _`SectionHud` has 2 INFERRED edges - model-reasoned connections that need verification._
- **What connects `Charges`, `FoundIds`, `Width` to the rest of the system?**
  _308 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `ShelfInspector` be split into smaller, more focused modules?**
  _Cohesion score 0.09090909090909091 - nodes in this community are weakly interconnected._
- **Why does `SectionHud` connect `SectionHud` to `unityengine`, `GameFlow`, `SectionController`, `.ShowBookPage`, `DirtMask`, `OverviewController`, `PlaceholderFactory`, `GameBootstrap`, `Wallet`, `.Run`, `VenueDefinition`, `ShelfView`, `CollectibleDefinition`, `M6 architecture (quick map)`, `.Get`, `.Format`, `M1 architecture (quick map)`, `.OnItemPlaced`, `Mode`, `DragController`, `.Bind`, `.Awake`, `.ShowMap`, `SectionDefinition`?**
  _High betweenness centrality (0.102) - this node is a cross-community bridge._
- **Are the 3 inferred relationships involving `GameBootstrap` (e.g. with `2026-10-06 — M3 implemented` and `M1 architecture (quick map)`) actually correct?**
  _`GameBootstrap` has 3 INFERRED edges - model-reasoned connections that need verification._
- **Should `unityengine` be split into smaller, more focused modules?**
  _Cohesion score 0.06437833714721587 - nodes in this community are weakly interconnected._