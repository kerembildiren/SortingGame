using NUnit.Framework;
using SortingGame.Core;

namespace SortingGame.Tests
{
    public class WalletTests
    {
        [Test]
        public void Add_IncreasesCoins_AndRaisesChanged()
        {
            var wallet = new Wallet(10);
            long reportedDelta = 0;
            wallet.Changed += (_, delta) => reportedDelta = delta;

            wallet.Add(5);

            Assert.AreEqual(15, wallet.Coins);
            Assert.AreEqual(5, reportedDelta);
        }

        [Test]
        public void TrySpend_FailsWithoutChange_WhenNotEnoughCoins()
        {
            var wallet = new Wallet(3);

            Assert.IsFalse(wallet.TrySpend(4));
            Assert.AreEqual(3, wallet.Coins);
        }

        [Test]
        public void TrySpend_Succeeds_WhenAffordable()
        {
            var wallet = new Wallet(10);

            Assert.IsTrue(wallet.TrySpend(10));
            Assert.AreEqual(0, wallet.Coins);
        }
    }
}
