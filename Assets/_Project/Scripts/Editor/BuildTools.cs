using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace SortingGame.EditorTools
{
    /// <summary>
    /// Android build for device tests and as a per-milestone "does it still build" check.
    /// Batchmode: Unity.exe -batchmode -quit -projectPath . -executeMethod SortingGame.EditorTools.BuildTools.BuildAndroid
    /// </summary>
    public static class BuildTools
    {
        public const string AndroidOutput = "Builds/Android/SortingGame.apk";

        [MenuItem("Sorting Game/Build/Android APK (development)")]
        public static void BuildAndroid()
        {
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
                EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);

            EditorUserBuildSettings.buildAppBundle = false; // APK for sideloading; AAB later for the store

            var options = new BuildPlayerOptions
            {
                scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray(),
                locationPathName = AndroidOutput,
                target = BuildTarget.Android,
                options = BuildOptions.Development
            };

            var report = BuildPipeline.BuildPlayer(options);
            var summary = report.summary;
            Debug.Log($"[BuildTools] Android build {summary.result}: {summary.totalSize / (1024f * 1024f):0.0} MB, " +
                      $"{summary.totalTime.TotalSeconds:0}s, {summary.totalErrors} errors, {summary.totalWarnings} warnings -> {AndroidOutput}");

            if (Application.isBatchMode && summary.result != BuildResult.Succeeded)
                EditorApplication.Exit(1);
        }
    }
}
