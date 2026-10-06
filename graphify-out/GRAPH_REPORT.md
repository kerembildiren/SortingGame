# Graph Report - SortingGame  (2026-10-06)

## Corpus Check
- 68 files · ~40,806 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 260 file(s) not represented in the graph (top: .meta 169, .asset 74, (none) 4)

## Summary
- 1154 nodes · 2881 edges · 49 communities (38 shown, 11 thin omitted)
- Extraction: 89% EXTRACTED · 11% INFERRED · 0% AMBIGUOUS · INFERRED: 321 edges (avg confidence: 0.85)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `af3fdca7`
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
- 5. Mekânlar ve bölümler
- CollectionViewer
- unity.sh
- SaveData
- GameFlow
- 12. Görsel yön
- 4. Oyun döngüleri
- CollectibleDefinition
- OverviewController
- SectionHud
- SectionController
- ToolDefinition
- SectionProgress
- GameBootstrap
- 11. Ekonomi ve gelir modeli
- 7. Etkileşim ve kontroller
- 16. MVP kapsamı
- 6. Görünümler ve kamera
- CategoryDefinition
- DirtMask
- 15. Teknik notlar (Unity)
- PlaceholderFactory
- .Sweep
- ProceduralTextures
- Session log
- ContainerView
- .Burst
- .Run
- .OnLanded
- MonoBehaviour
- M1 architecture (quick map)
- VenueProgressTests
- RoomView
- ShelfSlot
- ItemState
- SectionDefinition
- 8. Eşyalar ve kategoriler
- VenueState
- .FinishSellAndBuy_WalksUpTheVenueLadder

## God Nodes (most connected - your core abstractions)
1. `SectionController` - 92 edges
2. `SectionHud` - 89 edges
3. `ItemView` - 63 edges
4. `GameBootstrap` - 51 edges
5. `SortingGame.Data` - 45 edges
6. `DragController` - 45 edges
7. `SortingGame.Core` - 43 edges
8. `M1 architecture (quick map)` - 42 edges
9. `SectionDefinition` - 40 edges
10. `GameFlow` - 38 edges

## Surprising Connections (you probably didn't know these)
- `8.1 Eşya türleri` --references--> `ContainerDefinition`  [INFERRED]
  Docs/GDD.md → Assets/_Project/Scripts/Runtime/Data/ContainerDefinition.cs
- `9.1.1 Vitrin (`CollectionViewer`) [KARAR]` --references--> `CollectionViewer`  [INFERRED]
  Docs/GDD.md → Assets/_Project/Scripts/Runtime/Section/CollectionViewer.cs
- `M1 architecture (quick map)` --references--> `ContentBuilder`  [INFERRED]
  Docs/PROGRESS.md → Assets/_Project/Scripts/Editor/ContentBuilder.cs
- `M4 architecture (quick map)` --references--> `ContentBuilder`  [INFERRED]
  Docs/PROGRESS.md → Assets/_Project/Scripts/Editor/ContentBuilder.cs
- `10.2 Kategori Ustalığı (`CategoryMastery`) [KARAR]` --references--> `CategoryMastery`  [INFERRED]
  Docs/GDD.md → Assets/_Project/Scripts/Runtime/Core/CategoryMastery.cs

## Import Cycles
- None detected.

## Communities (49 total, 11 thin omitted)

### Community 1 - "GDD — Chubby's Clutter (çalışma adı)"
Cohesion: 0.20
Nodes (9): 0. Bu belge nasıl okunmalı, 13. Fikri mülkiyet ve özgünlük kuralları [KARAR], 14. Ses ve haptik, 18. Açık sorular, 19. Karar günlüğü, 1. Özet, 2. Değişmez prensipler, 3. Referanslar ve farklılaşma (+1 more)

### Community 3 - "unityengine"
Cohesion: 0.07
Nodes (7): SortingGame.Tests, SortingGame.Data, SortingGame.UI, SortingGame.Core, SortingGame.Overview, SortingGame.EditorTools, SortingGame.Section

### Community 4 - "ContentBuilder"
Cohesion: 0.06
Nodes (23): ContentBuilder, ProjectSetup, BalanceConfig, ContainerDefinition, ItemRarity, Common, Mascot, Rare (+15 more)

### Community 5 - "ItemView"
Cohesion: 0.16
Nodes (8): ItemView, CanPick, CanTapToFind, Definition, IsCollectible, SavePose, State, VisualSize

### Community 6 - "Sfx"
Cohesion: 0.06
Nodes (28): Sfx, BookStamp, CleanAmbienceLoop, Coin, DuplicateSold, Magnet, Mastery, Pickup (+20 more)

