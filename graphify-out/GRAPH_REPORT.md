# Graph Report - SortingGame  (2026-10-06)

## Corpus Check
- 74 files · ~50,095 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 280 file(s) not represented in the graph (top: .meta 182, .asset 81, (none) 4)

## Summary
- 1297 nodes · 3280 edges · 75 communities (49 shown, 26 thin omitted)
- Extraction: 89% EXTRACTED · 11% INFERRED · 0% AMBIGUOUS · INFERRED: 373 edges (avg confidence: 0.88)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `4b748b97`
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
- DirtLayerView
- 5. Mekânlar ve bölümler
- CollectionViewer
- unity.sh
- Fx
- GameFlow
- 12. Görsel yön
- 4. Oyun döngüleri
- .Sweep
- FeelConfig
- SectionHud
- SectionController
- ToolDefinition
- SectionProgress
- .Capture
- 11. Ekonomi ve gelir modeli
- 7. Etkileşim ve kontroller
- 16. MVP kapsamı
- 6. Görünümler ve kamera
- .Awake
- DirtMask
- VenueProgress
- OverviewController
- GameDatabase
- CollectionBook
- GameBootstrap
- SaveData
- CategoryDefinition
- .Run
- Sorting Game (working title: Chubby's Clutter)
- SectionDefinition
- ShelfView
- CollectibleDefinition
- Wallet
- SectionLayout
- .OnLanded
- 15. Teknik notlar (Unity)
- Progress & Handoff
- SaveData.cs
- CoreLoopTests
- RareFindPresenter
- .Garage_IsBig_AndPansEndToEnd
- ItemState
- ShelfSlot
- ShelfInspectTests
- .Format
- ProgressionTests
- MonoBehaviour
- 9. Koleksiyon sistemi
- VenueState
- .Generate
- 10. Otomasyon ve ilerleme
- Mode
- GameContext
- .Read
- 8. Eşyalar ve kategoriler

## God Nodes (most connected - your core abstractions)
1. `SectionHud` - 99 edges
2. `SectionController` - 95 edges
3. `ItemView` - 63 edges
4. `GameBootstrap` - 58 edges
5. `DragController` - 52 edges
6. `SortingGame.Core` - 49 edges
7. `SortingGame.Data` - 47 edges
8. `ShelfView` - 44 edges
9. `SectionDefinition` - 42 edges
10. `M1 architecture (quick map)` - 42 edges

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

## Communities (75 total, 26 thin omitted)

### Community 1 - "GDD — Chubby's Clutter (çalışma adı)"
Cohesion: 0.20
Nodes (9): 0. Bu belge nasıl okunmalı, 13. Fikri mülkiyet ve özgünlük kuralları [KARAR], 14. Ses ve haptik, 18. Açık sorular, 19. Karar günlüğü, 1. Özet, 2. Değişmez prensipler, 3. Referanslar ve farklılaşma (+1 more)

### Community 2 - "ShelfInspector"
Cohesion: 0.09
Nodes (13): ShelfInspectView, Distance, FitDistance, Focus, IsGliding, PanLimit, ShelfInspector, IsOpen (+5 more)

### Community 3 - "unityengine"
Cohesion: 0.07
Nodes (9): BuildTools, SortingGame.Tests, SortingGame.Data, SortingGame.UI, SortingGame.Core, SortingGame.Overview, SortingGame.EditorTools, SortingGame.Section (+1 more)

### Community 4 - "ContentBuilder"
Cohesion: 0.08
Nodes (14): ContentBuilder, ProjectSetup, ItemRarity, Common, Mascot, Rare, PlaceholderShape, Book (+6 more)

### Community 5 - "ItemView"
Cohesion: 0.16
Nodes (9): ItemView, CanPick, CanTapToFind, Definition, IsCollectible, SavePose, State, VisualSize (+1 more)

### Community 6 - "Sfx"
Cohesion: 0.06
Nodes (28): Sfx, BookStamp, CleanAmbienceLoop, Coin, DuplicateSold, Magnet, Mastery, Pickup (+20 more)

### Community 7 - "DragController"
Cohesion: 0.06
Nodes (16): ToolType, Broom, Hand, Magnet, CameraFitter, CanPan, PanMax, PanMin (+8 more)

### Community 9 - "5. Mekânlar ve bölümler"
Cohesion: 0.29
Nodes (7): 5.1 Yapı [KARAR], 5.2 İlerleme sırası [VARSAYILAN], 5.3 Mekân merdiveni (taslak), 5.4 Bölüm kilitleri [VARSAYILAN], 5.5 Hafif renovasyon [KARAR], 5.6 Mekânın tamamlanması [KARAR], 5. Mekânlar ve bölümler

### Community 10 - "CollectionViewer"
Cohesion: 0.06
Nodes (23): OrbitView, IsIdleSpinning, Pan, Pitch, Yaw, Zoom, CollectionViewer, FrustumHeight (+15 more)

### Community 13 - "GameFlow"
Cohesion: 0.08
Nodes (11): GameFlow, ActiveSection, Busy, Current, Data, Venue, Screen, Overview (+3 more)

### Community 14 - "12. Görsel yön"
Cohesion: 0.40
Nodes (5): 12.1 Stil [KARAR], 12.2 Arayüz [KARAR], 12.3 Konsept görseller, 12.4 Konsept görsellerdeki bilinen sorunlar [KARAR: oyunda düzeltilecek], 12. Görsel yön

### Community 15 - "4. Oyun döngüleri"
Cohesion: 0.40
Nodes (5): 4.1 Anlık döngü (saniyeler), 4.2 Oturum döngüsü (1–5 dakika), 4.3 Meta döngü (günler/haftalar), 4.4 Kaynak akışı, 4. Oyun döngüleri

### Community 17 - "FeelConfig"
Cohesion: 0.08
Nodes (7): Ease, FeelConfig, ContainerView, Definition, IsOpened, ShelfShowcase, IsPlaying

### Community 18 - "SectionHud"
Cohesion: 0.08
Nodes (8): SectionHud, CurrentMode, IsBannerVisible, IsBookOpen, IsInspecting, IsMapOpen, UiScale, M4.3 architecture (quick map)

### Community 19 - "SectionController"
Cohesion: 0.08
Nodes (14): SectionController, Collectibles, CommonItems, Containers, Definition, DirtCleaned, HasDirt, IsComplete (+6 more)

### Community 20 - "ToolDefinition"
Cohesion: 0.22
Nodes (6): ToolProgress, Level, ToolDefinition, MaxLevel, StartsOwned, 2026-10-06 — M3 implemented

### Community 21 - "SectionProgress"
Cohesion: 0.17
Nodes (12): SectionProgress, CollectiblesRemaining, DirtCleaned, Fraction, HasDirt, IsComplete, OnlyCollectiblesLeft, Percent (+4 more)

### Community 22 - ".Capture"
Cohesion: 0.15
Nodes (7): TestGame, Boot, VenueFlowTests, Boot, Flow, Section, 2026-10-06 — M4 implemented

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
Cohesion: 0.13
Nodes (4): FileSaveStorage, ISaveStorage, SaveSystem, MemoryStorage

### Community 28 - "DirtMask"
Cohesion: 0.16
Nodes (7): DirtMask, CleanedFraction, HasDirt, Height, InitialTotal, Width, DirtMaskTests

### Community 29 - "VenueProgress"
Cohesion: 0.21
Nodes (5): SectionStatus, Percent, VenueProgress, VenueStatus, Fraction

### Community 30 - "OverviewController"
Cohesion: 0.05
Nodes (15): PlaceholderVisual, SectionVisuals, OverviewController, BuildingBounds, InputEnabled, IsActive, Rooms, RoomView (+7 more)

### Community 31 - "GameDatabase"
Cohesion: 0.22
Nodes (3): BalanceConfig, GameDatabase, GameDatabaseTests

### Community 32 - "CollectionBook"
Cohesion: 0.19
Nodes (4): CollectionBook, FoundIds, FindResult, CollectionBookTests

### Community 33 - "GameBootstrap"
Cohesion: 0.10
Nodes (14): GameBootstrap, Book, Context, Data, Drag, Flow, Hud, Inspector (+6 more)

### Community 34 - "SaveData"
Cohesion: 0.25
Nodes (3): SaveData, SectionSave, VenueProgressTests

### Community 37 - "Sorting Game (working title: Chubby's Clutter)"
Cohesion: 0.29
Nodes (6): Conventions, Git rules, graphify, Session routine, Sorting Game (working title: Chubby's Clutter), Verifying (editor must be closed for batchmode)

### Community 38 - "SectionDefinition"
Cohesion: 0.20
Nodes (3): SectionDefinition, TotalSlotCount, ShelfEntry

### Community 39 - "ShelfView"
Cohesion: 0.13
Nodes (7): ShelfView, Category, FilledCount, HasFreeSlot, IsFull, LabelAnchor, Size

### Community 40 - "CollectibleDefinition"
Cohesion: 0.15
Nodes (8): CollectibleDefinition, IsMascot, ContainerDefinition, ItemDefinition, CoinValue, IsCollectible, ContainerEntry, 15.2 Veri odaklı tasarım

### Community 41 - "Wallet"
Cohesion: 0.38
Nodes (3): Wallet, Coins, WalletTests

### Community 42 - "SectionLayout"
Cohesion: 0.31
Nodes (4): ContainerContent, SectionLayout, TotalItems, SectionLayoutGenerator

### Community 45 - "15. Teknik notlar (Unity)"
Cohesion: 0.33
Nodes (6): 15.1 Genel, 15.3 Performans, 15.4 Kayıt, 15.5 Entegrasyonlar [AÇIK], 15.6 Dil, 15. Teknik notlar (Unity)

### Community 47 - "Progress & Handoff"
Cohesion: 0.29
Nodes (6): Continuing on a new machine (checklist), Current state, Environment notes, M4.2 plan + task list (tick as done), M5 plan (not started), Progress & Handoff

### Community 48 - "SaveData.cs"
Cohesion: 0.21
Nodes (8): ContainerSave, IdCount, ItemSave, ItemSaveState, Buried, Floor, Placed, VenueSave

### Community 51 - "RareFindPresenter"
Cohesion: 0.16
Nodes (5): RareFindPresenter, DisplayHeight, DisplaySize, IsPresenting, PresentedItem

### Community 53 - "ItemState"
Cohesion: 0.25
Nodes (8): ItemState, Buried, Dragging, Flying, Found, Physics, Placed, Resting

### Community 54 - "ShelfSlot"
Cohesion: 0.24
Nodes (5): ShelfSlot, IsFree, IsReserved, Occupant, WorldBase

### Community 56 - "ShelfInspectTests"
Cohesion: 0.25
Nodes (4): ShelfInspectTests, Boot, Section, 2026-10-06 — M4.3 implemented (pre-M5 user requests)

### Community 60 - "9. Koleksiyon sistemi"
Cohesion: 0.29
Nodes (7): 9.1.1 Vitrin (`CollectionViewer`) [KARAR], 9.1 Koleksiyon Kitabı (`CollectionBook`) [KARAR], 9.2 Maskot [KARAR], 9.3 Chubby bulma anı [KARAR], 9.4 Nadir (mavi) eşyalar [KARAR], 9.5 Set bonusları [AÇIK], 9. Koleksiyon sistemi

### Community 62 - "VenueState"
Cohesion: 0.50
Nodes (4): VenueState, Completed, Locked, Open

### Community 65 - "10. Otomasyon ve ilerleme"
Cohesion: 0.40
Nodes (5): 10.1 Aletler (`Tool`) [KARAR], 10.2 Oto Sort güçlendirmesi (`AutoSortBoost`) [KARAR], 10.3 Yardımcılar (`Helper`) [KARAR], 10.4 Ölçek büyüdükçe oyuncunun rolü, 10. Otomasyon ve ilerleme

### Community 66 - "Mode"
Cohesion: 0.67
Nodes (3): Mode, Overview, Section

### Community 67 - "GameContext"
Cohesion: 0.17
Nodes (3): GameContext, Feel, ShelfLabel

### Community 76 - "8. Eşyalar ve kategoriler"
Cohesion: 0.50
Nodes (4): 8.1 Eşya türleri, 8.2 Kategoriler [KARAR], 8.3 Eşya çeşitliliği, 8. Eşyalar ve kategoriler

## Knowledge Gaps
- **252 isolated node(s):** `FoundIds`, `Width`, `Height`, `HasDirt`, `CleanedFraction` (+247 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 443 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **26 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `SectionController` connect `SectionController` to `.PlaceBuried`, `unityengine`, `ItemView`, `DragController`, `DirtLayerView`, `Fx`, `GameFlow`, `.Sweep`, `FeelConfig`, `SectionHud`, `SectionProgress`, `.Capture`, `.Awake`, `DirtMask`, `OverviewController`, `GameDatabase`, `CollectionBook`, `GameBootstrap`, `SectionDefinition`, `ShelfView`, `Wallet`, `.OnLanded`, `.Restore`, `Progress & Handoff`, `SaveData.cs`, `CoreLoopTests`, `ShelfInspectTests`, `MonoBehaviour`, `GameContext`?**
  _High betweenness centrality (0.199) - this node is a cross-community bridge._
- **Are the 2 inferred relationships involving `SectionHud` (e.g. with `M1 architecture (quick map)` and `M4 architecture (quick map)`) actually correct?**
  _`SectionHud` has 2 INFERRED edges - model-reasoned connections that need verification._
- **What connects `FoundIds`, `Width`, `Height` to the rest of the system?**
  _252 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `ShelfInspector` be split into smaller, more focused modules?**
  _Cohesion score 0.08787878787878788 - nodes in this community are weakly interconnected._
- **Why does `GameBootstrap` connect `GameBootstrap` to `ShelfInspector`, `unityengine`, `ContentBuilder`, `ItemView`, `DragController`, `CollectionViewer`, `GameFlow`, `.Sweep`, `FeelConfig`, `SectionHud`, `SectionController`, `ToolDefinition`, `.Capture`, `.Awake`, `OverviewController`, `GameDatabase`, `CollectionBook`, `SaveData`, `Wallet`, `CoreLoopTests`, `RareFindPresenter`, `ShelfInspectTests`, `MonoBehaviour`, `GameContext`?**
  _High betweenness centrality (0.133) - this node is a cross-community bridge._
- **Are the 2 inferred relationships involving `SectionController` (e.g. with `M1 architecture (quick map)` and `M5 plan (not started)`) actually correct?**
  _`SectionController` has 2 INFERRED edges - model-reasoned connections that need verification._
- **Should `unityengine` be split into smaller, more focused modules?**
  _Cohesion score 0.06843949701092558 - nodes in this community are weakly interconnected._