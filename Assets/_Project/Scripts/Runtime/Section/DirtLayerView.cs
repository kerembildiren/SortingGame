using System.Collections.Generic;
using SortingGame.Core;
using SortingGame.Data;
using UnityEngine;

namespace SortingGame.Section
{
    /// <summary>
    /// Draws a <see cref="DirtMask"/> as a transparent layer lying on the floor.
    /// Colour detail (dust variation, paper specks) is baked once; sweeping only changes alpha.
    /// Long floors are split into tiles so a broom stroke re-uploads only the tile it touched (mobile bandwidth).
    /// </summary>
    public class DirtLayerView : MonoBehaviour
    {
        const int TileWidthPixels = 256;

        class Tile
        {
            public int X0;
            public int Width;
            public Texture2D Texture;
            public Color32[] Pixels;
            public Material Material;
            public bool Dirty;
        }

        readonly List<Tile> _tiles = new();
        DirtMask _mask;
        byte[] _maxAlpha;
        Color32[] _baseColours;

        public void Build(DirtMask mask, Vector2 floorSize, SectionVisuals visuals, int seed)
        {
            _mask = mask;

            // Bake colour variation + paper specks for the whole mask once.
            var random = new System.Random(seed * 31 + 7);
            var count = mask.Width * mask.Height;
            _baseColours = new Color32[count];
            _maxAlpha = new byte[count];
            var dirt = visuals.DirtColor;
            var speck = visuals.DirtSpeckColor;
            for (var i = 0; i < count; i++)
            {
                var isSpeck = random.NextDouble() < 0.012;
                var shade = 0.8f + (float)random.NextDouble() * 0.35f;
                var c = isSpeck ? speck : dirt * shade;
                _baseColours[i] = new Color32((byte)(Mathf.Clamp01(c.r) * 255), (byte)(Mathf.Clamp01(c.g) * 255), (byte)(Mathf.Clamp01(c.b) * 255), 0);
                _maxAlpha[i] = (byte)(isSpeck ? 255 : 225);
            }

            var metresPerPixel = floorSize.x / mask.Width;
            for (var x0 = 0; x0 < mask.Width; x0 += TileWidthPixels)
            {
                var width = Mathf.Min(TileWidthPixels, mask.Width - x0);
                var tile = new Tile
                {
                    X0 = x0,
                    Width = width,
                    Pixels = new Color32[width * mask.Height],
                    Texture = new Texture2D(width, mask.Height, TextureFormat.RGBA32, false)
                    {
                        name = $"DirtTile{x0}",
                        wrapMode = TextureWrapMode.Clamp,
                        filterMode = FilterMode.Bilinear
                    }
                };

                var quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                DestroyImmediate(quad.GetComponent<Collider>());
                quad.name = tile.Texture.name;
                quad.transform.SetParent(transform, false);
                var tileWidthMetres = width * metresPerPixel;
                var centreX = -floorSize.x / 2f + (x0 + width / 2f) * metresPerPixel;
                quad.transform.localPosition = new Vector3(centreX, 0.004f, 0f);
                quad.transform.localRotation = Quaternion.Euler(90f, 0f, 0f); // local +Y -> world +Z, faces up
                quad.transform.localScale = new Vector3(tileWidthMetres, floorSize.y, 1f);

                tile.Material = new Material(visuals.DirtMaterial);
                tile.Material.SetTexture("_BaseMap", tile.Texture);
                var renderer = quad.GetComponent<MeshRenderer>();
                renderer.sharedMaterial = tile.Material;
                renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                _tiles.Add(tile);
            }
            RefreshAll();
        }

        public void RefreshRect(int xMin, int yMin, int xMax, int yMax)
        {
            foreach (var tile in _tiles)
            {
                var from = Mathf.Max(xMin, tile.X0);
                var to = Mathf.Min(xMax, tile.X0 + tile.Width - 1);
                if (from > to) continue;
                for (var y = yMin; y <= yMax; y++)
                for (var x = from; x <= to; x++)
                {
                    var source = y * _mask.Width + x;
                    var colour = _baseColours[source];
                    colour.a = (byte)(_mask[x, y] * _maxAlpha[source] / 255);
                    tile.Pixels[y * tile.Width + (x - tile.X0)] = colour;
                }
                tile.Dirty = true;
            }
        }

        public void RefreshAll() => RefreshRect(0, 0, _mask.Width - 1, _mask.Height - 1);

        /// <summary>Remaining dust dissolves (auto-finish and the 100% renovation).</summary>
        public void FadeOut(float duration)
        {
            var start = _tiles.Count > 0 ? _tiles[0].Material.GetColor("_BaseColor") : Color.white;
            Tween.Run(this, duration, t =>
            {
                var c = start;
                c.a = start.a * (1f - t);
                foreach (var tile in _tiles) tile.Material.SetColor("_BaseColor", c);
            }, Ease.InOutQuad, () =>
            {
                _mask.ClearAll();
                RefreshAll();
                foreach (var tile in _tiles) tile.Material.SetColor("_BaseColor", start);
            });
        }

        void LateUpdate()
        {
            foreach (var tile in _tiles)
            {
                if (!tile.Dirty) continue;
                tile.Dirty = false;
                tile.Texture.SetPixels32(tile.Pixels);
                tile.Texture.Apply(false);
            }
        }
    }
}
