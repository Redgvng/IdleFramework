using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace UpgradeLibrary {
    public enum EffectKind
    {
        Number,
        Cal,
        Click,
        Produce
    }
    public enum CalculateWay
    {
        additive,
        multiplicative
    }
    public class Upgrade_Mono : MonoBehaviour, ILevel
    {
        public long level { get; set; }
        [SerializeField]
        CostKind costkind;
        [SerializeField]
        NumbersName[] resourseNames;
        [SerializeField]
        double[] linear_initialValue, linear_steep;
        [SerializeField]
        CalculateWay calway;
        [SerializeField]
        NumbersName targetNumber;
        [SerializeField]
        CalsName targetCal;
        [SerializeField]
        double valuePerLevel;

        [SerializeField]
        EffectKind effectKind;
        // アップグレードを作成します。最終的にはfactory methodを作ったほうがイイカモ？
        void Awake()
        {
            //コスト情報を入力します。
            ICost cost = null;
            if (costkind == CostKind.linear)
            {
                cost = new LinearCost(linear_initialValue[0], linear_steep[0], this);
            }else if(costkind == CostKind.exponential)
            {
                //指数のやつを書く
            }

            //取引情報を入力します。
            ITransaction transaction;
            if(resourseNames.Length == 1)
            {
                transaction = new Transaction(DataContainer<NUMBER>.GetInstance().GetDataByName(resourseNames[0]), cost);
            }
            else
            {
                var transactionList = new List<ITransaction>();
                resourseNames.ToList().Select((x,index) => transactionList.Add(new Transaction(resourseNames[])))
                transaction = new MultipleTransaction(new Transaction[] {

                });
            }
        }

    }
}
