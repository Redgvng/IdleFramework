using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UniRx;

namespace IdleLibrary.Upgrade {

    public interface IUpgrade
    {
        void Pay();
        bool CanBuy();
        double Cost { get; }
    }

    public class OneTimeUpgrade : IUpgrade
    {
        private readonly FixedCost fixedCost;
        private readonly IDecrementableNumber costNumber;
        public virtual bool isPurchased { get; protected set; }
        public double Cost => fixedCost.Cost;
        public OneTimeUpgrade(IDecrementableNumber costNumber,FixedCost fixedCost)
        {
            this.fixedCost = fixedCost;
            this.costNumber = costNumber;
        }
        public bool CanBuy()
        {
            return costNumber.Number >= fixedCost.Cost && !isPurchased;
        }
        public void Pay()
        {
            if (!CanBuy())
                return;
            costNumber.Decrement(fixedCost.Cost);
            isPurchased = true;
        }
        public void Refund()
        {
            isPurchased = false;
        }
    }

    public class EffectInfo
    {
        private readonly MultiplierInfoWithLevel info;
        public EffectInfo(MultiplierInfoWithLevel info)
        {
            this.info = info;
        }
        public bool isMultiplied => info.multiplierType == MultiplierType.mul;
        public double CurrentValue()
        {
            if (info == null)
            {
                Debug.LogError($"Effectが実装されていません. ");
                return 0;
            }
            return info.CurrentValue();
        }
        public double NextValue()
        {
            if (info == null)
            {
                Debug.LogError($"Effectが実装されていません. ");
                return 0;
            }
            return info.NextValue();
        }
        public double NextIncrement()
        {
            if (info == null)
            {
                Debug.LogError($"Effectが実装されていません.");
                return 0;
            }
            return info.NextIncrement();
        }
    }

    //MaxLevelを持っているときの処理


    public class Upgrade : IUpgrade, IResettable, ILevel
    {
        private ILevel _level;
        public long level { get { return _level == null ? 0 : _level.level; } set => _level.level = value; }
        public IDecrementableNumber number;
        public IMaxableCost cost;
        public double Cost => cost.Cost;
        public double initialiCost => cost.InitialiCost;
        public Action OnUpgraded = () => { };
        public Multiplier maxLevelMultiplier { get; } = new Multiplier();

        public long maxLevel => _level.maxLevel;
        public bool isMaxLevel => _level.isMaxLevel;

        public Upgrade(ILevel level, IDecrementableNumber number, IMaxableCost cost)
        {
            this._level = level;
            this.number = number;
            this.cost = cost;
        }

        public bool CanBuy()
        {
            return number.Number >= cost.Cost && !isMaxLevel;
        }

        public void Pay()
        {
            if (!CanBuy())
                return;

            if (isMaxLevel)
                return;

            number.Decrement(cost.Cost);
            OnUpgraded();
            _level.level++;
        }

        public void MaxPay()
        {
            if (!CanBuy())
                return;

            long tempLevel = cost.LevelAtMaxCost(number);
            if(tempLevel >= maxLevel)
            {
                tempLevel = maxLevel;
                number.Decrement(cost.FixedNumCost(number, maxLevel - level));
                //_level.level = MaxLevel;
                _level.level += maxLevel - _level.level;
                return;
            }
            number.Decrement(cost.MaxCost(number));
            _level.level += tempLevel - _level.level;
        }

        public void FixedAmountPay(long fixedNum)
        {
            var num = fixedNum;

            if (!CanBuy())
                return;

            if(level + fixedNum >= maxLevel)
                num = maxLevel - level;

            if(cost.LevelAtMaxCost(number) > level + num)
            {
                number.Decrement(cost.FixedNumCost(number, num));
                _level.level += num;
            }
            else
            {
                long tempLevel = cost.LevelAtMaxCost(number);
                number.Decrement(cost.MaxCost(number));
                _level.level = tempLevel;
            }
        }

        public void OnReset()
        {
            _level.level = 0;
        }
    }

    //Upgradeと同じようにふるまってほしい
    public class MultipleUpgrade : IUpgrade
    {
        private readonly IEnumerable<(IDecrementableNumber number, IMaxableCost cost)> info;
        private readonly ILevel level;
        public double Cost => 0;
        public long maxLevel => level.maxLevel;
        public bool isMaxLevel => level.isMaxLevel;

        public MultipleUpgrade(ILevel level, params (IDecrementableNumber number, IMaxableCost cost)[] info)
        {
            this.info = info;
            this.level = level;
        }
        public bool CanBuy()
        {
            return info.All((info) => info.number.Number >= info.cost.Cost);
        }

        public void Pay()
        {
            if (!CanBuy())
                return;

            if (isMaxLevel)
                return;

            foreach (var item in info)
            {
                item.number.Decrement(item.cost.Cost);
            }
            level.level++;
        }

        public void MaxPay()
        {
            if (!CanBuy())
                return;

            var minLevel = info.Select((x) => x.cost.LevelAtMaxCost(x.number)).Min();
            if (minLevel >= maxLevel)
            {
                foreach (var item in info)
                {
                    item.number.Decrement(item.cost.FixedNumCost(item.number, maxLevel - level.level));
                }
                level.level += maxLevel - level.level;
                return;
            }
            foreach (var item in info)
            {
                item.number.Decrement(item.cost.MaxCost(item.number));
            }
            level.level = minLevel;
        }

        public void FixedAmountPay(long fixedNum)
        {
            var num = fixedNum;
            if (!CanBuy())
                return;

            if (level.level + fixedNum >= maxLevel)
                num = maxLevel - level.level;

            var minLevel = info.Select((x) => x.cost.LevelAtMaxCost(x.number)).Min();
            if (minLevel > num + level.level)
            {
                foreach (var item in info)
                {
                    item.number.Decrement(item.cost.FixedNumCost(item.number, num));
                }
                level.level += num;
            }
            else
            {
                foreach (var item in info)
                {
                    item.number.Decrement(item.cost.MaxCost(item.number));
                }
                level.level = minLevel;
            }
        }
    }
}
