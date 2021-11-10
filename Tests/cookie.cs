using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System;

using System.Linq;
using IdleLibrary;
using IdleLibrary.Upgrade;

public class UpgradeFactory
{
    public Upgrade GetUpgradeById(int id) => upgrades[id];

    private Upgrade[] upgrades;
    private Level[] upgradeLevels;
    private NUMBER cookie;
    public UpgradeFactory(Level[] upgradeLevels, NUMBER cookie)
    {
        this.upgradeLevels = upgradeLevels;
        this.cookie = cookie;
        upgrades = new Upgrade[]
        {
            new Upgrade(upgradeLevels[0], cookie, new ExponentialCost(15,1.15, upgradeLevels[0])),
            new Upgrade(upgradeLevels[1], cookie, new ExponentialCost(100,1.15, upgradeLevels[1])),
            new Upgrade(upgradeLevels[2], cookie, new ExponentialCost(1100,1.15, upgradeLevels[2])),
            new Upgrade(upgradeLevels[3], cookie, new ExponentialCost(12000,1.15, upgradeLevels[3])),
            new Upgrade(upgradeLevels[4], cookie, new ExponentialCost(130000,1.15, upgradeLevels[4])),
            new Upgrade(upgradeLevels[5], cookie, new ExponentialCost(1400000,1.15, upgradeLevels[5])),
            new Upgrade(upgradeLevels[6], cookie, new ExponentialCost(20000000,1.15, upgradeLevels[6])),
            new Upgrade(upgradeLevels[7], cookie, new ExponentialCost(330*1E6,1.15, upgradeLevels[7])),
            new Upgrade(upgradeLevels[8], cookie, new ExponentialCost(5.1*1E9,1.15, upgradeLevels[8])),
            new Upgrade(upgradeLevels[9], cookie, new ExponentialCost(75 * 1E9,1.15, upgradeLevels[9])),
            new Upgrade(upgradeLevels[10], cookie, new ExponentialCost(1*1E12,1.15, upgradeLevels[10])),
            new Upgrade(upgradeLevels[11], cookie, new ExponentialCost(14 * 1E12,1.15, upgradeLevels[11])),
            new Upgrade(upgradeLevels[12], cookie, new ExponentialCost(170 * 1E12,1.15, upgradeLevels[12])),
            new Upgrade(upgradeLevels[13], cookie, new ExponentialCost(2.1 * 1E15,1.15, upgradeLevels[13])),
            new Upgrade(upgradeLevels[14], cookie, new ExponentialCost(26 * 1E15,1.15, upgradeLevels[14])),
            new Upgrade(upgradeLevels[15], cookie, new ExponentialCost(310 * 1E15,1.15, upgradeLevels[15])),
            new Upgrade(upgradeLevels[16], cookie, new ExponentialCost(71 * 1E18,1.15, upgradeLevels[16])),
            new Upgrade(upgradeLevels[17], cookie, new ExponentialCost(12 * 1E21 ,1.15, upgradeLevels[17])),
        };
    }
}
public class CookieIntegration
{
    private NUMBER cookie = new NUMBER();
    private NUMBER prestigePoint = new NUMBER();
    private Upgrade[] upgrades;
    private Upgrade[] prestigeUpgrades;
    private Upgrade[] onetimeUpgrades;
    private Level[] upgradeLevels = new Level[18];
    private Level[] prestigeLevels = new Level[1];
    private long[] maxLevelReached = new long[18];


