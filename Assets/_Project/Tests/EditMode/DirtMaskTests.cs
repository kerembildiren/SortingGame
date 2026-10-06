using NUnit.Framework;
using SortingGame.Core;

namespace SortingGame.Tests
{
    public class DirtMaskTests
    {
        static float DirtyShare(DirtMask mask)
        {
            var dirty = 0;
            for (var y = 0; y < mask.Height; y++)
            for (var x = 0; x < mask.Width; x++)
                if (mask[x, y] > 127) dirty++;
            return (float)dirty / (mask.Width * mask.Height);
        }

        [Test]
        public void Generate_CoversRoughlyTheRequestedShare()
        {
            var mask = new DirtMask(128, 160);
            mask.Generate(0.6f, 5);

            Assert.That(DirtyShare(mask), Is.InRange(0.5f, 0.7f));
            Assert.AreEqual(0f, mask.CleanedFraction, 0.0001f);
        }

        [Test]
        public void ZeroCoverage_HasNoDirt_AndCountsAsClean()
        {
            var mask = new DirtMask(32, 32);
            mask.Generate(0f, 1);

            Assert.IsFalse(mask.HasDirt);
            Assert.AreEqual(1f, mask.CleanedFraction);
        }

        [Test]
        public void Brush_RemovesDirt_OnlyNearTheStroke()
        {
            var mask = new DirtMask(100, 100);
            mask.Generate(1f, 2);

            var rect = mask.Brush(0.5f, 0.5f, 0.1f, 1f);

            Assert.IsNotNull(rect);
            Assert.AreEqual(0, mask[50, 50]);
            Assert.Greater(mask[5, 5], 0);
            Assert.That(mask.CleanedFraction, Is.GreaterThan(0f).And.LessThan(0.1f));
        }

        [Test]
        public void SweepingEverywhere_ReachesFullyClean()
        {
            var mask = new DirtMask(64, 64);
            mask.Generate(0.7f, 9);

            for (var pass = 0; pass < 3; pass++)
            for (var v = 0f; v <= 1f; v += 0.05f)
            for (var u = 0f; u <= 1f; u += 0.05f)
                mask.Brush(u, v, 0.08f, 1f);

            Assert.AreEqual(1f, mask.CleanedFraction, 0.0001f);
        }

        [Test]
        public void Sample_IsHighOnDirt_AndZeroAfterBrushing()
        {
            var mask = new DirtMask(64, 64);
            mask.Generate(1f, 4);
            Assert.Greater(mask.Sample(0.5f, 0.5f, 0.02f), 0.5f);

            mask.Brush(0.5f, 0.5f, 0.15f, 1f);
            Assert.AreEqual(0f, mask.Sample(0.5f, 0.5f, 0.02f), 0.0001f);
        }
    }
}
