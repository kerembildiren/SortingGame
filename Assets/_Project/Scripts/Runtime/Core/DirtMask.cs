using System;

namespace SortingGame.Core
{
    /// <summary>
    /// GDD 7.3 DirtLayer: dust and paper are not items but a layer over the floor.
    /// Each pixel stores how much dirt is left (0..255). Plain C# so it can be unit tested;
    /// the view turns it into a texture. u/v are 0..1 across the floor.
    /// </summary>
    public class DirtMask
    {
        public int Width { get; }
        public int Height { get; }

        readonly byte[] _amount;
        long _initialTotal;
        long _currentTotal;

        public DirtMask(int width, int height)
        {
            if (width <= 0 || height <= 0) throw new ArgumentOutOfRangeException();
            Width = width;
            Height = height;
            _amount = new byte[width * height];
        }

        public byte this[int x, int y] => _amount[y * Width + x];

        public bool HasDirt => _initialTotal > 0;

        /// <summary>0 at start, 1 when everything is swept.</summary>
        public float CleanedFraction => _initialTotal == 0 ? 1f : 1f - (float)_currentTotal / _initialTotal;

        /// <summary>
        /// Blotchy coverage: roughly <paramref name="coverage"/> of the floor gets dirt, with soft edges.
        /// </summary>
        public void Generate(float coverage, int seed)
        {
            coverage = Math.Clamp(coverage, 0f, 1f);
            var noise = new float[_amount.Length];
            var random = new Random(seed);
            var offsetX = (float)random.NextDouble() * 100f;
            var offsetY = (float)random.NextDouble() * 100f;
            for (var y = 0; y < Height; y++)
            for (var x = 0; x < Width; x++)
            {
                // Two octaves of value noise; scale is in "floor widths" so blotch size is resolution independent.
                var u = (float)x / Width * 3.2f + offsetX;
                var v = (float)y / Width * 3.2f + offsetY;
                noise[y * Width + x] = ValueNoise(u, v, seed) * 0.7f + ValueNoise(u * 2.7f, v * 2.7f, seed + 1) * 0.3f;
            }

            // Threshold so that the requested share of the floor is dirty.
            var sorted = (float[])noise.Clone();
            Array.Sort(sorted);
            var index = Math.Clamp((int)((1f - coverage) * (sorted.Length - 1)), 0, sorted.Length - 1);
            var threshold = coverage <= 0f ? float.MaxValue : sorted[index];
            const float softness = 0.06f;

            _initialTotal = 0;
            for (var i = 0; i < _amount.Length; i++)
            {
                var t = Math.Clamp((noise[i] - threshold) / softness + 0.5f, 0f, 1f);
                _amount[i] = (byte)(t * 255f);
                _initialTotal += _amount[i];
            }
            _currentTotal = _initialTotal;
        }

        /// <summary>
        /// Removes dirt in a soft circle. Returns the dirty pixel rect (xMin, yMin, xMax, yMax) or null if nothing changed.
        /// </summary>
        public (int xMin, int yMin, int xMax, int yMax)? Brush(float u, float v, float radiusU, float strength)
        {
            if (_currentTotal == 0 || strength <= 0f) return null;
            var cx = u * Width;
            var cy = v * Height;
            var radius = radiusU * Width;
            var xMin = Math.Max(0, (int)(cx - radius));
            var xMax = Math.Min(Width - 1, (int)(cx + radius));
            var yMin = Math.Max(0, (int)(cy - radius));
            var yMax = Math.Min(Height - 1, (int)(cy + radius));
            if (xMin > xMax || yMin > yMax) return null;

            var changed = false;
            var radiusSq = radius * radius;
            for (var y = yMin; y <= yMax; y++)
            for (var x = xMin; x <= xMax; x++)
            {
                var dx = x + 0.5f - cx;
                var dy = y + 0.5f - cy;
                var distSq = dx * dx + dy * dy;
                if (distSq > radiusSq) continue;
                var i = y * Width + x;
                if (_amount[i] == 0) continue;
                // Full-strength core, soft outer rim: strokes clean decisively but blend at the edges.
                var falloff = Math.Min(1f, (1f - distSq / radiusSq) * 2f);
                var remove = (int)Math.Ceiling(strength * falloff * 255f);
                var next = Math.Max(0, _amount[i] - remove);
                _currentTotal -= _amount[i] - next;
                _amount[i] = (byte)next;
                changed = true;
            }
            return changed ? (xMin, yMin, xMax, yMax) : null;
        }

        /// <summary>Average dirt (0..1) in a small square around u/v.</summary>
        public float Sample(float u, float v, float radiusU)
        {
            var cx = (int)(u * Width);
            var cy = (int)(v * Height);
            var r = Math.Max(1, (int)(radiusU * Width));
            long sum = 0;
            var count = 0;
            for (var y = Math.Max(0, cy - r); y <= Math.Min(Height - 1, cy + r); y++)
            for (var x = Math.Max(0, cx - r); x <= Math.Min(Width - 1, cx + r); x++)
            {
                sum += _amount[y * Width + x];
                count++;
            }
            return count == 0 ? 0f : sum / (count * 255f);
        }

        public void ClearAll()
        {
            Array.Clear(_amount, 0, _amount.Length);
            _currentTotal = 0;
        }

        static float ValueNoise(float x, float y, int seed)
        {
            var x0 = (int)Math.Floor(x);
            var y0 = (int)Math.Floor(y);
            var tx = Smooth(x - x0);
            var ty = Smooth(y - y0);
            var a = Hash(x0, y0, seed);
            var b = Hash(x0 + 1, y0, seed);
            var c = Hash(x0, y0 + 1, seed);
            var d = Hash(x0 + 1, y0 + 1, seed);
            return Lerp(Lerp(a, b, tx), Lerp(c, d, tx), ty);
        }

        static float Smooth(float t) => t * t * (3f - 2f * t);
        static float Lerp(float a, float b, float t) => a + (b - a) * t;

        static float Hash(int x, int y, int seed)
        {
            unchecked
            {
                var h = x * 374761393 + y * 668265263 + seed * 1442695041;
                h = (h ^ (h >> 13)) * 1274126177;
                h ^= h >> 16;
                return (h & 0xFFFFFF) / (float)0xFFFFFF;
            }
        }
    }
}