### Community 7 - "DragController"
Cohesion: 0.10
Nodes (11): GameContext, Feel, ToolType, Broom, Hand, Magnet, DragController, Capacity (+3 more)

### Community 9 - "5. Mekânlar ve bölümler"
Cohesion: 0.29
Nodes (7): 5.1 Yapı [KARAR], 5.2 İlerleme sırası [VARSAYILAN], 5.3 Mekân merdiveni (taslak), 5.4 Bölüm kilitleri [VARSAYILAN], 5.5 Hafif renovasyon [KARAR], 5.6 Mekânın satışı [KARAR], 5. Mekânlar ve bölümler

### Community 10 - "CollectionViewer"
Cohesion: 0.05
Nodes (20): OrbitView, IsIdleSpinning, Pan, Pitch, Yaw, Zoom, FeelConfig, CameraFitter (+12 more)

### Community 12 - "SaveData"
Cohesion: 0.29
Nodes (6): SaveData, SectionStatus, Percent, VenueProgress, VenueStatus, Fraction

### Community 13 - "GameFlow"
Cohesion: 0.13
Nodes (9): GameFlow, ActiveSection, Busy, Current, Data, Venue, Screen, Overview (+1 more)

### Community 14 - "12. Görsel yön"
Cohesion: 0.40
Nodes (5): 12.1 Stil [KARAR], 12.2 Arayüz [KARAR], 12.3 Konsept görseller, 12.4 Konsept görsellerdeki bilinen sorunlar [KARAR: oyunda düzeltilecek], 12. Görsel yön

### Community 15 - "4. Oyun döngüleri"
Cohesion: 0.40
Nodes (5): 4.1 Anlık döngü (saniyeler), 4.2 Oturum döngüsü (1–5 dakika), 4.3 Meta döngü (günler/haftalar), 4.4 Kaynak akışı, 4. Oyun döngüleri

### Community 16 - "CollectibleDefinition"
Cohesion: 0.11
Nodes (12): CollectionBook, FoundIds, FindResult, CollectibleDefinition, IsMascot, CollectionBookTests, 9.1.1 Vitrin (`CollectionViewer`) [KARAR], 9.1 Koleksiyon Kitabı (`CollectionBook`) [KARAR] (+4 more)

### Community 17 - "OverviewController"
Cohesion: 0.13
Nodes (6): OverviewController, BuildingBounds, InputEnabled, IsActive, Rooms, M4 architecture (quick map)

### Community 18 - "SectionHud"
Cohesion: 0.05
Nodes (10): Loc, Mode, Overview, Section, SectionHud, CurrentMode, IsBookOpen, IsMapOpen (+2 more)

### Community 19 - "SectionController"
Cohesion: 0.09
Nodes (14): SectionController, Collectibles, CommonItems, Containers, Definition, DirtCleaned, HasDirt, IsComplete (+6 more)

### Community 20 - "ToolDefinition"
Cohesion: 0.05
Nodes (21): ContainerSave, ItemSave, ItemSaveState, Buried, Floor, Placed, SectionSave, FileSaveStorage (+13 more)

### Community 21 - "SectionProgress"
Cohesion: 0.21
Nodes (9): SectionProgress, DirtCleaned, Fraction, HasDirt, IsComplete, Percent, PlacedItems, TotalItems (+1 more)

### Community 22 - "GameBootstrap"
Cohesion: 0.05
Nodes (17): GameBootstrap, Book, Context, Data, Drag, Flow, Hud, Overview (+9 more)

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

### Community 27 - "CategoryDefinition"
Cohesion: 0.05
Nodes (18): CategoryMastery, ContainerContent, SectionLayout, TotalItems, SectionLayoutGenerator, CategoryDefinition, GameDatabase, ItemDefinition (+10 more)

### Community 28 - "DirtMask"
Cohesion: 0.16
Nodes (7): DirtMask, CleanedFraction, HasDirt, Height, InitialTotal, Width, DirtMaskTests

### Community 29 - "15. Teknik notlar (Unity)"
Cohesion: 0.33
Nodes (6): 15.1 Genel, 15.3 Performans, 15.4 Kayıt ve çevrimdışı ilerleme, 15.5 Entegrasyonlar [AÇIK], 15.6 Dil, 15. Teknik notlar (Unity)

### Community 30 - "PlaceholderFactory"
Cohesion: 0.12
Nodes (3): SectionVisuals, PlaceholderFactory, DashedTexture

### Community 32 - "ProceduralTextures"
Cohesion: 0.40
Nodes (3): ProceduralTextures, Rays, SoftDot

