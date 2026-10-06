using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SortingGame.Core
{
    public static class Ease
    {
        public static float Linear(float t) => t;
        public static float InQuad(float t) => t * t;
        public static float OutQuad(float t) => 1f - (1f - t) * (1f - t);
        public static float InOutQuad(float t) => t < 0.5f ? 2f * t * t : 1f - Mathf.Pow(-2f * t + 2f, 2f) / 2f;
        public static float OutCubic(float t) => 1f - Mathf.Pow(1f - t, 3f);

        public static float OutBack(float t)
        {
            const float c1 = 1.70158f, c3 = c1 + 1f;
            return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
        }

        /// <summary>0 -> 1 -> 0, for punches and flashes.</summary>
        public static float Pulse(float t) => Mathf.Sin(t * Mathf.PI);
    }

    /// <summary>Tiny coroutine tweener. Tweens stop by themselves when their owner object is destroyed.</summary>
    public static class Tween
    {
        static TweenRunner _runner;

        static TweenRunner Runner
        {
            get
            {
                if (_runner != null) return _runner;
                var go = new GameObject("[Tween]") { hideFlags = HideFlags.HideInHierarchy };
                Object.DontDestroyOnLoad(go);
                _runner = go.AddComponent<TweenRunner>();
                return _runner;
            }
        }

        /// <param name="step">Receives eased progress 0..1 every frame, guaranteed to end with 1.</param>
        public static Coroutine Run(Object owner, float duration, Action<float> step, Func<float, float> ease = null, Action done = null)
        {
            return Runner.StartCoroutine(Routine(owner, duration, step, ease ?? Ease.Linear, done));
        }

        public static Coroutine Delay(Object owner, float seconds, Action done) => Run(owner, seconds, null, null, done);

        public static void Stop(Coroutine coroutine)
        {
            if (coroutine != null && _runner != null) _runner.StopCoroutine(coroutine);
        }

        static IEnumerator Routine(Object owner, float duration, Action<float> step, Func<float, float> ease, Action done)
        {
            var hasOwner = owner != null;
            var time = 0f;
            while (time < duration)
            {
                if (hasOwner && owner == null) yield break;
                step?.Invoke(ease(time / duration));
                yield return null;
                time += Time.deltaTime;
            }
            if (hasOwner && owner == null) yield break;
            step?.Invoke(1f);
            done?.Invoke();
        }

        class TweenRunner : MonoBehaviour { }
    }
}
