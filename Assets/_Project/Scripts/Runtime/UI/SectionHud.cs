using System;
using System.Collections.Generic;
using SortingGame.Core;
using SortingGame.Data;
using SortingGame.Overview;
using SortingGame.Section;
using UnityEngine;
using UnityEngine.UIElements;

namespace SortingGame.UI
{
    /// <summary>
    /// Section screen HUD (GDD 6.2, 12.2): top bar with name + %, coins, Collection Book button, tool bar,
    /// shelf labels anchored to the 3D signs, coin popups, rare-find card, book page and completion banner.
    /// Built in code with UI Toolkit; styling lives in SectionHud.uss.
    /// </summary>
    [RequireComponent(typeof(UIDocument))]
    public class SectionHud : MonoBehaviour
    {
        const float ReferenceWidth = 1080f;

        public event Action RestartRequested;
        public event Action<ToolType> ToolSelected;
        public event Action RareCardClosed;
        public event Action<CollectibleDefinition> CollectibleViewRequested;
        public event Action ViewerClosed;
        public event Action ResetProgressRequested;
        public event Action BackRequested;
        public event Action SellRequested;
        public event Action<VenueDefinition> VenueOpenRequested;
        public event Action<VenueDefinition> VenueBuyRequested;
        public event Action<SectionDefinition> UnlockRequested;

        public enum Mode { Section, Overview }
        public Mode CurrentMode { get; private set; } = Mode.Section;

        UIDocument _document;
        VisualElement _root;
        VisualElement _worldLayer;
        VisualElement _popupLayer;
        VisualElement _topBar;
        VisualElement _coinRow;
        VisualElement _toolbar;
        Label _title;
        Label _percent;
        VisualElement _progressFill;
        Label _coins;
        VisualElement _coinPill;
        Button _bookButton;
        Label _bookBadge;
        Label _toast;
        VisualElement _banner;
        VisualElement _settings;
        VisualElement _rareCard;
        Label _rareName;
        Label _rareDescription;
        VisualElement _bookPage;
        Label _bookCount;
        VisualElement _bookGrid;
        VisualElement _viewer;
        Label _viewerName;
        Label _viewerDescription;
        Button _soundToggle;
        Button _hapticsToggle;
        IVisualElementScheduledItem _toastHide;
        readonly Dictionary<ToolType, Button> _toolButtons = new();

        SectionController _section;
        Wallet _wallet;
        CollectionBook _book;
        VenueDefinition _venue;
        Camera _camera;
        readonly List<ShelfLabel> _shelfLabels = new();
        GameContext _ctx;
        VisualElement _shop;
        VisualElement _shopList;
        Label _bigToast;
        IVisualElementScheduledItem _bigToastHide;

        readonly List<(RoomView room, VisualElement label)> _roomLabels = new();
        Button _sellButton;
        VisualElement _map;
        VisualElement _mapList;
        VisualElement _unlock;
        Label _unlockTitle;
        Label _unlockText;
        Button _unlockBuy;
        SectionDefinition _unlockSection;
        VisualElement _fade;
        Label _bookTitle;
        int _bookPageIndex;

        class ShelfLabel
        {
            public ShelfView Shelf;
            public VisualElement Root;
            public Label Name;
            public VisualElement MasteryTrack;
            public VisualElement MasteryFill;
        }

        /// <summary>Panel units per screen pixel is 1/UiScale. Used to convert reference-pixel tunables.</summary>
        public float UiScale => Screen.width / ReferenceWidth;

