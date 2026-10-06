using System;

namespace SortingGame.Core
{
    /// <summary>
    /// GDD 17 SectionProgress. Combines shelved items and cleaned dirt into one percentage.
    /// 100% is only reported when everything is truly done, never by rounding.
    /// </summary>
    public class SectionProgress
    {
        public int TotalItems { get; }
        public int PlacedItems { get; private set; }
        public bool HasDirt { get; }
        public float DirtCleaned { get; private set; }

        readonly float _dirtWeight;

        public event Action<SectionProgress> Changed;

        public SectionProgress(int totalItems, bool hasDirt, float dirtWeight)
        {
            if (totalItems < 0) throw new ArgumentOutOfRangeException(nameof(totalItems));
            TotalItems = totalItems;
            HasDirt = hasDirt;
            _dirtWeight = hasDirt ? Math.Clamp(dirtWeight, 0f, 1f) : 0f;
        }

        /// <summary>Collectibles still somewhere in the section. The room is not done until they are picked up too.</summary>
        public int CollectiblesRemaining { get; private set; }

        /// <summary>Everything sorted and swept, only collectibles are left to pick up.</summary>
        public bool OnlyCollectiblesLeft => SortedAndSwept && CollectiblesRemaining > 0;

        bool SortedAndSwept => PlacedItems >= TotalItems && (!HasDirt || DirtCleaned >= 1f);

        public bool IsComplete => SortedAndSwept && CollectiblesRemaining == 0;

        /// <summary>0..1</summary>
        public float Fraction
        {
            get
            {
                if (IsComplete) return 1f;
                var itemFraction = TotalItems == 0 ? 1f : (float)PlacedItems / TotalItems;
                var value = itemFraction * (1f - _dirtWeight) + DirtCleaned * _dirtWeight;
                return Math.Min(value, 0.999f);
            }
        }

        /// <summary>0..100, floored so 100 only appears when complete.</summary>
        public int Percent => IsComplete ? 100 : Math.Min(99, (int)Math.Floor(Fraction * 100f));

        public void SetCollectiblesRemaining(int count)
        {
            count = Math.Max(0, count);
            if (count == CollectiblesRemaining) return;
            CollectiblesRemaining = count;
            Changed?.Invoke(this);
        }

        public void AddPlaced()
        {
            if (PlacedItems >= TotalItems) return;
            PlacedItems++;
            Changed?.Invoke(this);
        }

        /// <summary>Loading a save: set state without raising Changed.</summary>
        public void Restore(int placedItems, float dirtCleaned)
        {
            PlacedItems = Math.Clamp(placedItems, 0, TotalItems);
            DirtCleaned = HasDirt ? Math.Clamp(dirtCleaned, 0f, 1f) : 0f;
        }

        public void SetDirtCleaned(float fraction)
        {
            if (!HasDirt) return;
            fraction = Math.Clamp(fraction, 0f, 1f);
            if (Math.Abs(fraction - DirtCleaned) < 0.0001f) return;
            DirtCleaned = fraction;
            Changed?.Invoke(this);
        }
    }
}
