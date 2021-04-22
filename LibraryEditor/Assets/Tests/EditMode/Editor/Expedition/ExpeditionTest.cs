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
        var expedition = new Expedition(new Transaction(new NUMBER(), new NullCost()), 1);
        Assert.IsTrue(expedition.CanStart());
    }
    [Test]
    public void ShouldStart()
    {
        var number = new NUMBER(100);
        var cost = new FixedCost(30);
        var transaction = new Transaction(number, cost);
        var expedition = new Expedition(transaction, 1);
        Assert.IsTrue(expedition.CanStart());
    }
    [Test]
    public void ShouldIncreaseCurrentTime()
    {
        var expedition = new Expedition(new Transaction(new NUMBER(), new NullCost()), 1);
        expedition.IncreaseCurrentTime(1 * 3600);
        Assert.IsTrue(expedition.CanClaim());
    }
    [Test]
    public void ShouldClaimAndCost()
    {
        var number = new NUMBER(100);
        var cost = new FixedCost(30);
        var transaction = new Transaction(number, cost);
        var expedition = new Expedition(transaction, 1);
        expedition.StartExpedition();
        expedition.IncreaseCurrentTime(1 * 3600);
        Assert.IsTrue(expedition.CanClaim());
        Assert.AreEqual(70, number.Number);
    }    
}