        public void Init(StyleSheet styleSheet)
        {
            _document = GetComponent<UIDocument>();
            _root = _document.rootVisualElement;
            _root.Clear();
            if (styleSheet != null) _root.styleSheets.Add(styleSheet);
            _root.AddToClassList("hud");
            _root.pickingMode = PickingMode.Ignore;

            _worldLayer = Layer("world-layer");
            _popupLayer = Layer("popup-layer");

            // Top bar: back | title card | settings
            _topBar = Add(_root, new VisualElement(), "top-bar");
            var back = Add(_topBar, new Button(() => BackRequested?.Invoke()) { text = "<" }, "round-button");
            back.name = "back";
            var card = Add(_topBar, new VisualElement(), "title-card");
            var titleRow = Add(card, new VisualElement(), "title-row");
            _title = Add(titleRow, new Label(), "title");
            _percent = Add(titleRow, new Label("0%"), "percent");
            var track = Add(card, new VisualElement(), "progress-track");
            _progressFill = Add(track, new VisualElement(), "progress-fill");
            Add(_topBar, new Button(ToggleSettings) { text = "=" }, "round-button").name = "settings";

            // Second row: Collection Book button + coins (GDD 6.1 "Items" button, 9.1 north star).
            _coinRow = Add(_root, new VisualElement(), "coin-row");
            _coinRow.pickingMode = PickingMode.Ignore;
            var shopButton = Add(_coinRow, new Button(OpenShop), "book-button");
            Add(shopButton, new Label(Loc.Get("hud.shop")), "book-button-text");
            _bookButton = Add(_coinRow, new Button(OpenBook), "book-button");
            Add(_bookButton, new Label(Loc.Get("hud.book")), "book-button-text");
            _bookBadge = Add(_bookButton, new Label("!"), "book-badge");
            _bookBadge.AddToClassList("hidden");
            _coinPill = Add(_coinRow, new VisualElement(), "coin-pill");
            Add(_coinPill, new VisualElement(), "coin-icon");
            _coins = Add(_coinPill, new Label("0"), "coin-text");

            _toast = Add(_root, new Label(), "toast");

            BuildToolbar();
            BuildRareCard();
            BuildBookPage();
            BuildViewer();
            BuildBanner();
            BuildSettings();
            BuildShop();
            BuildOverviewUi();
            _bigToast = Add(_root, new Label(), "big-toast");
            _fade = Add(_root, new VisualElement(), "fade");
            _fade.pickingMode = PickingMode.Ignore;
            ApplySafeArea();
        }

        public void Bind(SectionController section, GameContext context, VenueDefinition venue, Camera cam)
        {
            Unbind();
            _section = section;
            _ctx = context;
            _wallet = context.Wallet;
            _book = context.Book;
            _venue = venue;
            _camera = cam;
            var wallet = _wallet;

            _title.text = Loc.Get(section.Definition.DisplayNameKey);
            section.ProgressChanged += OnProgress;
            section.ItemPlaced += OnItemPlaced;
            section.ShelfCompleted += OnShelfCompleted;
            section.SectionCompleted += OnSectionCompleted;
            section.CategoryMastered += OnCategoryMastered;
            wallet.Changed += OnCoins;

            SetMode(Mode.Section);
            _worldLayer.Clear();
            _shelfLabels.Clear();
            _roomLabels.Clear();
            foreach (var shelf in section.Shelves)
            {
                var entry = new ShelfLabel { Shelf = shelf };
                entry.Root = Add(_worldLayer, new VisualElement(), "shelf-label");
                entry.Name = Add(entry.Root, new Label(), "shelf-label-text");
                entry.MasteryTrack = Add(entry.Root, new VisualElement(), "mastery-track");
                entry.MasteryFill = Add(entry.MasteryTrack, new VisualElement(), "mastery-fill");
                entry.Root.pickingMode = PickingMode.Ignore;
                entry.MasteryTrack.pickingMode = PickingMode.Ignore;
                entry.MasteryFill.pickingMode = PickingMode.Ignore;
                _shelfLabels.Add(entry);
            }
            RefreshMastery();

            _banner.AddToClassList("hidden");
            SetMomentMode(false);
            OnProgress(section.Progress);
            _coins.text = wallet.Coins.ToString("N0", Loc.Culture);
            RefreshToolbar();
        }

        /// <summary>Loaded straight into a finished section: show the banner without replaying the renovation.</summary>
        public void ShowCompleteBanner()
        {
            _banner.RemoveFromClassList("hidden");
            _banner.style.opacity = 1f;
            _banner.style.scale = new Scale(Vector3.one);
        }

        void Unbind()
        {
            if (_section != null)
            {
                _section.ProgressChanged -= OnProgress;
                _section.ItemPlaced -= OnItemPlaced;
                _section.ShelfCompleted -= OnShelfCompleted;
                _section.SectionCompleted -= OnSectionCompleted;
                _section.CategoryMastered -= OnCategoryMastered;
            }
            if (_wallet != null) _wallet.Changed -= OnCoins;
        }

        void OnDestroy() => Unbind();

        /// <summary>True when the screen point (Input System coordinates, origin bottom-left) is on an interactive UI element.</summary>
        public bool IsOverUi(Vector2 screenPosition)
        {
            if (_root?.panel == null) return false;
            var panelPoint = RuntimePanelUtils.ScreenToPanel(_root.panel, new Vector2(screenPosition.x, Screen.height - screenPosition.y));
            return _root.panel.Pick(panelPoint) != null;
        }

        /// <summary>Centre of the book button in screen pixels (origin bottom-left), where found items fly to.</summary>
        public Vector2 BookButtonScreenPoint()
        {
            var bound = _bookButton.worldBound;
            var size = _root.layout.size;
            if (size.x <= 0f) return new Vector2(Screen.width * 0.8f, Screen.height * 0.88f);
            return new Vector2(bound.center.x / size.x * Screen.width, (1f - bound.center.y / size.y) * Screen.height);
        }

