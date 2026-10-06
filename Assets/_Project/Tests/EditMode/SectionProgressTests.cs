using NUnit.Framework;
using SortingGame.Core;

namespace SortingGame.Tests
{
    public class SectionProgressTests
    {
        [Test]
        public void ItemsOnly_PercentFollowsPlacedItems()
        {
            var progress = new SectionProgress(4, false, 0.2f);
            progress.AddPlaced();

            Assert.AreEqual(25, progress.Percent);
        }

        [Test]
        public void NeverShows100_UntilComplete()
        {
            var progress = new SectionProgress(1000, false, 0f);
            for (var i = 0; i < 999; i++) progress.AddPlaced();

            Assert.AreEqual(99, progress.Percent);
            Assert.IsFalse(progress.IsComplete);

            progress.AddPlaced();
            Assert.AreEqual(100, progress.Percent);
            Assert.IsTrue(progress.IsComplete);
        }

        [Test]
        public void WithDirt_BothPartsCount()
        {
            var progress = new SectionProgress(2, true, 0.2f);
            progress.AddPlaced();
            progress.AddPlaced();

            Assert.AreEqual(80, progress.Percent);
            Assert.IsFalse(progress.IsComplete);

            progress.SetDirtCleaned(1f);
            Assert.IsTrue(progress.IsComplete);
        }

        [Test]
        public void NotComplete_WhileCollectiblesRemain()
        {
            var progress = new SectionProgress(1, false, 0f);
            progress.SetCollectiblesRemaining(1);
            progress.AddPlaced();

            Assert.IsFalse(progress.IsComplete);
            Assert.IsTrue(progress.OnlyCollectiblesLeft);
            Assert.AreEqual(99, progress.Percent);

            progress.SetCollectiblesRemaining(0);
            Assert.IsTrue(progress.IsComplete);
            Assert.AreEqual(100, progress.Percent);
        }

        [Test]
        public void AddPlaced_DoesNotExceedTotal()
        {
            var progress = new SectionProgress(1, false, 0f);
            progress.AddPlaced();
            progress.AddPlaced();

            Assert.AreEqual(1, progress.PlacedItems);
        }
    }
}
