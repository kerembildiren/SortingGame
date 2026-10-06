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

        [Header("Drag & drop")]
        [Tooltip("Pick tolerance around the finger, in metres. Larger = easier to grab small items.")]
        public float PickRadius = 0.1f;
        public float DragLiftHeight = 0.55f;
        [Tooltip("Carried item is drawn this many reference pixels above the finger so the finger does not hide it.")]
        public float FingerOffsetPixels = 110f;
        public float DragFollowSharpness = 28f;
        public float DragScale = 1.2f;

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

        [Header("Physics")]
        [Tooltip("Spilled items become static after resting this long (GDD 15.3).")]
        public float SettleTime = 0.35f;
        public float MaxPhysicsTime = 4f;
    }
}
