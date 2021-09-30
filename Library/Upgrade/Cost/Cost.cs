using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace IdleLibrary
{
    public interface ICost
    {
        double Cost { get; }
    }

    public interface IMaxableCost : ICost
    {
        /// <summary>
        /// レベルは増分ではなく、最終的なレベルを表します。
        /// </summary>
        long LevelAtMaxCost(INumber number);
        double MaxCost(INumber number);
        /// <summary>
        /// 現在のレベルを基準とした増分を入力します。例えば現在のレベルが10で、レベル12にするのに必要なコストであれば、引数は2を入力してください。
        /// </summary>
        double FixedNumCost(INumber number, int fixedNum);
    }

    public class NullCost : IMaxableCost
    {
        public double Cost => 0;

        public double FixedNumCost(INumber number, int fixedNum)
        {
            return 0;
        }

        public long LevelAtMaxCost(INumber number)
        {
            return 0;
        }

        public double MaxCost(INumber number)
        {
            return 0;
        }
    }

    public class FixedCost : ICost
    {
        public double Cost => cost;
        private readonly double cost;
        public FixedCost(double cost)
        {
            this.cost = cost;
        }
    }

    //リソースのみを計算する。
    public class LinearCost : IMaxableCost
    {
        readonly double initialValue;
        readonly double steep;
        readonly ILevel level;
        //private CalDL cost { get; }
        public double Cost => initialValue + level.level * steep;
        public LinearCost(double initialValue, double steep, ILevel level)
        {
            this.initialValue = initialValue;
            this.steep = steep;
            this.level = level;
            //cost = new CalDL((l) => initialValue + l * steep, level);
        }

        public long LevelAtMaxCost(INumber number)
        {
            double n = number.Number;
            double a = initialValue;
            double b = steep;
            long level = this.level.level;
            //現在のレベルを基準にして...

            long SolveX() => b != 0 ? (long)((Math.Sqrt(4 * Math.Pow(a, 2) + 4 * a * b * (2 * level - 1) + b *
                (b * Math.Pow(1 - 2 * level, 2) + 8 * n)) - 2 * a + b) / 2 / b) : (long)(n / a + level);

            return SolveX();
        }

        public double MaxCost(INumber number)
        {
            double n = number.Number;
            double a = initialValue;
            double b = steep;
            long level = this.level.level;
            double TotalCost(long maxLevel) => -a * level + a * maxLevel - b * Math.Pow(level, 2) / 2 +
            b * level / 2 + b * Math.Pow(maxLevel, 2) / 2 - b * maxLevel / 2;

            return TotalCost(LevelAtMaxCost(number));
        }

        public double FixedNumCost(INumber number, int fixedNum)
        {
            double n = number.Number;
            double a = initialValue;
            double b = steep;
            long level = this.level.level;
            double TotalCost(long maxLevel) => -a * level + a * maxLevel - b * Math.Pow(level, 2) / 2 +
            b * level / 2 + b * Math.Pow(maxLevel, 2) / 2 - b * maxLevel / 2;

            return TotalCost(this.level.level + fixedNum);
        }
    }

    public class ExponentialCost : IMaxableCost
    {
        readonly double initialValue;
        readonly double factor;
        readonly ILevel level;
        //private CalDL cost { get; }
        public double Cost => initialValue * Math.Pow(factor, level.level);
        public ExponentialCost(double initialValue, double factor, ILevel level)
        {
            if(factor == 1)
            {
                Debug.LogError("1入れないで〜");
            }else if(factor < 1)
            {
                Debug.LogError("1より大きい値入れて〜");
            }
            this.initialValue = initialValue;
            this.factor = factor;
            this.level = level;
            //cost = new CalDL((l) => Math.Pow(factor, l), level);
        }
        public long LevelAtMaxCost(INumber number)
        {
            double n = number.Number;
            double a = initialValue;
            double b = factor;
            long level = this.level.level;

            long SolveX() => (long)(Math.Log((b - 1) * n / a + Math.Pow(b, level)) / Math.Log(b));

            return SolveX();
        }

        public double MaxCost(INumber number)
        {
            double n = number.Number;
            double a = initialValue;
            double b = factor;
            long level = this.level.level;
            double TotalCost(long maxLevel) => a * (Math.Pow(b, maxLevel) - Math.Pow(b, level)) / (b - 1);

            return TotalCost(LevelAtMaxCost(number));
        }

        public double FixedNumCost(INumber number, int fixedNum)
        {
            double n = number.Number;
            double a = initialValue;
            double b = factor;
            long level = this.level.level;
            double TotalCost(long maxLevel) => a * (Math.Pow(b, maxLevel) - Math.Pow(b, level)) / (b - 1);

            return TotalCost(this.level.level + fixedNum);
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

