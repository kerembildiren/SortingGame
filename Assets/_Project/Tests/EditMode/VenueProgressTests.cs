using System.Collections.Generic;
using NUnit.Framework;
using SortingGame.Core;
using SortingGame.Data;
using UnityEngine;

namespace SortingGame.Tests
{
    public class VenueProgressTests
    {
        readonly List<Object> _created = new();
        List<VenueDefinition> _ladder;
        SectionDefinition _office, _dock, _basement;

        [SetUp]
        public void SetUp()
        {
            _office = MakeSection("office", 20);
            _dock = MakeSection("dock", 20);
            _basement = MakeSection("basement", 10);
            _basement.StartsLocked = true;
            _basement.UnlockAtVenuePercent = 60;
            _basement.UnlockCoinCost = 150;
            _ladder = new List<VenueDefinition>
            {
                MakeVenue("box", MakeSection("box_room", 12)),
                MakeVenue("warehouse", _office, _dock, _basement)
            };
        }

        [TearDown]
        public void TearDown()
        {
            foreach (var o in _created) Object.DestroyImmediate(o);
            _created.Clear();
        }

        SectionDefinition MakeSection(string id, int slots)
        {
            var category = ScriptableObject.CreateInstance<CategoryDefinition>();
            var section = ScriptableObject.CreateInstance<SectionDefinition>();
            _created.Add(category);
            _created.Add(section);
            section.Id = id;
            section.Shelves.Add(new SectionDefinition.ShelfEntry { Category = category, SlotCount = slots });
            return section;
        }

        VenueDefinition MakeVenue(string id, params SectionDefinition[] sections)
        {
            var venue = ScriptableObject.CreateInstance<VenueDefinition>();
            _created.Add(venue);
            venue.Id = id;
            venue.Sections.AddRange(sections);
            return venue;
        }

        static void SetSection(SaveData data, string id, int placed, int total, bool completed = false) =>
            data.SetSection(new SectionSave { SectionId = id, PlacedItems = placed, TotalItems = total, Fraction = (float)placed / total, Completed = completed });

        [Test]
        public void Ladder_FirstIsForSale_NextOnlyAfterSelling()
        {
            var data = new SaveData();
            Assert.AreEqual(VenueProgress.VenueState.ForSale, VenueProgress.State(_ladder, 0, data));
            Assert.AreEqual(VenueProgress.VenueState.Locked, VenueProgress.State(_ladder, 1, data));

            data.Venue("box").Owned = true;
            Assert.AreEqual(VenueProgress.VenueState.Owned, VenueProgress.State(_ladder, 0, data));
            Assert.AreEqual(VenueProgress.VenueState.Locked, VenueProgress.State(_ladder, 1, data));

            data.Venue("box").Sold = true;
            Assert.AreEqual(VenueProgress.VenueState.Sold, VenueProgress.State(_ladder, 0, data));
            Assert.AreEqual(VenueProgress.VenueState.ForSale, VenueProgress.State(_ladder, 1, data));
        }

        [Test]
        public void VenueStatus_SumsSections_UnvisitedCountsItsSlots()
        {
            var data = new SaveData();
            SetSection(data, "office", 10, 20);

            var status = VenueProgress.Venue(_ladder[1], data);

            Assert.AreEqual(10, status.Placed);
            Assert.AreEqual(50, status.Total);
            Assert.IsFalse(status.AllComplete);
        }

        [Test]
        public void CanSell_OnlyWhenOwnedAndEverySectionComplete()
        {
            var data = new SaveData();
            data.Venue("box").Owned = true;
            Assert.IsFalse(VenueProgress.CanSell(_ladder[0], data));

            SetSection(data, "box_room", 12, 12, true);
            Assert.IsTrue(VenueProgress.CanSell(_ladder[0], data));

            data.Venue("box").Sold = true;
            Assert.IsFalse(VenueProgress.CanSell(_ladder[0], data), "Cannot sell twice.");
        }

        [Test]
        public void LockedSection_OpensWhenOthersReachThreshold()
        {
            var data = new SaveData();
            Assert.IsFalse(VenueProgress.IsUnlocked(_basement, data));

            SetSection(data, "office", 10, 20);  // 50%
            SetSection(data, "dock", 14, 20);    // 70%  -> average 60%
            CollectionAssert.AreEqual(new[] { _basement }, VenueProgress.NewlyUnlockable(_ladder[1], data));

            data.UnlockedSections.Add("basement");
            Assert.IsTrue(VenueProgress.IsUnlocked(_basement, data));
            Assert.IsEmpty(VenueProgress.NewlyUnlockable(_ladder[1], data));
        }

        [Test]
        public void LockedSection_StaysLockedBelowThreshold()
        {
            var data = new SaveData();
            SetSection(data, "office", 10, 20);
            SetSection(data, "dock", 10, 20);
            Assert.IsEmpty(VenueProgress.NewlyUnlockable(_ladder[1], data));
        }
    }
}
