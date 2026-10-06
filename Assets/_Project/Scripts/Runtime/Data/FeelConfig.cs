using UnityEngine;

namespace SortingGame.Data
{
    /// <summary>
    /// "Game feel" tunables: camera, drag, animation timings. Separate from BalanceConfig (economy).
    /// Edit in Play Mode to tune; changes to the asset persist after Play Mode ends.
    /// </summary>
    [CreateAssetMenu(menuName = "Sorting Game/Feel Config", fileName = "FeelConfig")]
    public class FeelConfig : ScriptableObject
    {
        [Header("Section camera (GDD 6.2)")]
        [Range(30f, 80f)] public float CameraPitch = 52f;
        [Range(20f, 70f)] public float CameraFov = 38f;
        [Range(0f, 0.3f), Tooltip("Screen share reserved for the top bar.")]
        public float TopSafeArea = 0.11f;
        [Range(0f, 0.3f), Tooltip("Screen share reserved for the tool bar.")]
        public float BottomSafeArea = 0.15f;
        [Tooltip("Width in metres of the slice of a section shown at once. Wider sections pan sideways.")]
        public float SectionViewWidth = 4.6f;
        [Range(0f, 0.25f), Tooltip("Screen share at the left/right edge that scrolls the view while carrying or sweeping.")]
        public float EdgeScrollZone = 0.1f;
        [Tooltip("Edge scroll speed in metres per second at the very edge.")]
        public float EdgeScrollSpeed = 5f;

        [Header("Drag & drop. Grab radius comes from the Hand tool level.")]
        public float DragLiftHeight = 0.55f;
        [Tooltip("Carried item is drawn this many reference pixels above the finger so the finger does not hide it.")]
        public float FingerOffsetPixels = 110f;
        public float DragFollowSharpness = 28f;
        public float DragScale = 1.2f;

        [Tooltip("Seconds resting over a shelf before the carried items that belong there jump in. The rest stay in hand.")]
        public float ShelfDepositDwell = 0.3f;

        [Header("Placement")]
        public float PlaceDuration = 0.22f;
        public float PlacePunchScale = 1.25f;
        public float ReturnDuration = 0.35f;
        public float WrongShelfHintDuration = 0.8f;

        [Header("Containers")]
        public float TipDuration = 0.32f;
        public float SpillInterval = 0.025f;
        public Vector2 SpillForwardSpeed = new(1.6f, 3.2f);
        public Vector2 SpillUpSpeed = new(1.2f, 2.4f);
        public float SpillSideSpeed = 1.1f;
        [Tooltip("Seconds after spilling before the empty container shrinks away.")]
        public float EmptyContainerVanishDelay = 1.0f;

        [Header("Broom & dirt (GDD 7.1, 7.3). Brush radius comes from the Broom tool level.")]
        [Tooltip("Dirt removed per second at the brush centre (1 = a full layer).")]
        public float BroomStrength = 6f;
        [Range(0.5f, 1f), Tooltip("Once this much is clean, the rest fades away by itself. Nobody wants to hunt the last speck.")]
        public float DirtAutoFinish = 0.93f;
        [Range(0f, 1f), Tooltip("A buried item pops out when the dirt over it drops below this.")]
        public float RevealThreshold = 0.35f;
        [Tooltip("Dirt texture pixels across the floor width.")]
        public int DirtResolution = 192;

        [Header("Rare find moment (GDD 9.3)")]
        public float RareRiseDuration = 0.7f;
        [Tooltip("Distance in front of the camera where the rare item is presented.")]
        public float RareDisplayDistance = 2.2f;
        [Range(0.1f, 0.9f), Tooltip("Presented item size as a share of the screen width.")]
        public float RareDisplayScreenShare = 0.42f;
        [Range(0f, 1f)] public float RareDimAlpha = 0.7f;

        [Header("Collection viewer (GDD 9.1.1)")]
        [Range(0.1f, 0.9f), Tooltip("Item size at zoom 1 as a share of the screen width.")]
        public float ViewerScreenShare = 0.55f;
        [Tooltip("Degrees of turn for a drag across the full screen width.")]
        public float ViewerRotateDegreesPerScreen = 320f;
        public float ViewerMinZoom = 0.6f;
        public float ViewerMaxZoom = 2.6f;
        [Range(0f, 1f)] public float ViewerDimAlpha = 0.88f;
        public float ViewerIdleSpinSpeed = 18f;

        [Header("Section 100% renovation (GDD 5.5)")]
        public float RenovationDuration = 2.2f;
        [Range(0f, 1f), Tooltip("How much of the clean look is already reached at 99% progress. Gives continuous 'getting nicer' feedback.")]
        public float ProgressMoodShare = 0.35f;

        [Header("Physics")]
        [Tooltip("Spilled items become static after resting this long (GDD 15.3).")]
        public float SettleTime = 0.35f;
        public float MaxPhysicsTime = 4f;
    }
}
