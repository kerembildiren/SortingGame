# Graph Report - SortingGame  (2026-10-06)

## Corpus Check
- 74 files · ~47,490 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 280 file(s) not represented in the graph (top: .meta 182, .asset 81, (none) 4)

## Summary
- 1292 nodes · 3241 edges · 69 communities (45 shown, 24 thin omitted)
- Extraction: 90% EXTRACTED · 10% INFERRED · 0% AMBIGUOUS · INFERRED: 340 edges (avg confidence: 0.86)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `8a52b483`
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
- .Awake
- 5. Mekânlar ve bölümler
- CollectionViewer
- unity.sh
- .Burst
- GameFlow
- 12. Görsel yön
- 4. Oyun döngüleri
- .DeliverStack
- OverviewController
- SectionHud
- SectionController
- CategoryDefinition
- SectionProgress
- .Capture
- 11. Ekonomi ve gelir modeli
- 7. Etkileşim ve kontroller
- 16. MVP kapsamı
- 6. Görünümler ve kamera
- ItemDefinition
- DirtMask
- 15. Teknik notlar (Unity)
- PlaceholderFactory
- VenueProgressTests
- ProceduralTextures
- GameBootstrap
- SaveData
- DirtLayerView
- .Run
- SectionDefinition
- .PlayDuplicate
- VenueProgress
- GameContext
- .PlayCollectionComplete
- ShelfInspectView
- .OnLanded
- ShelfView
- TestGame
- Wallet
- CoreLoopTests
- RareFindPresenter
- ItemState
- ShelfSlot
- ShelfInspectTests
- .Format
- VenueFlowTests
- MonoBehaviour
- ShelfInspectViewTests
- 9. Koleksiyon sistemi
- CollectibleDefinition
- 10. Otomasyon ve ilerleme
- Mode

## God Nodes (most connected - your core abstractions)
1. `SectionHud` - 99 edges
2. `SectionController` - 94 edges
3. `ItemView` - 63 edges
4. `GameBootstrap` - 58 edges
5. `DragController` - 52 edges
6. `SortingGame.Core` - 49 edges
7. `SortingGame.Data` - 47 edges
8. `ShelfView` - 44 edges
9. `SectionDefinition` - 42 edges
10. `M1 architecture (quick map)` - 42 edges

## Surprising Connections (you probably didn't know these)
- `10.2 Kategori Ustalığı (`CategoryMastery`) [KARAR]` --references--> `CategoryMastery`  [INFERRED]
  Docs/GDD.md → Assets/_Project/Scripts/Runtime/Core/CategoryMastery.cs
- `9.1 Koleksiyon Kitabı (`CollectionBook`) [KARAR]` --references--> `CollectionBook`  [INFERRED]
  Docs/GDD.md → Assets/_Project/Scripts/Runtime/Core/CollectionBook.cs
- `9.1.1 Vitrin (`CollectionViewer`) [KARAR]` --references--> `CollectionViewer`  [INFERRED]
  Docs/GDD.md → Assets/_Project/Scripts/Runtime/Section/CollectionViewer.cs
- `M1 architecture (quick map)` --references--> `ContentBuilder`  [INFERRED]
  Docs/PROGRESS.md → Assets/_Project/Scripts/Editor/ContentBuilder.cs
- `M4 architecture (quick map)` --references--> `ContentBuilder`  [INFERRED]
  Docs/PROGRESS.md → Assets/_Project/Scripts/Editor/ContentBuilder.cs

## Import Cycles
- None detected.

## Communities (69 total, 24 thin omitted)

### Community 1 - "GDD — Chubby's Clutter (çalışma adı)"
Cohesion: 0.20
Nodes (9): 0. Bu belge nasıl okunmalı, 13. Fikri mülkiyet ve özgünlük kuralları [KARAR], 14. Ses ve haptik, 18. Açık sorular, 19. Karar günlüğü, 1. Özet, 2. Değişmez prensipler, 3. Referanslar ve farklılaşma (+1 more)

### Community 2 - "ShelfInspector"
Cohesion: 0.16
Nodes (6): ShelfInspector, IsOpen, Rotation, Shelf, View, WorldPerPixel

### Community 3 - "unityengine"
Cohesion: 0.07
Nodes (9): BuildTools, SortingGame.Tests, SortingGame.Data, SortingGame.UI, SortingGame.Core, SortingGame.Overview, SortingGame.EditorTools, SortingGame.Section (+1 more)

### Community 4 - "ContentBuilder"
Cohesion: 0.06
Nodes (21): ContentBuilder, ProjectSetup, BalanceConfig, ItemRarity, Common, Mascot, Rare, PlaceholderShape (+13 more)

### Community 5 - "ItemView"
Cohesion: 0.16
Nodes (9): ItemView, CanPick, CanTapToFind, Definition, IsCollectible, SavePose, State, VisualSize (+1 more)

### Community 6 - "Sfx"
Cohesion: 0.06
Nodes (28): Sfx, BookStamp, CleanAmbienceLoop, Coin, DuplicateSold, Magnet, Mastery, Pickup (+20 more)

