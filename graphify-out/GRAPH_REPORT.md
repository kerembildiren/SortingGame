# Graph Report - SortingGame  (2026-10-06)

## Corpus Check
- 70 files · ~44,550 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 276 file(s) not represented in the graph (top: .meta 178, .asset 81, (none) 4)

## Summary
- 1220 nodes · 3065 edges · 68 communities (44 shown, 24 thin omitted)
- Extraction: 89% EXTRACTED · 11% INFERRED · 0% AMBIGUOUS · INFERRED: 326 edges (avg confidence: 0.86)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `d1dd363a`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- .Build
- GDD — Chubby's Clutter (çalışma adı)
- SaveSystem
- unityengine
- ContentBuilder
- ItemView
- Sfx
- DragController
- ShelfShowcase
- 5. Mekânlar ve bölümler
- CollectionViewer
- unity.sh
- .Burst
- GameFlow
- 12. Görsel yön
- 4. Oyun döngüleri
- .TryPlace
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
- .Generate
- DirtMask
- 15.2 Veri odaklı tasarım
- PlaceholderFactory
- VenueProgressTests
- ProceduralTextures
- GameBootstrap
- ItemDefinition
- M1 architecture (quick map)
- SectionDefinition
- FeelConfig
- VenueDefinition
- SaveData
- .OnLanded
- .LoadIntoSection
- ShelfView
- 8. Eşyalar ve kategoriler
- Wallet
- CoreLoopTests
- RareFindPresenter
- ItemState
- ShelfSlot
- GameContext
- .ShowOverviewNow
- ContainerContent
- MonoBehaviour
- VenueState
- CollectibleDefinition
- 10. Otomasyon ve ilerleme
- Mode

## God Nodes (most connected - your core abstractions)
1. `SectionController` - 93 edges
2. `SectionHud` - 93 edges
3. `ItemView` - 63 edges
4. `GameBootstrap` - 55 edges
5. `DragController` - 51 edges
6. `SortingGame.Data` - 46 edges
7. `SortingGame.Core` - 45 edges
8. `SectionDefinition` - 42 edges
9. `M1 architecture (quick map)` - 42 edges
10. `ShelfView` - 39 edges

## Surprising Connections (you probably didn't know these)
- `10.2 Kategori Ustalığı (`CategoryMastery`) [KARAR]` --references--> `CategoryMastery`  [INFERRED]
  Docs/GDD.md → Assets/_Project/Scripts/Runtime/Core/CategoryMastery.cs
- `9.1 Koleksiyon Kitabı (`CollectionBook`) [KARAR]` --references--> `CollectionBook`  [INFERRED]
  Docs/GDD.md → Assets/_Project/Scripts/Runtime/Core/CollectionBook.cs
- `8.1 Eşya türleri` --references--> `ContainerDefinition`  [INFERRED]
  Docs/GDD.md → Assets/_Project/Scripts/Runtime/Data/ContainerDefinition.cs
- `M1 architecture (quick map)` --references--> `ContentBuilder`  [INFERRED]
  Docs/PROGRESS.md → Assets/_Project/Scripts/Editor/ContentBuilder.cs
- `M4 architecture (quick map)` --references--> `ContentBuilder`  [INFERRED]
  Docs/PROGRESS.md → Assets/_Project/Scripts/Editor/ContentBuilder.cs

## Import Cycles
- None detected.

## Communities (68 total, 24 thin omitted)

### Community 1 - "GDD — Chubby's Clutter (çalışma adı)"
Cohesion: 0.20
Nodes (9): 0. Bu belge nasıl okunmalı, 13. Fikri mülkiyet ve özgünlük kuralları [KARAR], 14. Ses ve haptik, 18. Açık sorular, 19. Karar günlüğü, 1. Özet, 2. Değişmez prensipler, 3. Referanslar ve farklılaşma (+1 more)

### Community 2 - "SaveSystem"
Cohesion: 0.13
Nodes (4): FileSaveStorage, ISaveStorage, SaveSystem, MemoryStorage

### Community 3 - "unityengine"
Cohesion: 0.08
Nodes (7): SortingGame.Tests, SortingGame.Data, SortingGame.UI, SortingGame.Core, SortingGame.Overview, SortingGame.EditorTools, SortingGame.Section

### Community 4 - "ContentBuilder"
Cohesion: 0.08
Nodes (14): ContentBuilder, ProjectSetup, ItemRarity, Common, Mascot, Rare, PlaceholderShape, Book (+6 more)

