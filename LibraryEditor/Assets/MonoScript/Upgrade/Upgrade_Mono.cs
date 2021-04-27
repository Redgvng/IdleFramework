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

    public class Upgrade_Mono : MonoBehaviour, ILevel, IObserver
    {
        public long level { get; set; }
        //Maxアップグレード用
        //public MaxUpgrade maxUpgrade;
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

        ////ウインドウ
        //[SerializeField]
        //PopUp popUp;
        //[SerializeField]
        //string windowText;
        //[SerializeField]
        //RectTransform canvas;

        //private variable
        IMaxableCost[] cost;
        // アップグレードを作成します。最終的にはfactory methodを作ったほうがイイカモ？
        void Awake()
        {
            List<(NUMBER, IMaxableCost)> info = new List<(NUMBER, IMaxableCost)>();
            MultipleUpgrade upgrade;
            cost = new IMaxableCost[resourceNum];
            for (int i = 0; i < resourceNum; i++)
            {
                switch (costInfo[i].costKind)
                {
                    case CostKind.linear:
                        cost[i] = new LinearCost(costInfo[i].factor1, costInfo[i].factor2, this);
                        break;
                    case CostKind.exponential:
                        cost[i] = new ExponentialCost(costInfo[i].factor1, costInfo[i].factor2, this);
                        break;
                    default:
                        break;
                }
                info.Add((DataContainer<NUMBER>.GetInstance().GetDataByName(costInfo[i].resource), cost[i]));
            }
            upgrade = new MultipleUpgrade(this, info.ToArray());
            gameObject.GetComponent<Button>().OnClickAsObservable().Subscribe(_ =>
            {
                Debug.Log(buyAmount);
                switch (buyAmount)
                {
                    case 1:
                        upgrade.Pay();
                        Debug.Log("呼んでる？");
                        break;
                    case -1:
                        upgrade.MaxPay();
                        break;
                    default:
                        upgrade.FixedAmountPay(buyAmount);
                        break;
                }
            });
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

            ////ウインドウの設定
            //var pop = popUp.StartPopUp(this.gameObject, canvas);
            //string Text()
            //{
            //    var text = $"- Current Level : {level}\n\n";
            //    text += "<cost>\n";
            //    for (int i = 0; i < resourceNum; i++)
            //    {
            //        text += $"{costInfo[i].resource} : {cost[i].Cost} " +
            //            $"(Currently you have {tDigit(DataContainer<NUMBER>.GetInstance().GetDataByName(costInfo[i].resource).Number)})";
            //    }
            //    return text;
            //}
            //pop.UpdateAsObservable().Where(_ => pop.gameObject.activeSelf).Subscribe(_ => pop.text.text = Text());
        }

        //倍率によって変えるやつ
        int buyAmount;
        public void _Update(ISubject subject)
        {
            if (subject is BuyAmountMultiplier)
            {
                var multiplier = subject as BuyAmountMultiplier;
                buyAmount = multiplier.multiplierNum;
            }
        }

    }
}
