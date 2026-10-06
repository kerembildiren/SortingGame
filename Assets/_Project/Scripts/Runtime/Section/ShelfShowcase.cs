using System;
using System.Collections;
using System.Collections.Generic;
using SortingGame.Core;
using SortingGame.Data;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SortingGame.Section
{
    /// <summary>
    /// A shelf has just been filled: the camera swoops to the shelf, glides from the top row to the bottom row
    /// so the player can admire the sorted result, then returns. Tap to skip. Several finished shelves queue up.
    /// </summary>
    public class ShelfShowcase : MonoBehaviour
    {
        const float FlyIn = 0.55f;
        const float Glide = 1.5f;
        const float Hold = 0.25f;
        const float FlyOut = 0.5f;
        const float Pitch = 10f;

        readonly Queue<ShelfView> _queue = new();
        Camera _camera;
        FeelConfig _feel;
        Action<bool> _setInput;
        Coroutine _routine;
        bool _skip;

        public bool IsPlaying => _routine != null;

        public void Init(Camera cam, FeelConfig feel, Action<bool> setInput)
        {
            _camera = cam;
            _feel = feel;
            _setInput = setInput;
        }

        public void Enqueue(ShelfView shelf)
        {
            _queue.Enqueue(shelf);
            _routine ??= StartCoroutine(Run());
        }

        /// <summary>Leaving the section: drop everything, give the camera back immediately.</summary>
        public void StopNow()
        {
            _queue.Clear();
            if (_routine == null) return;
            StopCoroutine(_routine);
            _routine = null;
            if (_camera.TryGetComponent<CameraFitter>(out var fitter)) fitter.enabled = true;
        }

        void Update()
        {
            // Any tap skips the rest of the showcase.
            if (_routine != null && Pointer.current != null && Pointer.current.press.wasPressedThisFrame) _skip = true;
        }

        IEnumerator Run()
        {
            _skip = false;
            _setInput?.Invoke(false);
            var fitter = _camera.GetComponent<CameraFitter>();
            if (fitter != null) fitter.enabled = false;
            var homePosition = _camera.transform.position;
            var homeRotation = _camera.transform.rotation;

            while (_queue.Count > 0 && !_skip)
            {
                var shelf = _queue.Dequeue();
                if (shelf == null) continue;
                yield return Show(shelf);
            }
            _queue.Clear();

            // Back to the room view.
            var fromPosition = _camera.transform.position;
            var fromRotation = _camera.transform.rotation;
            yield return Animate(FlyOut, t =>
            {
                _camera.transform.position = Vector3.Lerp(fromPosition, homePosition, Ease.InOutQuad(t));
                _camera.transform.rotation = Quaternion.Slerp(fromRotation, homeRotation, Ease.InOutQuad(t));
            }, false);

            if (fitter != null)
            {
                fitter.enabled = true;
                fitter.Fit();
            }
            _setInput?.Invoke(true);
            _routine = null;
        }

        IEnumerator Show(ShelfView shelf)
        {
            var (top, bottom, rotation) = Poses(shelf);
            var startPosition = _camera.transform.position;
            var startRotation = _camera.transform.rotation;
            SfxPlayer.Instance?.PlayPitched(Sfx.Magnet, 0.6f, 0.5f);

            yield return Animate(FlyIn, t =>
            {
                _camera.transform.position = Vector3.Lerp(startPosition, top, Ease.OutCubic(t));
                _camera.transform.rotation = Quaternion.Slerp(startRotation, rotation, Ease.OutCubic(t));
            }, true);
            yield return Animate(Glide, t => _camera.transform.position = Vector3.Lerp(top, bottom, Ease.InOutQuad(t)), true);
            yield return Animate(Hold, null, true);
        }

        /// <summary>Camera poses in front of the shelf: framing its width, looking at the top row, then the bottom row.</summary>
        (Vector3 top, Vector3 bottom, Quaternion rotation) Poses(ShelfView shelf)
        {
            var size = shelf.Size;
            var origin = shelf.transform.position;
            var frontZ = origin.z - size.z / 2f;
            var rotation = Quaternion.Euler(Pitch, 0f, 0f);
            var forward = rotation * Vector3.forward;

            var halfHorizontal = Mathf.Atan(Mathf.Tan(_camera.fieldOfView * 0.5f * Mathf.Deg2Rad) * _camera.aspect);
            var distance = size.x * 0.58f / Mathf.Tan(halfHorizontal); // whole shelf width plus a little air

            var topTarget = new Vector3(origin.x, size.y * 0.8f, frontZ);
            var bottomTarget = new Vector3(origin.x, size.y * 0.22f, frontZ);
            return (topTarget - forward * distance, bottomTarget - forward * distance, rotation);
        }

        IEnumerator Animate(float duration, Action<float> step, bool skippable)
        {
            var time = 0f;
            while (time < duration)
            {
                if (skippable && _skip) yield break;
                time += Time.deltaTime;
                step?.Invoke(Mathf.Clamp01(time / duration));
                yield return null;
            }
        }
    }
}
