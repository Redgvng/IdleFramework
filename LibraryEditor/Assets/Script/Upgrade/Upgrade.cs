using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdleLibrary.Upgrade {

    //外部からは買った時の処理と、買えるかどうかが必要(ITransactionでアップグレードも表現しよう。)
    public class Upgrade : ITransaction
    {
        ILevel level;
        ITransaction transaction;
        public Upgrade(ILevel level, ITransaction transaction)
        {
            this.level = level;
            this.transaction = transaction;
        }

        public bool CanBuy()
        {
            return transaction.CanBuy();
        }

        public void Pay()
        {
            if (!transaction.CanBuy())
                return;
            transaction.Pay();
            level.level++;
        }
    }
    public class MaxUpgrade : ITransaction
    {
        ITransaction upgrade;
        public MaxUpgrade(ITransaction upgrade)
        {
            this.upgrade = upgrade;
        }

        public bool CanBuy()
        {
            return upgrade.CanBuy();
        }

        public void Pay()
        {
            if (!upgrade.CanBuy())
                return;

            //買えなくなるまで買います。 
            int count = 0;
            while(upgrade.CanBuy() && count <= 10000)
            {
                count++;
                upgrade.Pay();
            }
        }
    }

    public class FixedNumberUpgrade : ITransaction
    {
        private ITransaction upgrade;
        private readonly int fixedNum = 1;
        public FixedNumberUpgrade(ITransaction upgrade, int fixedNum)
        {
            this.upgrade = upgrade;
            this.fixedNum = fixedNum;
        }

        public bool CanBuy()
        {
            return upgrade.CanBuy();
        }

        public void Pay()
        {
            if (!upgrade.CanBuy())
                return;

            //買えなくなるまで買います。
            int count = 0;
            while (upgrade.CanBuy() && count < fixedNum)
            {
                count++;
                upgrade.Pay();
            }
        }
    }
}
