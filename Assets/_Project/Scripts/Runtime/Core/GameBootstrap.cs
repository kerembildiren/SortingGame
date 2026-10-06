using System.Collections.Generic;
using System.Linq;
using SortingGame.Data;
using SortingGame.Overview;
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
        public OverviewController Overview { get; private set; }
        public GameFlow Flow { get; private set; }
        public ShelfShowcase Showcase { get; private set; }
        public ShelfInspector Inspector { get; private set; }
        public SaveData Data => _data;

        Camera _camera;
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

            if (_database == null || _visuals == null || _database.Venues.Count == 0)
            {
                Debug.LogError("[GameBootstrap] Missing references. Run 'Sorting Game/Setup/Run Full Setup'.");
                enabled = false;
                return;
            }

            foreach (var error in _database.Validate())
                Debug.LogError($"[GameDatabase] {error}");

            _camera = Camera.main;
            _camera.clearFlags = CameraClearFlags.SolidColor;

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
            Viewer.Init(_camera, _database.Feel, _visuals, Hud.IsOverUi, SetWorldInput);

            Overview = new GameObject("Overview").AddComponent<OverviewController>();
            Overview.Init(Context, _camera, Hud.IsOverUi);
            Flow = gameObject.AddComponent<GameFlow>();

            Showcase = _camera.gameObject.AddComponent<ShelfShowcase>();
            Showcase.Init(_camera, _database.Feel, on => Drag.InputEnabled = on);

            Inspector = _camera.gameObject.AddComponent<ShelfInspector>();
            Inspector.Init(_camera, _database.Feel, Hud.IsOverUi, on => Drag.InputEnabled = on);
        }

        /// <summary>Viewer/rare moment pause world input; give it back to whichever screen is active.</summary>
        void SetWorldInput(bool on)
        {
            if (Flow == null || Flow.Current == GameFlow.Screen.Section) Drag.InputEnabled = on;
            else Overview.InputEnabled = on;
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
            RareFind.Finished += OnCollectibleStored;
            Section.ShelfCompleted += Showcase.Enqueue;
            Drag.FullShelfTapped += Inspector.Open;
            Inspector.Opened += Hud.ShowInspect;
            Inspector.Closed += Hud.HideInspect;
            Hud.InspectBackRequested += Inspector.Close;
            Hud.IsBusyWithMoment = () => Showcase.IsPlaying || RareFind.IsPresenting || Inspector.IsOpen || _bookCelebrationPending;
            Section.CollectibleFound += OnCollectibleFound;
            Section.StateChanged += MarkDirty;
            Wallet.Changed += (_, _) => MarkDirty();
            Context.Tools.Upgraded += (_, _) => MarkDirty();

            Flow.Init(this, Overview, _camera);
            Flow.Resume();
        }

        bool _bookCelebrationPending;

        VenueDefinition CompletedPageOf(CollectibleDefinition collectible)
        {
            var venue = _database.Venues.FirstOrDefault(v => v.CollectionPage.Contains(collectible));
            return venue != null && Book.CountFound(venue.CollectionPage) == venue.CollectionPage.Count ? venue : null;
        }

        /// <summary>After a find has flown into the book: celebrate when that venue's page is now complete.</summary>
        void OnCollectibleStored(CollectibleDefinition collectible)
        {
            Hud.OnCollectibleStored();
            var venue = CompletedPageOf(collectible);
            if (venue == null) return;
            Tween.Delay(this, 0.4f, () =>
            {
                Hud.PlayCollectionComplete(venue); // the open book then holds back the section banner
                _bookCelebrationPending = false;
            });
        }

        void OnCollectibleFound(ItemView item, CollectionBook.FindResult result)
        {
            // Known right away, so the section-complete banner waits for the book to open and close.
            if (result.IsNew && item.Definition is CollectibleDefinition found && CompletedPageOf(found) != null)
                _bookCelebrationPending = true;
            if (!result.IsNew && item.Definition is CollectibleDefinition collectible)
                Hud.OnDuplicateSold(collectible, result.DuplicateCoins, item.transform.position);
            RareFind.Present(item, result);
        }

        /// <summary>Loads a section from its save (or generates it the first time) and shows it.</summary>
        public void BuildSection(SectionDefinition section, VenueDefinition venue, bool fresh = false)
        {
            Inspector.StopNow();
            Drag.CancelDrag();
            Drag.InputEnabled = true;
            var save = fresh ? null : VenueProgress.ValidSave(section, _data);
            var seed = save != null ? save.Seed : (fresh ? Random.Range(1, int.MaxValue) : section.Seed);
            Section.Build(section, Context, seed, save);
            if (!_camera.TryGetComponent<CameraFitter>(out var fitter)) fitter = _camera.gameObject.AddComponent<CameraFitter>();
            fitter.enabled = true;
            fitter.Frame(Section.ViewBounds, _database.Feel);
            Hud.Bind(Section, Context, venue, _camera);
            if (save != null && Section.IsComplete) Hud.ShowCompleteBanner();
            MarkDirty();
        }

        /// <summary>Dev helper (Settings): reshuffle the current section. Book, coins, tools and mastery stay.</summary>
        public void Restart()
        {
            if (Flow.Current != GameFlow.Screen.Section || Flow.ActiveSection == null) return;
            BuildSection(Flow.ActiveSection, Flow.Venue, true);
        }

        /// <summary>Prototype helper: wipe the save and start over.</summary>
        public void ResetProgress()
        {
            _suppressSave = true;
            _save.Delete();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void MarkDirty()
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
            if (_suppressSave || Context == null || Flow == null) return;
            _data.Coins = Wallet.Coins;
            _data.Collection = Book.FoundIds.ToList();
            _data.Mastery = Context.Mastery.Export().Select(m => new IdCount(m.Key, m.Value)).ToList();
            _data.Tools = Context.Tools.Export().Select(t => new IdCount(t.Key, t.Value)).ToList();
            if (Section.IsLoaded) _data.SetSection(Section.Capture());
            _save.Save(_data);
            _dirty = false;
        }
    }
}
