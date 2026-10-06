using System;

namespace SortingGame.Core
{
    /// <summary>
    /// GDD 10.2: Auto Sort is a boost, not a permanent unlock. One use makes one shelf's category sort itself
    /// until that room is finished (the room keeps that state, see <c>SectionController.StartAutoSort</c>).
    /// A use is paid with a rewarded ad or with a charge; charges are bought with real money, never with coins.
    /// This class only keeps the charges.
    /// </summary>
    public class AutoSortBoost
    {
        public int Charges { get; private set; }

        public event Action<int> Changed;

        public AutoSortBoost(int charges = 0) => Charges = Math.Max(0, charges);

        public void Add(int amount)
        {
            if (amount <= 0) return;
            Charges += amount;
            Changed?.Invoke(Charges);
        }

        public bool TrySpend()
        {
            if (Charges <= 0) return false;
            Charges--;
            Changed?.Invoke(Charges);
            return true;
        }
    }
}
