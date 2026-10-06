# Graph Report - SortingGame  (2026-10-06)

## Corpus Check
- 69 files · ~42,659 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 261 file(s) not represented in the graph (top: .meta 170, .asset 74, (none) 4)

## Summary
- 1187 nodes · 2979 edges · 70 communities (42 shown, 28 thin omitted)
- Extraction: 89% EXTRACTED · 11% INFERRED · 0% AMBIGUOUS · INFERRED: 319 edges (avg confidence: 0.85)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `b24c6d3f`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- GDD — Chubby's Clutter (çalışma adı)
- .DeliverStack
- unityengine
- ContentBuilder
- ItemView
- Sfx
- DragController
- ShelfView
- 5. Mekânlar ve bölümler
- CollectionViewer
- unity.sh
- VenueDefinition
- GameFlow
- 12. Görsel yön
- 4. Oyun döngüleri
- GameDatabase
- OverviewController
- SectionHud
- SectionController
- CategoryDefinition
- SectionProgress
- VenueFlowTests
- 11. Ekonomi ve gelir modeli
- 7. Etkileşim ve kontroller
- 16. MVP kapsamı
- 6. Görünümler ve kamera
- CollectibleDefinition
- DirtMask
- 15. Teknik notlar (Unity)
- PlaceholderFactory
- ProceduralTextures
- GameBootstrap
- ContainerDefinition
- .Create
- .Run
- .OnLanded
- DirtLayerView
- ToolType
- SectionDefinition
- SaveData
- ShelfSlot
- ItemState
- Session log
- 8. Eşyalar ve kategoriler
- VenueState
- GameContext
- CoreLoopTests
- .Burst
- PlaceholderShape
- .CreateMainScene
- .Present
- .Format
- Wallet
- BalanceConfig
- TestGame
- 9. Koleksiyon sistemi
- 10. Otomasyon ve ilerleme
- Mode

## God Nodes (most connected - your core abstractions)
1. `SectionController` - 93 edges
2. `SectionHud` - 93 edges
3. `ItemView` - 63 edges
4. `GameBootstrap` - 55 edges
5. `SortingGame.Data` - 46 edges
6. `DragController` - 45 edges
7. `SortingGame.Core` - 44 edges
8. `M1 architecture (quick map)` - 42 edges
9. `SectionDefinition` - 40 edges
10. `ShelfView` - 39 edges

## Surprising Connections (you probably didn't know these)
- `10.2 Kategori Ustalığı (`CategoryMastery`) [KARAR]` --references--> `CategoryMastery`  [INFERRED]
  Docs/GDD.md → Assets/_Project/Scripts/Runtime/Core/CategoryMastery.cs
- `9.1 Koleksiyon Kitabı (`CollectionBook`) [KARAR]` --references--> `CollectionBook`  [INFERRED]
  Docs/GDD.md → Assets/_Project/Scripts/Runtime/Core/CollectionBook.cs
- `8.1 Eşya türleri` --references--> `ContainerDefinition`  [INFERRED]
  Docs/GDD.md → Assets/_Project/Scripts/Runtime/Data/ContainerDefinition.cs
- `9.1.1 Vitrin (`CollectionViewer`) [KARAR]` --references--> `CollectionViewer`  [INFERRED]
  Docs/GDD.md → Assets/_Project/Scripts/Runtime/Section/CollectionViewer.cs
- `M1 architecture (quick map)` --references--> `ContentBuilder`  [INFERRED]
  Docs/PROGRESS.md → Assets/_Project/Scripts/Editor/ContentBuilder.cs

## Import Cycles
- None detected.

## Communities (70 total, 28 thin omitted)

### Community 1 - "GDD — Chubby's Clutter (çalışma adı)"
Cohesion: 0.20
Nodes (9): 0. Bu belge nasıl okunmalı, 13. Fikri mülkiyet ve özgünlük kuralları [KARAR], 14. Ses ve haptik, 18. Açık sorular, 19. Karar günlüğü, 1. Özet, 2. Değişmez prensipler, 3. Referanslar ve farklılaşma (+1 more)

