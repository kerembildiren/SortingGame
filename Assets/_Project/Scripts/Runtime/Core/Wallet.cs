using System;

namespace SortingGame.Core
{
    /// <summary>Soft currency (Coin). Plain C# so it can be unit tested and saved as-is.</summary>
    public class Wallet
    {
        public long Coins { get; private set; }

        public event Action<long, long> Changed; // (newTotal, delta)

        public Wallet(long startingCoins = 0)
        {
            if (startingCoins < 0) throw new ArgumentOutOfRangeException(nameof(startingCoins));
            Coins = startingCoins;
        }

        public void Add(long amount)
        {
            if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount), "Use TrySpend to remove coins.");
            if (amount == 0) return;
            Coins += amount;
            Changed?.Invoke(Coins, amount);
        }

        public bool CanAfford(long amount) => amount >= 0 && Coins >= amount;

        public bool TrySpend(long amount)
        {
            if (!CanAfford(amount)) return false;
            if (amount == 0) return true;
            Coins -= amount;
            Changed?.Invoke(Coins, -amount);
            return true;
        }
    }
}
