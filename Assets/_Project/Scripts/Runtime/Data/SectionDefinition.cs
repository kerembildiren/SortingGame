using System;
using System.Collections.Generic;
using UnityEngine;

namespace SortingGame.Data
{
    /// <summary>
    /// GDD 5.1 / 15.2. One playable room. Item count is derived from shelf slots:
    /// the generator creates exactly enough items to fill every shelf (a rare item takes one slot of its category).
    /// </summary>
    [CreateAssetMenu(menuName = "Sorting Game/Section", fileName = "Section_")]
    public class SectionDefinition : ScriptableObject
    {
        [Serializable]
        public struct ShelfEntry
        {
            public CategoryDefinition Category;
            [Min(1)] public int SlotCount;
        }

        [Serializable]
        public struct ContainerEntry
        {
            public ContainerDefinition Container;
            [Min(1)] public int Count;
        }

        public string Id;
        public string DisplayNameKey;

        [Tooltip("Floor size in metres (x = width, y = depth).")]
        public Vector2 FloorSize = new(6f, 8f);

        public List<ShelfEntry> Shelves = new();
        public List<ContainerEntry> Containers = new();
        [Tooltip("Chubby hidden in this room, if any (GDD 9.2: one per venue).")]
        public List<CollectibleDefinition> Collectibles = new();
        [Tooltip("Blue-glowing rare items (GDD 9.4). Each replaces one common item of its category, so that category needs a shelf here.")]
        public List<ItemDefinition> RareItems = new();

        [Range(0f, 1f), Tooltip("Share of items lying loose on the floor instead of inside containers.")]
        public float LooseItemRatio = 0.2f;

        [Range(0f, 1f), Tooltip("Share of the floor covered by the dirt layer at start. 0 = no dirt, no broom needed.")]
        public float DirtCoverage = 0.6f;

        [Range(0f, 1f), Tooltip("Share of items hidden under the dirt, revealed by sweeping (GDD 7.1). Needs DirtCoverage > 0.")]
        public float BuriedItemRatio = 0.15f;

        [Tooltip("Same seed = same layout every time.")]
        public int Seed = 1;

        [Tooltip("Locked at start (GDD 5.4).")]
        public bool StartsLocked;
        [Tooltip("Coins to unlock right away. 0 = cannot be bought.")]
        public int UnlockCoinCost;
        [Range(0, 100), Tooltip("Unlocks by itself when the venue's other unlocked sections reach this average %. 0 = never.")]
        public int UnlockAtVenuePercent;

        public int TotalSlotCount
        {
            get
            {
                var total = 0;
                foreach (var shelf in Shelves) total += shelf.SlotCount;
                return total;
            }
        }
    }
}