        void LateUpdate()
        {
            if (_camera == null || _root?.panel == null) return;
            foreach (var entry in _shelfLabels)
            {
                if (entry.Shelf == null) continue;
                var p = WorldToPanel(entry.Shelf.LabelAnchor);
                entry.Root.style.left = p.x;
                entry.Root.style.top = p.y;
            }
            foreach (var (room, label) in _roomLabels)
            {
                if (room == null) continue;
                var p = WorldToPanel(room.LabelAnchor);
                label.style.left = p.x;
                label.style.top = p.y;
            }
        }

        /// <summary>
        /// Via viewport space so it is correct whatever the camera renders into
        /// (RuntimePanelUtils.CameraTransformWorldToPanel assumes the camera renders to the screen).
        /// </summary>
        Vector2 WorldToPanel(Vector3 world)
        {
            var viewport = _camera.WorldToViewportPoint(world);
            var size = _root.layout.size;
            return new Vector2(viewport.x * size.x, (1f - viewport.y) * size.y);
        }

        // ---------- Section events ----------

        void OnProgress(SectionProgress progress)
        {
            _percent.text = $"{progress.Percent}%";
            _progressFill.style.width = Length.Percent(progress.Fraction * 100f);
        }

        void OnCoins(long total, long delta)
        {
            _coins.text = total.ToString("N0", Loc.Culture);
            Pulse(_coinPill);
            if (!_shop.ClassListContains("hidden")) RefreshShop();
        }

        void OnItemPlaced(ItemView item, ShelfView shelf, int coins)
        {
            RefreshMastery();
            if (coins > 0) CoinPopup(item.transform.position, coins);
        }

        void CoinPopup(Vector3 world, int coins)
        {
            if (_camera == null) return;
            var start = WorldToPanel(world);
            var popup = Add(_popupLayer, new Label($"+{coins}"), "coin-popup");
            popup.style.left = start.x;
            popup.style.top = start.y;
            Tween.Run(this, 0.75f, t =>
            {
                popup.style.translate = new Translate(Length.Percent(-50), new Length(-50f - 110f * t, LengthUnit.Pixel));
                popup.style.opacity = 1f - t * t;
            }, Ease.OutQuad, popup.RemoveFromHierarchy);
        }

        void OnShelfCompleted(ShelfView shelf) =>
            ShowToast(Loc.Format("hud.shelf_full", Loc.Get(shelf.Category.DisplayNameKey)));

        // ---------- Overview, map, unlock (GDD 5, 6.1) ----------

        public void SetMode(Mode mode)
        {
            CurrentMode = mode;
            _toolbar.style.display = mode == Mode.Section ? DisplayStyle.Flex : DisplayStyle.None;
            if (mode == Mode.Section) _sellButton.style.display = DisplayStyle.None;
            _banner.AddToClassList("hidden");
        }

        /// <summary>GDD 6.1: venue name + item counter on top, a label per room, sell button when everything is done.</summary>
        public void BindOverview(VenueDefinition venue, VenueProgress.VenueStatus status, IReadOnlyList<RoomView> rooms, GameContext context, Camera cam, bool canSell)
        {
            Unbind();
            _section = null;
            _ctx = context;
            _wallet = context.Wallet;
            _book = context.Book;
            _venue = venue;
            _camera = cam;
            _wallet.Changed += OnCoins;
            _coins.text = _wallet.Coins.ToString("N0", Loc.Culture);
            SetMode(Mode.Overview);

            _title.text = Loc.Get(venue.DisplayNameKey);
            _percent.text = Loc.Format("hud.items_count", status.Placed, status.Total);
            _progressFill.style.width = Length.Percent(status.Fraction * 100f);

            _worldLayer.Clear();
            _shelfLabels.Clear();
            _roomLabels.Clear();
            foreach (var room in rooms)
            {
                var label = Add(_worldLayer, new VisualElement(), "room-label");
                label.pickingMode = PickingMode.Ignore;
                Add(label, new Label(Loc.Get(room.Section.DisplayNameKey)), "room-label-name");
                var badge = Add(label, new Label(room.Status.Unlocked ? $"{room.Status.Percent}%" : Loc.Get("hud.locked_room")), "room-label-badge");
                badge.AddToClassList(!room.Status.Unlocked ? "room-badge--locked"
                    : room.Status.Completed ? "room-badge--done"
                    : room.Status.Fraction < 0.2f ? "room-badge--low" : "room-badge--mid");
                _roomLabels.Add((room, label));
            }

            _sellButton.text = Loc.Format("hud.sell_venue", venue.SellValue);
            _sellButton.style.display = canSell ? DisplayStyle.Flex : DisplayStyle.None;
            RefreshToolbar();
        }

