# Graph Report - SortingGame  (2026-10-06)

## Corpus Check
- 21 files · ~6,996 words
- Verdict: corpus is large enough that graph structure adds value.
- Unclassified: 58 file(s) not represented in the graph (top: .meta 41, .asset 8, (none) 4)

## Summary
- 183 nodes · 260 edges · 17 communities (14 shown, 3 thin omitted)
- Extraction: 94% EXTRACTED · 6% INFERRED · 0% AMBIGUOUS · INFERRED: 16 edges (avg confidence: 0.89)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `4042bd05`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- GameDatabase
- GDD — Chubby's Clutter (çalışma adı)
- SectionDefinition
- SortingGame.Data
- PlaceholderShape
- PlaceholderVisual
- Milestones
- ProjectSetup
- Wallet
- 5. Mekânlar ve bölümler
- Progress & Handoff
- unity.sh
- 15. Teknik notlar (Unity)
- 10. Otomasyon ve ilerleme
- 12. Görsel yön
- 4. Oyun döngüleri
- 9. Koleksiyon sistemi

## God Nodes (most connected - your core abstractions)
1. `GDD — Chubby's Clutter (çalışma adı)` - 21 edges
2. `GameDatabase` - 17 edges
3. `SortingGame.Data` - 13 edges
4. `ItemDefinition` - 13 edges
5. `SectionDefinition` - 12 edges
6. `CategoryDefinition` - 10 edges
7. `Wallet` - 8 edges
8. `ContainerDefinition` - 8 edges
9. `PlaceholderShape` - 8 edges
10. `CollectibleDefinition` - 7 edges

## Surprising Connections (you probably didn't know these)
- `Conventions` --references--> `BalanceConfig`  [INFERRED]
  CLAUDE.md → Assets/_Project/Scripts/Runtime/Data/BalanceConfig.cs
- `15.2 Veri odaklı tasarım` --references--> `CategoryDefinition`  [INFERRED]
  Docs/GDD.md → Assets/_Project/Scripts/Runtime/Data/CategoryDefinition.cs
- `8.1 Eşya türleri` --references--> `ContainerDefinition`  [INFERRED]
  Docs/GDD.md → Assets/_Project/Scripts/Runtime/Data/ContainerDefinition.cs
- `15.2 Veri odaklı tasarım` --references--> `ItemDefinition`  [INFERRED]
  Docs/GDD.md → Assets/_Project/Scripts/Runtime/Data/ItemDefinition.cs
- `15.2 Veri odaklı tasarım` --references--> `CollectibleDefinition`  [INFERRED]
  Docs/GDD.md → Assets/_Project/Scripts/Runtime/Data/CollectibleDefinition.cs

## Import Cycles
- None detected.

## Communities (17 total, 3 thin omitted)

### Community 0 - "GameDatabase"
Cohesion: 0.16
Nodes (8): BalanceConfig, CategoryDefinition, GameDatabase, ItemDefinition, CoinValue, IsCollectible, ShelfEntry, GameDatabaseTests

### Community 1 - "GDD — Chubby's Clutter (çalışma adı)"
Cohesion: 0.07
Nodes (29): 0. Bu belge nasıl okunmalı, 11.1 Para birimleri, 11.2 Ödüllü reklamlar [KARAR], 11.3 Uygulama içi satın almalar [KARAR], 11.4 Gelir modeli kuralları [KARAR], 11.5 Dengeleme, 11. Ekonomi ve gelir modeli, 13. Fikri mülkiyet ve özgünlük kuralları [KARAR] (+21 more)

### Community 2 - "SectionDefinition"
Cohesion: 0.13
Nodes (12): CollectibleDefinition, IsMascot, ContainerDefinition, ContainerEntry, SectionDefinition, TotalSlotCount, VenueDefinition, 15.2 Veri odaklı tasarım (+4 more)

### Community 3 - "SortingGame.Data"
Cohesion: 0.14
Nodes (4): SortingGame.Tests, SortingGame.Data, SortingGame.Core, SortingGame.EditorTools

### Community 4 - "PlaceholderShape"
Cohesion: 0.17
Nodes (11): ItemRarity, Common, Mascot, Rare, PlaceholderShape, Book, Capsule, Cube (+3 more)

### Community 5 - "PlaceholderVisual"
Cohesion: 0.20
Nodes (7): PlaceholderVisual, Conventions, Git rules, graphify, Session routine, Sorting Game (working title: Chubby's Clutter), Verifying (editor must be closed for batchmode)

### Community 8 - "Wallet"
Cohesion: 0.38
Nodes (3): Wallet, Coins, WalletTests

### Community 9 - "5. Mekânlar ve bölümler"
Cohesion: 0.29
Nodes (7): 5.1 Yapı [KARAR], 5.2 İlerleme sırası [VARSAYILAN], 5.3 Mekân merdiveni (taslak), 5.4 Bölüm kilitleri [VARSAYILAN], 5.5 Hafif renovasyon [KARAR], 5.6 Mekânın satışı [KARAR], 5. Mekânlar ve bölümler

### Community 10 - "Progress & Handoff"
Cohesion: 0.29
Nodes (6): 2026-10-06 — M0 done, approved, Current state, Environment notes, Open questions for the user, Progress & Handoff, Session log

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

## Knowledge Gaps
- **78 isolated node(s):** `SortingGame.EditorTools`, `Coins`, `IsMascot`, `Common`, `Rare` (+73 more)
  These have ≤1 connection - possible missing edges or undocumented components. (Counts symbols only; 100 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **3 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `GDD — Chubby's Clutter (çalışma adı)` connect `GDD — Chubby's Clutter (çalışma adı)` to `SectionDefinition`, `5. Mekânlar ve bölümler`, `15. Teknik notlar (Unity)`, `10. Otomasyon ve ilerleme`, `12. Görsel yön`, `4. Oyun döngüleri`, `9. Koleksiyon sistemi`?**
  _High betweenness centrality (0.478) - this node is a cross-community bridge._
- **Why does `15. Teknik notlar (Unity)` connect `15. Teknik notlar (Unity)` to `GDD — Chubby's Clutter (çalışma adı)`, `SectionDefinition`?**
  _High betweenness centrality (0.365) - this node is a cross-community bridge._
- **Why does `15.2 Veri odaklı tasarım` connect `SectionDefinition` to `GameDatabase`, `15. Teknik notlar (Unity)`?**
  _High betweenness centrality (0.356) - this node is a cross-community bridge._
- **What connects `SortingGame.EditorTools`, `Coins`, `IsMascot` to the rest of the system?**
  _78 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `GDD — Chubby's Clutter (çalışma adı)` be split into smaller, more focused modules?**
  _Cohesion score 0.06666666666666667 - nodes in this community are weakly interconnected._
- **Should `SectionDefinition` be split into smaller, more focused modules?**
  _Cohesion score 0.1286549707602339 - nodes in this community are weakly interconnected._
- **Should `SortingGame.Data` be split into smaller, more focused modules?**
  _Cohesion score 0.14333333333333334 - nodes in this community are weakly interconnected._