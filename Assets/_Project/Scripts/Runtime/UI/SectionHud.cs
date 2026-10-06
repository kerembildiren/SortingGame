using System;
using System.Collections.Generic;
using SortingGame.Core;
using SortingGame.Section;
using UnityEngine;
using UnityEngine.UIElements;

namespace SortingGame.UI
{
    /// <summary>
    /// Section screen HUD (GDD 6.2, 12.2): top bar with name + %, coin counter, tool bar,
    /// shelf labels anchored to the 3D signs, coin popups and the completion banner.
    /// Built in code with UI Toolkit; styling lives in SectionHud.uss.
    /// </summary>
    [RequireComponent(typeof(UIDocument))]
    public class SectionHud : MonoBehaviour
    {
        const float ReferenceWidth = 1080f;

        public event Action RestartRequested;

        UIDocument _document;
        VisualElement _root;
        VisualElement _worldLayer;
        VisualElement _popupLayer;
        VisualElement _topBar;
        Label _title;
        Label _percent;
        VisualElement _progressFill;
        Label _coins;
        VisualElement _coinPill;
        Label _toast;
        VisualElement _banner;
        VisualElement _settings;
        Button _soundToggle;
        Button _hapticsToggle;
        IVisualElementScheduledItem _toastHide;

        SectionController _section;
        Wallet _wallet;
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

            var coinRow = Add(_root, new VisualElement(), "coin-row");
            coinRow.pickingMode = PickingMode.Ignore;
            _coinPill = Add(coinRow, new VisualElement(), "coin-pill");
            Add(_coinPill, new VisualElement(), "coin-icon");
            _coins = Add(_coinPill, new Label("0"), "coin-text");

            _toast = Add(_root, new Label(), "toast");

            BuildToolbar();
            BuildBanner();
            BuildSettings();
            ApplySafeArea();
        }

        public void Bind(SectionController section, Wallet wallet, Camera cam)
        {
            Unbind();
            _section = section;
            _wallet = wallet;
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

        // ---------- Event handlers ----------

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
            if (coins <= 0 || _camera == null) return;
            var start = WorldToPanel(item.transform.position);
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
            _banner.RemoveFromClassList("hidden");
            _banner.style.opacity = 0f;
            Tween.Run(this, 0.5f, t =>
            {
                _banner.style.opacity = t;
                _banner.style.scale = new Scale(Vector3.one * Mathf.LerpUnclamped(0.8f, 1f, t));
            }, Ease.OutBack);
        }

        // ---------- Building blocks ----------

        void BuildToolbar()
        {
            var bar = Add(_root, new VisualElement(), "toolbar");
            (string key, string glyph, bool active)[] tools =
            {
                ("tool.hand", "H", true),
                ("tool.broom", "B", false),
                ("tool.magnet", "M", false),
                ("tool.magnifier", "?", false),
            };
            foreach (var (key, glyph, active) in tools)
            {
                var button = Add(bar, new Button(), "tool");
                if (active) button.AddToClassList("tool--selected");
                else button.AddToClassList("tool--locked");
                Add(button, new Label(glyph), "tool-glyph");
                Add(button, new Label(Loc.Get(key)), "tool-name");
                if (!active) button.clicked += () => ShowToast($"{Loc.Get(key)}: {Loc.Get("tool.coming_soon")}");
            }
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
            _toastHide.ExecuteLater(1600);
        }

        void ApplySafeArea()
        {
            // Keep the top bar below notches / camera cut-outs.
            var safe = Screen.safeArea;
            var topInset = (Screen.height - safe.yMax) * (ReferenceWidth / Mathf.Max(1, Screen.width));
            var bottomInset = safe.yMin * (ReferenceWidth / Mathf.Max(1, Screen.width));
            _topBar.style.marginTop = topInset;
            _root.Q(className: "toolbar").style.marginBottom = bottomInset;
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
