# Graph Report - SortingGame  (2026-10-06)

## Corpus Check
- 61 files · ~34,271 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 188 file(s) not represented in the graph (top: .meta 129, .asset 42, (none) 4)

## Summary
- 986 nodes · 2363 edges · 47 communities (34 shown, 13 thin omitted)
- Extraction: 89% EXTRACTED · 11% INFERRED · 0% AMBIGUOUS · INFERRED: 255 edges (avg confidence: 0.85)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `f9c553fe`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- .Build
- GDD — Chubby's Clutter (çalışma adı)
- .DeliverStack
- unityengine
- .Build
- ItemView
- Sfx
- DragController
- RareFindPresenter
- 5. Mekânlar ve bölümler
- CollectionViewer
- unity.sh
- DirtLayerView
- 10. Otomasyon ve ilerleme
- 12. Görsel yön
- 4. Oyun döngüleri
- CollectibleDefinition
- ItemDefinition
- SectionHud
- SectionController
- CategoryDefinition
- SectionProgress
- GameBootstrap
- 11. Ekonomi ve gelir modeli
- 7. Etkileşim ve kontroller
- 16. MVP kapsamı
- 6. Görünümler ve kamera
- .Generate
- DirtMask
- 15. Teknik notlar (Unity)
- PlaceholderFactory
- ProceduralTextures
- M1 architecture (quick map)
- .Generate
- SectionDefinition
- .Run
- .Present
- MonoBehaviour
- .OnLanded
- ContainerContent
- ShelfSlot
- ItemState
- BalanceConfig
- 8. Eşyalar ve kategoriler

## God Nodes (most connected - your core abstractions)
1. `SectionController` - 88 edges
2. `SectionHud` - 71 edges
3. `ItemView` - 63 edges
4. `GameBootstrap` - 45 edges
5. `DragController` - 44 edges
6. `M1 architecture (quick map)` - 42 edges
7. `SortingGame.Data` - 39 edges
8. `SortingGame.Core` - 36 edges
9. `ShelfView` - 35 edges
10. `CollectionViewer` - 31 edges

## Surprising Connections (you probably didn't know these)
- `10.2 Kategori Ustalığı (`CategoryMastery`) [KARAR]` --references--> `CategoryMastery`  [INFERRED]
  Docs/GDD.md → Assets/_Project/Scripts/Runtime/Core/CategoryMastery.cs
- `9.1 Koleksiyon Kitabı (`CollectionBook`) [KARAR]` --references--> `CollectionBook`  [INFERRED]
  Docs/GDD.md → Assets/_Project/Scripts/Runtime/Core/CollectionBook.cs
- `8.1 Eşya türleri` --references--> `ContainerDefinition`  [INFERRED]
  Docs/GDD.md → Assets/_Project/Scripts/Runtime/Data/ContainerDefinition.cs
- `M1 architecture (quick map)` --references--> `ContentBuilder`  [INFERRED]
  Docs/PROGRESS.md → Assets/_Project/Scripts/Editor/ContentBuilder.cs
- `17. Sözlük` --references--> `CategoryMastery`  [INFERRED]
  Docs/GDD.md → Assets/_Project/Scripts/Runtime/Core/CategoryMastery.cs

## Import Cycles
- None detected.

## Communities (47 total, 13 thin omitted)

### Community 1 - "GDD — Chubby's Clutter (çalışma adı)"
Cohesion: 0.20
Nodes (9): 0. Bu belge nasıl okunmalı, 13. Fikri mülkiyet ve özgünlük kuralları [KARAR], 14. Ses ve haptik, 18. Açık sorular, 19. Karar günlüğü, 1. Özet, 2. Değişmez prensipler, 3. Referanslar ve farklılaşma (+1 more)

### Community 2 - ".DeliverStack"
Cohesion: 0.18
Nodes (3): ProgressionPlayTests, Section, TestSnapshots

### Community 3 - "unityengine"
Cohesion: 0.08
Nodes (6): SortingGame.Tests, SortingGame.Data, SortingGame.UI, SortingGame.Core, SortingGame.EditorTools, SortingGame.Section

### Community 4 - ".Build"
Cohesion: 0.07
Nodes (16): ContentBuilder, ProjectSetup, ItemRarity, Common, Mascot, Rare, PlaceholderShape, Book (+8 more)

