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
    public class CostInfo : PropertyAttribute
    {
        public CostKind costKind;
        public NumbersName resource;
        public double factor1, factor2;
    }

    public class Upgrade_Mono : MonoBehaviour, ILevel
    {
        public long level { get; set; }
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
            foreach (var item in costInfo)
            {
                if (item == null)
                    Debug.Log("nullです");

                Debug.Log(item.costKind);
            }
        }

    }
}
