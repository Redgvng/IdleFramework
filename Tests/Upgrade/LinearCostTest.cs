
using NUnit.Framework;
using System;
using UnityEngine.TestTools;
using IdleLibrary.Upgrade;
using IdleLibrary;

namespace Tests.Cost
{
    public class FixedCostTest
    {
        [Test]
        public void CanSolveLevelAndCostAtMax()
        {
            var level = new MockLevel();
            var cost = new FixedCost(10);
            var gold = new NUMBER();
            gold.Number = 95;

            Assert.IsTrue(false);
        }
    }
    public class LinearCostTest
    {
        [Test]
        public void CanSolveLevelAndCostAtMax()
        {
            ILevel level = new MockLevel();
            LinearCost cost = new LinearCost(1, 2, level);
            NUMBER gold = new NUMBER();
            gold.Number = 9;
            //3個かえて、goldは1残るはず
            var l = cost.LevelAtMaxCost(gold);
            var c = cost.MaxCost(gold);
            Assert.AreEqual(3, l);
            Assert.AreEqual(9, c);
        }
        [Test]
        public void CanSolveLevelAndCostAtMax2()
        {
            ILevel level = new MockLevel();
            level.level = 2;
            LinearCost cost = new LinearCost(1, 2, level);
            NUMBER gold = new NUMBER();
            gold.Number = 13;
            //2個かえて、goldは1残るはず
            var l = cost.LevelAtMaxCost(gold);
            var c = cost.MaxCost(gold);
            Assert.AreEqual(4, l);
            Assert.AreEqual(12, c);
        }
        [Test]
        public void CanSolveLevelAndCostAtMax3()
        {
            ILevel level = new MockLevel();
            level.level = 0;
            LinearCost cost = new LinearCost(2, 0, level);
            NUMBER gold = new NUMBER();
            gold.Number = 100;
            //2個かえて、goldは1残るはず
            var l = cost.LevelAtMaxCost(gold);
            var c = cost.MaxCost(gold);
            //50回アップグレードできるはず
            Assert.AreEqual(50, l);
            Assert.AreEqual(100, c);
        }
    }

    public class ExponentialCostTest
    {
        [Test]
        public void CanSolveLevelAndCostAtMax()
        {
            ILevel level = new MockLevel();
            ExponentialCost cost = new ExponentialCost(1, 1.5, level);
            NUMBER gold = new NUMBER();
            gold.Number = 5;
            //3個かえて、goldは1残るはず
            var l = cost.LevelAtMaxCost(gold);
            var c = cost.MaxCost(gold);
            Assert.AreEqual(3, l);
            Assert.AreEqual(4.75, c);
        }
    }
}
