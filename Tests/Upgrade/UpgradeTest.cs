using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using IdleLibrary.Upgrade;
using IdleLibrary;

namespace Tests
{
    public class UpgradeTests
    {
        [Test]
        public void ShouldLevelUp()
        {
            var level = new MockLevel();
            var gold = new NUMBER();
            var upgrade = new Upgrade(level, gold, new NullCost());
            upgrade.Pay();
            Assert.AreEqual(1, level.level);
        }

        [Test]
        public void ShouldCost()
        {
            var level = new MockLevel();
            var gold = new NUMBER();
            var cost = new LinearCost(1, 2, level);
            var upgrade = new Upgrade(level, gold, cost);
            gold.Number = 10;
            upgrade.Pay();
            Assert.AreEqual(9, gold.Number);
            Assert.AreEqual(1, level.level);
        }

        [Test]
        public void ShouldCostEachResources()
        {
            var level = new MockLevel();
            var stone = new NUMBER();
            var crystal = new NUMBER();
            var leaf = new NUMBER();
            var gold = new NUMBER();
            stone.Number = 10;
            crystal.Number = 20;
            leaf.Number = 30;
            gold.Number = 40;
            var multipleUpgrade = new MultipleUpgrade(level,
                new (INumber, IMaxableCost)[]
                {
                    (stone, new LinearCost(1,2,level)),
                    (crystal, new LinearCost(3,2,level)),
                    (leaf, new LinearCost(5,2,level)),
                    (gold, new LinearCost(1,2,level)),
                }
                );
            multipleUpgrade.Pay();
            Assert.AreEqual(9, stone.Number);
            Assert.AreEqual(17, crystal.Number);
            Assert.AreEqual(25, leaf.Number);
            Assert.AreEqual(39, gold.Number);      
        }

        [Test]
        public void ShouldDoMaxCost()
        {
            var level = new MockLevel();
            var gold = new NUMBER();
            var upgrade = new Upgrade(level, gold, new LinearCost(1, 2, level));
            gold.Number = 10;
            upgrade.MaxPay();
            Assert.AreEqual(1, gold.Number);
            Assert.AreEqual(3, level.level);
        }
        [Test]
        public void ShouldDoMaCostForMultipleUpgrade()
        {
            var level = new MockLevel();
            var gold = new NUMBER();
            var upgrade = new MultipleUpgrade(level, (gold, new LinearCost(1, 2, level)));
            gold.Number = 10;
            upgrade.MaxPay();
            Assert.AreEqual(1, gold.Number);
            Assert.AreEqual(3, level.level);
        }
        [Test]
        public void ShouldDoMaxCostWithVeryBigNumber()
        {
            var level = new MockLevel();
            var gold = new NUMBER();
            var upgrade = new Upgrade(level, gold, new LinearCost(1, 2, level));
            gold.Number = 1000;
            upgrade.MaxPay();
            Assert.AreEqual(39, gold.Number);
            Assert.AreEqual(31, level.level);
        }
        [Test]
        public void ShouldDoFixedNumberCost()
        {
            var level = new MockLevel();
            var gold = new NUMBER();
            var upgrade = new Upgrade(level, gold, new LinearCost(1, 2, level));
            gold.Number = 1000;
            upgrade.FixedAmountPay(10);
            Assert.AreEqual(10, level.level);
        }
        [Test]
        public void ShouldDoFixedNumberCostWhenNotEnoughCase()
        {
            var level = new MockLevel();
            var gold = new NUMBER();
            var upgrade = new Upgrade(level, gold, new LinearCost(1, 2, level));
            gold.Number = 10;
            upgrade.FixedAmountPay(10);
            Assert.AreEqual(3, level.level);
        }

    }
}