### Community 3 - "unityengine"
Cohesion: 0.07
Nodes (9): BuildTools, SortingGame.Tests, SortingGame.Data, SortingGame.UI, SortingGame.Core, SortingGame.Overview, SortingGame.EditorTools, SortingGame.Section (+1 more)

### Community 5 - "ItemView"
Cohesion: 0.16
Nodes (8): ItemView, CanPick, CanTapToFind, Definition, IsCollectible, SavePose, State, VisualSize

### Community 6 - "Sfx"
Cohesion: 0.06
Nodes (28): Sfx, BookStamp, CleanAmbienceLoop, Coin, DuplicateSold, Magnet, Mastery, Pickup (+20 more)

### Community 7 - "DragController"
Cohesion: 0.08
Nodes (8): ContainerView, Definition, IsOpened, DragController, Capacity, Carried, InputEnabled, Tool

### Community 8 - "ShelfView"
Cohesion: 0.05
Nodes (17): TweenRunner, FeelConfig, CameraFitter, RareFindPresenter, DisplayHeight, DisplaySize, IsPresenting, PresentedItem (+9 more)

### Community 9 - "5. Mekânlar ve bölümler"
Cohesion: 0.29
Nodes (7): 5.1 Yapı [KARAR], 5.2 İlerleme sırası [VARSAYILAN], 5.3 Mekân merdiveni (taslak), 5.4 Bölüm kilitleri [VARSAYILAN], 5.5 Hafif renovasyon [KARAR], 5.6 Mekânın satışı [KARAR], 5. Mekânlar ve bölümler

### Community 10 - "CollectionViewer"
Cohesion: 0.10
Nodes (12): OrbitView, IsIdleSpinning, Pan, Pitch, Yaw, Zoom, CollectionViewer, FrustumHeight (+4 more)

### Community 12 - "VenueDefinition"
Cohesion: 0.22
Nodes (6): SectionStatus, Percent, VenueProgress, VenueStatus, Fraction, VenueDefinition

### Community 13 - "GameFlow"
Cohesion: 0.11
Nodes (10): GameFlow, ActiveSection, Busy, Current, Data, Venue, Screen, Overview (+2 more)

### Community 14 - "12. Görsel yön"
Cohesion: 0.40
Nodes (5): 12.1 Stil [KARAR], 12.2 Arayüz [KARAR], 12.3 Konsept görseller, 12.4 Konsept görsellerdeki bilinen sorunlar [KARAR: oyunda düzeltilecek], 12. Görsel yön

### Community 15 - "4. Oyun döngüleri"
Cohesion: 0.40
Nodes (5): 4.1 Anlık döngü (saniyeler), 4.2 Oturum döngüsü (1–5 dakika), 4.3 Meta döngü (günler/haftalar), 4.4 Kaynak akışı, 4. Oyun döngüleri

### Community 17 - "OverviewController"
Cohesion: 0.14
Nodes (5): OverviewController, BuildingBounds, InputEnabled, IsActive, Rooms

### Community 18 - "SectionHud"
Cohesion: 0.11
Nodes (6): SectionHud, CurrentMode, IsBookOpen, IsMapOpen, UiScale, M1 architecture (quick map)

### Community 19 - "SectionController"
Cohesion: 0.09
Nodes (14): SectionController, Collectibles, CommonItems, Containers, Definition, DirtCleaned, HasDirt, IsComplete (+6 more)

### Community 20 - "CategoryDefinition"
Cohesion: 0.06
Nodes (13): CategoryMastery, FileSaveStorage, ISaveStorage, SaveSystem, ToolProgress, CategoryDefinition, Level, ToolDefinition (+5 more)