    private long prestigeNum;
    // A Test behaves as an ordinary method
    [Test]
    public void CookieIntegrationTest()
    {
        upgradeLevels = upgradeLevels.Select(_ => new Level()).ToArray();
        prestigeLevels = prestigeLevels.Select(_ => new Level()).ToArray();
        upgrades = new Upgrade[]
        {
            new Upgrade(upgradeLevels[0], cookie, new ExponentialCost(15,1.15, upgradeLevels[0])),
            new Upgrade(upgradeLevels[1], cookie, new ExponentialCost(100,1.15, upgradeLevels[1])),
            new Upgrade(upgradeLevels[2], cookie, new ExponentialCost(1100,1.15, upgradeLevels[2])),
            new Upgrade(upgradeLevels[3], cookie, new ExponentialCost(12000,1.15, upgradeLevels[3])),
            new Upgrade(upgradeLevels[4], cookie, new ExponentialCost(130000,1.15, upgradeLevels[4])),
            new Upgrade(upgradeLevels[5], cookie, new ExponentialCost(1400000,1.15, upgradeLevels[5])),
            new Upgrade(upgradeLevels[6], cookie, new ExponentialCost(20000000,1.15, upgradeLevels[6])),
            new Upgrade(upgradeLevels[7], cookie, new ExponentialCost(330*1E6,1.15, upgradeLevels[7])),
            new Upgrade(upgradeLevels[8], cookie, new ExponentialCost(5.1*1E9,1.15, upgradeLevels[8])),
            new Upgrade(upgradeLevels[9], cookie, new ExponentialCost(75 * 1E9,1.15, upgradeLevels[9])),
            new Upgrade(upgradeLevels[10], cookie, new ExponentialCost(1*1E12,1.15, upgradeLevels[10])),
            new Upgrade(upgradeLevels[11], cookie, new ExponentialCost(14 * 1E12,1.15, upgradeLevels[11])),
            new Upgrade(upgradeLevels[12], cookie, new ExponentialCost(170 * 1E12,1.15, upgradeLevels[12])),
            new Upgrade(upgradeLevels[13], cookie, new ExponentialCost(2.1 * 1E15,1.15, upgradeLevels[13])),
            new Upgrade(upgradeLevels[14], cookie, new ExponentialCost(26 * 1E15,1.15, upgradeLevels[14])),
            new Upgrade(upgradeLevels[15], cookie, new ExponentialCost(310 * 1E15,1.15, upgradeLevels[15])),
            new Upgrade(upgradeLevels[16], cookie, new ExponentialCost(71 * 1E18,1.15, upgradeLevels[16])),
            new Upgrade(upgradeLevels[17], cookie, new ExponentialCost(12 * 1E21 ,1.15, upgradeLevels[17])),
        };

        prestigeUpgrades = new Upgrade[]
        {
            new Upgrade(prestigeLevels[0],prestigePoint, new LinearCost(1, 1, prestigeLevels[0])),
        };
        var multiplier =
            new double[] { 0.1, 1, 8, 47, 260, 1400, 7800, 44000, 260000, 1.6 * 1E6, 10 * 1E6, 65 * 1E6, 430 * 1E6, 2.9 * 1E6, 21 * 1E9, 150 * 1E9, 1.1 * 1E12, 8.3 * 1E12 };
        var upgradeMultiplier = new Multiplier[18];
        upgradeMultiplier = upgradeMultiplier.Select(_ => new Multiplier()).ToArray();

        var CookieAutoGenerate = new Multiplier();
        upgrades.Select((upgrade, index) => new { upgrade, index }).ToList()
            .ForEach(upgrade => CookieAutoGenerate.RegisterMultiplier(new MultiplierInfo(() =>
            multiplier[upgrade.index]
            * upgradeMultiplier[upgrade.index].CaluculatedNumber(1.0)
            * upgradeLevels[upgrade.index].level
            , MultiplierType.add)));
        CookieAutoGenerate.RegisterMultiplier(new MultiplierInfo(() => 1 + prestigeUpgrades[0].level * 0.1, MultiplierType.mul));
        //カーソルを10個買うたびに、2つ目のアップグレードの倍率を2倍にしていく
        upgradeMultiplier[3].RegisterMultiplier(new MultiplierInfo(() => Math.Pow(2, upgrades[0].level / 10), MultiplierType.mul));

        //1秒が1ループ。
        int second = 0;
        while (second <= 3600 * 24 * 3)
        {
            second++;
            //1秒に一回クリックする
            cookie.Increment(1);
            //Cookieの自動generateをする
            cookie.Increment(CookieAutoGenerate.CaluculatedNumber(0));
            //買えるアップグレードがあったら買う
            var isPayedOnce = AutoBuy(upgrades);
            AutoBuy(prestigeUpgrades);
            OnPrestige(!isPayedOnce && cookie.Number >= 1E6);
        }

        var levelsString = upgradeLevels.Select(level => level.level.ToString("F0"));
        Debug.Log($"Upgrade購入状況: {string.Join(", ", maxLevelReached)}");
        Debug.Log($"Prestige Upgrade購入状況: {prestigeUpgrades[0].level}個. Multiplier : {prestigeUpgrades[0].level * 10}%");
        //Debug.Log($"Cookie最大所持数 : {UsefulMethod.tDigit(cookie.MaxNumber)}");
    }

    //Prestige
    private void OnPrestige(bool isPrestige)
    {
        if (!isPrestige) return;
        prestigeNum++;
        prestigePoint.Increment(Math.Log10(cookie.Number));
        cookie.ResetNumberToZero();
        for (int i = 0; i < upgrades.Length; i++)
        {
            upgrades[i].OnPrestige();
        }
        //Debug.Log($"Prestiged {prestigeNum} times. Current Prestige Point {UsefulMethod.tDigit(prestigePoint.Number)}");
    }

    private bool AutoBuy(Upgrade[] upgrades)
    {
        for (int i = 0; i < upgrades.Length; i++)
        {
            if (upgrades[i].CanBuy())
            {
                Debug.Log($"{i}番目のUpgradeを購入");
                upgrades[i].Pay();
                UpdateMaxLevel();
                return true;
            }
        }
        return false;
    }

    private void AutoMaxBuy(Upgrade[] upgrades)
    {
        while (true)
        {
            var trigger = AutoBuy(upgrades);
            if (!trigger) return;
        }
    }

    private void UpdateMaxLevel()
    {
        for (int i = 0; i < upgrades.Length; i++)
        {
            if (maxLevelReached[i] < upgrades[i].level)
            {
                maxLevelReached[i] = upgrades[i].level;
            }
        }
    }
}
