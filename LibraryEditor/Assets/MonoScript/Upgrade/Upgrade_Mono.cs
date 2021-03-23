using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

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
    public class LinearInfo : PropertyAttribute
    {
        public NumbersName resource;
        public double initialValue, initialSteep;
    }

    public class Upgrade_Mono : MonoBehaviour, ILevel
    {
        public long level { get; set; }
        [SerializeField]
        int resourceNum;
        [SerializeField]
        CostKind costkind;
        [SerializeField]
        LinearInfo[] linearInfo;
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
            //コストの設定をします。
            List<ICost> cost = new List<ICost>();
            if(costkind == CostKind.linear)
            {
                linearInfo.ToList().ForEach(x => cost.Add(new ))
            }
        }

    }
}