### Community 21 - "SectionProgress"
Cohesion: 0.16
Nodes (12): SectionProgress, CollectiblesRemaining, DirtCleaned, Fraction, HasDirt, IsComplete, OnlyCollectiblesLeft, Percent (+4 more)

### Community 22 - "VenueFlowTests"
Cohesion: 0.17
Nodes (5): TestSnapshots, VenueFlowTests, Boot, Flow, Section

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

### Community 27 - "CollectibleDefinition"
Cohesion: 0.06
Nodes (15): CollectionBook, FoundIds, FindResult, ContainerContent, SectionLayout, TotalItems, SectionLayoutGenerator, CollectibleDefinition (+7 more)

### Community 28 - "DirtMask"
Cohesion: 0.13
Nodes (7): DirtMask, CleanedFraction, HasDirt, Height, InitialTotal, Width, DirtMaskTests

### Community 29 - "15. Teknik notlar (Unity)"
Cohesion: 0.33
Nodes (6): 15.1 Genel, 15.3 Performans, 15.4 Kayıt ve çevrimdışı ilerleme, 15.5 Entegrasyonlar [AÇIK], 15.6 Dil, 15. Teknik notlar (Unity)

### Community 30 - "PlaceholderFactory"
Cohesion: 0.09
Nodes (9): SectionVisuals, RoomView, Centre, LabelAnchor, Section, Size, Status, PlaceholderFactory (+1 more)

### Community 32 - "ProceduralTextures"
Cohesion: 0.40
Nodes (3): ProceduralTextures, Rays, SoftDot

### Community 33 - "GameBootstrap"
Cohesion: 0.11
Nodes (13): GameBootstrap, Book, Context, Data, Drag, Flow, Hud, Overview (+5 more)

### Community 39 - "ToolType"
Cohesion: 0.25
Nodes (5): ToolType, Broom, Hand, Magnet, ShelfLabel

### Community 40 - "SectionDefinition"
Cohesion: 0.12
Nodes (5): SectionDefinition, TotalSlotCount, ShelfEntry, VenueProgressTests, 15.2 Veri odaklı tasarım

### Community 41 - "SaveData"
Cohesion: 0.22
Nodes (10): ContainerSave, IdCount, ItemSave, ItemSaveState, Buried, Floor, Placed, SaveData (+2 more)

### Community 42 - "ShelfSlot"
Cohesion: 0.27
Nodes (5): ShelfSlot, IsFree, IsReserved, Occupant, WorldBase

### Community 43 - "ItemState"
Cohesion: 0.25
Nodes (8): ItemState, Buried, Dragging, Flying, Found, Physics, Placed, Resting

### Community 45 - "Session log"
Cohesion: 0.17
Nodes (11): 2026-10-06 — M0 done, approved, 2026-10-06 — M1 implemented and approved (committed), 2026-10-06 — M2 approved; Collection viewer added, 2026-10-06 — M3 round 2 (user feedback), 2026-10-06 — M4.1 implemented (playtest follow-ups), 2026-10-06 — M4 implemented, Current state, Environment notes (+3 more)

### Community 46 - "8. Eşyalar ve kategoriler"
Cohesion: 0.50
Nodes (4): 8.1 Eşya türleri, 8.2 Kategoriler [KARAR], 8.3 Eşya çeşitliliği, 8. Eşyalar ve kategoriler

### Community 47 - "VenueState"
Cohesion: 0.50
Nodes (4): VenueState, Completed, Locked, Open

### Community 52 - "PlaceholderShape"
Cohesion: 0.17
Nodes (11): ItemRarity, Common, Mascot, Rare, PlaceholderShape, Book, Capsule, Cube (+3 more)

### Community 58 - "Wallet"
Cohesion: 0.27
Nodes (3): Wallet, Coins, WalletTests

