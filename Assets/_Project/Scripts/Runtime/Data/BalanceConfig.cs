using System;
using System.Collections.Generic;
using UnityEngine;

namespace SortingGame.Data
{
    /// <summary>GDD 11.5. Every tunable number lives here, never in code.</summary>
    [CreateAssetMenu(menuName = "Sorting Game/Balance Config", fileName = "BalanceConfig")]
    public class BalanceConfig : ScriptableObject
    {
        [Header("Section progress")]
        [Range(0f, 1f), Tooltip("How much of the section % comes from cleaning dirt. The rest comes from shelved items. Ignored for sections without dirt.")]
        public float DirtProgressWeight = 0.2f;

        [Header("Economy")]
        public int StartingCoins;

        [Serializable]
        public struct AutoSortPack
        {
            public string Id;
            public int Charges;
            [Tooltip("Stand-in until a real store supplies localised prices.")]
            public string PriceLabel;
        }

        [Header("Auto Sort boost (GDD 10.2, 11.3)")]
        [Tooltip("Real-money packs of Auto Sort charges. Never sold for coins.")]
        public List<AutoSortPack> AutoSortPacks = new();
    }
}
