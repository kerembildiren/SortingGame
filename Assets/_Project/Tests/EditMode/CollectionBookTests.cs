using NUnit.Framework;
using SortingGame.Core;
using SortingGame.Data;
using UnityEngine;

namespace SortingGame.Tests
{
    public class CollectionBookTests
    {
        CollectibleDefinition _robot;
        CollectibleDefinition _chubby;

        [SetUp]
        public void SetUp()
        {
            _robot = ScriptableObject.CreateInstance<CollectibleDefinition>();
            _robot.Id = "golden_robot";
            _robot.DuplicateSellValue = 60;
            _chubby = ScriptableObject.CreateInstance<CollectibleDefinition>();
            _chubby.Id = "captain_chubby";
            _chubby.Rarity = ItemRarity.Mascot;
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_robot);
            Object.DestroyImmediate(_chubby);
        }

        [Test]
        public void FirstCopy_GoesToTheBook_WithoutCoins()
        {
            var book = new CollectionBook();
            CollectibleDefinition added = null;
            book.Added += c => added = c;

            var result = book.Register(_robot);

            Assert.IsTrue(result.IsNew);
            Assert.AreEqual(0, result.DuplicateCoins);
            Assert.IsTrue(book.Has(_robot));
            Assert.AreSame(_robot, added);
        }

        [Test]
        public void Duplicate_IsSoldForItsValue_AndBookUnchanged()
        {
            var book = new CollectionBook();
            book.Register(_robot);
            var addedCount = 0;
            book.Added += _ => addedCount++;

            var result = book.Register(_robot);

            Assert.IsFalse(result.IsNew);
            Assert.AreEqual(60, result.DuplicateCoins);
            Assert.AreEqual(0, addedCount);
            Assert.AreEqual(1, book.FoundIds.Count);
        }

        [Test]
        public void CountFound_CountsOnlyThePage()
        {
            var book = new CollectionBook();
            book.Register(_chubby);

            Assert.AreEqual(1, book.CountFound(new[] { _robot, _chubby }));
        }
    }
}
