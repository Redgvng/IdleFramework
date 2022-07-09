using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace IdleLibrary
{
    public interface ICost : IMultiplier
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
        double FixedNumCost(INumber number, long fixedNum);
        double InitialiCost { get; }
    }

    public class NullCost : IMaxableCost
    {
        public double Cost => 0;
        public Multiplier multiplier { get; } = new Multiplier();
        public double FixedNumCost(INumber number, long fixedNum)
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
        public double InitialiCost => 0;
    }

    public class FixedCost : ICost, IMaxableCost
    {
        public double Cost => multiplier.CaluculatedNumber(cost);
        public Multiplier multiplier { get; } = new Multiplier();
        public double InitialiCost => cost;
        private readonly double cost;
        private readonly ILevel level;
        public FixedCost(double cost, ILevel level = null)
        {
            this.cost = cost;
            this.level = level;
        }

        public long LevelAtMaxCost(INumber number)
        {
            if (level == null) return 0;
            var increment = number.Number / Cost;
            return level.level + (long)increment;
        }

        public double MaxCost(INumber number)
        {
            if (level == null) return 0;
            var increment = number.Number / Cost;
            return (long)increment * Cost;
        }

        public double FixedNumCost(INumber number, long fixedNum)
        {
            return fixedNum * Cost;
        }
    }

    //リソースのみを計算する。
    public class LinearCost : IMaxableCost
    {
        readonly double initialValue;
        readonly double steep;
        readonly ILevel level;
        public Multiplier multiplier { get; } = new Multiplier();
        public double Cost => multiplier.CaluculatedNumber(initialValue + level.level * steep);
        public double InitialiCost => initialValue;
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
            if (LevelAtMaxCost(number) == level) return Cost;

            return multiplier.CaluculatedNumber(TotalCost(LevelAtMaxCost(number)));
        }

        public double FixedNumCost(INumber number, long fixedNum)
        {
            double n = number.Number;
            double a = initialValue;
            double b = steep;
            long level = this.level.level;
            double TotalCost(long maxLevel) => -a * level + a * maxLevel - b * Math.Pow(level, 2) / 2 +
            b * level / 2 + b * Math.Pow(maxLevel, 2) / 2 - b * maxLevel / 2;
            if (LevelAtMaxCost(number) == level) return Cost;

            return multiplier.CaluculatedNumber(TotalCost(this.level.level + fixedNum));
        }
    }

    public class ExponentialCost : IMaxableCost
    {
        readonly double initialValue;
        public double InitialiCost => initialValue;
        readonly double factor;
        readonly ILevel level;
        public Multiplier multiplier { get; } = new Multiplier();
        public double Cost => multiplier.CaluculatedNumber(initialValue * Math.Pow(factor, level.level));
        public ExponentialCost(double initialValue, double factor, ILevel level)
        {
            if (factor == 1)
            {
                Debug.LogError("1入れないで〜");
            }
            else if (factor < 1)
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
            if (LevelAtMaxCost(number) == level) return Cost;

            return multiplier.CaluculatedNumber(TotalCost(LevelAtMaxCost(number)));
        }

        public double FixedNumCost(INumber number, long fixedNum)
        {
            double n = number.Number;
            double a = initialValue;
            double b = factor;
            long level = this.level.level;
            double TotalCost(long maxLevel) => a * (Math.Pow(b, maxLevel) - Math.Pow(b, level)) / (b - 1);
            if (LevelAtMaxCost(number) == level) return Cost;

            return multiplier.CaluculatedNumber(TotalCost(this.level.level + fixedNum));
        }
    }

    /*
     * Animatter Costでは、MaxCostは次のtierまでの必要コストとして定義されます。
     * つまり、ゲーム内での仕様と全く同じ挙動をします。
     */
    public class AntimatterCost : IMaxableCost
    {
        readonly double initialValue;
        readonly int step;
        private readonly double costMultiplier;
        readonly ILevel level;
        public long currentTier => level.level / step;
        public Multiplier multiplier { get; } = new Multiplier();
        public double Cost => multiplier.CaluculatedNumber(initialValue * Math.Pow(costMultiplier, level.level / step));
        public double InitialiCost => initialValue;
        public AntimatterCost(double initialValue, int step, double costMultiplier, ILevel level)
        {
            this.initialValue = initialValue;
            this.step = step;
            this.costMultiplier = costMultiplier;
            this.level = level;
        }

        public long LevelAtMaxCost(INumber number)
        {
            var rank = level.level / step;
            var maxLevel = (rank + 1) * step;
            var cost = (maxLevel - level.level) * Cost;
            if (number.Number >= cost) return maxLevel;
            var extraLevel = (long)(number.Number / Cost) + level.level;
            return extraLevel;
        }

        public double MaxCost(INumber number)
        {
            var rank = level.level / step;
            var maxLevel = (rank + 1) * step;
            return multiplier.CaluculatedNumber((maxLevel - level.level) * Cost);
        }

        public double FixedNumCost(INumber number, long fixedNum)
        {
            Debug.LogError("実装していません");
            return 0;
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

