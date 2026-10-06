using NUnit.Framework;
using SortingGame.Core;
using UnityEngine;

namespace SortingGame.Tests
{
    public class ShelfInspectViewTests
    {
        static ShelfInspectView WideShelf() => new()
        {
            HalfSize = new Vector2(2f, 1f),
            ViewPerDistance = new Vector2(0.2f, 0.35f),
            MinDistance = 1f,
            MaxDistance = 5f,
            EdgeShare = 1f
        };

        [Test]
        public void Reset_FramesAsMuchAsAllowed_Centred()
        {
            var narrow = new ShelfInspectView { HalfSize = new Vector2(0.5f, 1f), ViewPerDistance = new Vector2(0.2f, 0.35f), MinDistance = 1f, MaxDistance = 10f };
            narrow.Reset();
            Assert.AreEqual(Vector2.zero, narrow.Focus);
            Assert.That(narrow.FitDistance * 0.2f, Is.GreaterThanOrEqualTo(0.5f), "Whole shelf width fits.");
            Assert.That(narrow.FitDistance * 0.35f, Is.GreaterThanOrEqualTo(1f), "Whole shelf height fits.");
            Assert.Less(narrow.PanLimit.magnitude, 0.0001f, "Nothing to pan when the shelf fits.");

            var wide = WideShelf();
            wide.Reset();
            Assert.AreEqual(5f, wide.Distance, "A wide shelf is capped to the widest allowed frame.");
            Assert.Greater(wide.PanLimit.x, 0f, "...and can be panned sideways.");
        }

        [Test]
        public void Pan_StopsAtTheShelfEdge()
        {
            var view = WideShelf();
            view.Reset();
            view.PanBy(new Vector2(100f, 100f));
            // At distance 5 the visible half is 1 x 1.75: the edge of a 2 x 1 half shelf is 1 m away sideways, nothing vertically.
            Assert.AreEqual(1f, view.Focus.x, 0.001f);
            Assert.AreEqual(0f, view.Focus.y, 0.001f);
        }

        [Test]
        public void ZoomingIn_AllowsMorePan_ZoomingOut_PullsBackInside()
        {
            var view = WideShelf();
            view.Reset();
            view.ZoomBy(5f); // distance 1: visible half 0.2 x 0.35
            Assert.AreEqual(1f, view.Distance, 0.001f);
            view.PanBy(new Vector2(100f, -100f));
            Assert.AreEqual(1.8f, view.Focus.x, 0.001f);
            Assert.AreEqual(-0.65f, view.Focus.y, 0.001f);

            view.ZoomBy(0.01f); // back to the far limit
            Assert.AreEqual(5f, view.Distance, 0.001f);
            Assert.AreEqual(1f, view.Focus.x, 0.001f, "Focus is pulled back so the view stays on the shelf.");
            Assert.AreEqual(0f, view.Focus.y, 0.001f);
        }

        [Test]
        public void Flick_Glides_ThenStops_HoldStopsAtOnce()
        {
            var view = WideShelf();
            view.Reset();
            view.ZoomBy(5f);
            view.PanBy(new Vector2(0.04f, 0f), 0.02f); // 2 m/s flick
            var afterFlick = view.Focus.x;
            view.Tick(0.1f);
            Assert.Greater(view.Focus.x, afterFlick);
            for (var i = 0; i < 200; i++) view.Tick(0.05f);
            Assert.IsFalse(view.IsGliding);

            view.PanBy(new Vector2(-0.04f, 0f), 0.02f);
            view.Hold();
            var held = view.Focus.x;
            view.Tick(0.1f);
            Assert.AreEqual(held, view.Focus.x, 0.0001f);
        }
    }
}
