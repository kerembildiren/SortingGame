using SortingGame.Core;
using SortingGame.Data;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace SortingGame.EditorTools
{
    /// <summary>
    /// Idempotent project setup. Safe to run any number of times, from the menu or from batchmode:
    /// Unity.exe -batchmode -quit -projectPath . -executeMethod SortingGame.EditorTools.ProjectSetup.RunFullSetup
    /// </summary>
    public static class ProjectSetup
    {
        public const string DataFolder = "Assets/_Project/Data";
        public const string ScenesFolder = "Assets/_Project/Scenes";
        public const string DatabasePath = DataFolder + "/GameDatabase.asset";
        public const string BalancePath = DataFolder + "/BalanceConfig.asset";
        public const string MainScenePath = ScenesFolder + "/Main.unity";

        const string BundleId = "com.keremb.sortinggame";

        [MenuItem("Sorting Game/Setup/Run Full Setup")]
        public static void RunFullSetup()
        {
            ApplyPlayerSettings();
            var database = EnsureDatabase();
            CreateMainScene(database);
            AssetDatabase.SaveAssets();
            Debug.Log("[ProjectSetup] Done.");
        }

        [MenuItem("Sorting Game/Setup/Apply Player Settings")]
        public static void ApplyPlayerSettings()
        {
            PlayerSettings.companyName = "KeremB";
            PlayerSettings.productName = "Sorting Game";
            PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.Android, BundleId);
            PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.iOS, BundleId);

            // GDD 6.3: portrait only.
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;
            PlayerSettings.allowedAutorotateToPortrait = true;
            PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
            PlayerSettings.allowedAutorotateToLandscapeLeft = false;
            PlayerSettings.allowedAutorotateToLandscapeRight = false;

            // Play Store requires 64-bit, which requires IL2CPP.
            PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android, ScriptingImplementation.IL2CPP);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
        }

        static GameDatabase EnsureDatabase()
        {
            var balance = LoadOrCreate<BalanceConfig>(BalancePath);
            var database = LoadOrCreate<GameDatabase>(DatabasePath);
            if (database.Balance == null)
            {
                database.Balance = balance;
                EditorUtility.SetDirty(database);
            }
            return database;
        }

        static void CreateMainScene(GameDatabase database)
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

            var bootstrap = new GameObject("GameBootstrap").AddComponent<GameBootstrap>();
            var serialized = new SerializedObject(bootstrap);
            serialized.FindProperty("_database").objectReferenceValue = database;
            serialized.ApplyModifiedPropertiesWithoutUndo();

            EditorSceneManager.SaveScene(scene, MainScenePath);
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(MainScenePath, true) };
        }

        public static T LoadOrCreate<T>(string path) where T : ScriptableObject
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset != null) return asset;

            asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, path);
            return asset;
        }
    }
}
