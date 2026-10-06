using System.Collections.Generic;
using System.Linq;
using SortingGame.Data;
using SortingGame.Section;
using SortingGame.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace SortingGame.Core
{
    /// <summary>
    /// Scene entry point: loads the save, wires services, builds the start section, hooks up input and HUD,
    /// and autosaves (GDD 15.4: what the player sorted stays sorted).
    /// </summary>
    public class GameBootstrap : MonoBehaviour
    {
        const float AutosaveDelay = 1.5f;

        [SerializeField] GameDatabase _database;
        [SerializeField] SectionDefinition _startSection;
        [SerializeField] SectionVisuals _visuals;
        [SerializeField] PanelSettings _panelSettings;
        [SerializeField] StyleSheet _hudStyle;

        public GameContext Context { get; private set; }
        public Wallet Wallet => Context.Wallet;
        public CollectionBook Book => Context.Book;
        public SectionController Section { get; private set; }
        public SectionHud Hud { get; private set; }
        public DragController Drag { get; private set; }
        public RareFindPresenter RareFind { get; private set; }
        public CollectionViewer Viewer { get; private set; }

        Camera _camera;
        VenueDefinition _venue;
        SaveSystem _save;
        SaveData _data;
        bool _dirty;
        float _dirtyTimer;
        bool _suppressSave;

        void Awake()
        {
            Application.targetFrameRate = 60;
            Screen.orientation = ScreenOrientation.Portrait;
            Haptics.EnsurePermissionIsIncluded();

            if (_database == null || _startSection == null || _visuals == null)
            {
                Debug.LogError("[GameBootstrap] Missing references. Run 'Sorting Game/Setup/Run Full Setup'.");
                enabled = false;
                return;
            }

            foreach (var error in _database.Validate())
                Debug.LogError($"[GameDatabase] {error}");

            _camera = Camera.main;
            _camera.clearFlags = CameraClearFlags.SolidColor;
            _venue = _database.Venues.FirstOrDefault(v => v.Sections.Contains(_startSection));

            _save = SaveSystem.CreateDefault();
            _data = _save.Load() ?? new SaveData { Coins = _database.Balance.StartingCoins };
            Context = new GameContext
            {
                Database = _database,
                Visuals = _visuals,
                Wallet = new Wallet(System.Math.Max(0, _data.Coins)),
                Book = new CollectionBook(),
                Mastery = new CategoryMastery(),
                Tools = new ToolProgress()
            };
            Context.Book.Restore(_data.Collection);
            Context.Mastery.Restore(_data.Mastery.Select(m => new KeyValuePair<string, int>(m.Id, m.Count)));
            Context.Tools.Restore(_data.Tools.Select(t => new KeyValuePair<string, int>(t.Id, t.Count)));

            new GameObject("Sfx").AddComponent<SfxPlayer>();
            Section = new GameObject("Section").AddComponent<SectionController>();

            var hudObject = new GameObject("HUD");
            var document = hudObject.AddComponent<UIDocument>();
            document.panelSettings = _panelSettings;
            Hud = hudObject.AddComponent<SectionHud>();

            Drag = _camera.gameObject.AddComponent<DragController>();
            Drag.Init(_camera, Section, Context, Hud.IsOverUi, () => Hud.UiScale);

            RareFind = new GameObject("RareFind").AddComponent<RareFindPresenter>();
            RareFind.Init(_camera, _database.Feel, _visuals, on => Drag.InputEnabled = on, () => Hud.BookButtonScreenPoint());

            Viewer = new GameObject("CollectionViewer").AddComponent<CollectionViewer>();
            Viewer.Init(_camera, _database.Feel, _visuals, Hud.IsOverUi, on => Drag.InputEnabled = on);
        }

        void Start()
        {
            Hud.Init(_hudStyle);
            Hud.RestartRequested += Restart;
            Hud.ResetProgressRequested += ResetProgress;
            Hud.ToolSelected += Drag.SetTool;
            Hud.RareCardClosed += RareFind.Dismiss;
            Hud.CollectibleViewRequested += Viewer.Open;
            Hud.ViewerClosed += Viewer.Close;
            RareFind.CardRequested += Hud.ShowRareCard;
            RareFind.Finished += _ => Hud.OnCollectibleStored();
            Section.CollectibleFound += OnCollectibleFound;
            Section.StateChanged += MarkDirty;
            Wallet.Changed += (_, _) => MarkDirty();
            Context.Tools.Upgraded += (_, _) => MarkDirty();

            var saved = _data.SectionById(_startSection.Id);
            BuildSection(saved != null ? saved.Seed : _startSection.Seed, saved);
        }

        void OnCollectibleFound(ItemView item, CollectionBook.FindResult result)
        {
            if (!result.IsNew && item.Definition is CollectibleDefinition collectible)
                Hud.OnDuplicateSold(collectible, result.DuplicateCoins, item.transform.position);
            RareFind.Present(item, result);
        }

        void BuildSection(int seed, SectionSave save)
        {
            Drag.CancelDrag();
            Drag.InputEnabled = true;
            Section.Build(_startSection, Context, seed, save);
            if (!_camera.TryGetComponent<CameraFitter>(out var fitter)) fitter = _camera.gameObject.AddComponent<CameraFitter>();
            fitter.Frame(Section.ViewBounds, _database.Feel);
            Hud.Bind(Section, Context, _venue, _camera);
            if (save != null && Section.IsComplete) Hud.ShowCompleteBanner();
            MarkDirty();
        }

        /// <summary>"Play again": a fresh shuffle of the same section. Book, coins, tools and mastery stay.</summary>
        public void Restart() => BuildSection(Random.Range(1, int.MaxValue), null);

        /// <summary>Prototype helper: wipe the save and start over.</summary>
        public void ResetProgress()
        {
            _suppressSave = true;
            _save.Delete();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        void MarkDirty()
        {
            if (!_dirty) _dirtyTimer = 0f;
            _dirty = true;
        }

        void Update()
        {
            if (!_dirty) return;
            _dirtyTimer += Time.unscaledDeltaTime;
            if (_dirtyTimer >= AutosaveDelay) SaveNow();
        }

        void OnApplicationPause(bool paused)
        {
            if (paused) SaveNow();
        }

        void OnApplicationQuit() => SaveNow();

        public void SaveNow()
        {
            if (_suppressSave || Context == null || Section == null || Section.Definition == null) return;
            _data.Coins = Wallet.Coins;
            _data.Collection = Book.FoundIds.ToList();
            _data.Mastery = Context.Mastery.Export().Select(m => new IdCount(m.Key, m.Value)).ToList();
            _data.Tools = Context.Tools.Export().Select(t => new IdCount(t.Key, t.Value)).ToList();
            _data.SetSection(Section.Capture());
            _save.Save(_data);
            _dirty = false;
        }
    }
}