### Community 60 - "BalanceConfig"
Cohesion: 0.25
Nodes (7): BalanceConfig, Conventions, Git rules, graphify, Session routine, Sorting Game (working title: Chubby's Clutter), Verifying (editor must be closed for batchmode)

### Community 64 - "9. Koleksiyon sistemi"
Cohesion: 0.33
Nodes (6): 9.1.1 Vitrin (`CollectionViewer`) [KARAR], 9.1 Koleksiyon Kitabı (`CollectionBook`) [KARAR], 9.2 Maskot [KARAR], 9.3 Nadir eşya bulma anı [KARAR], 9.4 Set bonusları [KARAR], 9. Koleksiyon sistemi

### Community 65 - "10. Otomasyon ve ilerleme"
Cohesion: 0.40
Nodes (5): 10.1 Aletler (`Tool`) [KARAR], 10.2 Kategori Ustalığı (`CategoryMastery`) [KARAR], 10.3 Yardımcılar (`Helper`) [KARAR], 10.4 Ölçek büyüdükçe oyuncunun rolü, 10. Otomasyon ve ilerleme

### Community 66 - "Mode"
Cohesion: 0.67
Nodes (3): Mode, Overview, Section

## Knowledge Gaps
- **229 isolated node(s):** `FoundIds`, `Width`, `Height`, `HasDirt`, `CleanedFraction` (+224 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 407 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **28 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `SectionController` connect `SectionController` to `.Build`, `.DeliverStack`, `unityengine`, `ItemView`, `DragController`, `ShelfView`, `GameFlow`, `GameDatabase`, `SectionHud`, `SectionProgress`, `VenueFlowTests`, `CollectibleDefinition`, `DirtMask`, `PlaceholderFactory`, `.Sweep`, `GameBootstrap`, `.Run`, `.OnLanded`, `DirtLayerView`, `SectionDefinition`, `SaveData`, `Vector3`, `GameContext`, `CoreLoopTests`, `.Burst`, `Wallet`?**
  _High betweenness centrality (0.184) - this node is a cross-community bridge._
- **Why does `SectionHud` connect `SectionHud` to `unityengine`, `ShelfView`, `VenueDefinition`, `GameFlow`, `SectionController`, `CollectibleDefinition`, `PlaceholderFactory`, `GameBootstrap`, `.Run`, `ToolType`, `SectionDefinition`, `GameContext`, `.Get`, `.OnDuplicateSold`, `.Format`, `Wallet`, `.ShowOverviewNow`, `.PlayCollectionComplete`, `Mode`, `.ShowMap`?**
  _High betweenness centrality (0.128) - this node is a cross-community bridge._
- **Why does `GameBootstrap` connect `GameBootstrap` to `.DeliverStack`, `unityengine`, `DragController`, `ShelfView`, `CollectionViewer`, `GameFlow`, `GameDatabase`, `OverviewController`, `SectionHud`, `SectionController`, `CategoryDefinition`, `VenueFlowTests`, `CollectibleDefinition`, `PlaceholderFactory`, `SaveData`, `GameContext`, `CoreLoopTests`, `.CreateMainScene`, `Wallet`, `.ShowOverviewNow`, `TestGame`?**
  _High betweenness centrality (0.126) - this node is a cross-community bridge._
- **Are the 2 inferred relationships involving `SectionHud` (e.g. with `M1 architecture (quick map)` and `M4 architecture (quick map)`) actually correct?**
  _`SectionHud` has 2 INFERRED edges - model-reasoned connections that need verification._
- **Are the 2 inferred relationships involving `GameBootstrap` (e.g. with `2026-10-06 — M3 implemented` and `M1 architecture (quick map)`) actually correct?**
  _`GameBootstrap` has 2 INFERRED edges - model-reasoned connections that need verification._
- **What connects `FoundIds`, `Width`, `Height` to the rest of the system?**
  _229 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `unityengine` be split into smaller, more focused modules?**
  _Cohesion score 0.0688629604209563 - nodes in this community are weakly interconnected._