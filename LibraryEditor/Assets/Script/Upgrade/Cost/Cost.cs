using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UpgradeLibrary
{
    public enum BuyMode
    {
        Buy1,
        Buy10,
        Buy25,
        BuyMax
    }
    public interface ICost
    {
        Cal Cost { get; }
    }

    //リソースのみを計算する。
    public class LinearCost : ICost
    {
        readonly double initialValue;
        readonly double steep;
        public Cal Cost { get; }
        public LinearCost(double initialValue, double steep, ILevel level)
        {
            this.initialValue = initialValue;
            this.steep = steep;
            Cost = new Cal(initialValue);
            Cost.multiplier.AddAddtiveMultiplier(() => level.level * steep);
        }
        public LinearCost(double initialValue, double steep, int level)
        {
            this.initialValue = initialValue;
            this.steep = steep;
            Cost = new Cal(initialValue);
            Cost.multiplier.AddAddtiveMultiplier(() => level * steep);
        }
    }
}

/*
public class CostInMax : ICost
{
    public INumber resource => cost.resource;
    readonly ICost cost;
    int upgradeNum;
    public CostInMax(ICost cost)
    {
        this.cost = cost;
    }

    public bool CanBuy(ILevel level)
    {
        return resource.Number >= MaxUpgradeNumAndCost(level).totalCost;
    }

    public double Cost(ILevel level)
    {
        return MaxUpgradeNumAndCost(level).totalCost;
    }

    public int BuyNum(ILevel level)
    {
        return MaxUpgradeNumAndCost(level).buyNum;
    }

    public (int buyNum, double totalCost) MaxUpgradeNumAndCost(ILevel level)
    {
        ILevel currentLevel = new DummyLevel(level.level);
        INumber currentNumber = new DummyNumber(resource.Number);
        int count = 0;
        double tempCost = 0;
        //resourceが尽きるまで繰り返す。
        for (; ; )
        {
            if (currentNumber.Number >= cost.Cost(currentLevel))
            {
                count++;
                tempCost += cost.Cost(currentLevel);
                currentNumber.DecrementNumber(cost.Cost(currentLevel));
                currentLevel.LevelUp();
            }
            else
            {
                break;
            }
        }
        return (count, tempCost);
    }

}
*/

