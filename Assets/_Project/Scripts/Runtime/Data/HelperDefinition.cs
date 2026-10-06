using System;
using System.Collections.Generic;
using UnityEngine;

namespace SortingGame.Data
{
    /// <summary>
    /// GDD 10.3. A helper: a small creature that works next to the player in whatever room the player is in,
    /// shelving common items one trip at a time. Hired and upgraded with coins; level 1 cost = hire price.
    /// Its slot in the Shop opens with venue progress, not with a player level.
    /// </summary>
    [CreateAssetMenu(menuName = "Sorting Game/Helper", fileName = "Helper_")]
    public class HelperDefinition : ScriptableObject
    {
        [Serializable]
        public struct Level
        {
            public int Cost;
            [Tooltip("Walking speed in metres per second.")]
            public float Speed;
            [Tooltip("Items carried per trip.")]
            public int Capacity;
        }

        public string Id;
        public string DisplayNameKey;

        [Header("Placeholder look")]
        public Color BodyColor = new(0.62f, 0.9f, 0.78f);
        public Color AccentColor = new(1f, 0.62f, 0.45f);

        [Header("Shop slot (GDD 10.3: tied to venue progress)")]
        [Tooltip("The slot opens when this venue has reached RequiredVenuePercent. Empty = open from the start.")]
        public VenueDefinition RequiredVenue;
        [Range(1, 100), Tooltip("Share of the venue's items that must be shelved. 100 = every room finished.")]
        public int RequiredVenuePercent = 100;

        public List<Level> Levels = new();

        public int MaxLevel => Levels.Count;

        /// <summary>Stats for a level (1-based). Level 0 (not hired) returns level 1 stats, for previews.</summary>
        public Level Stats(int level) => Levels[Mathf.Clamp(level, 1, Levels.Count) - 1];
    }
}
