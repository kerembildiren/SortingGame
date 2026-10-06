using NUnit.Framework;
using SortingGame.Core;
using SortingGame.Data;
using UnityEngine;

namespace SortingGame.Tests
{
    public class CollectionBookTests
    {
        CollectibleDefinition _captain;
        CollectibleDefinition _mechanic;

        [SetUp]
        public void SetUp()
        {
            _captain = ScriptableObject.CreateInstance<CollectibleDefinition>();
            _captain.Id = "captain_chubby";
            _captain.Rarity = ItemRarity.Mascot;
            _mechanic = ScriptableObject.CreateInstance<CollectibleDefinition>();
            _mechanic.Id = "mechanic_chubby";
            _mechanic.Rarity = ItemRarity.Mascot;
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_captain);
            Object.DestroyImmediate(_mechanic);
        }

        [Test]
        public void FoundChubby_GoesToTheBook()
        {
            var book = new CollectionBook();
            CollectibleDefinition added = null;
            book.Added += c => added = c;

            Assert.IsTrue(book.Register(_captain));

            Assert.IsTrue(book.Has(_captain));
            Assert.AreSame(_captain, added);
        }

        [Test]
        public void SameChubby_IsNeverAddedTwice()
        {
            var book = new CollectionBook();
            book.Register(_captain);
            var addedCount = 0;
            book.Added += _ => addedCount++;

            Assert.IsFalse(book.Register(_captain));

            Assert.AreEqual(0, addedCount);
            Assert.AreEqual(1, book.FoundIds.Count);
        }

        [Test]
        public void CountFound_CountsTheAlbumEntries()
        {
            var book = new CollectionBook();
            book.Register(_mechanic);

            Assert.AreEqual(1, book.CountFound(new[] { _captain, _mechanic }));
        }

        [Test]
        public void OnlyMascots_AreCollectibles_RareItemsGoOnShelves()
        {
            var rare = ScriptableObject.CreateInstance<ItemDefinition>();
            rare.Rarity = ItemRarity.Rare;

            Assert.IsTrue(_captain.IsCollectible);
            Assert.IsFalse(rare.IsCollectible);
            Assert.IsTrue(rare.IsRare);
            Object.DestroyImmediate(rare);
        }
    }
}