### Community 7 - "DragController"
Cohesion: 0.06
Nodes (16): ToolType, Broom, Hand, Magnet, CameraFitter, CanPan, PanMax, PanMin (+8 more)

### Community 8 - ".Awake"
Cohesion: 0.16
Nodes (3): FeelConfig, ShelfShowcase, IsPlaying

### Community 9 - "5. Mekânlar ve bölümler"
Cohesion: 0.29
Nodes (7): 5.1 Yapı [KARAR], 5.2 İlerleme sırası [VARSAYILAN], 5.3 Mekân merdiveni (taslak), 5.4 Bölüm kilitleri [VARSAYILAN], 5.5 Hafif renovasyon [KARAR], 5.6 Mekânın satışı [KARAR], 5. Mekânlar ve bölümler

### Community 10 - "CollectionViewer"
Cohesion: 0.07
Nodes (22): OrbitView, IsIdleSpinning, Pan, Pitch, Yaw, Zoom, CollectionViewer, FrustumHeight (+14 more)

### Community 13 - "GameFlow"
Cohesion: 0.12
Nodes (9): GameFlow, ActiveSection, Busy, Current, Data, Venue, Screen, Overview (+1 more)

### Community 14 - "12. Görsel yön"
Cohesion: 0.40
Nodes (5): 12.1 Stil [KARAR], 12.2 Arayüz [KARAR], 12.3 Konsept görseller, 12.4 Konsept görsellerdeki bilinen sorunlar [KARAR: oyunda düzeltilecek], 12. Görsel yön

### Community 15 - "4. Oyun döngüleri"
Cohesion: 0.40
Nodes (5): 4.1 Anlık döngü (saniyeler), 4.2 Oturum döngüsü (1–5 dakika), 4.3 Meta döngü (günler/haftalar), 4.4 Kaynak akışı, 4. Oyun döngüleri

### Community 17 - "OverviewController"
Cohesion: 0.13
Nodes (6): OverviewController, BuildingBounds, InputEnabled, IsActive, Rooms, M4 architecture (quick map)

### Community 18 - "SectionHud"
Cohesion: 0.09
Nodes (7): SectionHud, CurrentMode, IsBannerVisible, IsBookOpen, IsInspecting, IsMapOpen, UiScale

### Community 19 - "SectionController"
Cohesion: 0.09
Nodes (14): SectionController, Collectibles, CommonItems, Containers, Definition, DirtCleaned, HasDirt, IsComplete (+6 more)

### Community 20 - "CategoryDefinition"
Cohesion: 0.07
Nodes (11): CategoryMastery, ToolProgress, CategoryDefinition, GameDatabase, Level, ToolDefinition, MaxLevel, StartsOwned (+3 more)

### Community 21 - "SectionProgress"
Cohesion: 0.17
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

### Community 27 - "ItemDefinition"
Cohesion: 0.07
Nodes (17): ContainerContent, SectionLayout, TotalItems, SectionLayoutGenerator, ContainerDefinition, ItemDefinition, CoinValue, IsCollectible (+9 more)

### Community 28 - "DirtMask"
Cohesion: 0.08
Nodes (11): DirtMask, CleanedFraction, HasDirt, Height, InitialTotal, Width, FileSaveStorage, ISaveStorage (+3 more)

### Community 29 - "15. Teknik notlar (Unity)"
Cohesion: 0.33
Nodes (6): 15.1 Genel, 15.3 Performans, 15.4 Kayıt ve çevrimdışı ilerleme, 15.5 Entegrasyonlar [AÇIK], 15.6 Dil, 15. Teknik notlar (Unity)

### Community 30 - "PlaceholderFactory"
Cohesion: 0.09
Nodes (10): PlaceholderVisual, SectionVisuals, RoomView, Centre, LabelAnchor, Section, Size, Status (+2 more)

### Community 32 - "ProceduralTextures"
Cohesion: 0.40
Nodes (3): ProceduralTextures, Rays, SoftDot

### Community 33 - "GameBootstrap"
Cohesion: 0.10
Nodes (14): GameBootstrap, Book, Context, Data, Drag, Flow, Hud, Inspector (+6 more)

### Community 34 - "SaveData"
Cohesion: 0.20
Nodes (10): ContainerSave, IdCount, ItemSave, ItemSaveState, Buried, Floor, Placed, SaveData (+2 more)

### Community 37 - "SectionDefinition"
Cohesion: 0.17
Nodes (4): SectionDefinition, TotalSlotCount, VenueDefinition, 15.2 Veri odaklı tasarım

### Community 40 - "VenueProgress"
Cohesion: 0.18
Nodes (9): SectionStatus, Percent, VenueProgress, VenueState, Completed, Locked, Open, VenueStatus (+1 more)

### Community 41 - "GameContext"
Cohesion: 0.14
Nodes (3): GameContext, Feel, ShelfLabel

### Community 43 - "ShelfInspectView"
Cohesion: 0.21
Nodes (6): ShelfInspectView, Distance, FitDistance, Focus, IsGliding, PanLimit

