using System;
using System.Collections.Generic;
using UnityEngine;

namespace SortingGame.Data
{
    /// <summary>
    /// GDD 10.1. A tool and its coin upgrade ladder. Level 1 is the first entry; its cost is the unlock price
    /// (0 = owned from the start). What Primary/Secondary mean depends on the tool:
    /// Hand: how many items can be carried at once / pick-up radius (m).
    /// Broom: brush radius (m) / unused. Magnet: pull radius (m) / max extra items pulled.
    /// </summary>
    [CreateAssetMenu(menuName = "Sorting Game/Tool", fileName = "Tool_")]
    public class ToolDefinition : ScriptableObject
    {
        [Serializable]
        public struct Level
        {
            public int Cost;
            public float Primary;
            public float Secondary;
        }

        public string Id;
        public ToolType Type;
        public string DisplayNameKey;
        [Tooltip("Shown in the shop, formatted with {0} = Primary and {1} = Secondary of the next level.")]
        public string EffectKey;
        public List<Level> Levels = new();

        public int MaxLevel => Levels.Count;
        public bool StartsOwned => Levels.Count > 0 && Levels[0].Cost == 0;

        /// <summary>Stats for a level (1-based). Level 0 (locked) returns level 1 stats.</summary>
        public Level Stats(int level) => Levels[Mathf.Clamp(level, 1, Levels.Count) - 1];
    }
}
