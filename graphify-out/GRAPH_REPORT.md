# Graph Report - SortingGame  (2026-10-06)

## Corpus Check
- 49 files · ~25,977 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 170 file(s) not represented in the graph (top: .meta 114, .asset 39, (none) 4)

## Summary
- 752 nodes · 1665 edges · 33 communities (28 shown, 5 thin omitted)
- Extraction: 91% EXTRACTED · 9% INFERRED · 0% AMBIGUOUS · INFERRED: 146 edges (avg confidence: 0.86)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `17d0ef31`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- .Build
- GDD — Chubby's Clutter (çalışma adı)
- 8. Eşyalar ve kategoriler
- unityengine
- .Build
- ItemView
- Sfx
- DragController
- .Generate
- 5. Mekânlar ve bölümler
- Progress & Handoff
- unity.sh
- DirtLayerView
- 10. Otomasyon ve ilerleme
- 12. Görsel yön
- 4. Oyun döngüleri
- CollectibleDefinition
- ShelfView
- SectionHud
- SectionController
- Wallet
- SectionProgress
- GameBootstrap
- 11. Ekonomi ve gelir modeli
- 7. Etkileşim ve kontroller
- 16. MVP kapsamı
- 6. Görünümler ve kamera
- ItemDefinition
- DirtMask
- ItemState
- PlaceholderFactory
- ProceduralTextures

## God Nodes (most connected - your core abstractions)
1. `SectionController` - 71 edges
2. `ItemView` - 56 edges
3. `SectionHud` - 54 edges
4. `M1 architecture (quick map)` - 36 edges
5. `ShelfView` - 33 edges
6. `SortingGame.Data` - 32 edges
7. `DragController` - 32 edges
8. `GameBootstrap` - 30 edges
9. `SortingGame.Core` - 26 edges
10. `RareFindPresenter` - 26 edges

## Surprising Connections (you probably didn't know these)
- `8.1 Eşya türleri` --references--> `ContainerDefinition`  [INFERRED]
  Docs/GDD.md → Assets/_Project/Scripts/Runtime/Data/ContainerDefinition.cs
- `M1 architecture (quick map)` --references--> `ContentBuilder`  [INFERRED]
  Docs/PROGRESS.md → Assets/_Project/Scripts/Editor/ContentBuilder.cs
- `17. Sözlük` --references--> `CollectionBook`  [INFERRED]
  Docs/GDD.md → Assets/_Project/Scripts/Runtime/Core/CollectionBook.cs
- `9.1 Koleksiyon Kitabı (`CollectionBook`) [KARAR]` --references--> `CollectionBook`  [INFERRED]
  Docs/GDD.md → Assets/_Project/Scripts/Runtime/Core/CollectionBook.cs
- `M1 architecture (quick map)` --references--> `DirtMask`  [INFERRED]
  Docs/PROGRESS.md → Assets/_Project/Scripts/Runtime/Core/DirtMask.cs

## Import Cycles
- None detected.

## Communities (33 total, 5 thin omitted)

### Community 1 - "GDD — Chubby's Clutter (çalışma adı)"
Cohesion: 0.20
Nodes (9): 0. Bu belge nasıl okunmalı, 13. Fikri mülkiyet ve özgünlük kuralları [KARAR], 14. Ses ve haptik, 18. Açık sorular, 19. Karar günlüğü, 1. Özet, 2. Değişmez prensipler, 3. Referanslar ve farklılaşma (+1 more)

### Community 2 - "8. Eşyalar ve kategoriler"
Cohesion: 0.50
Nodes (4): 8.1 Eşya türleri, 8.2 Kategoriler [KARAR], 8.3 Eşya çeşitliliği, 8. Eşyalar ve kategoriler

### Community 3 - "unityengine"
Cohesion: 0.07
Nodes (8): BuildTools, SortingGame.Tests, SortingGame.Data, SortingGame.UI, SortingGame.Core, SortingGame.EditorTools, SortingGame.Section, 2026-10-06 — M2 implemented

### Community 4 - ".Build"
Cohesion: 0.06
Nodes (21): ContentBuilder, ProjectSetup, ItemRarity, Common, Mascot, Rare, PlaceholderShape, Book (+13 more)

### Community 5 - "ItemView"
Cohesion: 0.05
Nodes (15): Haptics, Vibrator, Ease, Tween, Runner, TweenRunner, ItemView, CanPick (+7 more)

### Community 6 - "Sfx"
Cohesion: 0.07
Nodes (25): Sfx, BookStamp, CleanAmbienceLoop, Coin, DuplicateSold, Pickup, PlaceMetal, PlacePaper (+17 more)

### Community 7 - "DragController"
Cohesion: 0.08
Nodes (10): SectionVisuals, ContainerView, Definition, IsOpened, DragController, InputEnabled, Tool, ToolType (+2 more)

### Community 9 - "5. Mekânlar ve bölümler"
Cohesion: 0.29
Nodes (7): 5.1 Yapı [KARAR], 5.2 İlerleme sırası [VARSAYILAN], 5.3 Mekân merdiveni (taslak), 5.4 Bölüm kilitleri [VARSAYILAN], 5.5 Hafif renovasyon [KARAR], 5.6 Mekânın satışı [KARAR], 5. Mekânlar ve bölümler

