using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UniRx;

namespace IdleLibrary.Upgrade {

    public class SetSaveProperty
    {
        public void SetProperty()
        {
            //セーブするべき変数を書き換える。
        }
    }

    //購入したかどうかをシリアライズする
    [Serializable]
    public class OneTimeUpgrade
    {
        private readonly FixedCost fixedCost;
        private readonly INumber costNumber;
        public bool isPurchased;
        public OneTimeUpgrade(INumber costNumber,FixedCost fixedCost)
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

        //save用ユーティリティ
        public static void Load(IEnumerable<OneTimeUpgrade> upgrades, bool[] loadedBools)
        {
            upgrades
            .Select((upgrade, index) => new { upgrade, index })
            .ToList()
            .ForEach(pair => pair.upgrade.isPurchased = loadedBools[pair.index]);
        }
        public static void Save(IEnumerable<OneTimeUpgrade> upgrades, bool[] loadedBools)
        {
            upgrades
            .Select((upgrade, index) => new { upgrade, index })
            .ToList()
            .ForEach(pair => pair.upgrade.ObserveEveryValueChanged(x => x.isPurchased)
            .Subscribe(_ => loadedBools[pair.index] = pair.upgrade.isPurchased));
        }
    }

    public class Upgrade
    {
        private ILevel level;
        public INumber number;
        public IMaxableCost cost;
        public Upgrade(ILevel level, INumber number, IMaxableCost cost)
        {
            this.level = level;
            this.number = number;
            this.cost = cost;
        }

        public bool CanBuy()
        {
            return number.Number >= cost.Cost;
        }

        public void Pay()
        {
            if (!CanBuy())
                return;
            number.Decrement(cost.Cost);
            level.level++;
        }

        public void MaxPay()
        {
            if (!CanBuy())
                return;

            long tempLevel = cost.LevelAtMaxCost(number);
            number.Decrement(cost.MaxCost(number));
            level.level = tempLevel;
        }

        public void FixedAmountPay(int fixedNum)
        {
            if (!CanBuy())
                return;

            if(cost.LevelAtMaxCost(number) > fixedNum)
            {
                number.Decrement(cost.FixedNumCost(number,fixedNum));
                level.level += fixedNum;
            }
            else
            {
                long tempLevel = cost.LevelAtMaxCost(number);
                number.Decrement(cost.MaxCost(number));
                level.level = tempLevel;
            }
        }
    }

    //Upgradeと同じようにふるまってほしい
    public class MultipleUpgrade
    {
        private readonly IEnumerable<(INumber number, IMaxableCost cost)> info;
        private readonly ILevel level;
        public MultipleUpgrade(ILevel level, params (INumber number, IMaxableCost cost)[] info)
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
