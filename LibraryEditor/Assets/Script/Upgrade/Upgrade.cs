using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace IdleLibrary.Upgrade {

    //外部からは買った時の処理と、買えるかどうかが必要(ITransactionでアップグレードも表現しよう。)
    //UpgradeからCostの情報が取れないのは明らかにおかしい
    public class Upgrade
    {
        private ILevel level;
        public NUMBER number;
        public ICost cost;
        public Upgrade(ILevel level, NUMBER number, ICost cost)
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
            number.DecrementNumber(cost.Cost);
            level.level++;
        }

        public void MaxPay()
        {
            if (!CanBuy())
                return;

            number.DecrementNumber(cost.MaxCost(number));
            level.level = cost.LevelAtMaxCost(number);
        }

        public void FixedAmountPay(int fixedNum)
        {
            if (!CanBuy())
                return;

            if(cost.LevelAtMaxCost(number) > fixedNum)
            {
                number.DecrementNumber(cost.FixedNumCost(number,fixedNum));
                level.level += fixedNum;
            }
            else
            {
                number.DecrementNumber(cost.MaxCost(number));
                level.level = cost.LevelAtMaxCost(number);
            }
        }
    }

    //Upgradeと同じようにふるまってほしい
    public class MultipleUpgrade
    {
        private readonly IEnumerable<(NUMBER number, ICost cost)> info;
        private readonly ILevel level;
        public MultipleUpgrade(ILevel level, IEnumerable<(NUMBER number,ICost cost)> info)
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
                item.number.DecrementNumber(item.cost.Cost);
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
                item.number.DecrementNumber(item.cost.Cost);
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
                    item.number.DecrementNumber(item.cost.FixedNumCost(item.number, fixedNum));
                }
                level.level += fixedNum;
            }
            else
            {
                foreach (var item in info)
                {
                    item.number.DecrementNumber(item.cost.MaxCost(item.number));
                }
                level.level = minLevel;
            }
        }
    }
}