### Community 5 - "ItemView"
Cohesion: 0.15
Nodes (8): ItemView, CanPick, CanTapToFind, Definition, IsCollectible, SavePose, State, VisualSize

### Community 6 - "Sfx"
Cohesion: 0.06
Nodes (28): Sfx, BookStamp, CleanAmbienceLoop, Coin, DuplicateSold, Magnet, Mastery, Pickup (+20 more)

### Community 7 - "DragController"
Cohesion: 0.06
Nodes (16): ToolType, Broom, Hand, Magnet, CameraFitter, CanPan, PanMax, PanMin (+8 more)

### Community 9 - "5. Mekânlar ve bölümler"
Cohesion: 0.29
Nodes (7): 5.1 Yapı [KARAR], 5.2 İlerleme sırası [VARSAYILAN], 5.3 Mekân merdiveni (taslak), 5.4 Bölüm kilitleri [VARSAYILAN], 5.5 Hafif renovasyon [KARAR], 5.6 Mekânın satışı [KARAR], 5. Mekânlar ve bölümler

### Community 10 - "CollectionViewer"
Cohesion: 0.05
Nodes (31): BuildTools, OrbitView, IsIdleSpinning, Pan, Pitch, Yaw, Zoom, CollectionViewer (+23 more)

### Community 13 - "GameFlow"
Cohesion: 0.10
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
Nodes (6): SectionHud, CurrentMode, IsBookOpen, IsMapOpen, UiScale, ShelfLabel

### Community 19 - "SectionController"
Cohesion: 0.09
Nodes (14): SectionController, Collectibles, CommonItems, Containers, Definition, DirtCleaned, HasDirt, IsComplete (+6 more)

### Community 20 - "CategoryDefinition"
Cohesion: 0.06
Nodes (18): CategoryMastery, ToolProgress, BalanceConfig, CategoryDefinition, GameDatabase, Level, ToolDefinition, MaxLevel (+10 more)

### Community 21 - "SectionProgress"
Cohesion: 0.16
Nodes (12): SectionProgress, CollectiblesRemaining, DirtCleaned, Fraction, HasDirt, IsComplete, OnlyCollectiblesLeft, Percent (+4 more)

### Community 22 - ".Capture"
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

### Community 27 - ".Generate"
Cohesion: 0.20
Nodes (3): ContainerDefinition, ContainerEntry, SectionLayoutGeneratorTests

### Community 28 - "DirtMask"
Cohesion: 0.08
Nodes (9): DirtMask, CleanedFraction, HasDirt, Height, InitialTotal, Width, DirtLayerView, Tile (+1 more)

### Community 29 - "15.2 Veri odaklı tasarım"
Cohesion: 0.29
Nodes (7): 15.1 Genel, 15.2 Veri odaklı tasarım, 15.3 Performans, 15.4 Kayıt ve çevrimdışı ilerleme, 15.5 Entegrasyonlar [AÇIK], 15.6 Dil, 15. Teknik notlar (Unity)

### Community 30 - "PlaceholderFactory"
Cohesion: 0.08
Nodes (10): PlaceholderVisual, SectionVisuals, RoomView, Centre, LabelAnchor, Section, Size, Status (+2 more)

### Community 32 - "ProceduralTextures"
Cohesion: 0.40
Nodes (3): ProceduralTextures, Rays, SoftDot

### Community 33 - "GameBootstrap"
Cohesion: 0.10
Nodes (13): GameBootstrap, Book, Context, Data, Drag, Flow, Hud, Overview (+5 more)

### Community 34 - "ItemDefinition"
Cohesion: 0.15
Nodes (6): ItemDefinition, CoinValue, IsCollectible, ContainerView, Definition, IsOpened

### Community 36 - "M1 architecture (quick map)"
Cohesion: 0.14
Nodes (3): Tween, Runner, M1 architecture (quick map)

### Community 37 - "SectionDefinition"
Cohesion: 0.29
Nodes (3): SectionDefinition, TotalSlotCount, ShelfEntry

### Community 40 - "VenueDefinition"
Cohesion: 0.21
Nodes (6): SectionStatus, Percent, VenueProgress, VenueStatus, Fraction, VenueDefinition

### Community 41 - "SaveData"
Cohesion: 0.21
Nodes (10): ContainerSave, IdCount, ItemSave, ItemSaveState, Buried, Floor, Placed, SaveData (+2 more)

### Community 43 - ".LoadIntoSection"
Cohesion: 0.27
Nodes (3): BigRoomTests, TestGame, Boot