        void BuildOverviewUi()
        {
            _sellButton = Add(_root, new Button(() => SellRequested?.Invoke()), "sell-button");
            _sellButton.style.display = DisplayStyle.None;

            _map = Add(_root, new VisualElement(), "settings");
            _map.AddToClassList("hidden");
            var card = Add(_map, new VisualElement(), "banner-card");
            card.AddToClassList("book-card");
            Add(card, new Label(Loc.Get("hud.map_title")), "banner-title");
            _mapList = Add(card, new VisualElement(), "shop-list");
            Add(card, new Button(CloseMap) { text = Loc.Get("hud.close") }, "primary-button");

            _unlock = Add(_root, new VisualElement(), "settings");
            _unlock.AddToClassList("hidden");
            var unlockCard = Add(_unlock, new VisualElement(), "banner-card");
            _unlockTitle = Add(unlockCard, new Label(), "banner-title");
            _unlockText = Add(unlockCard, new Label(), "rare-description");
            _unlockBuy = Add(unlockCard, new Button(() =>
            {
                _unlock.AddToClassList("hidden");
                UnlockRequested?.Invoke(_unlockSection);
            }), "primary-button");
            Add(unlockCard, new Button(() => _unlock.AddToClassList("hidden")) { text = Loc.Get("hud.close") }, "secondary-button");
        }

        /// <summary>GDD 5.2 venue ladder: sold trophies, the current venue, the next one for sale, later ones locked.</summary>
        public void ShowMap(IReadOnlyList<VenueDefinition> ladder, Func<int, VenueProgress.VenueState> stateOf)
        {
            _mapList.Clear();
            for (var i = 0; i < ladder.Count; i++)
            {
                var venue = ladder[i];
                var state = stateOf(i);
                var row = Add(_mapList, new VisualElement(), "shop-row");
                if (venue == _venue) row.AddToClassList("map-row--current");
                var info = Add(row, new VisualElement(), "shop-info");
                Add(info, new Label(Loc.Get(venue.DisplayNameKey)), "shop-name");
                var detail = state switch
                {
                    VenueProgress.VenueState.Sold => Loc.Get("hud.venue_sold"),
                    VenueProgress.VenueState.Owned => Loc.Format("hud.venue_rooms", venue.Sections.Count),
                    VenueProgress.VenueState.ForSale => Loc.Format("hud.venue_rooms", venue.Sections.Count),
                    _ => Loc.Format("hud.venue_locked", i > 0 ? Loc.Get(ladder[i - 1].DisplayNameKey) : "")
                };
                Add(info, new Label(detail), "shop-effect");

                var shown = venue;
                switch (state)
                {
                    case VenueProgress.VenueState.Owned:
                        Add(row, new Button(() => { CloseMap(); VenueOpenRequested?.Invoke(shown); }) { text = Loc.Get("hud.open") }, "map-open");
                        break;
                    case VenueProgress.VenueState.ForSale:
                        var buy = Add(row, new Button(() => VenueBuyRequested?.Invoke(shown)), "shop-buy");
                        Add(buy, new VisualElement(), "coin-icon");
                        Add(buy, new Label(venue.PurchasePrice.ToString("N0", Loc.Culture)), "shop-cost");
                        buy.EnableInClassList("shop-buy--poor", !_wallet.CanAfford(venue.PurchasePrice));
                        break;
                    case VenueProgress.VenueState.Sold:
                        Add(row, new Label(Loc.Get("hud.sold_tag")), "sold-tag");
                        break;
                }
            }
            _map.RemoveFromClassList("hidden");
        }

        public void CloseMap() => _map.AddToClassList("hidden");
        public bool IsMapOpen => !_map.ClassListContains("hidden");

        /// <summary>GDD 5.4: locked room card with the auto-unlock rule and the coin shortcut.</summary>
        public void ShowUnlock(SectionDefinition section)
        {
            _unlockSection = section;
            _unlockTitle.text = Loc.Get(section.DisplayNameKey);
            _unlockText.text = section.UnlockAtVenuePercent > 0
                ? Loc.Format("hud.unlock_rule", section.UnlockAtVenuePercent)
                : Loc.Get("hud.unlock_coins_only");
            _unlockBuy.text = Loc.Format("hud.unlock_now", section.UnlockCoinCost);
            _unlockBuy.style.display = section.UnlockCoinCost > 0 ? DisplayStyle.Flex : DisplayStyle.None;
            _unlockBuy.SetEnabled(_wallet != null && _wallet.CanAfford(section.UnlockCoinCost));
            _unlock.RemoveFromClassList("hidden");
        }

