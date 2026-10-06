using UnityEngine;

namespace SortingGame.Data
{
    /// <summary>GDD 11.5. Every tunable number lives here, never in code.</summary>
    [CreateAssetMenu(menuName = "Sorting Game/Balance Config", fileName = "BalanceConfig")]
    public class BalanceConfig : ScriptableObject
    {
        [Header("Section progress")]
        [Range(0f, 1f), Tooltip("How much of the section % comes from cleaning dirt. The rest comes from shelved items.")]
        public float DirtProgressWeight = 0.2f;

        [Header("Economy")]
        public int StartingCoins;

        [Header("Offline progress (GDD 15.4)")]
        public float OfflineCapHours = 8f;
    }
}
