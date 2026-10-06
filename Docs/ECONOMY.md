# Economy report

Generated from the content assets by `EconomyReport` (menu `Sorting Game/Reports/Write Economy Report`
or `tools/unity.sh economy`). Do not edit by hand; change the numbers in `ContentBuilder` and regenerate.

## Income: coins a room pays when everything is shelved

| Venue | Room | Items | Coins | Coins so far |
|---|---|---:|---:|---:|
| Comic Box | Comic Box | 60 | 84 | 84 |
| Garage | Garage | 200 | 412 | 496 |
| Warehouse | Office | 300 | 546 | 1042 |
| Warehouse | Loading Dock | 300 | 669 | 1711 |
| Warehouse | Aisle | 300 | 600 | 2311 |
| Warehouse | Basement | 300 | 606 | 2917 |

Whole content: **2917 coins**. Coin values per item: Comics 1, Toys 2, Tools 2, Stationery 2, Mugs 2, Tyres 3, Bottles 2; rare items Sealed First Issue 5, Golden Robot 8, Lucky Wrench 8, Brass Stapler 8, Chrome Hubcap 12, Message in a Bottle 8.

## Shop prices

| What | Level costs | Total | Opens |
|---|---|---:|---|
| Hand (tool) | 0 / 60 / 180 | 240 | from the start |
| Broom (tool) | 0 / 50 / 150 | 200 | from the start |
| Magnet (tool) | 500 / 900 / 1600 | 3000 | needs Hand at max level |
| Pip (helper) | 400 / 800 / 1300 | 2500 | Garage fully restored |
| Dot (helper) | 750 / 1200 / 1900 | 3850 | 50% of Warehouse |

Everything in the Shop: **9790 coins**, 3.4 times what the content pays.
The rest is there for the venues that come after the MVP (GDD 5.3).

## Reference player

Plays the rooms in order, earns evenly through each room and always saves for the cheapest thing the Shop
offers at that moment. Real players choose differently; this is a yardstick, not a prediction.

| # | Purchase | Cost | Bought in | At |
|---:|---|---:|---|---:|
| 1 | Broom level 2 | 50 | Comic Box | 60% |
| 2 | Hand level 2 | 60 | Garage | 7% |
| 3 | Broom level 3 | 150 | Garage | 43% |
| 4 | Hand level 3 | 180 | Garage | 87% |
| 5 | Pip level 1 | 400 | Office | 64% |
| 6 | Magnet level 1 | 500 | Loading Dock | 45% |
| 7 | Dot level 1 | 750 | Aisle | 64% |
| 8 | Pip level 2 | 800 | Basement | 96% |
| 9 | Magnet level 2 | 900 | not within this content |  |
| 10 | Dot level 2 | 1200 | not within this content |  |
| 11 | Pip level 3 | 1300 | not within this content |  |
| 12 | Magnet level 3 | 1600 | not within this content |  |
| 13 | Dot level 3 | 1900 | not within this content |  |

Not in this model: Auto Sort (ads or real money, never coins), the Basement shortcut
(150 coins, optional), and how fast a player actually sorts.