        /// <summary>Black screen fade used by the zoom transition.</summary>
        public void Fade(float to, float duration, Action done = null)
        {
            var from = _fade.resolvedStyle.opacity;
            _fade.pickingMode = to > 0.01f ? PickingMode.Position : PickingMode.Ignore;
            Tween.Run(this, duration, t => _fade.style.opacity = Mathf.Lerp(from, to, t), Ease.InOutQuad, () =>
            {
                _fade.pickingMode = to > 0.01f ? PickingMode.Position : PickingMode.Ignore;
                done?.Invoke();
            });
        }

        public void ShowCelebration(string text) => ShowBigToast(text);

        // ---------- Category Mastery (GDD 10.2) ----------

        void RefreshMastery()
        {
            if (_ctx == null) return;
            foreach (var entry in _shelfLabels)
            {
                var category = entry.Shelf.Category;
                var mastered = _ctx.Mastery.IsMastered(category);
                entry.Name.text = mastered ? $"{Loc.Get(category.DisplayNameKey)} *" : Loc.Get(category.DisplayNameKey);
                entry.Root.EnableInClassList("shelf-label--mastered", mastered);
                entry.MasteryTrack.style.display = mastered ? DisplayStyle.None : DisplayStyle.Flex;
                entry.MasteryFill.style.width = Length.Percent(_ctx.Mastery.ProgressOf(category) * 100f);
            }
        }

        void OnCategoryMastered(CategoryDefinition category)
        {
            RefreshMastery();
            ShowBigToast(Loc.Format("hud.mastered", Loc.Get(category.DisplayNameKey)));
        }

        void ShowBigToast(string text)
        {
            _bigToast.text = text;
            _bigToast.AddToClassList("big-toast--visible");
            _bigToastHide?.Pause();
            _bigToastHide = _bigToast.schedule.Execute(() => _bigToast.RemoveFromClassList("big-toast--visible"));
            _bigToastHide.ExecuteLater(2600);
        }

        // ---------- Shop (GDD 10.1 coin upgrades) ----------

        public void OpenShop()
        {
            RefreshShop();
            _shop.RemoveFromClassList("hidden");
        }

        public void CloseShop() => _shop.AddToClassList("hidden");

        void BuildShop()
        {
            _shop = Add(_root, new VisualElement(), "settings");
            _shop.AddToClassList("hidden");
            var card = Add(_shop, new VisualElement(), "banner-card");
            card.AddToClassList("book-card");
            Add(card, new Label(Loc.Get("hud.shop_title")), "banner-title");
            _shopList = Add(card, new VisualElement(), "shop-list");
            Add(card, new Button(CloseShop) { text = Loc.Get("hud.close") }, "primary-button");
        }

        void RefreshShop()
        {
            _shopList.Clear();
            if (_ctx == null) return;
            foreach (var tool in _ctx.Database.Tools)
            {
                var level = _ctx.Tools.LevelOf(tool);
                var row = Add(_shopList, new VisualElement(), "shop-row");
                var info = Add(row, new VisualElement(), "shop-info");
                Add(info, new Label($"{Loc.Get(tool.DisplayNameKey)}  {(level == 0 ? Loc.Get("hud.locked") : $"Lv {level}/{tool.MaxLevel}")}"), "shop-name");

                var maxed = _ctx.Tools.IsMaxed(tool);
                var shown = maxed ? tool.Stats(level) : tool.Stats(level + 1);
                var effect = Loc.Format(tool.EffectKey, shown.Primary, shown.Secondary);
                Add(info, new Label(maxed ? effect : $"{(level == 0 ? Loc.Get("hud.unlock") : Loc.Get("hud.next"))}: {effect}"), "shop-effect");

                if (maxed)
                {
                    Add(row, new Label(Loc.Get("hud.max")), "shop-max");
                    continue;
                }
                var cost = _ctx.Tools.NextCost(tool);
                var buy = Add(row, new Button(() => Buy(tool)), "shop-buy");
                Add(buy, new VisualElement(), "coin-icon");
                Add(buy, new Label(cost.ToString("N0", Loc.Culture)), "shop-cost");
                buy.EnableInClassList("shop-buy--poor", !_wallet.CanAfford(cost));
            }
        }

