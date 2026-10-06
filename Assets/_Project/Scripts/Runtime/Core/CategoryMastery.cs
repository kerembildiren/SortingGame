using System;
using System.Collections.Generic;
using SortingGame.Data;

namespace SortingGame.Core
{
    /// <summary>
    /// GDD 10.2, the signature mechanic: place enough items of a category correctly and that category
    /// sorts itself from then on, in every venue. Single tier for now (tiers are [AÇIK] in the GDD).
    /// Collectibles are never affected (principle 6).
    /// </summary>
    public class CategoryMastery
    {
        readonly Dictionary<string, int> _counts = new();

        public event Action<CategoryDefinition> Mastered;

        public int CountOf(CategoryDefinition category) =>
            category != null && _counts.TryGetValue(category.Id, out var count) ? count : 0;

        public bool IsMastered(CategoryDefinition category) =>
            category != null && category.MasteryThreshold > 0 && CountOf(category) >= category.MasteryThreshold;

        /// <summary>0..1 towards mastery.</summary>
        public float ProgressOf(CategoryDefinition category) =>
            category == null || category.MasteryThreshold <= 0 ? 0f : Math.Min(1f, (float)CountOf(category) / category.MasteryThreshold);

        /// <summary>Counts one correct placement. Returns true exactly once: when this placement masters the category.</summary>
        public bool RecordPlacement(CategoryDefinition category)
        {
            if (category == null) return false;
            var wasMastered = IsMastered(category);
            _counts[category.Id] = CountOf(category) + 1;
            if (wasMastered || !IsMastered(category)) return false;
            Mastered?.Invoke(category);
            return true;
        }

        public IEnumerable<KeyValuePair<string, int>> Export() => _counts;

        public void Restore(IEnumerable<KeyValuePair<string, int>> counts)
        {
            _counts.Clear();
            foreach (var pair in counts) _counts[pair.Key] = pair.Value;
        }
    }
}
