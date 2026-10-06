using SortingGame.Core;
using SortingGame.Data;
using UnityEngine;

namespace SortingGame.Section
{
    /// <summary>
    /// Draws a <see cref="DirtMask"/> as a transparent layer lying on the floor.
    /// Colour detail (dust variation, paper specks) is baked once; sweeping only changes alpha.
    /// </summary>
    public class DirtLayerView : MonoBehaviour
    {
        DirtMask _mask;
        Texture2D _texture;
        Color32[] _pixels;
        byte[] _maxAlpha;
        Material _material;
        bool _dirty;

        public void Build(DirtMask mask, Vector2 floorSize, SectionVisuals visuals, int seed)
        {
            _mask = mask;

            var quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            DestroyImmediate(quad.GetComponent<Collider>());
            quad.name = "DirtQuad";
            quad.transform.SetParent(transform, false);
            quad.transform.localPosition = new Vector3(0f, 0.004f, 0f);
            quad.transform.localRotation = Quaternion.Euler(90f, 0f, 0f); // local +Y -> world +Z, faces up
            quad.transform.localScale = new Vector3(floorSize.x, floorSize.y, 1f);

            _texture = new Texture2D(mask.Width, mask.Height, TextureFormat.RGBA32, false)
            {
                name = "DirtMask",
                wrapMode = TextureWrapMode.Clamp,
                filterMode = FilterMode.Bilinear
            };

            // Bake colour variation + paper specks.
            var random = new System.Random(seed * 31 + 7);
            _pixels = new Color32[mask.Width * mask.Height];
            _maxAlpha = new byte[_pixels.Length];
            var dirt = visuals.DirtColor;
            var speck = visuals.DirtSpeckColor;
            for (var i = 0; i < _pixels.Length; i++)
            {
                var isSpeck = random.NextDouble() < 0.012;
                var shade = 0.8f + (float)random.NextDouble() * 0.35f;
                var c = isSpeck ? speck : dirt * shade;
                _pixels[i] = new Color32((byte)(Mathf.Clamp01(c.r) * 255), (byte)(Mathf.Clamp01(c.g) * 255), (byte)(Mathf.Clamp01(c.b) * 255), 0);
                _maxAlpha[i] = (byte)(isSpeck ? 255 : 225);
            }
            RefreshRect(0, 0, mask.Width - 1, mask.Height - 1);

            _material = new Material(visuals.DirtMaterial);
            _material.SetTexture("_BaseMap", _texture);
            var renderer = quad.GetComponent<MeshRenderer>();
            renderer.sharedMaterial = _material;
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }

        public void RefreshRect(int xMin, int yMin, int xMax, int yMax)
        {
            var width = _mask.Width;
            for (var y = yMin; y <= yMax; y++)
            for (var x = xMin; x <= xMax; x++)
            {
                var i = y * width + x;
                _pixels[i].a = (byte)(_mask[x, y] * _maxAlpha[i] / 255);
            }
            _dirty = true;
        }

        public void RefreshAll() => RefreshRect(0, 0, _mask.Width - 1, _mask.Height - 1);

        /// <summary>Remaining dust dissolves (auto-finish and the 100% renovation).</summary>
        public void FadeOut(float duration)
        {
            var start = _material.GetColor("_BaseColor");
            Tween.Run(this, duration, t =>
            {
                var c = start;
                c.a = start.a * (1f - t);
                _material.SetColor("_BaseColor", c);
            }, Ease.InOutQuad, () =>
            {
                _mask.ClearAll();
                RefreshAll();
                _material.SetColor("_BaseColor", start);
            });
        }

        void LateUpdate()
        {
            if (!_dirty) return;
            _dirty = false;
            _texture.SetPixels32(_pixels);
            _texture.Apply(false);
        }
    }
}