        void Buy(ToolDefinition tool)
        {
            if (_ctx.Tools.TryUpgrade(tool, _wallet))
            {
                SfxPlayer.Instance?.Play(Sfx.Purchase, 0f);
                Haptics.Medium();
                ShowToast(Loc.Format("hud.bought", Loc.Get(tool.DisplayNameKey), _ctx.Tools.LevelOf(tool)));
            }
            else
            {
                SfxPlayer.Instance?.Play(Sfx.Wrong, 0f);
                ShowToast(Loc.Get("hud.not_enough"));
            }
            RefreshShop();
            RefreshToolbar();
        }

        void OnSectionCompleted()
        {
            // Let the renovation play before the banner covers it.
            Tween.Delay(this, 1.4f, () =>
            {
                _banner.RemoveFromClassList("hidden");
                _banner.style.opacity = 0f;
                Tween.Run(this, 0.5f, t =>
                {
                    _banner.style.opacity = t;
                    _banner.style.scale = new Scale(Vector3.one * Mathf.LerpUnclamped(0.8f, 1f, t));
                }, Ease.OutBack);
            });
        }

        // ---------- Collectibles ----------

        /// <summary>GDD 9.3 card: title, name, short description, one button. No sell/keep choice.</summary>
        public void ShowRareCard(CollectibleDefinition collectible)
        {
            SetMomentMode(true);
            _rareName.text = Loc.Get(collectible.DisplayNameKey);
            _rareDescription.text = Loc.Get(collectible.DescriptionKey);
            _rareCard.RemoveFromClassList("hidden");
            _rareCard.style.opacity = 0f;
            Tween.Run(this, 0.35f, t =>
            {
                _rareCard.style.opacity = t;
                _rareCard.style.translate = new Translate(0, new Length(120f * (1f - t), LengthUnit.Pixel));
            }, Ease.OutCubic);
        }

        void CloseRareCard()
        {
            _rareCard.AddToClassList("hidden");
            RareCardClosed?.Invoke();
        }

        /// <summary>After the item has flown into the book.</summary>
        public void OnCollectibleStored()
        {
            SetMomentMode(false);
            _bookBadge.RemoveFromClassList("hidden");
            Pulse(_bookButton);
        }

        public void OnDuplicateSold(CollectibleDefinition collectible, int coins, Vector3 world)
        {
            CoinPopup(world, coins);
            ShowToast(Loc.Format("hud.duplicate_sold", Loc.Get(collectible.DisplayNameKey), coins));
        }

        public void OpenBook()
        {
            var venues = _ctx?.Database.Venues;
            _bookPageIndex = venues != null && _venue != null ? Mathf.Max(0, venues.IndexOf(_venue)) : 0;
            ShowBookPage();
        }

        void TurnBookPage(int delta)
        {
            var count = _ctx?.Database.Venues.Count ?? 1;
            _bookPageIndex = (_bookPageIndex + delta + count) % count;
            ShowBookPage();
        }

        void ShowBookPage()
        {
            _bookBadge.AddToClassList("hidden");
            _bookGrid.Clear();
            var pageVenue = _ctx != null && _ctx.Database.Venues.Count > 0 ? _ctx.Database.Venues[_bookPageIndex] : _venue;
            var page = pageVenue != null ? pageVenue.CollectionPage : new List<CollectibleDefinition>();
            var found = 0;
            foreach (var collectible in page)
            {
                var has = _book != null && _book.Has(collectible);
                if (has) found++;
                var entry = Add(_bookGrid, new VisualElement(), "book-entry");
                var icon = Add(entry, new VisualElement(), "book-icon");
                if (has) icon.style.backgroundColor = collectible.Placeholder.Color;
                else icon.AddToClassList("book-icon--missing");
                Add(icon, new Label(has ? "" : "?"), "book-icon-text");
                Add(entry, new Label(has ? Loc.Get(collectible.DisplayNameKey) : "???"), "book-entry-name");
                if (collectible.Rarity == ItemRarity.Mascot) entry.AddToClassList("book-entry--mascot");
                if (has)
                {
                    // GDD 9.1.1: tap a found piece to display it in 3D.
                    entry.AddToClassList("book-entry--found");
                    var shown = collectible;
                    entry.RegisterCallback<ClickEvent>(_ => OpenViewer(shown));
                }
            }
            _bookTitle.text = pageVenue != null ? Loc.Get(pageVenue.DisplayNameKey) : "";
            _bookCount.text = $"{found} / {page.Count}";
            _bookPage.RemoveFromClassList("hidden");
        }

        public void CloseBook() => _bookPage.AddToClassList("hidden");

        public bool IsBookOpen => !_bookPage.ClassListContains("hidden");

