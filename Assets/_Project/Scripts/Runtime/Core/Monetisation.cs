using System;

namespace SortingGame.Core
{
    /// <summary>
    /// GDD 15.5: the ad network and the store are [AÇIK], so the game only talks to these interfaces.
    /// Rewarded ads are always the player's choice (GDD 11.2); nothing here interrupts play.
    /// </summary>
    public interface IAdProvider
    {
        bool IsRewardedReady { get; }

        /// <summary>Shows a rewarded ad. <paramref name="done"/> gets true only when the reward was earned.</summary>
        void ShowRewarded(Action<bool> done);
    }

    public interface IStoreProvider
    {
        /// <summary>Starts a real-money purchase. <paramref name="done"/> gets true when it went through.</summary>
        void Purchase(string productId, Action<bool> done);
    }

    /// <summary>Stand-in until a real ad SDK is chosen: the "ad" finishes at once. Tests can make it fail.</summary>
    public class FakeAdProvider : IAdProvider
    {
        public bool NextResult = true;
        public int ShownCount { get; private set; }

        public bool IsRewardedReady => true;

        public void ShowRewarded(Action<bool> done)
        {
            ShownCount++;
            done?.Invoke(NextResult);
        }
    }

    /// <summary>Stand-in until a real store is wired up: every purchase succeeds at once, no money involved.</summary>
    public class FakeStoreProvider : IStoreProvider
    {
        public bool NextResult = true;
        public string LastProductId { get; private set; }

        public void Purchase(string productId, Action<bool> done)
        {
            LastProductId = productId;
            done?.Invoke(NextResult);
        }
    }
}
