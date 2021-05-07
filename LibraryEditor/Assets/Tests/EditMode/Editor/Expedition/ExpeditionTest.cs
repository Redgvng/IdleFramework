using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using IdleLibrary;

public class ExpeditionTest
{
    [Test]
    public void ShouldStartWithoutCost()
    {
        var expedition = new Expedition(0, new Transaction(new NUMBER(), new NullCost()));
        Assert.IsTrue(expedition.CanStart());
    }
    [Test]
    public void ShouldStart()
    {
        var number = new NUMBER(100);
        var cost = new FixedCost(30);
        var transaction = new Transaction(number, cost);
        var expedition = new Expedition(0, transaction);
        Assert.IsTrue(expedition.CanStart());
    }
    [Test]
    public void ShouldIncreaseCurrentTime()
    {
        var expedition = new Expedition(0, new Transaction(new NUMBER(), new NullCost()));
        expedition.IncreaseCurrentTime(1 * 3600);
        Assert.IsTrue(expedition.CanClaim());
    }
    [Test]
    public void ShouldClaimAndCost()
    {
        var number = new NUMBER(100);
        var cost = new FixedCost(30);
        var transaction = new Transaction(number, cost);
        var expedition = new Expedition(0, transaction);
        expedition.StartExpedition();
        expedition.IncreaseCurrentTime(1 * 3600);
        Assert.IsTrue(expedition.CanClaim());
        Assert.AreEqual(70, number.Number);
    }    

    [Test]
    public void CanRewardSomething()
    {
        var gold = new NUMBER();
        var stone = new NUMBER();
        var reward = new NumberReward((gold, 100), (stone, 50));
        var expedition = new Expedition(0, null, reward);

        expedition.StartExpedition();
        expedition.IncreaseCurrentTime(1 * 3600);
        expedition.Claim();

        Assert.AreEqual(100, gold.Number);
        Assert.AreEqual(50, stone.Number);
    }
}
