using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using CommonLibrary;

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
    [Serializable]
    public class CostInfo : PropertyAttribute
    {
        public CostKind costKind;
        public NumbersName resource;
        public double factor1, factor2;
    }

    public class Upgrade_Mono : MonoBehaviour, ILevel
    {
        public long level { get; set; }
        //Maxアップグレード用
        public MaxUpgrade maxUpgrade;
        [SerializeField]
        int resourceNum;
        public CostInfo[] costInfo = new CostInfo[4];
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
            ITransaction[] transactions = new ITransaction[resourceNum];
            for (int i = 0; i < resourceNum; i++)
            {
                switch (costInfo[i].costKind)
                {
                    case CostKind.linear:
                        transactions[i] = new Transaction(
                            DataContainer<NUMBER>.GetInstance().GetDataByName(costInfo[i].resource),
                            new LinearCost(costInfo[i].factor1, costInfo[i].factor2, this));
                        break;
                    case CostKind.exponential:
                        break;
                    default:
                        break;
                }
            }
            Upgrade upgrade = new Upgrade(this, new MultipleTransaction(transactions));
            maxUpgrade = new MaxUpgrade(upgrade);
            gameObject.GetComponent<Button>().OnClickAsObservable().Subscribe(_ => upgrade.Pay());
            //効果の設定
            switch (effectKind)
            {
                case EffectKind.Number:
                    switch (calway)
                    {
                        case CalculateWay.additive:
                            DataContainer<NUMBER>.GetInstance().GetDataByName(targetNumber).multiplier.AddAddtiveMultiplier(() => valuePerLevel * level);
                            break;
                        case CalculateWay.multiplicative:
                            DataContainer<NUMBER>.GetInstance().GetDataByName(targetNumber).multiplier.AddAddtiveMultiplier(() => 1.0  + valuePerLevel * level);
                            break;
                        default:
                            break;
                    }
                    break;
                case EffectKind.Cal:
                    switch (calway)
                    {
                        case CalculateWay.additive:
                            DataContainer<Cal>.GetInstance().GetDataByName(targetCal).multiplier.AddAddtiveMultiplier(() => valuePerLevel * level);
                            break;
                        case CalculateWay.multiplicative:
                            DataContainer<Cal>.GetInstance().GetDataByName(targetCal).multiplier.AddAddtiveMultiplier(() => 1.0 + valuePerLevel * level);
                            break;
                        default:
                            break;
                    }
                    break;
                case EffectKind.Click:
                    switch (calway)
                    {
                        case CalculateWay.additive:
                            DataContainer<ClickProduce>.GetInstance().GetDataByName(targetNumber).multiplier.AddAddtiveMultiplier(() => valuePerLevel * level);
                            break;
                        case CalculateWay.multiplicative:
                            DataContainer<ClickProduce>.GetInstance().GetDataByName(targetNumber).multiplier.AddAddtiveMultiplier(() => 1.0 + valuePerLevel * level);
                            break;
                        default:
                            break;
                    }
                    break;
                case EffectKind.Produce:
                    switch (calway)
                    {
                        case CalculateWay.additive:
                            DataContainer<IdleProduce>.GetInstance().GetDataByName(targetNumber).multiplier.AddAddtiveMultiplier(() => valuePerLevel * level);
                            break;
                        case CalculateWay.multiplicative:
                            DataContainer<IdleProduce>.GetInstance().GetDataByName(targetNumber).multiplier.AddAddtiveMultiplier(() => 1.0 + valuePerLevel * level);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }

        }

    }
}