### Community 5 - "ItemView"
Cohesion: 0.16
Nodes (8): ItemView, CanPick, CanTapToFind, Definition, IsCollectible, SavePose, State, VisualSize

### Community 6 - "Sfx"
Cohesion: 0.06
Nodes (28): Sfx, BookStamp, CleanAmbienceLoop, Coin, DuplicateSold, Magnet, Mastery, Pickup (+20 more)

### Community 7 - "DragController"
Cohesion: 0.08
Nodes (8): ContainerView, Definition, IsOpened, DragController, Capacity, Carried, InputEnabled, Tool

### Community 8 - "RareFindPresenter"
Cohesion: 0.11
Nodes (7): FeelConfig, CameraFitter, RareFindPresenter, DisplayHeight, DisplaySize, IsPresenting, PresentedItem

### Community 9 - "5. Mekânlar ve bölümler"
Cohesion: 0.29
Nodes (7): 5.1 Yapı [KARAR], 5.2 İlerleme sırası [VARSAYILAN], 5.3 Mekân merdiveni (taslak), 5.4 Bölüm kilitleri [VARSAYILAN], 5.5 Hafif renovasyon [KARAR], 5.6 Mekânın satışı [KARAR], 5. Mekânlar ve bölümler

### Community 10 - "CollectionViewer"
Cohesion: 0.06
Nodes (27): BuildTools, OrbitView, IsIdleSpinning, Pan, Pitch, Yaw, Zoom, CollectionViewer (+19 more)

### Community 13 - "10. Otomasyon ve ilerleme"
Cohesion: 0.40
Nodes (5): 10.1 Aletler (`Tool`) [KARAR], 10.2 Kategori Ustalığı (`CategoryMastery`) [KARAR], 10.3 Yardımcılar (`Helper`) [KARAR], 10.4 Ölçek büyüdükçe oyuncunun rolü, 10. Otomasyon ve ilerleme

### Community 14 - "12. Görsel yön"
Cohesion: 0.40
Nodes (5): 12.1 Stil [KARAR], 12.2 Arayüz [KARAR], 12.3 Konsept görseller, 12.4 Konsept görsellerdeki bilinen sorunlar [KARAR: oyunda düzeltilecek], 12. Görsel yön

### Community 15 - "4. Oyun döngüleri"
Cohesion: 0.40
Nodes (5): 4.1 Anlık döngü (saniyeler), 4.2 Oturum döngüsü (1–5 dakika), 4.3 Meta döngü (günler/haftalar), 4.4 Kaynak akışı, 4. Oyun döngüleri

### Community 16 - "CollectibleDefinition"
Cohesion: 0.14
Nodes (6): CollectionBook, FoundIds, FindResult, CollectibleDefinition, IsMascot, CollectionBookTests

### Community 17 - "ItemDefinition"
Cohesion: 0.19
Nodes (5): GameDatabase, ItemDefinition, CoinValue, IsCollectible, GameDatabaseTests

### Community 18 - "SectionHud"
Cohesion: 0.07
Nodes (5): Loc, SectionHud, IsBookOpen, UiScale, ShelfLabel

### Community 19 - "SectionController"
Cohesion: 0.09
Nodes (13): SectionController, Collectibles, CommonItems, Containers, Definition, DirtCleaned, HasDirt, IsComplete (+5 more)

### Community 20 - "CategoryDefinition"
Cohesion: 0.06
Nodes (18): CategoryMastery, GameContext, Feel, ToolProgress, Wallet, Coins, CategoryDefinition, ToolType (+10 more)

### Community 21 - "SectionProgress"
Cohesion: 0.21
Nodes (9): SectionProgress, DirtCleaned, Fraction, HasDirt, IsComplete, Percent, PlacedItems, TotalItems (+1 more)

### Community 22 - "GameBootstrap"
Cohesion: 0.05
Nodes (24): GameBootstrap, Book, Context, Drag, Hud, RareFind, Section, Viewer (+16 more)

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
Cohesion: 0.15
Nodes (6): DirtMask, CleanedFraction, HasDirt, Height, InitialTotal, Width

### Community 29 - "15. Teknik notlar (Unity)"
Cohesion: 0.33
Nodes (6): 15.1 Genel, 15.3 Performans, 15.4 Kayıt ve çevrimdışı ilerleme, 15.5 Entegrasyonlar [AÇIK], 15.6 Dil, 15. Teknik notlar (Unity)