        public void OpenViewer(CollectibleDefinition collectible)
        {
            CloseBook();
            SetMomentMode(true);
            _viewerName.text = Loc.Get(collectible.DisplayNameKey);
            _viewerDescription.text = Loc.Get(collectible.DescriptionKey);
            _viewer.RemoveFromClassList("hidden");
            CollectibleViewRequested?.Invoke(collectible);
        }

        public void CloseViewer()
        {
            if (_viewer.ClassListContains("hidden")) return;
            _viewer.AddToClassList("hidden");
            SetMomentMode(false);
            ViewerClosed?.Invoke();
            ShowBookPage(); // back to the page the player came from
        }

        /// <summary>During the rare-find moment only the card is visible (GDD 9.3: background darkens).</summary>
        void SetMomentMode(bool on)
        {
            foreach (var element in new[] { _topBar, _coinRow, _toolbar, _worldLayer })
                element.style.opacity = on ? 0f : 1f;
            _toolbar.SetEnabled(!on);
            _topBar.SetEnabled(!on);
            _coinRow.SetEnabled(!on);
            if (!on) _rareCard.AddToClassList("hidden");
        }

        // ---------- Building blocks ----------

        void BuildToolbar()
        {
            _toolbar = Add(_root, new VisualElement(), "toolbar");
            (string key, string glyph, ToolType tool)[] tools =
            {
                ("tool.hand", "H", ToolType.Hand),
                ("tool.broom", "B", ToolType.Broom),
                ("tool.magnet", "M", ToolType.Magnet),
            };
            foreach (var (key, glyph, tool) in tools)
            {
                var button = Add(_toolbar, new Button(), "tool");
                Add(button, new Label(glyph), "tool-glyph");
                Add(button, new Label(Loc.Get(key)), "tool-name");
                var price = Add(button, new Label(), "tool-price");
                price.name = "price";
                _toolButtons[tool] = button;
                button.clicked += () => OnToolClicked(tool);
            }
            SelectTool(ToolType.Hand);
        }

        ToolType _selectedTool = ToolType.Hand;

        void OnToolClicked(ToolType tool)
        {
            if (_ctx != null && !_ctx.Owns(tool))
            {
                OpenShop(); // locked tools are bought, not waited for (GDD 11.4)
                return;
            }
            SelectTool(tool);
        }

        public void SelectTool(ToolType tool)
        {
            _selectedTool = tool;
            foreach (var (type, button) in _toolButtons)
                button.EnableInClassList("tool--selected", type == tool);
            ToolSelected?.Invoke(tool);
        }

        void RefreshToolbar()
        {
            if (_ctx == null) return;
            foreach (var (type, button) in _toolButtons)
            {
                var owned = _ctx.Owns(type);
                button.EnableInClassList("tool--locked", !owned);
                var price = button.Q<Label>("price");
                var definition = _ctx.Database.ToolFor(type);
                price.text = owned || definition == null ? "" : _ctx.Tools.NextCost(definition).ToString("N0", Loc.Culture);
                price.style.display = owned ? DisplayStyle.None : DisplayStyle.Flex;
            }
            if (!_ctx.Owns(_selectedTool)) SelectTool(ToolType.Hand);
        }

        void BuildRareCard()
        {
            _rareCard = Add(_root, new VisualElement(), "rare-card-layer");
            _rareCard.AddToClassList("hidden");
            _rareCard.pickingMode = PickingMode.Ignore;
            var card = Add(_rareCard, new VisualElement(), "rare-card");
            var tab = Add(card, new VisualElement(), "rare-tab");
            Add(tab, new Label(Loc.Get("hud.rare_find")), "rare-tab-text");
            _rareName = Add(card, new Label(), "rare-name");
            _rareDescription = Add(card, new Label(), "rare-description");
            Add(card, new Button(CloseRareCard) { text = Loc.Get("hud.continue") }, "primary-button");
        }

        void BuildBookPage()
        {
            _bookPage = Add(_root, new VisualElement(), "settings");
            _bookPage.AddToClassList("hidden");
            var card = Add(_bookPage, new VisualElement(), "banner-card");
            card.AddToClassList("book-card");
            Add(card, new Label(Loc.Get("hud.collection_book")), "banner-title");
            var pager = Add(card, new VisualElement(), "book-pager");
            Add(pager, new Button(() => TurnBookPage(-1)) { text = "<" }, "pager-button");
            var pageInfo = Add(pager, new VisualElement(), "book-page-info");
            _bookTitle = Add(pageInfo, new Label(), "book-page-title");
            _bookCount = Add(pageInfo, new Label(), "book-count");
            Add(pager, new Button(() => TurnBookPage(1)) { text = ">" }, "pager-button");
            _bookGrid = Add(card, new VisualElement(), "book-grid");
            Add(card, new Button(CloseBook) { text = Loc.Get("hud.close") }, "primary-button");
        }

