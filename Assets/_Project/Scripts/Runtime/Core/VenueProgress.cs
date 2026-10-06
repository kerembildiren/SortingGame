using System.Collections.Generic;
using SortingGame.Data;

namespace SortingGame.Core
{
    /// <summary>
    /// GDD 5 rules on top of the save: which sections are open, how far a venue is, when it can be sold,
    /// which venue comes next. Pure (no scene access) so it is unit tested.
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
            Locked,     // previous venue not sold yet
            ForSale,    // can be bought now
            Owned,
            Sold
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

        /// <summary>GDD 5.2 linear ladder: venue N is for sale once venue N-1 is sold. The first one is free.</summary>
        public static VenueState State(IReadOnlyList<VenueDefinition> ladder, int index, SaveData data)
        {
            var save = data.Venues.Find(v => v.Id == ladder[index].Id);
            if (save != null && save.Sold) return VenueState.Sold;
            if (save != null && save.Owned) return VenueState.Owned;
            if (index == 0) return VenueState.ForSale;
            var previous = data.Venues.Find(v => v.Id == ladder[index - 1].Id);
            return previous != null && previous.Sold ? VenueState.ForSale : VenueState.Locked;
        }

        /// <summary>GDD 5.6: sell only when every section is at 100%.</summary>
        public static bool CanSell(VenueDefinition venue, SaveData data)
        {
            var save = data.Venues.Find(v => v.Id == venue.Id);
            return save != null && save.Owned && !save.Sold && Venue(venue, data).AllComplete;
        }
    }
}
