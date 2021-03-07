using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UpgradeLibrary {

    //外部からは買った時の処理と、買えるかどうかが必要(ITransactionでアップグレードも表現しよう。)
    public class Upgrade : ITransaction
    {
        NUMBER level;
        ITransaction transaction;
        public Upgrade(NUMBER level, ITransaction transaction)
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
            level.IncrementNumber();
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
                upgrade.Pay();
            }
        }
    }
}