### Community 33 - "Session log"
Cohesion: 0.15
Nodes (10): BuildTools, 2026-10-06 — M0 done, approved, 2026-10-06 — M1 implemented and approved (committed), 2026-10-06 — M2 implemented, 2026-10-06 — M3 round 2 (user feedback), 2026-10-06 — M4 implemented, Current state, Environment notes (+2 more)

### Community 34 - "ContainerView"
Cohesion: 0.12
Nodes (4): Ease, ContainerView, Definition, IsOpened

### Community 39 - "M1 architecture (quick map)"
Cohesion: 0.14
Nodes (8): ShelfView, Category, FilledCount, HasFreeSlot, IsFull, LabelAnchor, Size, M1 architecture (quick map)

### Community 41 - "RoomView"
Cohesion: 0.15
Nodes (6): RoomView, Centre, LabelAnchor, Section, Size, Status

### Community 42 - "ShelfSlot"
Cohesion: 0.24
Nodes (5): ShelfSlot, IsFree, IsReserved, Occupant, WorldBase

### Community 43 - "ItemState"
Cohesion: 0.25
Nodes (8): ItemState, Buried, Dragging, Flying, Found, Physics, Placed, Resting

### Community 45 - "SectionDefinition"
Cohesion: 0.18
Nodes (5): SectionDefinition, TotalSlotCount, ShelfEntry, VenueDefinition, 15.2 Veri odaklı tasarım

### Community 46 - "8. Eşyalar ve kategoriler"
Cohesion: 0.50
Nodes (4): 8.1 Eşya türleri, 8.2 Kategoriler [KARAR], 8.3 Eşya çeşitliliği, 8. Eşyalar ve kategoriler

### Community 47 - "VenueState"
Cohesion: 0.40
Nodes (5): VenueState, ForSale, Locked, Owned, Sold

### Community 57 - ".FinishSellAndBuy_WalksUpTheVenueLadder"
Cohesion: 0.14
Nodes (5): TestSnapshots, VenueFlowTests, Boot, Flow, Section

## Knowledge Gaps
- **224 isolated node(s):** `FoundIds`, `Width`, `Height`, `HasDirt`, `CleanedFraction` (+219 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 401 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **11 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `SectionController` connect `SectionController` to `.Build`, `.DeliverStack`, `unityengine`, `ItemView`, `DragController`, `CollectionViewer`, `GameFlow`, `CollectibleDefinition`, `SectionHud`, `ToolDefinition`, `SectionProgress`, `GameBootstrap`, `CategoryDefinition`, `DirtMask`, `PlaceholderFactory`, `.Sweep`, `ContainerView`, `.Burst`, `.Run`, `.OnLanded`, `MonoBehaviour`, `M1 architecture (quick map)`, `Vector3`, `SectionDefinition`, `.FinishSellAndBuy_WalksUpTheVenueLadder`?**
  _High betweenness centrality (0.189) - this node is a cross-community bridge._
- **Why does `SectionHud` connect `SectionHud` to `unityengine`, `.Run`, `MonoBehaviour`, `DragController`, `M1 architecture (quick map)`, `RoomView`, `CollectionViewer`, `GameFlow`, `SectionDefinition`, `CollectibleDefinition`, `OverviewController`, `SectionController`, `ToolDefinition`, `GameBootstrap`?**
  _High betweenness centrality (0.125) - this node is a cross-community bridge._
- **Why does `M1 architecture (quick map)` connect `M1 architecture (quick map)` to `.Build`, `.DeliverStack`, `ContentBuilder`, `ItemView`, `Sfx`, `DragController`, `CollectionViewer`, `CollectibleDefinition`, `SectionHud`, `SectionController`, `ToolDefinition`, `GameBootstrap`, `CategoryDefinition`, `DirtMask`, `PlaceholderFactory`, `.Sweep`, `Session log`, `ContainerView`, `.Run`, `MonoBehaviour`, `ShelfSlot`, `SectionDefinition`?**
  _High betweenness centrality (0.107) - this node is a cross-community bridge._
- **Are the 2 inferred relationships involving `SectionHud` (e.g. with `M1 architecture (quick map)` and `M4 architecture (quick map)`) actually correct?**
  _`SectionHud` has 2 INFERRED edges - model-reasoned connections that need verification._
- **Are the 2 inferred relationships involving `GameBootstrap` (e.g. with `2026-10-06 — M3 implemented` and `M1 architecture (quick map)`) actually correct?**
  _`GameBootstrap` has 2 INFERRED edges - model-reasoned connections that need verification._
- **What connects `FoundIds`, `Width`, `Height` to the rest of the system?**
  _224 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `unityengine` be split into smaller, more focused modules?**
  _Cohesion score 0.07290886392009988 - nodes in this community are weakly interconnected._