### Community 45 - "ShelfView"
Cohesion: 0.12
Nodes (7): ShelfView, Category, FilledCount, HasFreeSlot, IsFull, LabelAnchor, Size

### Community 46 - "8. Eşyalar ve kategoriler"
Cohesion: 0.50
Nodes (4): 8.1 Eşya türleri, 8.2 Kategoriler [KARAR], 8.3 Eşya çeşitliliği, 8. Eşyalar ve kategoriler

### Community 47 - "Wallet"
Cohesion: 0.27
Nodes (3): Wallet, Coins, WalletTests

### Community 51 - "RareFindPresenter"
Cohesion: 0.16
Nodes (5): RareFindPresenter, DisplayHeight, DisplaySize, IsPresenting, PresentedItem

### Community 53 - "ItemState"
Cohesion: 0.25
Nodes (8): ItemState, Buried, Dragging, Flying, Found, Physics, Placed, Resting

### Community 54 - "ShelfSlot"
Cohesion: 0.25
Nodes (5): ShelfSlot, IsFree, IsReserved, Occupant, WorldBase

### Community 60 - "VenueState"
Cohesion: 0.50
Nodes (4): VenueState, Completed, Locked, Open

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
- **234 isolated node(s):** `FoundIds`, `Width`, `Height`, `HasDirt`, `CleanedFraction` (+229 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 418 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **24 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `SectionController` connect `SectionController` to `.Build`, `unityengine`, `ItemView`, `DragController`, `.Burst`, `GameFlow`, `.TryPlace`, `SectionHud`, `CategoryDefinition`, `SectionProgress`, `.Capture`, `DirtMask`, `PlaceholderFactory`, `GameBootstrap`, `ItemDefinition`, `M1 architecture (quick map)`, `SectionDefinition`, `.Restore`, `FeelConfig`, `SaveData`, `.OnLanded`, `.Sweep`, `ShelfView`, `Wallet`, `CoreLoopTests`, `RareFindPresenter`, `GameContext`, `MonoBehaviour`, `CollectibleDefinition`?**
  _High betweenness centrality (0.178) - this node is a cross-community bridge._
- **Why does `M1 architecture (quick map)` connect `M1 architecture (quick map)` to `.Build`, `ContentBuilder`, `ItemView`, `Sfx`, `DragController`, `CollectionViewer`, `.TryPlace`, `SectionHud`, `SectionController`, `CategoryDefinition`, `.Capture`, `DirtMask`, `PlaceholderFactory`, `GameBootstrap`, `ItemDefinition`, `.ShowBookPage`, `SectionDefinition`, `FeelConfig`, `.Sweep`, `ShelfView`, `Wallet`, `CoreLoopTests`, `.Get`, `RareFindPresenter`, `GameContext`, `ContainerContent`, `MonoBehaviour`, `CollectibleDefinition`?**
  _High betweenness centrality (0.128) - this node is a cross-community bridge._
- **Why does `GameBootstrap` connect `GameBootstrap` to `SaveSystem`, `unityengine`, `ContentBuilder`, `DragController`, `ShelfShowcase`, `CollectionViewer`, `GameFlow`, `.TryPlace`, `OverviewController`, `SectionHud`, `SectionController`, `CategoryDefinition`, `.Capture`, `PlaceholderFactory`, `M1 architecture (quick map)`, `SaveData`, `.LoadIntoSection`, `Wallet`, `CoreLoopTests`, `RareFindPresenter`, `GameContext`, `MonoBehaviour`, `.PlayCollectionComplete`, `CollectibleDefinition`?**
  _High betweenness centrality (0.113) - this node is a cross-community bridge._
- **Are the 2 inferred relationships involving `SectionHud` (e.g. with `M1 architecture (quick map)` and `M4 architecture (quick map)`) actually correct?**
  _`SectionHud` has 2 INFERRED edges - model-reasoned connections that need verification._
- **Are the 2 inferred relationships involving `GameBootstrap` (e.g. with `2026-10-06 — M3 implemented` and `M1 architecture (quick map)`) actually correct?**
  _`GameBootstrap` has 2 INFERRED edges - model-reasoned connections that need verification._
- **Are the 3 inferred relationships involving `DragController` (e.g. with `2026-10-06 — M3 round 2 (user feedback)` and `2026-10-06 — M4.2 implemented (big rooms)`) actually correct?**
  _`DragController` has 3 INFERRED edges - model-reasoned connections that need verification._
- **What connects `FoundIds`, `Width`, `Height` to the rest of the system?**
  _234 weakly-connected nodes found - possible documentation gaps or missing edges._