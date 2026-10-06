using System.Linq;
using SortingGame.Data;
using SortingGame.Overview;
using SortingGame.Section;
using SortingGame.UI;
using UnityEngine;

namespace SortingGame.Core
{
    /// <summary>
    /// Screen flow (GDD 5, 6): Map (venue ladder, a modal over the overview) -> Overview (isometric venue) -> Section.
    /// Owns transitions, venue progression (all rooms 100% opens the next venue) and section unlocking.
    /// Data lives in <see cref="SaveData"/>.
    /// </summary>
    public class GameFlow : MonoBehaviour
    {
        public enum Screen
        {
            Overview,
            Section
        }

        const float ZoomInTime = 0.45f;
        const float ZoomOutTime = 0.5f;
        const float FadeTime = 0.3f;

        public Screen Current { get; private set; } = Screen.Overview;
        public VenueDefinition Venue { get; private set; }
        public SectionDefinition ActiveSection { get; private set; }
        public bool Busy { get; private set; }

        GameBootstrap _boot;
        GameContext _ctx;
        SectionHud _hud;
        OverviewController _overview;
        SectionController _section;
        DragController _drag;
        Camera _camera;

        SaveData Data => _boot.Data;

        public void Init(GameBootstrap boot, OverviewController overview, Camera cam)
        {
            _boot = boot;
            _ctx = boot.Context;
            _hud = boot.Hud;
            _section = boot.Section;
            _drag = boot.Drag;
            _overview = overview;
            _camera = cam;

            _overview.RoomTapped += OnRoomTapped;
            _hud.BackRequested += OnBack;
            _hud.NextVenueRequested += GoToNextVenue;
            _hud.VenueOpenRequested += venue => SwitchVenue(venue);
            _hud.UnlockRequested += UnlockWithCoins;
        }

        /// <summary>App start: continue where the player left off (GDD 15.4).</summary>
        public void Resume()
        {
            var ladder = _ctx.Database.Venues;
            if (ladder.Count == 0) return;
            Data.Venue(ladder[0].Id).Owned = true; // the first place is always open (tutorial venue)

            // Last place visited if still open, otherwise the furthest open place.
            var venue = ladder.FirstOrDefault(v => v.Id == Data.CurrentVenueId && IsOwned(v)) ?? ladder.Last(IsOwned);
            var section = venue.Sections.FirstOrDefault(s => s.Id == Data.CurrentSectionId);
            if (section != null && VenueProgress.IsUnlocked(section, Data)) OpenSectionNow(venue, section);
            else ShowOverviewNow(venue);
        }

        bool IsOwned(VenueDefinition venue) => VenueProgress.IsOpen(_ctx.Database.Venues, venue, Data);

        // ---------- Overview ----------

        void ShowOverviewNow(VenueDefinition venue)
        {
            _boot.Inspector.StopNow();
            Venue = venue;
            ActiveSection = null;
            Current = Screen.Overview;
            Data.CurrentVenueId = venue.Id;
            Data.CurrentSectionId = "";

            var unlocked = VenueProgress.NewlyUnlockable(venue, Data);
            foreach (var section in unlocked) Data.UnlockedSections.Add(section.Id);

            _drag.CancelDrag();
            _drag.InputEnabled = false;
            if (_camera.TryGetComponent<CameraFitter>(out var fitter)) fitter.enabled = false;
            SfxPlayer.Instance?.SetLoop(Sfx.CleanAmbienceLoop, 0f);

            // All rooms done: the next place opens (announced once).
            var status = VenueProgress.Venue(venue, Data);
            var next = VenueProgress.Next(_ctx.Database.Venues, venue);
            var newlyOpened = status.AllComplete && next != null && !Data.Venue(next.Id).Owned;
            if (newlyOpened) Data.Venue(next.Id).Owned = true;

            _overview.Build(venue, Data);
            _overview.ApplyView();
            _overview.InputEnabled = true;
            _hud.BindOverview(venue, status, _overview.Rooms, _ctx, _camera, status.AllComplete ? next : null);

            if (newlyOpened)
            {
                _hud.ShowCelebration(Loc.Format("hud.venue_opened", Loc.Get(next.DisplayNameKey)));
                SfxPlayer.Instance?.Play(Sfx.SectionComplete, 0f);
                Haptics.Strong();
            }

            foreach (var section in unlocked)
            {
                _hud.ShowCelebration(Loc.Format("hud.room_unlocked", Loc.Get(section.DisplayNameKey)));
                SfxPlayer.Instance?.Play(Sfx.Mastery, 0f);
            }

            // GDD 10.3: a helper's Shop slot opened since the last look at an overview. Said once, after the other news.
            var announced = newlyOpened || unlocked.Count > 0 ? 2.8f : 0.2f;
            foreach (var helper in _ctx.Database.Helpers)
            {
                if (helper == null || Data.AnnouncedHelpers.Contains(helper.Id) || !VenueProgress.HelperSlotOpen(helper, Data)) continue;
                Data.AnnouncedHelpers.Add(helper.Id);
                if (_ctx.Helpers.IsHired(helper)) continue;
                var text = Loc.Format("hud.helper_available", Loc.Get(helper.DisplayNameKey));
                Tween.Delay(this, announced, () => _hud.ShowCelebration(text));
                announced += 2.8f;
            }
            _boot.MarkDirty();
        }

