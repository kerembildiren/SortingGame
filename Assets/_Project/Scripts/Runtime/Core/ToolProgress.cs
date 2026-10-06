using System;
using System.Collections.Generic;
using SortingGame.Data;

namespace SortingGame.Core
{
    /// <summary>GDD 10.1: owned tools and their levels, bought with coins. Level 0 = locked.</summary>
    public class ToolProgress
    {
        readonly Dictionary<string, int> _levels = new();

        public event Action<ToolDefinition, int> Upgraded;

        public int LevelOf(ToolDefinition tool)
        {
            if (tool == null) return 0;
            if (_levels.TryGetValue(tool.Id, out var level)) return level;
            return tool.StartsOwned ? 1 : 0;
        }

        public bool IsOwned(ToolDefinition tool) => LevelOf(tool) > 0;
        public bool IsMaxed(ToolDefinition tool) => LevelOf(tool) >= tool.MaxLevel;

        /// <summary>A locked tool may need another tool at max level first (GDD 10.1: Magnet needs Hand).</summary>
        public bool MeetsRequirement(ToolDefinition tool) => tool.RequiresMaxed == null || IsMaxed(tool.RequiresMaxed);

        /// <summary>Coins aside: may the next level be bought at all?</summary>
        public bool CanBuy(ToolDefinition tool) => !IsMaxed(tool) && (IsOwned(tool) || MeetsRequirement(tool));

        /// <summary>Cost of the next level (unlock price when locked); -1 when maxed.</summary>
        public int NextCost(ToolDefinition tool) => IsMaxed(tool) ? -1 : tool.Levels[LevelOf(tool)].Cost;

        /// <summary>Current stats (level 1 stats while locked, so previews work).</summary>
        public ToolDefinition.Level Stats(ToolDefinition tool) => tool.Stats(LevelOf(tool));

        public bool TryUpgrade(ToolDefinition tool, Wallet wallet)
        {
            if (tool == null || !CanBuy(tool)) return false;
            var cost = NextCost(tool);
            if (!wallet.TrySpend(cost)) return false;
            var level = LevelOf(tool) + 1;
            _levels[tool.Id] = level;
            Upgraded?.Invoke(tool, level);
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
