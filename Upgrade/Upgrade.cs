using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace IdleLibrary.Upgrade {

    public interface IUpgrade
    {
        bool CanBuy();
        void Pay();
        void MaxPay();
        void FixedAmountPay(int num);
    }
    
    //外部からは買った時の処理と、買えるかどうかが必要(ITransactionでアップグレードも表現しよう。)
    //UpgradeからCostの情報が取れないのは明らかにおかしい
    public class Upgrade : IUpgrade
    {
        private ILevel level;
        public NUMBER number;
        public IMaxableCost cost;
        public Upgrade(ILevel level, NUMBER number, IMaxableCost cost)
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
    public class MultipleUpgrade : IUpgrade
    {
        private readonly IEnumerable<(NUMBER number, IMaxableCost cost)> info;
        private readonly ILevel level;
        public MultipleUpgrade(ILevel level, params (NUMBER number, IMaxableCost cost)[] info)
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
