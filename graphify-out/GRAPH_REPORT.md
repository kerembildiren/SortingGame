# Graph Report - SortingGame  (2026-10-06)

## Corpus Check
- 40 files · ~18,061 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 149 file(s) not represented in the graph (top: .meta 99, .asset 35, (none) 4)

## Summary
- 582 nodes · 1136 edges · 31 communities (29 shown, 2 thin omitted)
- Extraction: 93% EXTRACTED · 7% INFERRED · 0% AMBIGUOUS · INFERRED: 79 edges (avg confidence: 0.88)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `07f13fb3`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- GameDatabase
- GDD — Chubby's Clutter (çalışma adı)
- 8. Eşyalar ve kategoriler
- unityengine
- .Build
- ItemView
- SfxPlayer
- GameBootstrap
- Wallet
- 5. Mekânlar ve bölümler
- Progress & Handoff
- unity.sh
- 15. Teknik notlar (Unity)
- 10. Otomasyon ve ilerleme
- 12. Görsel yön
- 4. Oyun döngüleri
- 9. Koleksiyon sistemi
- ShelfView
- SectionHud
- SectionController
- ContainerView
- SectionProgress
- FeelConfig
- 11. Ekonomi ve gelir modeli
- 7. Etkileşim ve kontroller
- 16. MVP kapsamı
- 6. Görünümler ve kamera
- .Generate
- PlaceholderShape
- ItemState

## God Nodes (most connected - your core abstractions)
1. `SectionController` - 48 edges
2. `ItemView` - 40 edges
3. `SectionHud` - 38 edges
4. `ShelfView` - 33 edges
5. `SortingGame.Data` - 26 edges
6. `M1 architecture (quick map)` - 26 edges
7. `GameBootstrap` - 23 edges
8. `DragController` - 23 edges
9. `GameDatabase` - 21 edges
10. `ContainerView` - 21 edges

## Surprising Connections (you probably didn't know these)
- `8.1 Eşya türleri` --references--> `ContainerDefinition`  [INFERRED]
  Docs/GDD.md → Assets/_Project/Scripts/Runtime/Data/ContainerDefinition.cs
- `M1 architecture (quick map)` --references--> `ContentBuilder`  [INFERRED]
  Docs/PROGRESS.md → Assets/_Project/Scripts/Editor/ContentBuilder.cs
- `M1 architecture (quick map)` --references--> `GameBootstrap`  [INFERRED]
  Docs/PROGRESS.md → Assets/_Project/Scripts/Runtime/Core/GameBootstrap.cs
- `M1 architecture (quick map)` --references--> `SectionLayoutGenerator`  [INFERRED]
  Docs/PROGRESS.md → Assets/_Project/Scripts/Runtime/Core/SectionLayoutGenerator.cs
- `17. Sözlük` --references--> `SectionProgress`  [INFERRED]
  Docs/GDD.md → Assets/_Project/Scripts/Runtime/Core/SectionProgress.cs

## Import Cycles
- None detected.

## Communities (31 total, 2 thin omitted)

### Community 0 - "GameDatabase"
Cohesion: 0.05
Nodes (27): BalanceConfig, CategoryDefinition, PlaceSoundType, Metal, Paper, Plastic, CollectibleDefinition, IsMascot (+19 more)

### Community 1 - "GDD — Chubby's Clutter (çalışma adı)"
Cohesion: 0.20
Nodes (9): 0. Bu belge nasıl okunmalı, 13. Fikri mülkiyet ve özgünlük kuralları [KARAR], 14. Ses ve haptik, 18. Açık sorular, 19. Karar günlüğü, 1. Özet, 2. Değişmez prensipler, 3. Referanslar ve farklılaşma (+1 more)

### Community 2 - "8. Eşyalar ve kategoriler"
Cohesion: 0.50
Nodes (4): 8.1 Eşya türleri, 8.2 Kategoriler [KARAR], 8.3 Eşya çeşitliliği, 8. Eşyalar ve kategoriler

### Community 3 - "unityengine"
Cohesion: 0.09
Nodes (6): SortingGame.Tests, SortingGame.Data, SortingGame.UI, SortingGame.Core, SortingGame.EditorTools, SortingGame.Section

### Community 4 - ".Build"
Cohesion: 0.09
Nodes (4): ContentBuilder, ProjectSetup, SectionVisuals, 17. Sözlük

### Community 5 - "ItemView"
Cohesion: 0.09
Nodes (8): Tween, Runner, TweenRunner, ItemView, CanPick, Definition, State, M1 architecture (quick map)

### Community 6 - "SfxPlayer"
Cohesion: 0.10
Nodes (14): Sfx, Coin, Pickup, PlaceMetal, PlacePaper, PlacePlastic, SectionComplete, ShelfFull (+6 more)

### Community 7 - "GameBootstrap"
Cohesion: 0.11
Nodes (7): GameBootstrap, Drag, Hud, Section, Wallet, DragController, InputEnabled

### Community 8 - "Wallet"
Cohesion: 0.31
Nodes (3): Wallet, Coins, WalletTests

### Community 9 - "5. Mekânlar ve bölümler"
Cohesion: 0.29
Nodes (7): 5.1 Yapı [KARAR], 5.2 İlerleme sırası [VARSAYILAN], 5.3 Mekân merdiveni (taslak), 5.4 Bölüm kilitleri [VARSAYILAN], 5.5 Hafif renovasyon [KARAR], 5.6 Mekânın satışı [KARAR], 5. Mekânlar ve bölümler

