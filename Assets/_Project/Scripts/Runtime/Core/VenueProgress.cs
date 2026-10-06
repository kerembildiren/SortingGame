using System.Collections.Generic;
using SortingGame.Data;

namespace SortingGame.Core
{
    /// <summary>
    /// GDD 5 rules on top of the save: which sections are open, how far a venue is, which venues are open.
    /// Since 2026-10-06 there is no selling or buying: when every room of a venue is 100% the next venue opens.
    /// Pure (no scene access) so it is unit tested.
    /// </summary>
    public static class VenueProgress
    {
        public struct SectionStatus
        {
            public bool Unlocked;
            public bool Completed;
            public int Placed;
            public int Total;
            public float Fraction;
            public int Percent => Completed ? 100 : System.Math.Min(99, (int)(Fraction * 100f));
        }

        public struct VenueStatus
        {
            public int Placed;
            public int Total;
            public bool AllComplete;
            public float Fraction => Total == 0 ? 0f : (float)Placed / Total;
        }

        public enum VenueState
        {
            Locked,     // the previous venue is not finished yet
            Open,       // playable
            Completed   // every room at 100%, stays visitable
        }

        public static SectionStatus Section(SectionDefinition section, SaveData data)
        {
            var save = data.SectionById(section.Id);
            return new SectionStatus
            {
                Unlocked = IsUnlocked(section, data),
                Completed = save != null && save.Completed,
                Placed = save?.PlacedItems ?? 0,
                Total = save != null && save.TotalItems > 0 ? save.TotalItems : section.TotalSlotCount,
                Fraction = save?.Fraction ?? 0f
            };
        }

        public static bool IsUnlocked(SectionDefinition section, SaveData data) =>
            !section.StartsLocked || data.UnlockedSections.Contains(section.Id);

        public static VenueStatus Venue(VenueDefinition venue, SaveData data)
        {
            var status = new VenueStatus { AllComplete = venue.Sections.Count > 0 };
            foreach (var section in venue.Sections)
            {
                var s = Section(section, data);
                status.Placed += s.Placed;
                status.Total += s.Total;
                if (!s.Completed) status.AllComplete = false;
            }
            return status;
        }

        /// <summary>GDD 5.4: a locked section opens when the venue's other open sections reach its threshold.</summary>
        public static bool MeetsAutoUnlock(SectionDefinition section, VenueDefinition venue, SaveData data)
        {
            if (!section.StartsLocked || section.UnlockAtVenuePercent <= 0) return false;
            var sum = 0f;
            var count = 0;
            foreach (var other in venue.Sections)
            {
                if (other == section || !IsUnlocked(other, data)) continue;
                var s = Section(other, data);
                sum += s.Completed ? 1f : s.Fraction;
                count++;
            }
            return count > 0 && sum / count * 100f >= section.UnlockAtVenuePercent;
        }

        /// <summary>Sections that just became eligible; caller records them in data.UnlockedSections.</summary>
        public static List<SectionDefinition> NewlyUnlockable(VenueDefinition venue, SaveData data)
        {
            var result = new List<SectionDefinition>();
            foreach (var section in venue.Sections)
                if (!IsUnlocked(section, data) && MeetsAutoUnlock(section, venue, data))
                    result.Add(section);
            return result;
        }

        /// <summary>Linear ladder: the first venue is open; each next one opens when the previous is fully complete.</summary>
        public static VenueState State(IReadOnlyList<VenueDefinition> ladder, int index, SaveData data)
        {
            if (Venue(ladder[index], data).AllComplete) return VenueState.Completed;
            if (index == 0 || Venue(ladder[index - 1], data).AllComplete) return VenueState.Open;
            return VenueState.Locked;
        }

        public static bool IsOpen(IReadOnlyList<VenueDefinition> ladder, VenueDefinition venue, SaveData data)
        {
            var index = IndexOf(ladder, venue);
            return index >= 0 && State(ladder, index, data) != VenueState.Locked;
        }

        /// <summary>The venue after <paramref name="venue"/>, or null at the end of the ladder.</summary>
        public static VenueDefinition Next(IReadOnlyList<VenueDefinition> ladder, VenueDefinition venue)
        {
            var index = IndexOf(ladder, venue);
            return index >= 0 && index + 1 < ladder.Count ? ladder[index + 1] : null;
        }

        static int IndexOf(IReadOnlyList<VenueDefinition> ladder, VenueDefinition venue)
        {
            for (var i = 0; i < ladder.Count; i++)
                if (ladder[i] == venue) return i;
            return -1;
        }
    }
}
