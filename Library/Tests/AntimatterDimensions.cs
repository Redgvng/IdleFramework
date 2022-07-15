using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using System.Linq;
using IdleLibrary;
using IdleLibrary.Upgrade;
using IdleLibrary.IntegrationTest;
using System;
using System.IO;
using static IdleLibrary.IntegrationTest.IntegrationUtiliity;


public class AntimatterDimensionsTest : MonoBehaviour
{
    NUMBER antimatter = new NUMBER();
    NUMBER[] dimensions;
    Upgrade tickUpgrade;
    Level tickUpgradeLevel = new Level();
    Upgrade[] upgrades = new Upgrade[8];
    List<Upgrade> unlockedUpgrades = new List<Upgrade>();
    AntimatterCost[] costs;
    Level[] upgradeLevels;

    private void UpdatePerSecond()
    {
        antimatter.ProducePerTime(1);
        foreach (var dimension in dimensions)
        {
            dimension.ProducePerTime(1);
        }
        AutoBuy(unlockedUpgrades);
        AutoBuy(new Upgrade[] { tickUpgrade});
        DimensionShift();
    }
    [SetUp]
    public void Initialize()
    {
        upgradeLevels = Enumerable.Range(0, 8).Select(_ => new Level()).ToArray();
        dimensions = Enumerable.Range(0, 8).Select(_ => _ == 0 ? new NUMBER() : new NUMBER()).ToArray();
        antimatter.Increment(10);
        costs = new AntimatterCost[]
        {
            new AntimatterCost(10, 10, 1E3, upgradeLevels[0]),
            new AntimatterCost(100, 10, 1E4, upgradeLevels[1]),
            new AntimatterCost(1E4, 10, 1E5, upgradeLevels[2]),
            new AntimatterCost(1E6, 10, 1E6, upgradeLevels[3]),
            new AntimatterCost(1E9, 10, 1E8, upgradeLevels[4]),
            new AntimatterCost(1E13, 10, 1E10, upgradeLevels[5]),
            new AntimatterCost(1E18, 10, 1E12, upgradeLevels[6]),
            new AntimatterCost(1E24, 10, 1E15, upgradeLevels[7]),
        };
        for (int i = 0; i < upgradeLevels.Length; i++)
        {
            upgrades[i] = new Upgrade(upgradeLevels[i], antimatter, costs[i]);
        }
        tickUpgrade = new Upgrade(tickUpgradeLevel, antimatter, new ExponentialCost(10, 10, tickUpgradeLevel));
        for (int i = 0; i < upgrades.Length; i++)
        {
            var count = i;
            //upgrades[count].ApplyEffect(dimensions[count].produceMultiplier, (level) => level, MultiplierType.add);
            upgrades[count].OnUpgraded += () => dimensions[count].Increment(1);
            dimensions[count].produceMultiplier.RegisterMultiplier(new MultiplierInfo(() => Math.Pow(2, costs[count].currentTier), MultiplierType.mul));
            dimensions[count].produceMultiplier.RegisterMultiplier(new MultiplierInfo(() => Math.Pow(1.1, tickUpgrade.level), MultiplierType.mul));
        }
        antimatter.produceMultiplier.RegisterMultiplier(new MultiplierInfo(() => dimensions[0].Number * Math.Pow(1.1, tickUpgrade.level), MultiplierType.add));
        for (int i = 0; i < dimensions.Length-1; i++)
        {
            var count = i;
            dimensions[count].produceMultiplier.RegisterMultiplier(new MultiplierInfo(() => dimensions[count + 1].Number / 10, MultiplierType.add));
        }
        unlockedUpgrades.Add(upgrades[0]);
        unlockedUpgrades.Add(upgrades[1]);
        unlockedUpgrades.Add(upgrades[2]);
        unlockedUpgrades.Add(upgrades[3]);
    }

    private int dimensionShiftNum;
    private void DimensionShift()
    {
        switch (dimensionShiftNum)
        {
            case 0:
                if (upgrades[3].level < 20) return;
                unlockedUpgrades.Add(upgrades[4]); break;
            case 1:
                if (upgrades[4].level < 20) return;
                unlockedUpgrades.Add(upgrades[5]); break;
            case 2:
                if (upgrades[5].level < 20) return;
                unlockedUpgrades.Add(upgrades[6]); break;
            case 3:
                if (upgrades[6].level < 20) return;
                unlockedUpgrades.Add(upgrades[7]); break;
            default: return;
        }
        Debug.Log("Shiftしたよ");
        dimensionShiftNum++;
        Reset();
    }
    private void Reset()
    {
        foreach (var dimension in dimensions)
        {
            dimension.ResetNumberToZero();
        }
        tickUpgradeLevel.level = 0;
        foreach (var level in upgradeLevels)
        {
            level.level = 0;
        }
    }

    [Test]
    public void Integration()
    {
        ExportExcel.OutPutHeader("Antimatter", new string[] {"Time", "APS", "Antimatter"}
        .Union(Enumerable.Range(0, 8).Select(index => $"Dimension:{index}")).ToArray());
        for (int i = 0; i < 3600 * 6; i++)
        {
            UpdatePerSecond();
            if((i + 60)% 60 == 0)
            {
                ExportExcel.OutPutData("Antimatter", new string[] {i.ToString(),antimatter.ProduceAmountPerSecond().ToString("F0"),$"{antimatter.Number.ToString("F0")}"}
                .Union(dimensions.Select(dimension => dimension.Number.ToString("F0"))).ToArray());
            }
        }
    }

    [Test]
    public void AntimatterCostTest()
    {
        var level = new Level();
        var number = new NUMBER();
        var upgrade = new Upgrade(level, number, new AntimatterCost(10, 10, 1000, level));

        Assert.AreEqual(10, upgrade.cost.Cost);
        Assert.AreEqual(0, upgrade.cost.LevelAtMaxCost(number));

        number.Increment(90);
        upgrade.Pay();

        Assert.AreEqual(10, upgrade.cost.Cost);
        Assert.AreEqual(9, upgrade.cost.LevelAtMaxCost(number));
        Assert.AreEqual(80, number.Number);

        upgrade.MaxPay();

        Assert.AreEqual(9, upgrade.level);

        number.Increment(1E10);
        upgrade.MaxPay();

        Assert.AreEqual(10, upgrade.level);

        upgrade.MaxPay();
        Assert.AreEqual(20, upgrade.level);
    }
}

