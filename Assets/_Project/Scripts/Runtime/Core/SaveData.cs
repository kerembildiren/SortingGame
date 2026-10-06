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
        public const int CurrentVersion = 1;

        public int Version = CurrentVersion;
        public string SavedAtUtc;
        public long Coins;
        public List<string> Collection = new();
        public List<IdCount> Mastery = new();
        public List<IdCount> Tools = new();
        public List<SectionSave> Sections = new();

        public SectionSave SectionById(string id) => Sections.Find(s => s.SectionId == id);

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
    public class SectionSave
    {
        public string SectionId;
        public int Seed;
        public bool Completed;
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
