using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

namespace IdleLibrary
{
    /*
    public class IdleProduce : IMultiplier
    {
        public Multiplier multiplier { get; }
        NUMBER targetResourse { get; }
        public IdleProduce(NumbersName Name, NUMBER targetResourse)
        {
            multiplier = new Multiplier();
            this.targetResourse = targetResourse;
            DataContainer<IdleProduce>.GetInstance().SetDataByName(this, Name);
            Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(_ => ProducePerMinute());
        }
        void ProducePerMinute()
        {
            targetResourse.IncrementNumber(multiplier.CaluculatedNumber(0), true);
        }
    }
    */
}
