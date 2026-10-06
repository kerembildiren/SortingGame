using System;
using System.Collections.Generic;
using SortingGame.Core;
using SortingGame.Data;
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
        readonly List<(ShelfView shelf, Label label)> _shelfLabels = new();

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
            var back = Add(_topBar, new Button(() => ShowToast(Loc.Get("hud.overview_soon"))) { text = "<" }, "round-button");
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
            ApplySafeArea();
        }

        public void Bind(SectionController section, Wallet wallet, CollectionBook book, VenueDefinition venue, Camera cam)
        {
            Unbind();
            _section = section;
            _wallet = wallet;
            _book = book;
            _venue = venue;
            _camera = cam;

            _title.text = Loc.Get(section.Definition.DisplayNameKey);
            section.ProgressChanged += OnProgress;
            section.ItemPlaced += OnItemPlaced;
            section.ShelfCompleted += OnShelfCompleted;
            section.SectionCompleted += OnSectionCompleted;
            wallet.Changed += OnCoins;

            _worldLayer.Clear();
            _shelfLabels.Clear();
            foreach (var shelf in section.Shelves)
            {
                var label = Add(_worldLayer, new Label(Loc.Get(shelf.Category.DisplayNameKey)), "shelf-label");
                _shelfLabels.Add((shelf, label));
            }

            _banner.AddToClassList("hidden");
            SetMomentMode(false);
            OnProgress(section.Progress);
            _coins.text = wallet.Coins.ToString("N0");
        }

        void Unbind()
        {
            if (_section != null)
            {
                _section.ProgressChanged -= OnProgress;
                _section.ItemPlaced -= OnItemPlaced;
                _section.ShelfCompleted -= OnShelfCompleted;
                _section.SectionCompleted -= OnSectionCompleted;
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
            foreach (var (shelf, label) in _shelfLabels)
            {
                if (shelf == null) continue;
                var p = WorldToPanel(shelf.LabelAnchor);
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
            _coins.text = total.ToString("N0");
            Pulse(_coinPill);
        }

        void OnItemPlaced(ItemView item, ShelfView shelf, int coins)
        {
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
            _bookBadge.AddToClassList("hidden");
            _bookGrid.Clear();
            var page = _venue != null ? _venue.CollectionPage : new List<CollectibleDefinition>();
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
            _bookCount.text = $"{(_venue != null ? Loc.Get(_venue.DisplayNameKey) : "")}  {found} / {page.Count}";
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
            OpenBook(); // back to where the player came from
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
            (string key, string glyph, ToolType? tool)[] tools =
            {
                ("tool.hand", "H", ToolType.Hand),
                ("tool.broom", "B", ToolType.Broom),
                ("tool.magnet", "M", null),
                ("tool.magnifier", "?", null),
            };
            foreach (var (key, glyph, tool) in tools)
            {
                var button = Add(_toolbar, new Button(), "tool");
                Add(button, new Label(glyph), "tool-glyph");
                Add(button, new Label(Loc.Get(key)), "tool-name");
                if (tool is { } type)
                {
                    _toolButtons[type] = button;
                    button.clicked += () => SelectTool(type);
                }
                else
                {
                    button.AddToClassList("tool--locked");
                    button.clicked += () => ShowToast($"{Loc.Get(key)}: {Loc.Get("tool.coming_soon")}");
                }
            }
            SelectTool(ToolType.Hand);
        }

        public void SelectTool(ToolType tool)
        {
            foreach (var (type, button) in _toolButtons)
                button.EnableInClassList("tool--selected", type == tool);
            ToolSelected?.Invoke(tool);
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
            _bookCount = Add(card, new Label(), "book-count");
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
            Add(card, new Button(() => RestartRequested?.Invoke()) { text = Loc.Get("hud.play_again") }, "primary-button");
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
