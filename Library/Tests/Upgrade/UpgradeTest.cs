using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using IdleLibrary.Upgrade;
using IdleLibrary;

public class UpgradeTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void SimpleUpgradeTest()
    {
        var level = new Level(2);
        var number = new NUMBER();
        number.Increment(100);
        var cost = new ExponentialCost(1, 2, level);
        var upgrade = new Upgrade(level, number, cost);

        upgrade.Pay();

        Assert.AreEqual(99, number.Number);
        Assert.AreEqual(1, level.level);

        upgrade.Pay();

        Assert.AreEqual(97, number.Number);
        Assert.AreEqual(2, level.level);

        upgrade.Pay();

        Assert.IsFalse(level.level == 3);
    }

    class DebugNumber
    {
        public NUMBER resource1 = new NUMBER();
        public NUMBER resource2 = new NUMBER();
        public (IDecrementableNumber number, IMaxableCost cost)[] resources;

        public void Initialize()
        {
            resource1.Increment(300);
            resource2.Increment(100);

            resources = new (IDecrementableNumber number, IMaxableCost cost)[]
            {
            (resource1, new LinearCost(30, 30, level)),
            (resource2, new LinearCost(3, 2, level)),
            };
        }
        ILevel level;
        public DebugNumber(ILevel level)
        {
            this.level = level;
            Initialize();
        }
    }

    [Test]
    public void MultipleResourceUpgradeTest()
    {
        var level = new Level(2);
        var debug = new DebugNumber(level);

        var upgrade = new MultipleUpgrade(level, debug.resources);

        upgrade.Pay();

        Assert.AreEqual(270, debug.resource1.Number);
        Assert.AreEqual(97, debug.resource2.Number);
        Assert.AreEqual(1, level.level);

        upgrade.Pay();
        Assert.AreEqual(210, debug.resource1.Number);
        Assert.AreEqual(92, debug.resource2.Number);
        Assert.AreEqual(2, level.level);

        upgrade.Pay();
        Debug.Log(level.level);
        Assert.IsFalse(level.level == 3);
    }

    [Test]
    public void MultipleMaxUpgradeText()
    {
        var level = new Level();
        var debug = new DebugNumber(level);
        var upgrade = new MultipleUpgrade(level, debug.resources);

        upgrade.MaxPay();
        Assert.IsTrue(level.level == 4);
    }

    [Test]
    public void MultipleFixnumUpgradeTest()
    {
        var level = new Level();
        var debug = new DebugNumber(level);
        var upgrade = new MultipleUpgrade(level, debug.resources);

        upgrade.FixedAmountPay(3);
        Assert.IsTrue(level.level == 3);
    }
}