        void BuildViewer()
        {
            _viewer = Add(_root, new VisualElement(), "viewer-layer");
            _viewer.AddToClassList("hidden");
            _viewer.pickingMode = PickingMode.Ignore; // the 3D viewer handles touches; only the button is UI
            var top = Add(_viewer, new VisualElement(), "viewer-top");
            top.pickingMode = PickingMode.Ignore;
            _viewerName = Add(top, new Label(), "viewer-name");
            Add(top, new Label(Loc.Get("hud.view_hint")), "viewer-hint");
            var bottom = Add(_viewer, new VisualElement(), "viewer-bottom");
            bottom.pickingMode = PickingMode.Ignore;
            _viewerDescription = Add(bottom, new Label(), "viewer-description");
            Add(bottom, new Button(CloseViewer) { text = Loc.Get("hud.close") }, "primary-button");
        }

        void BuildBanner()
        {
            _banner = Add(_root, new VisualElement(), "banner");
            _banner.AddToClassList("hidden");
            var card = Add(_banner, new VisualElement(), "banner-card");
            Add(card, new Label(Loc.Get("hud.section_complete")), "banner-title");
            Add(card, new Label("100%"), "banner-percent");
            Add(card, new Button(() =>
            {
                _banner.AddToClassList("hidden");
                BackRequested?.Invoke();
            }) { text = Loc.Get("hud.back_to_overview") }, "primary-button");
        }

        void BuildSettings()
        {
            _settings = Add(_root, new VisualElement(), "settings");
            _settings.AddToClassList("hidden");
            var card = Add(_settings, new VisualElement(), "banner-card");
            Add(card, new Label(Loc.Get("hud.settings")), "banner-title");
            _soundToggle = Add(card, new Button(() =>
            {
                AudioListener.volume = AudioListener.volume > 0f ? 0f : 1f;
                RefreshSettings();
            }), "secondary-button");
            _hapticsToggle = Add(card, new Button(() =>
            {
                Haptics.Enabled = !Haptics.Enabled;
                RefreshSettings();
            }), "secondary-button");
            Add(card, new Button(() =>
            {
                ToggleSettings();
                RestartRequested?.Invoke();
            }) { text = Loc.Get("hud.restart") }, "secondary-button");
            Add(card, new Button(() =>
            {
                ToggleSettings();
                ResetProgressRequested?.Invoke();
            }) { text = Loc.Get("hud.reset_progress") }, "secondary-button");
            Add(card, new Button(ToggleSettings) { text = Loc.Get("hud.close") }, "primary-button");
            RefreshSettings();
        }

        void RefreshSettings()
        {
            _soundToggle.text = $"{Loc.Get("hud.sound")}: {Loc.Get(AudioListener.volume > 0f ? "hud.on" : "hud.off")}";
            _hapticsToggle.text = $"{Loc.Get("hud.haptics")}: {Loc.Get(Haptics.Enabled ? "hud.on" : "hud.off")}";
        }

        void ToggleSettings() => _settings.ToggleInClassList("hidden");

        public void ShowToast(string text)
        {
            _toast.text = text;
            _toast.AddToClassList("toast--visible");
            _toastHide?.Pause();
            _toastHide = _toast.schedule.Execute(() => _toast.RemoveFromClassList("toast--visible"));
            _toastHide.ExecuteLater(1800);
        }

        void ApplySafeArea()
        {
            // Keep the top bar below notches / camera cut-outs.
            var safe = Screen.safeArea;
            var scale = ReferenceWidth / Mathf.Max(1, Screen.width);
            var topInset = (Screen.height - safe.yMax) * scale;
            var bottomInset = safe.yMin * scale;
            _topBar.style.marginTop = topInset;
            _coinRow.style.marginTop = topInset;
            _toolbar.style.marginBottom = bottomInset;
        }

        static void Pulse(VisualElement element)
        {
            element.AddToClassList("pulse");
            element.schedule.Execute(() => element.RemoveFromClassList("pulse")).ExecuteLater(120);
        }

        VisualElement Layer(string className)
        {
            var layer = Add(_root, new VisualElement(), className);
            layer.pickingMode = PickingMode.Ignore;
            return layer;
        }

        static T Add<T>(VisualElement parent, T child, string className) where T : VisualElement
        {
            child.AddToClassList(className);
            if (child is Label) child.pickingMode = PickingMode.Ignore;
            parent.Add(child);
            return child;
        }
    }
}
