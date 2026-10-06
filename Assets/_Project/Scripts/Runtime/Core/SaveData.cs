using System;
using System.Collections.Generic;
using UnityEngine;

namespace SortingGame.Core
{
    /// <summary>
    /// GDD 15.4: everything that must survive a restart. Principle 1: a sorted place stays sorted,
    /// so the full section state (shelves, floor, dirt, unopened boxes) is stored, not just numbers.
    /// JsonUtility-friendly: lists of structs/classes only, no dictionaries.
    /// </summary>
    [Serializable]
    public class SaveData
    {
        public const int CurrentVersion = 2;

        public int Version = CurrentVersion;
        public string SavedAtUtc;
        public long Coins;
        public List<string> Collection = new();
        public List<IdCount> Mastery = new();
        public List<IdCount> Tools = new();
        public List<SectionSave> Sections = new();

        // M4: where the player is and what they own.
        public string CurrentVenueId;
        public string CurrentSectionId; // empty = venue overview
        public List<VenueSave> Venues = new();
        public List<string> UnlockedSections = new();

        public SectionSave SectionById(string id) => Sections.Find(s => s.SectionId == id);

        public VenueSave Venue(string id)
        {
            var venue = Venues.Find(v => v.Id == id);
            if (venue != null) return venue;
            venue = new VenueSave { Id = id };
            Venues.Add(venue);
            return venue;
        }

        public void SetSection(SectionSave section)
        {
            Sections.RemoveAll(s => s.SectionId == section.SectionId);
            Sections.Add(section);
        }
    }

    [Serializable]
    public struct IdCount
    {
        public string Id;
        public int Count;

        public IdCount(string id, int count)
        {
            Id = id;
            Count = count;
        }
    }

    [Serializable]
    public class VenueSave
    {
        public string Id;
        public bool Owned;
        public bool Sold;
    }

    [Serializable]
    public class SectionSave
    {
        public string SectionId;
        public int Seed;
        public bool Completed;
        // Summary for the overview, so it never needs to rebuild a section to show its %.
        public int PlacedItems;
        public int TotalItems;
        public float Fraction;
        public List<ItemSave> Items = new();
        public List<ContainerSave> Containers = new();

        // Dirt mask, gzip + base64. Empty = no dirt layer.
        public int DirtWidth;
        public int DirtHeight;
        public long DirtInitialTotal;
        public string DirtData;
    }

    public enum ItemSaveState
    {
        Floor = 0,
        Placed = 1,
        Buried = 2
    }

    [Serializable]
    public struct ItemSave
    {
        public string Id;
        public ItemSaveState State;
        public int Shelf;   // Placed only
        public int Slot;    // Placed only
        public Vector3 Position;
        public Quaternion Rotation;
    }

    [Serializable]
    public class ContainerSave
    {
        public string Id;
        public Vector3 Position;
        public float Yaw;
        public List<string> Contents = new();
    }
}