### Community 10 - "Progress & Handoff"
Cohesion: 0.25
Nodes (7): 2026-10-06 — M0 done, approved, 2026-10-06 — M1 implemented and approved (committed), Current state, Environment notes, Open questions for the user, Progress & Handoff, Session log

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
Cohesion: 0.11
Nodes (11): CollectionBook, FoundIds, FindResult, CollectibleDefinition, IsMascot, CollectionBookTests, 9.1 Koleksiyon Kitabı (`CollectionBook`) [KARAR], 9.2 Maskot [KARAR] (+3 more)

### Community 17 - "ShelfView"
Cohesion: 0.11
Nodes (12): ShelfSlot, IsFree, IsReserved, Occupant, WorldBase, ShelfView, Category, FilledCount (+4 more)

### Community 18 - "SectionHud"
Cohesion: 0.08
Nodes (4): Loc, VenueDefinition, SectionHud, UiScale

### Community 19 - "SectionController"
Cohesion: 0.09
Nodes (12): SectionController, Collectibles, CommonItems, Containers, Definition, DirtCleaned, HasDirt, IsComplete (+4 more)

### Community 20 - "Wallet"
Cohesion: 0.31
Nodes (3): Wallet, Coins, WalletTests

### Community 21 - "SectionProgress"
Cohesion: 0.23
Nodes (9): SectionProgress, DirtCleaned, Fraction, HasDirt, IsComplete, Percent, PlacedItems, TotalItems (+1 more)

### Community 22 - "GameBootstrap"
Cohesion: 0.06
Nodes (16): GameBootstrap, Book, Drag, Hud, RareFind, Section, Wallet, FeelConfig (+8 more)

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
Cohesion: 0.06
Nodes (22): ContainerContent, SectionLayoutGenerator, BalanceConfig, CategoryDefinition, ContainerDefinition, GameDatabase, ItemDefinition, CoinValue (+14 more)

### Community 28 - "DirtMask"
Cohesion: 0.31
Nodes (5): DirtMask, CleanedFraction, HasDirt, Height, Width

### Community 29 - "ItemState"
Cohesion: 0.25
Nodes (8): ItemState, Buried, Dragging, Flying, Found, Physics, Placed, Resting

### Community 30 - "PlaceholderFactory"
Cohesion: 0.09
Nodes (3): Fx, PlaceholderFactory, DashedTexture

### Community 32 - "ProceduralTextures"
Cohesion: 0.40
Nodes (3): ProceduralTextures, Rays, SoftDot

## Knowledge Gaps
- **165 isolated node(s):** `FoundIds`, `Width`, `Height`, `HasDirt`, `CleanedFraction` (+160 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 300 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **5 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `SectionController` connect `SectionController` to `.Build`, `unityengine`, `ItemView`, `DragController`, `.Generate`, `DirtLayerView`, `CollectibleDefinition`, `ShelfView`, `SectionHud`, `Wallet`, `SectionProgress`, `GameBootstrap`, `ItemDefinition`, `DirtMask`, `PlaceholderFactory`, `.Sweep`?**
  _High betweenness centrality (0.221) - this node is a cross-community bridge._
- **Why does `M1 architecture (quick map)` connect `ItemView` to `.Build`, `.Build`, `Sfx`, `DragController`, `Progress & Handoff`, `DirtLayerView`, `CollectibleDefinition`, `ShelfView`, `SectionHud`, `SectionController`, `Wallet`, `GameBootstrap`, `ItemDefinition`, `DirtMask`, `PlaceholderFactory`, `.Sweep`?**
  _High betweenness centrality (0.178) - this node is a cross-community bridge._
- **Why does `GDD — Chubby's Clutter (çalışma adı)` connect `GDD — Chubby's Clutter (çalışma adı)` to `8. Eşyalar ve kategoriler`, `.Build`, `5. Mekânlar ve bölümler`, `10. Otomasyon ve ilerleme`, `12. Görsel yön`, `4. Oyun döngüleri`, `CollectibleDefinition`, `11. Ekonomi ve gelir modeli`, `7. Etkileşim ve kontroller`, `16. MVP kapsamı`, `6. Görünümler ve kamera`, `ItemDefinition`?**
  _High betweenness centrality (0.134) - this node is a cross-community bridge._
- **Are the 35 inferred relationships involving `M1 architecture (quick map)` (e.g. with `ContentBuilder` and `.CreateMainScene()`) actually correct?**
  _`M1 architecture (quick map)` has 35 INFERRED edges - model-reasoned connections that need verification._
- **What connects `FoundIds`, `Width`, `Height` to the rest of the system?**
  _165 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `unityengine` be split into smaller, more focused modules?**
  _Cohesion score 0.07237871674491393 - nodes in this community are weakly interconnected._
- **Should `.Build` be split into smaller, more focused modules?**
  _Cohesion score 0.06464646464646465 - nodes in this community are weakly interconnected._