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
| Abandoned Circus | Ticket Booth | 400 | 858 | 3775 |
| Abandoned Circus | Snack Stand | 400 | 900 | 4675 |
| Abandoned Circus | Costume Room | 400 | 1061 | 5736 |
| Abandoned Circus | Clown Caravan | 400 | 1061 | 6797 |
| Abandoned Circus | Music Wagon | 400 | 1014 | 7811 |
| Abandoned Circus | Prop Store | 400 | 1100 | 8911 |
| Abandoned Circus | Backstage | 400 | 1000 | 9911 |
| Abandoned Circus | Main Tent | 400 | 1161 | 11072 |

Whole content: **11072 coins**. Coin values per item: Comics 1, Toys 2, Tools 2, Stationery 2, Mugs 2, Tyres 3, Bottles 2, Tickets 2, Snacks 2, Hats 3, Costumes 3, Masks 3, Juggling 3, Instruments 4, Lanterns 3; rare items Sealed First Issue 5, Golden Robot 8, Lucky Wrench 8, Brass Stapler 8, Chrome Hubcap 12, Message in a Bottle 8, Golden Ticket 10, Sequined Top Hat 14, Lucky Clown Nose 14, Silver Trumpet 18, Star Lantern 14.

## Shop prices

| What | Level costs | Total | Opens |
|---|---|---:|---|
| Hand (tool) | 0 / 60 / 180 | 240 | from the start |
| Broom (tool) | 0 / 50 / 150 | 200 | from the start |
| Magnet (tool) | 500 / 900 / 1600 / 2400 | 5400 | needs Hand at max level |
| Pip (helper) | 400 / 800 / 1300 / 2000 | 4500 | Garage fully restored |
| Dot (helper) | 750 / 1200 / 1900 / 2600 | 6450 | 50% of Warehouse |

Everything in the Shop: **16790 coins**, 1.5 times what the content pays.
What the content does not pay for is left for later venues (GDD 5.3, 11.5).

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
| 9 | Magnet level 2 | 900 | Snack Stand | 2% |
| 10 | Dot level 2 | 1200 | Costume Room | 30% |
| 11 | Pip level 3 | 1300 | Clown Caravan | 53% |
| 12 | Magnet level 3 | 1600 | Prop Store | 8% |
| 13 | Dot level 3 | 1900 | Backstage | 88% |
| 14 | Pip level 4 | 2000 | not within this content |  |
| 15 | Magnet level 4 | 2400 | not within this content |  |
| 16 | Dot level 4 | 2600 | not within this content |  |

Not in this model: Auto Sort (ads or real money, never coins) and how fast a player actually sorts.
Coins buy tools and helpers only: venues and locked rooms open by progress.