        void OnRoomTapped(RoomView room)
        {
            if (Busy) return;
            if (!room.Status.Unlocked)
            {
                _hud.ShowUnlock(room.Section);
                return;
            }
            EnterSection(room.Section);
        }

        // ---------- Section ----------

        /// <summary>Zoom from the overview into a room, then swap to the section view (GDD 6.1).</summary>
        public void EnterSection(SectionDefinition section)
        {
            if (Busy || Current != Screen.Overview) return;
            Busy = true;
            _overview.InputEnabled = false;
            var room = _overview.RoomOf(section);
            SfxPlayer.Instance?.Play(Sfx.Pickup, 0f, 0.6f);
            if (room != null) _overview.Zoom(room, false, ZoomInTime, null);
            _hud.Fade(1f, ZoomInTime, () =>
            {
                OpenSectionNow(Venue, section);
                _hud.Fade(0f, FadeTime, () => Busy = false);
            });
        }

        /// <summary>Tests / debugging: open the room and jump straight in without the transition (ignores venue locks).</summary>
        public void OpenSectionImmediately(VenueDefinition venue, SectionDefinition section)
        {
            if (!VenueProgress.IsUnlocked(section, Data)) Data.UnlockedSections.Add(section.Id);
            if (Current == Screen.Section) _section.Unload();
            OpenSectionNow(venue, section);
        }

        /// <summary>Tests / debugging: show a venue's overview without the transition.</summary>
        public void ShowOverviewImmediately(VenueDefinition venue)
        {
            if (Current == Screen.Section) _section.Unload();
            ShowOverviewNow(venue);
        }

        void OpenSectionNow(VenueDefinition venue, SectionDefinition section)
        {
            Venue = venue;
            ActiveSection = section;
            Current = Screen.Section;
            Data.CurrentVenueId = venue.Id;
            Data.CurrentSectionId = section.Id;
            _overview.InputEnabled = false;
            _overview.Clear(); // only the active section is fully loaded (GDD 15.3)

            _camera.orthographic = false;
            _boot.BuildSection(section, venue);
        }

        /// <summary>Back from a section: save it, fade, rebuild the overview zoomed in on that room, zoom out.</summary>
        public void BackToOverview()
        {
            if (Busy || Current != Screen.Section) return;
            Busy = true;
            _boot.Showcase.StopNow();
            _boot.Inspector.StopNow();
            _boot.SaveNow();
            var leaving = ActiveSection;
            _hud.Fade(1f, FadeTime, () =>
            {
                _section.Unload();
                ShowOverviewNow(Venue);
                var room = _overview.RoomOf(leaving);
                _overview.InputEnabled = false;
                if (room != null)
                {
                    _overview.Zoom(room, true, ZoomOutTime, () => _overview.InputEnabled = true);
                }
                else
                {
                    _overview.InputEnabled = true;
                }
                _hud.Fade(0f, FadeTime, () => Busy = false);
            });
        }

        void OnBack()
        {
            if (Busy) return;
            if (Current == Screen.Section) BackToOverview();
            else ShowMap();
        }

        // ---------- Venues (GDD 5.2, 5.6) ----------

        public void ShowMap()
        {
            var ladder = _ctx.Database.Venues;
            _hud.ShowMap(ladder, i => VenueProgress.State(ladder, i, Data));
        }

        public void SwitchVenue(VenueDefinition venue)
        {
            if (Busy || !IsOwned(venue)) return;
            Busy = true;
            _hud.CloseMap();
            _hud.Fade(1f, FadeTime, () =>
            {
                if (Current == Screen.Section) _section.Unload();
                ShowOverviewNow(venue);
                _hud.Fade(0f, FadeTime, () => Busy = false);
            });
        }

        /// <summary>From a finished venue's overview: straight to the next place.</summary>
        public void GoToNextVenue()
        {
            var next = Venue != null ? VenueProgress.Next(_ctx.Database.Venues, Venue) : null;
            if (next != null && IsOwned(next)) SwitchVenue(next);
        }

        /// <summary>GDD 5.4: pay coins instead of waiting for the other rooms.</summary>
        public void UnlockWithCoins(SectionDefinition section)
        {
            if (section == null || VenueProgress.IsUnlocked(section, Data) || section.UnlockCoinCost <= 0) return;
            if (!_ctx.Wallet.TrySpend(section.UnlockCoinCost))
            {
                SfxPlayer.Instance?.Play(Sfx.Wrong, 0f);
                _hud.ShowToast(Loc.Get("hud.not_enough"));
                return;
            }
            Data.UnlockedSections.Add(section.Id);
            SfxPlayer.Instance?.Play(Sfx.Purchase, 0f);
            Haptics.Medium();
            ShowOverviewNow(Venue);
            _hud.ShowCelebration(Loc.Format("hud.room_unlocked", Loc.Get(section.DisplayNameKey)));
        }
    }
}
