using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SortingGame.Core;
using SortingGame.Data;
using UnityEngine;

namespace SortingGame.Tests
{
    public class HelperProgressTests
    {
        readonly List<Object> _created = new();

        [TearDown]
        public void TearDown()
        {
            foreach (var o in _created) Object.DestroyImmediate(o);
            _created.Clear();
        }

        HelperDefinition MakeHelper(string id, params int[] costs)
        {
            var helper = ScriptableObject.CreateInstance<HelperDefinition>();
            _created.Add(helper);
            helper.Id = id;
            for (var i = 0; i < costs.Length; i++)
                helper.Levels.Add(new HelperDefinition.Level { Cost = costs[i], Speed = 0.9f + 0.2f * i, Capacity = i + 1 });
            return helper;
        }

        [Test]
        public void Hiring_NeedsAnOpenSlot_AndCoins()
        {
            var pip = MakeHelper("pip", 400, 700);
            var progress = new HelperProgress();
            var hired = 0;
            progress.Changed += (_, _) => hired++;

            var rich = new Wallet(1000);
            Assert.IsFalse(progress.TryUpgrade(pip, rich, false), "Coins alone do not hire: the slot follows venue progress.");
            Assert.AreEqual(1000, rich.Coins);
            Assert.IsFalse(progress.TryUpgrade(pip, new Wallet(100), true), "An open slot alone does not hire either.");
            Assert.IsFalse(progress.IsHired(pip));

            Assert.IsTrue(progress.TryUpgrade(pip, rich, true));
            Assert.IsTrue(progress.IsHired(pip));
            Assert.AreEqual(600, rich.Coins);
            Assert.AreEqual(1, hired);
        }

        [Test]
        public void Upgrades_RaiseSpeedAndCapacity_AndStopAtMax()
        {
            var pip = MakeHelper("pip", 400, 700, 1200);
            var progress = new HelperProgress();
            var wallet = new Wallet(5000);

            Assert.AreEqual(1, progress.Stats(pip).Capacity, "Not hired: level 1 stats, for the Shop preview.");
            progress.TryUpgrade(pip, wallet, true);
            Assert.AreEqual(700, progress.NextCost(pip));

            // Once hired, the slot is not asked again.
            Assert.IsTrue(progress.TryUpgrade(pip, wallet, false));
            Assert.IsTrue(progress.TryUpgrade(pip, wallet, false));
            Assert.AreEqual(3, progress.Stats(pip).Capacity);
            Assert.Greater(progress.Stats(pip).Speed, pip.Stats(1).Speed);

            Assert.IsTrue(progress.IsMaxed(pip));
            Assert.AreEqual(-1, progress.NextCost(pip));
            Assert.IsFalse(progress.TryUpgrade(pip, wallet, true));
            Assert.AreEqual(5000 - 400 - 700 - 1200, wallet.Coins);
        }

        [Test]
        public void Levels_AreRestorable()
        {
            var pip = MakeHelper("pip", 400, 700);
            var dot = MakeHelper("dot", 900, 1300);
            var progress = new HelperProgress();
            progress.TryUpgrade(pip, new Wallet(5000), true);

            var restored = new HelperProgress();
            restored.Restore(progress.Export().ToList());

            Assert.AreEqual(1, restored.LevelOf(pip));
            Assert.IsFalse(restored.IsHired(dot));
        }
    }
}