### Community 30 - "PlaceholderFactory"
Cohesion: 0.09
Nodes (3): Fx, PlaceholderFactory, DashedTexture

### Community 32 - "ProceduralTextures"
Cohesion: 0.40
Nodes (3): ProceduralTextures, Rays, SoftDot

### Community 33 - "M1 architecture (quick map)"
Cohesion: 0.13
Nodes (8): ShelfView, Category, FilledCount, HasFreeSlot, IsFull, LabelAnchor, Size, M1 architecture (quick map)

### Community 35 - "SectionDefinition"
Cohesion: 0.19
Nodes (7): ContainerDefinition, ContainerEntry, SectionDefinition, TotalSlotCount, ShelfEntry, VenueDefinition, 15.2 Veri odaklı tasarım

### Community 42 - "ShelfSlot"
Cohesion: 0.24
Nodes (5): ShelfSlot, IsFree, IsReserved, Occupant, WorldBase

### Community 43 - "ItemState"
Cohesion: 0.25
Nodes (8): ItemState, Buried, Dragging, Flying, Found, Physics, Placed, Resting

### Community 44 - "BalanceConfig"
Cohesion: 0.25
Nodes (7): BalanceConfig, Conventions, Git rules, graphify, Session routine, Sorting Game (working title: Chubby's Clutter), Verifying (editor must be closed for batchmode)

### Community 46 - "8. Eşyalar ve kategoriler"
Cohesion: 0.50
Nodes (4): 8.1 Eşya türleri, 8.2 Kategoriler [KARAR], 8.3 Eşya çeşitliliği, 8. Eşyalar ve kategoriler

## Knowledge Gaps
- **189 isolated node(s):** `FoundIds`, `Width`, `Height`, `HasDirt`, `CleanedFraction` (+184 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 345 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **13 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `SectionController` connect `SectionController` to `.Build`, `.DeliverStack`, `unityengine`, `.Build`, `ItemView`, `DragController`, `RareFindPresenter`, `DirtLayerView`, `CollectibleDefinition`, `ItemDefinition`, `SectionHud`, `CategoryDefinition`, `SectionProgress`, `GameBootstrap`, `DirtMask`, `PlaceholderFactory`, `.Sweep`, `M1 architecture (quick map)`, `SectionDefinition`, `.Run`, `MonoBehaviour`, `.OnLanded`, `Vector3`?**
  _High betweenness centrality (0.199) - this node is a cross-community bridge._
- **Why does `M1 architecture (quick map)` connect `M1 architecture (quick map)` to `.Build`, `.Build`, `ItemView`, `Sfx`, `DragController`, `RareFindPresenter`, `CollectionViewer`, `DirtLayerView`, `CollectibleDefinition`, `SectionHud`, `SectionController`, `CategoryDefinition`, `GameBootstrap`, `DirtMask`, `PlaceholderFactory`, `.Sweep`, `SectionDefinition`, `.Run`, `MonoBehaviour`, `ContainerContent`, `ShelfSlot`, `BalanceConfig`?**
  _High betweenness centrality (0.181) - this node is a cross-community bridge._
- **Why does `SectionHud` connect `SectionHud` to `M1 architecture (quick map)`, `unityengine`, `SectionDefinition`, `.Run`, `MonoBehaviour`, `RareFindPresenter`, `CollectibleDefinition`, `SectionController`, `CategoryDefinition`, `GameBootstrap`?**
  _High betweenness centrality (0.101) - this node is a cross-community bridge._
- **Are the 2 inferred relationships involving `GameBootstrap` (e.g. with `2026-10-06 — M3 implemented` and `M1 architecture (quick map)`) actually correct?**
  _`GameBootstrap` has 2 INFERRED edges - model-reasoned connections that need verification._
- **Are the 2 inferred relationships involving `DragController` (e.g. with `2026-10-06 — M3 round 2 (user feedback)` and `M1 architecture (quick map)`) actually correct?**
  _`DragController` has 2 INFERRED edges - model-reasoned connections that need verification._
- **What connects `FoundIds`, `Width`, `Height` to the rest of the system?**
  _189 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `unityengine` be split into smaller, more focused modules?**
  _Cohesion score 0.07659850697825381 - nodes in this community are weakly interconnected._