# Graph Report - SortingGame  (2026-10-06)

## Corpus Check
- 75 files · ~52,751 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 281 file(s) not represented in the graph (top: .meta 183, .asset 81, (none) 4)

## Summary
- 1336 nodes · 3414 edges · 82 communities (52 shown, 30 thin omitted)
- Extraction: 88% EXTRACTED · 12% INFERRED · 0% AMBIGUOUS · INFERRED: 403 edges (avg confidence: 0.89)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `d668a3ee`
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
- DirtLayerView
- 5. Mekânlar ve bölümler
- CollectionViewer
- unity.sh
- Fx
- GameFlow
- 12. Görsel yön
- 4. Oyun döngüleri
- ProgressionPlayTests
- ShelfShowcase
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
- VenueProgress
- PlaceholderFactory
- GameDatabase
- GameContext
- GameBootstrap
- SectionDefinition
- Session log
- .Run
- Sorting Game (working title: Chubby's Clutter)
- CameraFitter
- ShelfView
- PlaceholderShape
- .ShowBookPage
- ProgressionTests
- PlaceholderVisual
- .OnLanded
- ContainerDefinition
- ToolType
- SaveData
- CoreLoopTests
- RareFindPresenter
- .Garage_IsBig_AndPansEndToEnd
- ItemState
- M1 architecture (quick map)
- ShelfInspectTests
- Wallet
- CategoryDefinition
- MonoBehaviour
- 9. Koleksiyon sistemi
- VenueFlowTests
- .CreateMainScene
- ItemDefinition
- 10. Otomasyon ve ilerleme
- Mode
- .Bind
- Progress & Handoff
- AutoSortBoost
- ItemSaveState
- FeelConfig
- 8. Eşyalar ve kategoriler
- Loc

## God Nodes (most connected - your core abstractions)
1. `SectionHud` - 114 edges
2. `SectionController` - 98 edges
3. `ItemView` - 62 edges
4. `GameBootstrap` - 57 edges
5. `DragController` - 52 edges
6. `SortingGame.Core` - 50 edges
7. `ShelfView` - 49 edges
8. `SortingGame.Data` - 46 edges
9. `SectionDefinition` - 43 edges
10. `M1 architecture (quick map)` - 42 edges

## Surprising Connections (you probably didn't know these)
- `10.2 Oto Sort güçlendirmesi (`AutoSortBoost`) [KARAR]` --references--> `AutoSortBoost`  [INFERRED]
  Docs/GDD.md → Assets/_Project/Scripts/Runtime/Core/AutoSortBoost.cs
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

## Communities (82 total, 30 thin omitted)

### Community 1 - "GDD — Chubby's Clutter (çalışma adı)"
Cohesion: 0.20
Nodes (9): 0. Bu belge nasıl okunmalı, 13. Fikri mülkiyet ve özgünlük kuralları [KARAR], 14. Ses ve haptik, 18. Açık sorular, 19. Karar günlüğü, 1. Özet, 2. Değişmez prensipler, 3. Referanslar ve farklılaşma (+1 more)

### Community 2 - "ShelfInspector"
Cohesion: 0.09
Nodes (13): ShelfInspectView, Distance, FitDistance, Focus, IsGliding, PanLimit, ShelfInspector, IsOpen (+5 more)

### Community 3 - "unityengine"
Cohesion: 0.07
Nodes (9): BuildTools, SortingGame.Tests, SortingGame.Data, SortingGame.UI, SortingGame.Core, SortingGame.Overview, SortingGame.EditorTools, SortingGame.Section (+1 more)

### Community 5 - "ItemView"
Cohesion: 0.15
Nodes (9): ItemView, CanPick, CanTapToFind, Definition, IsCollectible, IsRare, SavePose, State (+1 more)

### Community 6 - "Sfx"
Cohesion: 0.07
Nodes (24): Sfx, BookStamp, CleanAmbienceLoop, Coin, Magnet, Mastery, Pickup, PlaceMetal (+16 more)

### Community 7 - "DragController"
Cohesion: 0.12
Nodes (6): DragController, Capacity, Carried, Fitter, InputEnabled, Tool

### Community 9 - "5. Mekânlar ve bölümler"
Cohesion: 0.29
Nodes (7): 5.1 Yapı [KARAR], 5.2 İlerleme sırası [VARSAYILAN], 5.3 Mekân merdiveni (taslak), 5.4 Bölüm kilitleri [VARSAYILAN], 5.5 Hafif renovasyon [KARAR], 5.6 Mekânın tamamlanması [KARAR], 5. Mekânlar ve bölümler

### Community 10 - "CollectionViewer"
Cohesion: 0.09
Nodes (12): OrbitView, IsIdleSpinning, Pan, Pitch, Yaw, Zoom, CollectionViewer, FrustumHeight (+4 more)

### Community 13 - "GameFlow"
Cohesion: 0.10
Nodes (10): GameFlow, ActiveSection, Busy, Current, Data, Venue, Screen, Overview (+2 more)

### Community 14 - "12. Görsel yön"
Cohesion: 0.40
Nodes (5): 12.1 Stil [KARAR], 12.2 Arayüz [KARAR], 12.3 Konsept görseller, 12.4 Konsept görsellerdeki bilinen sorunlar [KARAR: oyunda düzeltilecek], 12. Görsel yön

### Community 15 - "4. Oyun döngüleri"
Cohesion: 0.40
Nodes (5): 4.1 Anlık döngü (saniyeler), 4.2 Oturum döngüsü (1–5 dakika), 4.3 Meta döngü (günler/haftalar), 4.4 Kaynak akışı, 4. Oyun döngüleri

### Community 17 - "ShelfShowcase"
Cohesion: 0.17
Nodes (3): ShelfShowcase, IsPlaying, 2026-10-06 — M4.1 implemented (playtest follow-ups)

### Community 18 - "SectionHud"
Cohesion: 0.08
Nodes (8): SectionHud, CurrentMode, IsBannerVisible, IsBookOpen, IsBoostOpen, IsInspecting, IsMapOpen, UiScale

### Community 19 - "SectionController"
Cohesion: 0.08
Nodes (16): SectionController, AutoSortCategory, CanStartAutoSort, Collectibles, Containers, Definition, DirtCleaned, HasDirt (+8 more)

### Community 20 - "ToolDefinition"
Cohesion: 0.29
Nodes (6): ToolProgress, Level, ToolDefinition, MaxLevel, StartsOwned, 2026-10-06 — M3 implemented

### Community 21 - "SectionProgress"
Cohesion: 0.17
Nodes (12): SectionProgress, CollectiblesRemaining, DirtCleaned, Fraction, HasDirt, IsComplete, OnlyCollectiblesLeft, Percent (+4 more)

### Community 22 - ".Capture"
Cohesion: 0.15
Nodes (3): TestGame, Boot, TestSnapshots

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
Cohesion: 0.11
Nodes (6): OverviewController, BuildingBounds, InputEnabled, IsActive, Rooms, M4 architecture (quick map)

### Community 28 - "DirtMask"
Cohesion: 0.17
Nodes (7): DirtMask, CleanedFraction, HasDirt, Height, InitialTotal, Width, DirtMaskTests

### Community 29 - "VenueProgress"
Cohesion: 0.18
Nodes (9): SectionStatus, Percent, VenueProgress, VenueState, Completed, Locked, Open, VenueStatus (+1 more)

### Community 30 - "PlaceholderFactory"
Cohesion: 0.08
Nodes (9): SectionVisuals, RoomView, Centre, LabelAnchor, Section, Size, Status, PlaceholderFactory (+1 more)

### Community 31 - "GameDatabase"
Cohesion: 0.18
Nodes (3): GameDatabase, Album, GameDatabaseTests

### Community 32 - "GameContext"
Cohesion: 0.19
Nodes (12): GameContext, Feel, FakeAdProvider, IsRewardedReady, ShownCount, FakeStoreProvider, LastProductId, IAdProvider (+4 more)

### Community 33 - "GameBootstrap"
Cohesion: 0.09
Nodes (14): GameBootstrap, Book, Context, Data, Drag, Flow, Hud, Inspector (+6 more)

### Community 34 - "SectionDefinition"
Cohesion: 0.15
Nodes (5): ContainerEntry, SectionDefinition, TotalSlotCount, ShelfEntry, VenueProgressTests

### Community 35 - "Session log"
Cohesion: 0.15
Nodes (11): ProceduralTextures, Rays, SoftDot, 2026-10-06 — M0 done, approved, 2026-10-06 — M1 implemented and approved (committed), 2026-10-06 — M2 approved; Collection viewer added, 2026-10-06 — M3 round 2 (user feedback), 2026-10-06 — M4.x approved; progression redesign written down (no code) (+3 more)

### Community 37 - "Sorting Game (working title: Chubby's Clutter)"
Cohesion: 0.33
Nodes (5): Git rules, graphify, Session routine, Sorting Game (working title: Chubby's Clutter), Verifying (editor must be closed for batchmode)

### Community 38 - "CameraFitter"
Cohesion: 0.18
Nodes (6): CameraFitter, CanPan, PanMax, PanMin, PanX, 2026-10-06 — M4.2 implemented (big rooms)

### Community 39 - "ShelfView"
Cohesion: 0.15
Nodes (7): ShelfView, Category, FilledCount, HasFreeSlot, IsFull, LabelAnchor, Size

### Community 40 - "PlaceholderShape"
Cohesion: 0.29
Nodes (7): PlaceholderShape, Book, Capsule, Cube, Cylinder, Rod, Sphere

### Community 45 - "ContainerDefinition"
Cohesion: 0.14
Nodes (10): AutoSortPack, BalanceConfig, ContainerDefinition, 15.1 Genel, 15.2 Veri odaklı tasarım, 15.3 Performans, 15.4 Kayıt, 15.5 Entegrasyonlar [AÇIK] (+2 more)

### Community 47 - "ToolType"
Cohesion: 0.24
Nodes (4): ToolType, Broom, Hand, Magnet

### Community 48 - "SaveData"
Cohesion: 0.14
Nodes (8): ContainerSave, IdCount, ItemSave, SaveData, SectionSave, VenueSave, ISaveStorage, SaveSystem

### Community 51 - "RareFindPresenter"
Cohesion: 0.17
Nodes (5): RareFindPresenter, DisplayHeight, DisplaySize, IsPresenting, PresentedItem

### Community 53 - "ItemState"
Cohesion: 0.25
Nodes (8): ItemState, Buried, Dragging, Flying, Found, Physics, Placed, Resting

### Community 54 - "M1 architecture (quick map)"
Cohesion: 0.19
Nodes (6): ShelfSlot, IsFree, IsReserved, Occupant, WorldBase, M1 architecture (quick map)

### Community 55 - "ShelfInspectTests"
Cohesion: 0.12
Nodes (5): FileSaveStorage, ShelfInspectTests, Boot, Section, 2026-10-06 — M4.3 implemented (pre-M5 user requests)

### Community 56 - "Wallet"
Cohesion: 0.38
Nodes (3): Wallet, Coins, WalletTests

### Community 58 - "CategoryDefinition"
Cohesion: 0.25
Nodes (5): CategoryDefinition, PlaceSoundType, Metal, Paper, Plastic

### Community 60 - "9. Koleksiyon sistemi"
Cohesion: 0.29
Nodes (7): 9.1.1 Vitrin (`CollectionViewer`) [KARAR], 9.1 Koleksiyon Kitabı (`CollectionBook`) [KARAR], 9.2 Maskot [KARAR], 9.3 Chubby bulma anı [KARAR], 9.4 Nadir (mavi) eşyalar [KARAR], 9.5 Set bonusları [AÇIK], 9. Koleksiyon sistemi

### Community 61 - "VenueFlowTests"
Cohesion: 0.12
Nodes (5): VenueFlowTests, Boot, Flow, Section, 2026-10-06 — M4 implemented

### Community 64 - "ItemDefinition"
Cohesion: 0.06
Nodes (16): CollectionBook, FoundIds, ContainerContent, SectionLayoutGenerator, CollectibleDefinition, IsMascot, ItemRarity, Common (+8 more)

### Community 65 - "10. Otomasyon ve ilerleme"
Cohesion: 0.40
Nodes (5): 10.1 Aletler (`Tool`) [KARAR], 10.2 Oto Sort güçlendirmesi (`AutoSortBoost`) [KARAR], 10.3 Yardımcılar (`Helper`) [KARAR], 10.4 Ölçek büyüdükçe oyuncunun rolü, 10. Otomasyon ve ilerleme

### Community 66 - "Mode"
Cohesion: 0.67
Nodes (3): Mode, Overview, Section

### Community 72 - "Progress & Handoff"
Cohesion: 0.33
Nodes (5): Continuing on a new machine (checklist), Current state, Environment notes, M4.2 plan + task list (tick as done), Progress & Handoff

### Community 75 - "ItemSaveState"
Cohesion: 0.50
Nodes (4): ItemSaveState, Buried, Floor, Placed

### Community 76 - "FeelConfig"
Cohesion: 0.12
Nodes (5): Ease, FeelConfig, ContainerView, Definition, IsOpened

### Community 78 - "8. Eşyalar ve kategoriler"
Cohesion: 0.50
Nodes (4): 8.1 Eşya türleri, 8.2 Kategoriler [KARAR], 8.3 Eşya çeşitliliği, 8. Eşyalar ve kategoriler

## Knowledge Gaps
- **263 isolated node(s):** `Charges`, `FoundIds`, `Width`, `Height`, `HasDirt` (+258 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 455 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **30 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `SectionController` connect `SectionController` to `.PlaceBuried`, `unityengine`, `ItemView`, `DragController`, `DirtLayerView`, `Fx`, `GameFlow`, `ProgressionPlayTests`, `SectionHud`, `SectionProgress`, `OverviewController`, `DirtMask`, `PlaceholderFactory`, `GameDatabase`, `GameContext`, `GameBootstrap`, `SectionDefinition`, `.Run`, `ShelfView`, `.OnLanded`, `Vector3`, `SaveData`, `CoreLoopTests`, `M1 architecture (quick map)`, `ShelfInspectTests`, `Wallet`, `CategoryDefinition`, `MonoBehaviour`, `VenueFlowTests`, `.OnRelease`, `ItemDefinition`, `.Bind`, `.Build`, `FeelConfig`?**
  _High betweenness centrality (0.196) - this node is a cross-community bridge._
- **Are the 2 inferred relationships involving `SectionHud` (e.g. with `M1 architecture (quick map)` and `M4 architecture (quick map)`) actually correct?**
  _`SectionHud` has 2 INFERRED edges - model-reasoned connections that need verification._
- **What connects `Charges`, `FoundIds`, `Width` to the rest of the system?**
  _263 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `ShelfInspector` be split into smaller, more focused modules?**
  _Cohesion score 0.08787878787878788 - nodes in this community are weakly interconnected._
- **Why does `M1 architecture (quick map)` connect `M1 architecture (quick map)` to `ContentBuilder`, `ItemView`, `Sfx`, `DragController`, `DirtLayerView`, `CollectionViewer`, `SectionHud`, `SectionController`, `ToolDefinition`, `DirtMask`, `PlaceholderFactory`, `GameContext`, `GameBootstrap`, `SectionDefinition`, `.Run`, `CameraFitter`, `ShelfView`, `.ShowBookPage`, `ContainerDefinition`, `ToolType`, `.Get`, `CoreLoopTests`, `RareFindPresenter`, `Wallet`, `MonoBehaviour`, `.CreateMainScene`, `ItemDefinition`, `.Build`, `Progress & Handoff`, `FeelConfig`?**
  _High betweenness centrality (0.128) - this node is a cross-community bridge._
- **Are the 2 inferred relationships involving `GameBootstrap` (e.g. with `2026-10-06 — M3 implemented` and `M1 architecture (quick map)`) actually correct?**
  _`GameBootstrap` has 2 INFERRED edges - model-reasoned connections that need verification._
- **Should `unityengine` be split into smaller, more focused modules?**
  _Cohesion score 0.06942983378918578 - nodes in this community are weakly interconnected._