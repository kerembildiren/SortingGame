using System;
using System.Collections.Generic;
using SortingGame.Data;

namespace SortingGame.Core
{
    /// <summary>
    /// GDD 9.1 / 9.3. A found Chubby goes into the book automatically (no "sell or keep" choice).
    /// There are no duplicates: a collectible that is in the book never spawns again.
    /// </summary>
    public class CollectionBook
    {
        readonly HashSet<string> _found = new();

        public event Action<CollectibleDefinition> Added;

        public IReadOnlyCollection<string> FoundIds => _found;

        public bool Has(CollectibleDefinition collectible) => collectible != null && _found.Contains(collectible.Id);

        /// <summary>Returns false when it was already in the book.</summary>
        public bool Register(CollectibleDefinition collectible)
        {
            if (collectible == null) throw new ArgumentNullException(nameof(collectible));
            if (!_found.Add(collectible.Id)) return false;
            Added?.Invoke(collectible);
            return true;
        }

        public int CountFound(IEnumerable<CollectibleDefinition> entries)
        {
            var count = 0;
            foreach (var c in entries)
                if (Has(c)) count++;
            return count;
        }

        /// <summary>Used by the save system. Ids that are no longer collectibles are dropped by the caller.</summary>
        public void Restore(IEnumerable<string> ids)
        {
            _found.Clear();
            foreach (var id in ids) _found.Add(id);
        }
    }
}
