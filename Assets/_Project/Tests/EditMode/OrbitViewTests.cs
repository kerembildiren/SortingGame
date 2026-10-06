using NUnit.Framework;
using SortingGame.Core;
using UnityEngine;

namespace SortingGame.Tests
{
    public class OrbitViewTests
    {
        [Test]
        public void Pitch_IsClamped_YawWraps()
        {
            var view = new OrbitView();
            view.Rotate(new Vector2(370f, 200f));

            Assert.AreEqual(10f, view.Yaw, 0.001f);
            Assert.AreEqual(view.MaxPitch, view.Pitch);
        }

        [Test]
        public void Zoom_AndPan_StayWithinLimits()
        {
            var view = new OrbitView { MinZoom = 0.5f, MaxZoom = 2f, PanLimit = new Vector2(1f, 1f) };
            view.ZoomBy(10f);
            view.PanBy(new Vector2(5f, -5f));

            Assert.AreEqual(2f, view.Zoom);
            Assert.AreEqual(new Vector2(1f, -1f), view.Pan);

            view.ZoomBy(0.01f);
            Assert.AreEqual(0.5f, view.Zoom);
        }

        [Test]
        public void Flick_KeepsSpinning_ThenSlowsDown()
        {
            var view = new OrbitView { Damping = 4f };
            view.Rotate(new Vector2(10f, 0f), 0.02f); // 500 deg/s flick
            var afterFlick = view.Yaw;

            view.Tick(0.1f);
            var firstStep = Mathf.DeltaAngle(afterFlick, view.Yaw);
            var before = view.Yaw;
            view.Tick(0.1f);
            var secondStep = Mathf.DeltaAngle(before, view.Yaw);

            Assert.Greater(firstStep, 0f);
            Assert.Less(secondStep, firstStep);
        }

        [Test]
        public void IdleSpin_StartsOnlyAfterDelay()
        {
            var view = new OrbitView { IdleDelay = 1f, IdleSpinSpeed = 10f };
            view.Tick(0.5f);
            Assert.AreEqual(0f, view.Yaw);

            view.Tick(0.6f);
            view.Tick(1f);
            Assert.Greater(view.Yaw, 0f);
            Assert.IsTrue(view.IsIdleSpinning);
        }

        [Test]
        public void Reset_ReturnsToDefault()
        {
            var view = new OrbitView();
            view.Rotate(new Vector2(30f, 20f));
            view.ZoomBy(2f);
            view.PanBy(Vector2.one * 0.1f);

            view.Reset();

            Assert.AreEqual(0f, view.Yaw);
            Assert.AreEqual(0f, view.Pitch);
            Assert.AreEqual(1f, view.Zoom);
            Assert.AreEqual(Vector2.zero, view.Pan);
        }
    }
}
