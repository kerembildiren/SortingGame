using NUnit.Framework;
using SortingGame.Data;
using UnityEditor;
using UnityEngine;

namespace SortingGame.Tests
{
    public class GameDatabaseTests
    {
        const string DatabasePath = "Assets/_Project/Data/GameDatabase.asset";

        [Test]
        public void ProjectDatabase_Exists_AndIsValid()
        {
            var database = AssetDatabase.LoadAssetAtPath<GameDatabase>(DatabasePath);
            Assert.IsNotNull(database, $"Missing {DatabasePath}. Run 'Sorting Game/Setup/Run Full Setup'.");

            var errors = database.Validate();
            Assert.IsEmpty(errors, string.Join("\n", errors));
        }

        [Test]
        public void Validate_ReportsDuplicateIds()
        {
            var database = ScriptableObject.CreateInstance<GameDatabase>();
            var a = ScriptableObject.CreateInstance<CategoryDefinition>();
            var b = ScriptableObject.CreateInstance<CategoryDefinition>();
            a.Id = b.Id = "comics";
            database.Categories.Add(a);
            database.Categories.Add(b);

            var errors = database.Validate();

            Assert.That(errors, Has.Some.Contains("Duplicate Category id 'comics'"));
            Object.DestroyImmediate(a);
            Object.DestroyImmediate(b);
            Object.DestroyImmediate(database);
        }

        [Test]
        public void Validate_ReportsCommonItemWithoutCategory()
        {
            var database = ScriptableObject.CreateInstance<GameDatabase>();
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.Id = "orphan";
            database.Items.Add(item);

            var errors = database.Validate();

            Assert.That(errors, Has.Some.Contains("'orphan' has no category"));
            Object.DestroyImmediate(item);
            Object.DestroyImmediate(database);
        }
    }
}
