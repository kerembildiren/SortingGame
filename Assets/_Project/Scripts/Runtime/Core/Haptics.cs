using UnityEngine;

namespace SortingGame.Core
{
    /// <summary>
    /// GDD 14. Short vibrations. Android uses VibrationEffect (API 26+) for light ticks.
    /// iOS needs a native plugin later; until then it is silent there.
    /// </summary>
    public static class Haptics
    {
        public static bool Enabled = true;

#if UNITY_ANDROID && !UNITY_EDITOR
        static AndroidJavaObject _vibrator;
        static bool _initialised;
        static int _sdk;

        static AndroidJavaObject Vibrator
        {
            get
            {
                if (_initialised) return _vibrator;
                _initialised = true;
                try
                {
                    using var version = new AndroidJavaClass("android.os.Build$VERSION");
                    _sdk = version.GetStatic<int>("SDK_INT");
                    using var player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                    var activity = player.GetStatic<AndroidJavaObject>("currentActivity");
                    _vibrator = activity.Call<AndroidJavaObject>("getSystemService", "vibrator");
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning($"[Haptics] Not available: {e.Message}");
                }
                return _vibrator;
            }
        }

        static void Vibrate(long milliseconds, int amplitude)
        {
            if (!Enabled || Vibrator == null) return;
            if (_sdk >= 26)
            {
                using var effectClass = new AndroidJavaClass("android.os.VibrationEffect");
                using var effect = effectClass.CallStatic<AndroidJavaObject>("createOneShot", milliseconds, amplitude);
                Vibrator.Call("vibrate", effect);
            }
            else
            {
                Handheld.Vibrate();
            }
        }
#else
        static void Vibrate(long milliseconds, int amplitude) { }
#endif

        /// <summary>Item snaps onto a shelf.</summary>
        public static void Light() => Vibrate(12, 70);

        /// <summary>Box tips over, shelf completes.</summary>
        public static void Medium() => Vibrate(25, 140);

        /// <summary>Rare find, section complete.</summary>
        public static void Strong() => Vibrate(60, 255);

        // Referencing Handheld.Vibrate makes Unity add the VIBRATE permission to the Android manifest.
        internal static void EnsurePermissionIsIncluded()
        {
            if (Time.frameCount < 0) Handheld.Vibrate();
        }
    }
}
