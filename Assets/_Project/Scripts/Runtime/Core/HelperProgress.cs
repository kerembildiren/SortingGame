using System;
using System.Collections.Generic;
using SortingGame.Data;

namespace SortingGame.Core
{
    /// <summary>
    /// GDD 10.3: hired helpers and their levels, bought with coins. Level 0 = not hired.
    /// Whether a helper's Shop slot is open is a venue rule (<see cref="VenueProgress.HelperSlotOpen"/>);
    /// the caller passes the answer in.
    /// </summary>
    public class HelperProgress
    {
        readonly Dictionary<string, int> _levels = new();

        public event Action<HelperDefinition, int> Changed;

        public int LevelOf(HelperDefinition helper) =>
            helper != null && _levels.TryGetValue(helper.Id, out var level) ? level : 0;

        public bool IsHired(HelperDefinition helper) => LevelOf(helper) > 0;
        public bool IsMaxed(HelperDefinition helper) => LevelOf(helper) >= helper.MaxLevel;

        /// <summary>Cost of the next level (hire price while not hired); -1 when maxed.</summary>
        public int NextCost(HelperDefinition helper) => IsMaxed(helper) ? -1 : helper.Levels[LevelOf(helper)].Cost;

        /// <summary>Current stats (level 1 stats while not hired, so previews work).</summary>
        public HelperDefinition.Level Stats(HelperDefinition helper) => helper.Stats(LevelOf(helper));

        /// <param name="slotOpen">Only matters for hiring; a hired helper can always be upgraded.</param>
        public bool TryUpgrade(HelperDefinition helper, Wallet wallet, bool slotOpen)
        {
            if (helper == null || IsMaxed(helper)) return false;
            if (!IsHired(helper) && !slotOpen) return false;
            if (!wallet.TrySpend(NextCost(helper))) return false;
            var level = LevelOf(helper) + 1;
            _levels[helper.Id] = level;
            Changed?.Invoke(helper, level);
            return true;
        }

        public IEnumerable<KeyValuePair<string, int>> Export() => _levels;

        public void Restore(IEnumerable<KeyValuePair<string, int>> levels)
        {
            _levels.Clear();
            foreach (var pair in levels) _levels[pair.Key] = pair.Value;
        }
    }
}
