﻿using System.Collections;
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
    }

    public class OneTimeUpgrade : IUpgrade
    {
        private readonly FixedCost fixedCost;
        private readonly IDecrementableNumber costNumber;
        public virtual bool isPurchased { get; protected set; }
        public double cost => fixedCost.Cost;
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
    }

    public class Upgrade : IUpgrade, IResettable
    {
        private ILevel _level;
        public long level { get { return _level == null ? 0 : _level.level; } set => _level.level = value; }
        public IDecrementableNumber number;
        public IMaxableCost cost;
        public double initialiCost => cost.InitialiCost;
        public Action OnUpgraded = () => { };
        public long MaxLevel;
        public bool isMaxLevel => level >= MaxLevel && hasMaxLevel;
        public bool hasMaxLevel => MaxLevel != 0;
        public Upgrade(ILevel level, IDecrementableNumber number, IMaxableCost cost)
        {
            this._level = level;
            this.number = number;
            this.cost = cost;
        }

        public bool CanBuy()
        {
            if (!hasMaxLevel)
                return number.Number >= cost.Cost;

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
            if(hasMaxLevel && tempLevel >= MaxLevel)
            {
                tempLevel = MaxLevel;
                number.Decrement(cost.FixedNumCost(number, MaxLevel - level));
                _level.level = MaxLevel;
                return;
            }
            number.Decrement(cost.MaxCost(number));
            _level.level = tempLevel;
        }

        public void FixedAmountPay(long fixedNum)
        {
            var num = fixedNum;
            if (!CanBuy())
                return;

            if(level + fixedNum >= MaxLevel)
                num = MaxLevel - level;

            if(cost.LevelAtMaxCost(number) > num)
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

        //MultiplierInfo
        private MultiplierInfoWithLevel multiplierInfo;
        public bool isMultiplied => multiplierInfo.multiplierType == MultiplierType.mul;
        public void ApplyEffect(Multiplier multiplied, Func<long, double> effect, MultiplierType type, string key)
        {
            multiplierInfo = new MultiplierInfoWithLevel((level) => effect(level) , type, _level);
            multiplied.RegisterMultiplier(multiplierInfo, key);
        }
        public double CurrentValue()
        {
            if (multiplierInfo == null)
            {
                Debug.LogError($"Effectが実装されていません. ");
                return 0;
            }
            return multiplierInfo.CurrentValue();
        }
        public double NextValue()
        {
            if (multiplierInfo == null)
            {
                Debug.LogError($"Effectが実装されていません. ");
                return 0;
            }
            return multiplierInfo.NextValue();
        }
        public double NextIncrement()
        {
            if (multiplierInfo == null)
            {
                Debug.LogError($"Effectが実装されていません.");
                return 0;
            }
            return multiplierInfo.NextIncrement();
        }
    }

    //Upgradeと同じようにふるまってほしい
    public class MultipleUpgrade : IUpgrade
    {
        private readonly IEnumerable<(IDecrementableNumber number, IMaxableCost cost)> info;
        private readonly ILevel level;
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
            foreach (var item in info)
            {
                item.number.Decrement(item.cost.MaxCost(item.number));
            }
            level.level = minLevel;
        }

        public void FixedAmountPay(int fixedNum)
        {
            if (!CanBuy())
                return;

            var minLevel = info.Select((x) => x.cost.LevelAtMaxCost(x.number)).Min();
            if (minLevel > fixedNum)
            {
                foreach (var item in info)
                {
                    item.number.Decrement(item.cost.FixedNumCost(item.number, fixedNum));
                }
                level.level += fixedNum;
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
