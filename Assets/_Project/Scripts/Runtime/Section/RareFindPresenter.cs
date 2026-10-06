using System;
using SortingGame.Core;
using SortingGame.Data;
using UnityEngine;

namespace SortingGame.Section
{
    /// <summary>
    /// GDD 9.3 Chubby find moment, 3D half: the figure rises in front of the camera with light rays and
    /// sparkles while the scene darkens. The HUD shows the card on top and calls <see cref="Dismiss"/>.
    /// </summary>
    public class RareFindPresenter : MonoBehaviour
    {
        /// <summary>A new collectible is on display; the HUD should show its card.</summary>
        public event Action<CollectibleDefinition> CardRequested;
        /// <summary>The item has flown into the book; the HUD can pulse its book button.</summary>
        public event Action<CollectibleDefinition> Finished;

        public bool IsPresenting => _item != null;
        public ItemView PresentedItem => _item;

        const float RaysAlpha = 0.6f;

        Camera _camera;
        FeelConfig _feel;
        Fx _fx;
        PlaceholderFactory _factory;
        Action<bool> _setInputEnabled;
        Func<Vector2> _bookScreenPoint;

        Transform _stage;
        MeshRenderer _dim;
        MeshRenderer _rays;
        ItemView _item;
        CollectibleDefinition _definition;
        Coroutine _spin;

        public void Init(Camera cam, FeelConfig feel, SectionVisuals visuals, Action<bool> setInputEnabled, Func<Vector2> bookScreenPoint)
        {
            _camera = cam;
            _feel = feel;
            _fx = new Fx(visuals);
            _factory = new PlaceholderFactory(visuals);
            _setInputEnabled = setInputEnabled;
            _bookScreenPoint = bookScreenPoint;

            // A stage parented to the camera: dimmer behind, rays behind the item, item in front.
            _stage = new GameObject("RareStage").transform;
            _stage.SetParent(cam.transform, false);
            _dim = _factory.CreateSoftQuad("Dim", _stage, new Color(0.05f, 0.03f, 0.02f, 0f), null);
            _dim.transform.localPosition = new Vector3(0f, 0f, feel.RareDisplayDistance + 0.6f);
            _rays = _factory.CreateSoftQuad("Rays", _stage, new Color(1f, 0.85f, 0.4f, 0f), ProceduralTextures.Rays);
            _rays.transform.localPosition = new Vector3(0f, DisplayHeight, feel.RareDisplayDistance + 0.3f);
            _stage.gameObject.SetActive(false);
        }

        /// <summary>World size that makes the item cover RareDisplayScreenShare of the screen width at the display distance.</summary>
        float DisplaySize
        {
            get
            {
                var height = 2f * _feel.RareDisplayDistance * Mathf.Tan(_camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
                return height * Mathf.Min(1f, _camera.aspect) * _feel.RareDisplayScreenShare;
            }
        }

        /// <summary>Upper middle of the screen, leaving room for the card below.</summary>
        float DisplayHeight => Mathf.Tan(_camera.fieldOfView * 0.5f * Mathf.Deg2Rad) * _feel.RareDisplayDistance * 0.3f;

        public void Present(ItemView item)
        {
            if (item.Definition is not CollectibleDefinition definition) return;

            _item = item;
            _definition = definition;
            _setInputEnabled?.Invoke(false);
            SfxPlayer.Instance?.Play(Sfx.RareFanfare, 0f);
            Haptics.Strong();
            _fx.Burst(item.transform.position + Vector3.up * 0.2f, new Color(1f, 0.85f, 0.4f), 24, 2f, 0.12f, 0.8f);

            // Size the dimmer to cover the whole view at its distance.
            var dimDistance = _feel.RareDisplayDistance + 0.6f;
            var height = 2f * dimDistance * Mathf.Tan(_camera.fieldOfView * 0.5f * Mathf.Deg2Rad) * 1.3f;
            _dim.transform.localScale = new Vector3(height * Mathf.Max(1f, _camera.aspect) * 1.3f, height, 1f);
            _rays.transform.localScale = Vector3.one * (DisplaySize * 2.8f);
            _rays.transform.localPosition = new Vector3(0f, DisplayHeight, _feel.RareDisplayDistance + 0.3f);
            _stage.gameObject.SetActive(true);

            // Parent to the camera stage so the presentation stays framed even if the camera re-fits.
            item.transform.SetParent(_stage, true);
            var startPosition = item.transform.localPosition;
            var startRotation = item.transform.localRotation;
            var startScale = item.transform.localScale;
            var targetScale = Vector3.one * (DisplaySize / Mathf.Max(0.05f, item.VisualSize));
            var target = new Vector3(0f, DisplayHeight, _feel.RareDisplayDistance);

            Tween.Run(this, _feel.RareRiseDuration, t =>
            {
                item.transform.localPosition = Vector3.Lerp(startPosition, target, t) + Vector3.up * (Ease.Pulse(t) * 0.3f);
                item.transform.localRotation = Quaternion.Slerp(startRotation, Quaternion.identity, t); // front (-Z) faces the camera
                item.transform.localScale = Vector3.LerpUnclamped(startScale, targetScale, t);
                SetAlpha(_dim, _feel.RareDimAlpha * Mathf.Clamp01(t * 1.5f));
                SetAlpha(_rays, RaysAlpha * t);
            }, Ease.OutCubic, () =>
            {
                _spin = Tween.Run(this, 3600f, _ =>
                {
                    if (_item == null) return;
                    _rays.transform.Rotate(0f, 0f, -20f * Time.deltaTime, Space.Self);
                    _item.transform.localRotation = Quaternion.Euler(0f, Mathf.Sin(Time.time * 1.6f) * 25f, 0f);
                    // Keep the on-screen share right if the screen/aspect changes mid-presentation.
                    _item.transform.localScale = Vector3.one * (DisplaySize / Mathf.Max(0.05f, _item.VisualSize));
                    _rays.transform.localScale = Vector3.one * (DisplaySize * 2.8f);
                });
                CardRequested?.Invoke(definition);
            });
        }

        /// <summary>Card button pressed: item flies into the book, scene comes back.</summary>
        public void Dismiss()
        {
            if (_item == null) return;
            Tween.Stop(_spin);
            var item = _item;
            var definition = _definition;
            _item = null;
            _definition = null;

            var start = item.transform.localPosition;
            var startScale = item.transform.localScale;
            var screen = _bookScreenPoint != null ? _bookScreenPoint() : new Vector2(Screen.width * 0.9f, Screen.height * 0.85f);
            var target = _stage.InverseTransformPoint(_camera.ScreenToWorldPoint(new Vector3(screen.x, screen.y, _feel.RareDisplayDistance)));
            SfxPlayer.Instance?.Play(Sfx.BookStamp, 0f);

            Tween.Run(this, 0.45f, t =>
            {
                item.transform.localPosition = Vector3.Lerp(start, target, t);
                item.transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);
                SetAlpha(_dim, _feel.RareDimAlpha * (1f - t));
                SetAlpha(_rays, RaysAlpha * (1f - t));
            }, Ease.InQuad, () =>
            {
                Destroy(item.gameObject);
                _stage.gameObject.SetActive(false);
                _setInputEnabled?.Invoke(true);
                Haptics.Light();
                Finished?.Invoke(definition);
            });
        }

        static void SetAlpha(MeshRenderer renderer, float alpha)
        {
            var material = renderer.sharedMaterial;
            var c = material.GetColor("_BaseColor");
            c.a = alpha;
            material.SetColor("_BaseColor", c);
        }
    }
}
