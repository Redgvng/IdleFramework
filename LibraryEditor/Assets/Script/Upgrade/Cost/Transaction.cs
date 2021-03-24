using System.Collections.Generic;
using CommonLibrary;
namespace UpgradeLibrary{
    
    //levelをここから消そう。
    public interface ITransaction
    {
        bool CanBuy();
        void Pay();
    }
    //トランザクションは決済を管理します。
    //単一のリソースを減らすかどうかチェックし、実際に減らす。
    public class Transaction : ITransaction
    {
        //コストの処理を委譲する。
        public ICost cost { get; }
        public NUMBER resource { get; }
        public bool CanBuy()
        {
            return resource.Number >= cost.Cost.GetValue();
        }
        public void Pay()
        {
            resource.DecrementNumber(cost.Cost.GetValue());
        }
        public Transaction(NUMBER resource, ICost cost)
        {
            this.resource = resource;
            this.cost = cost;
        }
    }
    //トランザクションの配列を受け取り、まとめて決済を行います。
    public class MultipleTransaction : ITransaction
    {
        IList<ITransaction> transactions { get; }
        public bool CanBuy()
        {
            //全ての決済が可能であれば、trueを返します。
            for (int i = 0; i < transactions.Count; i++)
            {
                if (!transactions[i].CanBuy())
                {
                    return false;
                }
            }
            return true;
        }

        public void Pay()
        {
            for (int i = 0; i < transactions.Count; i++)
            {
                transactions[i].Pay();
            }
        }
        public MultipleTransaction(params ITransaction[] transactions)
        {
            this.transactions = transactions;
        }
    }
    public class NullTransaction : ITransaction
    {
        public bool CanBuy()
        {
            return true;
        }

        public void Pay()
        {
            return;
        }
    }
}