using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            return number.Number >= cost.Cost.GetValue();
        }

        public void Pay()
        {
            if (!CanBuy())
                return;
            number.DecrementNumber(cost.Cost.GetValue());
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

    public class MultipleUpgrade
    {
        (NUMBER, ICost)[] costInfo;
        public MultipleUpgrade(params (NUMBER, ICost)[] ps)
        {
            this.costInfo = ps;
        }
    }
}
