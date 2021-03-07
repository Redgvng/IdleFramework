using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UpgradeLibrary;

namespace Tests
{
    public class UpgradeTests
    {
        [Test]
        public void ShouldLevelUp()
        {
            var level = new MockLevel();
            var gold = new NUMBER();
            ITransaction upgrade = new Upgrade(level, new NullTransaction());
            upgrade.Pay();
            Assert.AreEqual(1, level.level);
        }

        [Test]
        public void ShouldCost()
        {
            var level = new MockLevel();
            var gold = new NUMBER();
            ITransaction simpleTransaction = new Transaction(gold, new LinearCost(1, 2, level));
            ITransaction upgrade = new Upgrade(level, simpleTransaction);
            gold.Number = 10;
            upgrade.Pay();
            Assert.AreEqual(9, gold.Number);
            Assert.AreEqual(1, level.level);
        }

        [Test]
        public void ShouldCostEachResources()
        {
            var level = new MockLevel();
            var stone = new NUMBER(10);
            var crystal = new NUMBER(20);
            var leaf = new NUMBER(30);
            var gold = new NUMBER(40);
            var MultipleTransactoin = new MultipleTransaction(new Transaction[]
            {
                new Transaction(stone, new LinearCost(1,2,level)),
                new Transaction(crystal, new LinearCost(3,2,level)),
                new Transaction(leaf, new LinearCost(5,2,level)),
                new Transaction(gold, new LinearCost(1,2,level)),
            });
            ITransaction upgrade = new Upgrade(level, MultipleTransactoin);
            upgrade.Pay();
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
            ITransaction simpleTransaction = new Transaction(gold, new LinearCost(1, 2, level));
            ITransaction upgrade = new Upgrade(level, simpleTransaction);
            //Max化します。
            upgrade = new MaxUpgrade(upgrade);
            gold.Number = 10;
            upgrade.Pay();
            Assert.AreEqual(1, gold.Number);
            Assert.AreEqual(3, level.level);
        }
        [Test]
        public void ShouldDoMaxCostWithVeryBigNumber()
        {
            var level = new MockLevel();
            var gold = new NUMBER();
            ITransaction simpleTransaction = new Transaction(gold, new LinearCost(1, 2, level));
            ITransaction upgrade = new Upgrade(level, simpleTransaction);
            //Max化します。
            upgrade = new MaxUpgrade(upgrade);
            gold.Number = 1000;
            upgrade.Pay();
            Assert.AreEqual(39, gold.Number);
            Assert.AreEqual(31, level.level);
        }
    }
}