### Community 45 - "ShelfView"
Cohesion: 0.15
Nodes (7): ShelfView, Category, FilledCount, HasFreeSlot, IsFull, LabelAnchor, Size

### Community 46 - "TestGame"
Cohesion: 0.28
Nodes (3): TestGame, Boot, 2026-10-06 — M4.3 implemented (pre-M5 user requests)

### Community 47 - "Wallet"
Cohesion: 0.27
Nodes (3): Wallet, Coins, WalletTests

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
Cohesion: 0.22
Nodes (5): VenueFlowTests, Boot, Flow, Section, 2026-10-06 — M4 implemented

### Community 62 - "9. Koleksiyon sistemi"
Cohesion: 0.33
Nodes (6): 9.1.1 Vitrin (`CollectionViewer`) [KARAR], 9.1 Koleksiyon Kitabı (`CollectionBook`) [KARAR], 9.2 Maskot [KARAR], 9.3 Nadir eşya bulma anı [KARAR], 9.4 Set bonusları [KARAR], 9. Koleksiyon sistemi

### Community 64 - "CollectibleDefinition"
Cohesion: 0.13
Nodes (6): CollectionBook, FoundIds, FindResult, CollectibleDefinition, IsMascot, CollectionBookTests

### Community 65 - "10. Otomasyon ve ilerleme"
Cohesion: 0.40
Nodes (5): 10.1 Aletler (`Tool`) [KARAR], 10.2 Kategori Ustalığı (`CategoryMastery`) [KARAR], 10.3 Yardımcılar (`Helper`) [KARAR], 10.4 Ölçek büyüdükçe oyuncunun rolü, 10. Otomasyon ve ilerleme

### Community 66 - "Mode"
Cohesion: 0.67
Nodes (3): Mode, Overview, Section

## Knowledge Gaps
- **249 isolated node(s):** `FoundIds`, `Width`, `Height`, `HasDirt`, `CleanedFraction` (+244 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 440 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **24 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `SectionController` connect `SectionController` to `.PlaceBuried`, `unityengine`, `ItemView`, `DragController`, `.Awake`, `.Burst`, `GameFlow`, `.DeliverStack`, `SectionHud`, `CategoryDefinition`, `SectionProgress`, `ItemDefinition`, `DirtMask`, `PlaceholderFactory`, `GameBootstrap`, `SaveData`, `DirtLayerView`, `SectionDefinition`, `.Build`, `GameContext`, `.OnLanded`, `ShelfView`, `Wallet`, `Vector3`, `CoreLoopTests`, `ShelfInspectTests`, `VenueFlowTests`, `MonoBehaviour`, `CollectibleDefinition`?**
  _High betweenness centrality (0.202) - this node is a cross-community bridge._
- **Why does `GameBootstrap` connect `GameBootstrap` to `ShelfInspector`, `unityengine`, `ContentBuilder`, `ItemView`, `DragController`, `.Awake`, `CollectionViewer`, `GameFlow`, `.DeliverStack`, `OverviewController`, `SectionHud`, `SectionController`, `CategoryDefinition`, `DirtMask`, `PlaceholderFactory`, `SaveData`, `SectionDefinition`, `GameContext`, `.PlayCollectionComplete`, `TestGame`, `Wallet`, `CoreLoopTests`, `RareFindPresenter`, `ShelfInspectTests`, `VenueFlowTests`, `MonoBehaviour`, `CollectibleDefinition`?**
  _High betweenness centrality (0.136) - this node is a cross-community bridge._
- **Why does `M1 architecture (quick map)` connect `ItemView` to `ContentBuilder`, `Sfx`, `DragController`, `.Awake`, `CollectionViewer`, `.DeliverStack`, `SectionHud`, `SectionController`, `CategoryDefinition`, `ItemDefinition`, `DirtMask`, `PlaceholderFactory`, `GameBootstrap`, `DirtLayerView`, `.Run`, `SectionDefinition`, `.Build`, `GameContext`, `.PlayCollectionComplete`, `ShelfView`, `Wallet`, `.Get`, `CoreLoopTests`, `RareFindPresenter`, `ShelfSlot`, `MonoBehaviour`, `CollectibleDefinition`?**
  _High betweenness centrality (0.128) - this node is a cross-community bridge._
- **Are the 2 inferred relationships involving `SectionHud` (e.g. with `M1 architecture (quick map)` and `M4 architecture (quick map)`) actually correct?**
  _`SectionHud` has 2 INFERRED edges - model-reasoned connections that need verification._
- **Are the 2 inferred relationships involving `GameBootstrap` (e.g. with `2026-10-06 — M3 implemented` and `M1 architecture (quick map)`) actually correct?**
  _`GameBootstrap` has 2 INFERRED edges - model-reasoned connections that need verification._
- **What connects `FoundIds`, `Width`, `Height` to the rest of the system?**
  _249 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `unityengine` be split into smaller, more focused modules?**
  _Cohesion score 0.06843949701092558 - nodes in this community are weakly interconnected._