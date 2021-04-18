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
            var stone = new NUMBER(10);
            var crystal = new NUMBER(20);
            var leaf = new NUMBER(30);
            var gold = new NUMBER(40);
            var multipleUpgrade = new MultipleUpgrade(level,
                new (NUMBER, ICost)[]
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

        /*
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
        [Test]
        public void ShouldDoFixedNumberCost()
        {
            var level = new MockLevel();
            var gold = new NUMBER();
            ITransaction simpleTransaction = new Transaction(gold, new LinearCost(1, 2, level));
            ITransaction upgrade = new Upgrade(level, simpleTransaction);
            //Max化します。
            upgrade = new FixedNumberUpgrade(upgrade, 10);
            gold.Number = 1000;
            upgrade.Pay();
            Assert.AreEqual(10, level.level);
        }
        [Test]
        public void ShouldDoFixedNumberCostWhenNotEnoughCase()
        {
            var level = new MockLevel();
            var gold = new NUMBER();
            ITransaction simpleTransaction = new Transaction(gold, new LinearCost(1, 2, level));
            ITransaction upgrade = new Upgrade(level, simpleTransaction);
            //Max化します。
            upgrade = new FixedNumberUpgrade(upgrade, 10);
            gold.Number = 10;
            upgrade.Pay();
            Assert.AreEqual(3, level.level);
        }

        [Test]
        public void CanShowMaxCostEffectivelyWhenUsingLinear()
        {
            var level = new MockLevel();
            var gold = new NUMBER();
            ICost cost = new LinearCost(1, 2, level);
            ITransaction simpleTransaction = new Transaction(gold, cost);
            //この時点で...Maxのコストは出せる？ICostがeffectiveだったらeffectiveにだす。とか
            //ICost MaxCostClass = new MaxCostClass(NUMBER, ICost)

            ITransaction upgrade = new Upgrade(level, simpleTransaction);
            //Max化します。
            upgrade = new MaxUpgrade(upgrade);
            gold.Number = 10;
            upgrade.Pay();
            Assert.AreEqual(3, level.level);
        }
        */
    }
}
