using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using static UsefulMethod;
namespace IdleLibrary.Upgrade {
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

        //ウインドウ
        [SerializeField]
        PopUp popUp;
        [SerializeField]
        string windowText;
        [SerializeField]
        RectTransform canvas;

        //private variable
        ICost[] cost;
        // アップグレードを作成します。最終的にはfactory methodを作ったほうがイイカモ？
        void Awake()
        {
            ITransaction[] transactions = new ITransaction[resourceNum];
            cost = new ICost[resourceNum];
            for (int i = 0; i < resourceNum; i++)
            {
                switch (costInfo[i].costKind)
                {
                    case CostKind.linear:
                        cost[i] = new LinearCost(costInfo[i].factor1, costInfo[i].factor2, this);
                        transactions[i] = new Transaction(
                            DataContainer<NUMBER>.GetInstance().GetDataByName(costInfo[i].resource),
                            cost[i]);
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
            Multiplier targetMultiplier = null;
            switch (effectKind)
            {
                case EffectKind.Number:
                    targetMultiplier = DataContainer<NUMBER>.GetInstance().GetDataByName(targetNumber).multiplier;
                    break;
                case EffectKind.Cal:
                    targetMultiplier = DataContainer<Cal>.GetInstance().GetDataByName(targetNumber).multiplier;
                    break;
                case EffectKind.Click:
                    targetMultiplier = DataContainer<ClickProduce>.GetInstance().GetDataByName(targetNumber).multiplier;
                    break;
                case EffectKind.Produce:
                    targetMultiplier = DataContainer<IdleProduce>.GetInstance().GetDataByName(targetNumber).multiplier;
                    break;
                default:
                    break;
            }

            switch (calway)
            {
                case CalculateWay.additive:
                    targetMultiplier.AddAddtiveMultiplier(() => valuePerLevel * level);
                    break;
                case CalculateWay.multiplicative:
                    targetMultiplier.AddAddtiveMultiplier(() => 1.0 + valuePerLevel * level);
                    break;
                default:
                    break;
            }

            //ウインドウの設定
            var pop = popUp.StartPopUp(this.gameObject, canvas);
            string Text()
            {
                var text = $"- Current Level : {level}\n\n";
                text += "<cost>\n";
                for (int i = 0; i < resourceNum; i++)
                {
                    text += $"{costInfo[i].resource} : {cost[i].Cost.GetValue()} " +
                        $"(Currently you have {tDigit(DataContainer<NUMBER>.GetInstance().GetDataByName(costInfo[i].resource).Number)})";
                }
                return text;
            }
            pop.UpdateAsObservable().Where(_ => pop.gameObject.activeSelf).Subscribe(_ => pop.text.text = Text());
        }


    }
}
