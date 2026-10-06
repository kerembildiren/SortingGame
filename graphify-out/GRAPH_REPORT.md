# Graph Report - SortingGame  (2026-10-06)

## Corpus Check
- 74 files · ~47,679 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 280 file(s) not represented in the graph (top: .meta 182, .asset 81, (none) 4)

## Summary
- 1355 nodes · 3231 edges · 75 communities (46 shown, 29 thin omitted)
- Extraction: 89% EXTRACTED · 11% INFERRED · 0% AMBIGUOUS · INFERRED: 340 edges (avg confidence: 0.86)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `0018779a`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- GDD — Chubby's Clutter (çalışma adı)
- ShelfInspector
- unityengine
- ContentBuilder
- ItemView
- Sfx
- DragController
- .PlayDuplicate
- 5. Mekânlar ve bölümler
- CollectionViewer
- unity.sh
- .Burst
- GameFlow
- 12. Görsel yön
- 4. Oyun döngüleri
- ShelfView
- OverviewController
- SectionHud
- SectionController
- ToolDefinition
- SectionProgress
- .Capture
- 11. Ekonomi ve gelir modeli
- 7. Etkileşim ve kontroller
- 16. MVP kapsamı
- 6. Görünümler ve kamera
- .MakeSection
- DirtMask
- 15. Teknik notlar (Unity)
- PlaceholderFactory
- ItemDefinition
- ProceduralTextures
- GameBootstrap
- SaveData
- .Generate
- .Run
- Sorting Game (working title: Chubby's Clutter)
- SectionDefinition
- ContainerView
- Screen
- M4.3 architecture (quick map)
- ShelfInspectView
- .OnLanded
- TestGame
- Session log
- CoreLoopTests
- RareFindPresenter
- ItemState
- ShelfSlot
- ShelfInspectTests
- .ShowOverviewNow
- VenueFlowTests
- MonoBehaviour
- CollectibleDefinition
- 10. Otomasyon ve ilerleme
- Mode
- 8. Eşyalar ve kategoriler

## God Nodes (most connected - your core abstractions)
1. `SectionHud` - 99 edges
2. `SectionController` - 85 edges
3. `GameBootstrap` - 58 edges
4. `ItemView` - 58 edges
5. `DragController` - 52 edges
6. `SortingGame.Core` - 49 edges
7. `SortingGame.Data` - 47 edges
8. `M1 architecture (quick map)` - 42 edges
9. `GameFlow` - 37 edges
10. `SectionDefinition` - 34 edges

## Surprising Connections (you probably didn't know these)
- `9.1.1 Vitrin (`CollectionViewer`) [KARAR]` --references--> `CollectionViewer`  [INFERRED]
  Docs/GDD.md → Assets/_Project/Scripts/Runtime/Section/CollectionViewer.cs
- `10.2 Kategori Ustalığı (`CategoryMastery`) [KARAR]` --references--> `CategoryMastery`  [INFERRED]
  Docs/GDD.md → Assets/_Project/Scripts/Runtime/Core/CategoryMastery.cs
- `8.1 Eşya türleri` --references--> `ContainerDefinition`  [INFERRED]
  Docs/GDD.md → Assets/_Project/Scripts/Runtime/Data/ContainerDefinition.cs
- `M4.3 architecture (quick map)` --references--> `ShelfInspectView`  [INFERRED]
  Docs/PROGRESS.md → Assets/_Project/Scripts/Runtime/Core/ShelfInspectView.cs
- `M4.3 architecture (quick map)` --references--> `CameraFitter`  [INFERRED]
  Docs/PROGRESS.md → Assets/_Project/Scripts/Runtime/Section/CameraFitter.cs

## Import Cycles
- None detected.

## Communities (75 total, 29 thin omitted)

### Community 1 - "GDD — Chubby's Clutter (çalışma adı)"
Cohesion: 0.20
Nodes (9): 0. Bu belge nasıl okunmalı, 13. Fikri mülkiyet ve özgünlük kuralları [KARAR], 14. Ses ve haptik, 18. Açık sorular, 19. Karar günlüğü, 1. Özet, 2. Değişmez prensipler, 3. Referanslar ve farklılaşma (+1 more)

### Community 2 - "ShelfInspector"
Cohesion: 0.06
Nodes (14): FeelConfig, CameraFitter, CanPan, PanMax, PanMin, PanX, ShelfInspector, IsOpen (+6 more)

### Community 3 - "unityengine"
Cohesion: 0.07
Nodes (9): BuildTools, SortingGame.Tests, SortingGame.Data, SortingGame.UI, SortingGame.Core, SortingGame.Overview, SortingGame.EditorTools, SortingGame.Section (+1 more)

### Community 4 - "ContentBuilder"
Cohesion: 0.08
Nodes (14): ContentBuilder, ProjectSetup, ItemRarity, Common, Mascot, Rare, PlaceholderShape, Book (+6 more)

### Community 5 - "ItemView"
Cohesion: 0.15
Nodes (9): ItemView, CanPick, CanTapToFind, Definition, IsCollectible, SavePose, State, VisualSize (+1 more)

### Community 6 - "Sfx"
Cohesion: 0.06
Nodes (28): Sfx, BookStamp, CleanAmbienceLoop, Coin, DuplicateSold, Magnet, Mastery, Pickup (+20 more)

### Community 7 - "DragController"
Cohesion: 0.08
Nodes (6): DragController, Capacity, Carried, Fitter, InputEnabled, Tool

### Community 9 - "5. Mekânlar ve bölümler"
Cohesion: 0.29
Nodes (7): 5.1 Yapı [KARAR], 5.2 İlerleme sırası [VARSAYILAN], 5.3 Mekân merdiveni (taslak), 5.4 Bölüm kilitleri [VARSAYILAN], 5.5 Hafif renovasyon [KARAR], 5.6 Mekânın satışı [KARAR], 5. Mekânlar ve bölümler

### Community 10 - "CollectionViewer"
Cohesion: 0.09
Nodes (12): OrbitView, IsIdleSpinning, Pan, Pitch, Yaw, Zoom, CollectionViewer, FrustumHeight (+4 more)

### Community 13 - "GameFlow"
Cohesion: 0.11
Nodes (7): GameFlow, ActiveSection, Busy, Current, Data, Venue, M4 architecture (quick map)

### Community 14 - "12. Görsel yön"
Cohesion: 0.40
Nodes (5): 12.1 Stil [KARAR], 12.2 Arayüz [KARAR], 12.3 Konsept görseller, 12.4 Konsept görsellerdeki bilinen sorunlar [KARAR: oyunda düzeltilecek], 12. Görsel yön

### Community 15 - "4. Oyun döngüleri"
Cohesion: 0.40
Nodes (5): 4.1 Anlık döngü (saniyeler), 4.2 Oturum döngüsü (1–5 dakika), 4.3 Meta döngü (günler/haftalar), 4.4 Kaynak akışı, 4. Oyun döngüleri

### Community 16 - "ShelfView"
Cohesion: 0.12
Nodes (9): ShelfView, Category, FilledCount, HasFreeSlot, IsFull, LabelAnchor, Size, ProgressionPlayTests (+1 more)

### Community 17 - "OverviewController"
Cohesion: 0.13
Nodes (5): OverviewController, BuildingBounds, InputEnabled, IsActive, Rooms

### Community 18 - "SectionHud"
Cohesion: 0.09
Nodes (7): SectionHud, CurrentMode, IsBannerVisible, IsBookOpen, IsInspecting, IsMapOpen, UiScale

### Community 19 - "SectionController"
Cohesion: 0.07
Nodes (14): SectionController, Collectibles, CommonItems, Containers, Definition, DirtCleaned, HasDirt, IsComplete (+6 more)

### Community 20 - "ToolDefinition"
Cohesion: 0.07
Nodes (15): CategoryMastery, ToolProgress, Wallet, Coins, ToolType, Broom, Hand, Magnet (+7 more)

### Community 21 - "SectionProgress"
Cohesion: 0.16
Nodes (12): SectionProgress, CollectiblesRemaining, DirtCleaned, Fraction, HasDirt, IsComplete, OnlyCollectiblesLeft, Percent (+4 more)

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

### Community 28 - "DirtMask"
Cohesion: 0.08
Nodes (9): DirtMask, CleanedFraction, HasDirt, Height, InitialTotal, Width, DirtLayerView, Tile (+1 more)

### Community 29 - "15. Teknik notlar (Unity)"
Cohesion: 0.33
Nodes (6): 15.1 Genel, 15.3 Performans, 15.4 Kayıt ve çevrimdışı ilerleme, 15.5 Entegrasyonlar [AÇIK], 15.6 Dil, 15. Teknik notlar (Unity)

### Community 30 - "PlaceholderFactory"
Cohesion: 0.07
Nodes (12): GameContext, Feel, PlaceholderVisual, SectionVisuals, RoomView, Centre, LabelAnchor, Section (+4 more)

### Community 31 - "ItemDefinition"
Cohesion: 0.15
Nodes (5): GameDatabase, ItemDefinition, CoinValue, IsCollectible, GameDatabaseTests

### Community 32 - "ProceduralTextures"
Cohesion: 0.40
Nodes (3): ProceduralTextures, Rays, SoftDot

### Community 33 - "GameBootstrap"
Cohesion: 0.06
Nodes (14): GameBootstrap, Book, Context, Data, Drag, Flow, Hud, Inspector (+6 more)

### Community 34 - "SaveData"
Cohesion: 0.05
Nodes (25): ContainerSave, IdCount, ItemSave, ItemSaveState, Buried, Floor, Placed, SaveData (+17 more)

### Community 35 - ".Generate"
Cohesion: 0.27
Nodes (4): ContainerContent, SectionLayout, TotalItems, SectionLayoutGenerator

### Community 37 - "Sorting Game (working title: Chubby's Clutter)"
Cohesion: 0.33
Nodes (5): Git rules, graphify, Session routine, Sorting Game (working title: Chubby's Clutter), Verifying (editor must be closed for batchmode)

### Community 38 - "SectionDefinition"
Cohesion: 0.14
Nodes (9): BalanceConfig, CategoryDefinition, ContainerDefinition, ContainerEntry, SectionDefinition, TotalSlotCount, ShelfEntry, Conventions (+1 more)

### Community 39 - "ContainerView"
Cohesion: 0.20
Nodes (3): ContainerView, Definition, IsOpened

### Community 40 - "Screen"
Cohesion: 0.67
Nodes (3): Screen, Overview, Section

### Community 43 - "ShelfInspectView"
Cohesion: 0.17
Nodes (7): ShelfInspectView, Distance, FitDistance, Focus, IsGliding, PanLimit, ShelfInspectViewTests

### Community 46 - "TestGame"
Cohesion: 0.36
Nodes (3): TestGame, Boot, 2026-10-06 — M4.3 implemented (pre-M5 user requests)

### Community 47 - "Session log"
Cohesion: 0.15
Nodes (12): 2026-10-06 — M0 done, approved, 2026-10-06 — M1 implemented and approved (committed), 2026-10-06 — M2 approved; Collection viewer added, 2026-10-06 — M3 round 2 (user feedback), 2026-10-06 — M4.1 implemented (playtest follow-ups), 2026-10-06 — M4.2 implemented (big rooms), Continuing on a new machine (checklist), Current state (+4 more)

### Community 51 - "RareFindPresenter"
Cohesion: 0.17
Nodes (5): RareFindPresenter, DisplayHeight, DisplaySize, IsPresenting, PresentedItem

### Community 53 - "ItemState"
Cohesion: 0.25
Nodes (8): ItemState, Buried, Dragging, Flying, Found, Physics, Placed, Resting

### Community 54 - "ShelfSlot"
Cohesion: 0.21
Nodes (5): ShelfSlot, IsFree, IsReserved, Occupant, WorldBase

### Community 56 - "ShelfInspectTests"
Cohesion: 0.22
Nodes (3): ShelfInspectTests, Boot, Section

### Community 58 - "VenueFlowTests"
Cohesion: 0.26
Nodes (5): VenueFlowTests, Boot, Flow, Section, 2026-10-06 — M4 implemented

### Community 64 - "CollectibleDefinition"
Cohesion: 0.10
Nodes (12): CollectionBook, FoundIds, FindResult, CollectibleDefinition, IsMascot, CollectionBookTests, 9.1.1 Vitrin (`CollectionViewer`) [KARAR], 9.1 Koleksiyon Kitabı (`CollectionBook`) [KARAR] (+4 more)

### Community 65 - "10. Otomasyon ve ilerleme"
Cohesion: 0.40
Nodes (5): 10.1 Aletler (`Tool`) [KARAR], 10.2 Kategori Ustalığı (`CategoryMastery`) [KARAR], 10.3 Yardımcılar (`Helper`) [KARAR], 10.4 Ölçek büyüdükçe oyuncunun rolü, 10. Otomasyon ve ilerleme

### Community 66 - "Mode"
Cohesion: 0.67
Nodes (3): Mode, Overview, Section

### Community 76 - "8. Eşyalar ve kategoriler"
Cohesion: 0.50
Nodes (4): 8.1 Eşya türleri, 8.2 Kategoriler [KARAR], 8.3 Eşya çeşitliliği, 8. Eşyalar ve kategoriler

## Knowledge Gaps
- **249 isolated node(s):** `Current state`, `M4.2 plan + task list (tick as done)`, `Continuing on a new machine (checklist)`, `2026-10-06 — M1 implemented and approved (committed)`, `2026-10-06 — M0 done, approved` (+244 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 477 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **29 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `M1 architecture (quick map)` connect `ItemView` to `.Build`, `ShelfInspector`, `ContentBuilder`, `Sfx`, `DragController`, `CollectionViewer`, `ShelfView`, `SectionHud`, `SectionController`, `ToolDefinition`, `DirtMask`, `PlaceholderFactory`, `GameBootstrap`, `.Generate`, `.Run`, `SectionDefinition`, `ContainerView`, `Session log`, `.Get`, `CoreLoopTests`, `RareFindPresenter`, `ShelfSlot`, `MonoBehaviour`, `CollectibleDefinition`?**
  _High betweenness centrality (0.189) - this node is a cross-community bridge._
- **Why does `SectionController` connect `SectionController` to `.Build`, `ShelfInspector`, `unityengine`, `ItemView`, `DragController`, `.Burst`, `GameFlow`, `ShelfView`, `ToolDefinition`, `SectionProgress`, `DirtMask`, `PlaceholderFactory`, `ItemDefinition`, `SaveData`, `.Run`, `SectionDefinition`, `ContainerView`, `.OnLanded`, `CoreLoopTests`, `MonoBehaviour`, `CollectibleDefinition`?**
  _High betweenness centrality (0.159) - this node is a cross-community bridge._
- **Why does `SectionHud` connect `SectionHud` to `GameBootstrap`, `Mode`, `unityengine`, `.Buy`, `.OnDuplicateSold`, `.Run`, `ItemView`, `.OnCategoryMastered`, `.Bind`, `M4.3 architecture (quick map)`, `.BuildSection`, `GameFlow`, `.Get`, `.ShowOverviewNow`, `MonoBehaviour`?**
  _High betweenness centrality (0.147) - this node is a cross-community bridge._
- **Are the 2 inferred relationships involving `SectionHud` (e.g. with `M1 architecture (quick map)` and `M4 architecture (quick map)`) actually correct?**
  _`SectionHud` has 2 INFERRED edges - model-reasoned connections that need verification._
- **Are the 2 inferred relationships involving `GameBootstrap` (e.g. with `2026-10-06 — M3 implemented` and `M1 architecture (quick map)`) actually correct?**
  _`GameBootstrap` has 2 INFERRED edges - model-reasoned connections that need verification._
- **What connects `Current state`, `M4.2 plan + task list (tick as done)`, `Continuing on a new machine (checklist)` to the rest of the system?**
  _249 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `ShelfInspector` be split into smaller, more focused modules?**
  _Cohesion score 0.057859703020993344 - nodes in this community are weakly interconnected._