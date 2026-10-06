using SortingGame.Data;
using UnityEngine;

namespace SortingGame.Core
{
    /// <summary>Scene entry point. Owns global settings and, from M1 on, wires up the section.</summary>
    public class GameBootstrap : MonoBehaviour
    {
        [SerializeField] GameDatabase _database;

        void Awake()
        {
            Application.targetFrameRate = 60;
            Screen.orientation = ScreenOrientation.Portrait;

            if (_database == null)
            {
                Debug.LogError("[GameBootstrap] No GameDatabase assigned.");
                return;
            }

            foreach (var error in _database.Validate())
                Debug.LogError($"[GameDatabase] {error}");
        }
    }
}
