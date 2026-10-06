using System.Collections.Generic;
using SortingGame.Data;

namespace SortingGame.Core
{
    /// <summary>
    /// GDD 11.5: the economy on paper, computed from the content itself. How many coins each room pays, what
    /// everything in the Shop costs, and when a "reference player" can afford each purchase. Used by the balance
    /// tests (they guard the intended pacing) and by the economy report (Docs/ECONOMY.md). Pure: reads data only.
    /// </summary>
    public static class EconomyModel
    {
        public struct RoomIncome
        {
            public VenueDefinition Venue;
            public SectionDefinition Section;
            public int Items;
            public int Coins;
            /// <summary>Coins earned in all rooms before this one.</summary>
            public int CoinsBefore;
        }

        public class Purchase
        {
            public string Id;       // e.g. "tool.magnet"
            public int Level;
            public int Cost;
            public bool Reached;
            public SectionDefinition Room;   // where the reference player buys it
            public int RoomPercent;          // how far through that room
        }

        /// <summary>Coins a room pays when everything in it is shelved: common items at their category value, rare items at theirs.</summary>
        public static int CoinsIn(SectionDefinition section)
        {
            var coins = 0;
            foreach (var shelf in section.Shelves)
            {
                var rares = 0;
                foreach (var rare in section.RareItems)
                {
                    if (rare == null || rare.Category != shelf.Category || rares >= shelf.SlotCount) continue;
                    coins += rare.CoinValue;
                    rares++;
                }
                coins += (shelf.SlotCount - rares) * shelf.Category.BaseCoinValue;
            }
            return coins;
        }

        /// <summary>Every room in play order with its income.</summary>
        public static List<RoomIncome> Ladder(GameDatabase database)
        {
            var rooms = new List<RoomIncome>();
            var before = 0;
            foreach (var venue in database.Venues)
            foreach (var section in venue.Sections)
            {
                var coins = CoinsIn(section);
                rooms.Add(new RoomIncome { Venue = venue, Section = section, Items = section.TotalSlotCount, Coins = coins, CoinsBefore = before });
                before += coins;
            }
            return rooms;
        }

        public static int TotalIncome(GameDatabase database)
        {
            var total = 0;
            foreach (var room in Ladder(database)) total += room.Coins;
            return total;
        }

        public static int TotalCost(ToolDefinition tool)
        {
            var total = 0;
            foreach (var level in tool.Levels) total += level.Cost;
            return total;
        }

        public static int TotalCost(HelperDefinition helper)
        {
            var total = 0;
            foreach (var level in helper.Levels) total += level.Cost;
            return total;
        }

        /// <summary>Everything coins can buy in the Shop (room unlock shortcuts are optional and left out).</summary>
        public static int TotalSinks(GameDatabase database)
        {
            var total = 0;
            foreach (var tool in database.Tools) total += TotalCost(tool);
            foreach (var helper in database.Helpers) total += TotalCost(helper);
            return total;
        }

        /// <summary>
        /// The reference player plays the rooms in order, earns coins evenly through each room and always saves
        /// for the cheapest thing the Shop offers at that moment (tool gates and helper slots respected).
        /// Returns every purchase in the order it happens; what the content cannot pay for comes last, not reached.
        /// </summary>
        public static List<Purchase> ReferenceTimeline(GameDatabase database)
        {
            const int steps = 100;
            var done = new List<Purchase>();
            var toolLevels = new Dictionary<ToolDefinition, int>();
            var helperLevels = new Dictionary<HelperDefinition, int>();
            foreach (var tool in database.Tools) toolLevels[tool] = tool.Levels.Count > 0 && tool.Levels[0].Cost == 0 ? 1 : 0;
            foreach (var helper in database.Helpers) helperLevels[helper] = 0;

            var venueItems = new Dictionary<VenueDefinition, int>();
            var venueTotals = new Dictionary<VenueDefinition, int>();
            var venueRoomsDone = new Dictionary<VenueDefinition, int>();
            foreach (var venue in database.Venues)
            {
                venueItems[venue] = 0;
                venueRoomsDone[venue] = 0;
                var total = 0;
                foreach (var section in venue.Sections) total += section.TotalSlotCount;
                venueTotals[venue] = total;
            }

            bool SlotOpen(HelperDefinition helper)
            {
                var venue = helper.RequiredVenue;
                if (venue == null) return true;
                if (helper.RequiredVenuePercent >= 100) return venueRoomsDone[venue] >= venue.Sections.Count;
                return venueTotals[venue] > 0 && venueItems[venue] * 100f / venueTotals[venue] >= helper.RequiredVenuePercent;
            }

            // The cheapest thing on offer right now; null when the Shop has nothing left.
            Purchase Cheapest()
            {
                Purchase best = null;
                foreach (var tool in database.Tools)
                {
                    var level = toolLevels[tool];
                    if (level >= tool.Levels.Count) continue;
                    var gated = level == 0 && tool.RequiresMaxed != null && toolLevels[tool.RequiresMaxed] < tool.RequiresMaxed.Levels.Count;
                    if (gated) continue;
                    var cost = tool.Levels[level].Cost;
                    if (best == null || cost < best.Cost) best = new Purchase { Id = $"tool.{tool.Id}", Level = level + 1, Cost = cost };
                }
                foreach (var helper in database.Helpers)
                {
                    var level = helperLevels[helper];
                    if (level >= helper.Levels.Count || (level == 0 && !SlotOpen(helper))) continue;
                    var cost = helper.Levels[level].Cost;
                    if (best == null || cost < best.Cost) best = new Purchase { Id = $"helper.{helper.Id}", Level = level + 1, Cost = cost };
                }
                return best;
            }

            void Apply(Purchase purchase)
            {
                foreach (var tool in database.Tools)
                    if (purchase.Id == $"tool.{tool.Id}") toolLevels[tool] = purchase.Level;
                foreach (var helper in database.Helpers)
                    if (purchase.Id == $"helper.{helper.Id}") helperLevels[helper] = purchase.Level;
            }

            var coins = 0f;
            foreach (var room in Ladder(database))
            {
                var itemsAtRoomStart = venueItems[room.Venue];
                for (var step = 1; step <= steps; step++)
                {
                    coins += room.Coins / (float)steps;
                    venueItems[room.Venue] = itemsAtRoomStart + room.Items * step / steps;
                    if (step == steps) venueRoomsDone[room.Venue]++;

                    for (var next = Cheapest(); next != null && coins + 0.001f >= next.Cost; next = Cheapest())
                    {
                        coins -= next.Cost;
                        next.Reached = true;
                        next.Room = room.Section;
                        next.RoomPercent = step;
                        Apply(next);
                        done.Add(next);
                    }
                }
            }

            // What is left in the Shop when the content runs out, cheapest first.
            for (var next = Cheapest(); next != null; next = Cheapest())
            {
                Apply(next);
                done.Add(next);
            }
            return done;
        }
    }
}
