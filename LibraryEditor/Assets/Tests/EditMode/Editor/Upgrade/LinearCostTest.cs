/*
using NUnit.Framework;
using System;
using UnityEngine.TestTools;
using UpgradeLibrary;

[TestFixture]
public class LinearCostTest : IPrebuildSetup
{
    Upgrade upgrade;
    ICost cost;
    INumber money;
    INumber dummy;

    [OneTimeSetUp]
    public void Setup()
    {
        dummy = new DummyNumber(30);
        cost = new LinearCost(3, 5, money);
        int[] level = new int[Enum.GetValues(typeof(ID)).Length];
        UpgradeFactory upgradeFactory = new UpgradeFactory(level);
        upgrade = upgradeFactory.Create(ID.sample1, new LinearCost(3, 5, dummy))
                                .SetValueAsMultiplier(dummy, (x) => x * 3);
    }

    [Test]
    public void UpgradeLevelUpTestByMock()
    {
        int level = upgrade.level;
        upgrade.Purchase();
        Assert.AreEqual(level + 1, upgrade.level);
    }

    [Test]
    public void LinearMaxNumIsRight()
    {
        ILevel dummy = new DummyLevel(1);
        cost = new LinearCost(3, 5, new Money(30));
        cost = new CostInMax(cost);
        Assert.AreEqual(3,cost.BuyNum(dummy));
    }
    [Test]
    public void LinearMaxNumIsRight2()
    {
        cost = new LinearCost(0, 5, new Money(30));
        cost = new CostInMax(cost);
        ILevel dummy = new DummyLevel(1);
        Assert.AreEqual(4, cost.BuyNum(dummy));
    }

    [Test]
    public void LinearMaxNumIsRightBequalZero()
    {
        cost = new LinearCost(3, 0, new Money(30));
        cost = new CostInMax(cost);
        ILevel dummy = new DummyLevel(1);
        Assert.AreEqual(10, cost.BuyNum(dummy));
    }

    //次の3つのテストはレベル5スタートでMAXを試すものです。
    [Test]
    public void LinearMaxNumIsRightLevel5()
    {
        cost = new LinearCost(3, 5, new Money(120));
        cost = new CostInMax(cost);
        ILevel dummy = new DummyLevel(5);
        Assert.AreEqual(3, cost.BuyNum(dummy));
    }
    [Test]
    public void LinearMaxNumIsRight2Level5()
    {
        cost = new LinearCost(0, 5, new Money(120));
        cost = new CostInMax(cost);
        ILevel dummy = new DummyLevel(5);
        Assert.AreEqual(4, cost.BuyNum(dummy));
    }

    [Test]
    public void LinearMaxNumIsRightBequalZeroLevel5()
    {
        cost = new LinearCost(3, 0, new Money(120));
        cost = new CostInMax(cost);
        ILevel dummy = new DummyLevel(5);
        Assert.AreEqual(40, cost.BuyNum(dummy));
    }
}
*/