using SortingGame.Data;
using SortingGame.Section;
using SortingGame.UI;
using UnityEngine;
using UnityEngine.UIElements;

namespace SortingGame.Core
{
    /// <summary>Scene entry point: wires services, builds the start section, hooks up input and HUD.</summary>
    public class GameBootstrap : MonoBehaviour
    {
        [SerializeField] GameDatabase _database;
        [SerializeField] SectionDefinition _startSection;
        [SerializeField] SectionVisuals _visuals;
        [SerializeField] PanelSettings _panelSettings;
        [SerializeField] StyleSheet _hudStyle;

        public Wallet Wallet { get; private set; }
        public SectionController Section { get; private set; }
        public SectionHud Hud { get; private set; }
        public DragController Drag { get; private set; }

        Camera _camera;
        int _seedOffset;

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
            _camera.backgroundColor = _visuals.BackgroundColor;
            _camera.clearFlags = CameraClearFlags.SolidColor;

            new GameObject("Sfx").AddComponent<SfxPlayer>();
            Wallet = new Wallet(_database.Balance.StartingCoins);

            Section = new GameObject("Section").AddComponent<SectionController>();

            var hudObject = new GameObject("HUD");
            var document = hudObject.AddComponent<UIDocument>();
            document.panelSettings = _panelSettings;
            Hud = hudObject.AddComponent<SectionHud>();

            Drag = _camera.gameObject.AddComponent<DragController>();
            Drag.Init(_camera, Section, _database.Feel, Hud.IsOverUi, () => Hud.UiScale);
        }

        void Start()
        {
            Hud.Init(_hudStyle);
            Hud.RestartRequested += Restart;
            BuildSection();
        }

        void BuildSection()
        {
            Drag.CancelDrag();
            Section.Build(_startSection, _database, _visuals, Wallet, _startSection.Seed + _seedOffset);
            if (!_camera.TryGetComponent<CameraFitter>(out var fitter)) fitter = _camera.gameObject.AddComponent<CameraFitter>();
            fitter.Frame(Section.ViewBounds, _database.Feel);
            Hud.Bind(Section, Wallet, _camera);
        }

        /// <summary>Prototype only: rebuild with a new shuffle so repeated tests are not identical.</summary>
        public void Restart()
        {
            _seedOffset++;
            BuildSection();
        }
    }
}
