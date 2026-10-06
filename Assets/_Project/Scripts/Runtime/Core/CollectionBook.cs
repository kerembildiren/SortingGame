using System;
using System.Collections.Generic;
using SortingGame.Data;

namespace SortingGame.Core
{
    /// <summary>
    /// GDD 9.1 / 9.3. First copy of a collectible goes into the book automatically (no "sell or keep" choice),
    /// later copies are sold automatically for their duplicate value.
    /// </summary>
    public class CollectionBook
    {
        public readonly struct FindResult
        {
            public readonly bool IsNew;
            public readonly int DuplicateCoins;

            public FindResult(bool isNew, int duplicateCoins)
            {
                IsNew = isNew;
                DuplicateCoins = duplicateCoins;
            }
        }

        readonly HashSet<string> _found = new();

        public event Action<CollectibleDefinition> Added;

        public IReadOnlyCollection<string> FoundIds => _found;

        public bool Has(CollectibleDefinition collectible) => collectible != null && _found.Contains(collectible.Id);

        public FindResult Register(CollectibleDefinition collectible)
        {
            if (collectible == null) throw new ArgumentNullException(nameof(collectible));
            if (_found.Add(collectible.Id))
            {
                Added?.Invoke(collectible);
                return new FindResult(true, 0);
            }
            return new FindResult(false, collectible.DuplicateSellValue);
        }

        public int CountFound(IEnumerable<CollectibleDefinition> page)
        {
            var count = 0;
            foreach (var c in page)
                if (Has(c)) count++;
            return count;
        }

        /// <summary>Used by the save system (M3).</summary>
        public void Restore(IEnumerable<string> ids)
        {
            _found.Clear();
            foreach (var id in ids) _found.Add(id);
        }
    }
}