### Community 10 - "Progress & Handoff"
Cohesion: 0.29
Nodes (6): 2026-10-06 — M0 done, approved, 2026-10-06 — M1 implemented and approved (committed), Current state, Environment notes, Progress & Handoff, Session log

### Community 12 - "15. Teknik notlar (Unity)"
Cohesion: 0.33
Nodes (6): 15.1 Genel, 15.3 Performans, 15.4 Kayıt ve çevrimdışı ilerleme, 15.5 Entegrasyonlar [AÇIK], 15.6 Dil, 15. Teknik notlar (Unity)

### Community 13 - "10. Otomasyon ve ilerleme"
Cohesion: 0.40
Nodes (5): 10.1 Aletler (`Tool`) [KARAR], 10.2 Kategori Ustalığı (`CategoryMastery`) [KARAR], 10.3 Yardımcılar (`Helper`) [KARAR], 10.4 Ölçek büyüdükçe oyuncunun rolü, 10. Otomasyon ve ilerleme

### Community 14 - "12. Görsel yön"
Cohesion: 0.40
Nodes (5): 12.1 Stil [KARAR], 12.2 Arayüz [KARAR], 12.3 Konsept görseller, 12.4 Konsept görsellerdeki bilinen sorunlar [KARAR: oyunda düzeltilecek], 12. Görsel yön

### Community 15 - "4. Oyun döngüleri"
Cohesion: 0.40
Nodes (5): 4.1 Anlık döngü (saniyeler), 4.2 Oturum döngüsü (1–5 dakika), 4.3 Meta döngü (günler/haftalar), 4.4 Kaynak akışı, 4. Oyun döngüleri

### Community 16 - "9. Koleksiyon sistemi"
Cohesion: 0.40
Nodes (5): 9.1 Koleksiyon Kitabı (`CollectionBook`) [KARAR], 9.2 Maskot [KARAR], 9.3 Nadir eşya bulma anı [KARAR], 9.4 Set bonusları [KARAR], 9. Koleksiyon sistemi

### Community 17 - "ShelfView"
Cohesion: 0.07
Nodes (14): PlaceholderFactory, DashedTexture, ShelfSlot, IsFree, IsReserved, Occupant, WorldBase, ShelfView (+6 more)

### Community 18 - "SectionHud"
Cohesion: 0.11
Nodes (3): Loc, SectionHud, UiScale

### Community 19 - "SectionController"
Cohesion: 0.11
Nodes (7): SectionController, Containers, Definition, Items, Progress, Shelves, ViewBounds

### Community 20 - "ContainerView"
Cohesion: 0.08
Nodes (6): Haptics, Vibrator, Ease, ContainerView, Definition, IsOpened

### Community 21 - "SectionProgress"
Cohesion: 0.22
Nodes (9): SectionProgress, DirtCleaned, Fraction, HasDirt, IsComplete, Percent, PlacedItems, TotalItems (+1 more)

### Community 22 - "FeelConfig"
Cohesion: 0.14
Nodes (3): FeelConfig, CameraFitter, CoreLoopTests

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
Cohesion: 0.11
Nodes (5): ContainerContent, SectionLayout, TotalItems, SectionLayoutGenerator, SectionLayoutGeneratorTests

### Community 28 - "PlaceholderShape"
Cohesion: 0.17
Nodes (11): ItemRarity, Common, Mascot, Rare, PlaceholderShape, Book, Capsule, Cube (+3 more)

### Community 29 - "ItemState"
Cohesion: 0.33
Nodes (6): ItemState, Dragging, Flying, Physics, Placed, Resting

## Knowledge Gaps
- **131 isolated node(s):** `Wallet`, `Section`, `Hud`, `Drag`, `Vibrator` (+126 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 249 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **2 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `M1 architecture (quick map)` connect `ItemView` to `GameDatabase`, `.Build`, `SfxPlayer`, `GameBootstrap`, `Wallet`, `Progress & Handoff`, `ShelfView`, `SectionHud`, `SectionController`, `ContainerView`, `FeelConfig`, `.Generate`?**
  _High betweenness centrality (0.208) - this node is a cross-community bridge._
- **Why does `SectionController` connect `SectionController` to `GameDatabase`, `unityengine`, `.Build`, `ItemView`, `GameBootstrap`, `ShelfView`, `SectionHud`, `ContainerView`, `SectionProgress`, `FeelConfig`?**
  _High betweenness centrality (0.191) - this node is a cross-community bridge._
- **Why does `GDD — Chubby's Clutter (çalışma adı)` connect `GDD — Chubby's Clutter (çalışma adı)` to `8. Eşyalar ve kategoriler`, `.Build`, `5. Mekânlar ve bölümler`, `15. Teknik notlar (Unity)`, `10. Otomasyon ve ilerleme`, `12. Görsel yön`, `4. Oyun döngüleri`, `9. Koleksiyon sistemi`, `11. Ekonomi ve gelir modeli`, `7. Etkileşim ve kontroller`, `16. MVP kapsamı`, `6. Görünümler ve kamera`?**
  _High betweenness centrality (0.187) - this node is a cross-community bridge._
- **What connects `Wallet`, `Section`, `Hud` to the rest of the system?**
  _131 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `GameDatabase` be split into smaller, more focused modules?**
  _Cohesion score 0.05263157894736842 - nodes in this community are weakly interconnected._
- **Should `unityengine` be split into smaller, more focused modules?**
  _Cohesion score 0.08959899749373433 - nodes in this community are weakly interconnected._
- **Should `.Build` be split into smaller, more focused modules?**
  _Cohesion score 0.08961593172119488 - nodes in this community are weakly interconnected._