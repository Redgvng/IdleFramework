
using NUnit.Framework;
using System;
using UnityEngine.TestTools;
using IdleLibrary.Upgrade;
using IdleLibrary;

namespace Tests
{
    public class LinearCostTest
    {
        [Test]
        public void CanSolveLevelAndCostAtMax()
        {
            ILevel level = new MockLevel();
            LinearCost cost = new LinearCost(1, 2, level);
            NUMBER gold = new NUMBER(9);
            //3個かえて、goldは1残るはず
            var unko = cost.MaxCostInfo(gold);
            Assert.AreEqual(3, unko.level);
            Assert.AreEqual(9, unko.cost);
        }
        [Test]
        public void CanSolveLevelAndCostAtMax2()
        {
            ILevel level = new MockLevel();
            level.level = 2;
            LinearCost cost = new LinearCost(1, 2, level);
            NUMBER gold = new NUMBER(13);
            //2個かえて、goldは1残るはず
            var unko = cost.MaxCostInfo(gold);
            Assert.AreEqual(4, unko.level);
            Assert.AreEqual(12, unko.cost);
        }
        [Test]
        public void CanSolveLevelAndCostAtMax3()
        {
            ILevel level = new MockLevel();
            level.level = 0;
            LinearCost cost = new LinearCost(2, 0, level);
            NUMBER gold = new NUMBER(100);
            //2個かえて、goldは1残るはず
            var unko = cost.MaxCostInfo(gold);
            //50回アップグレードできるはず
            Assert.AreEqual(50, unko.level);
            Assert.AreEqual(100, unko.cost);
        }
    }
}
