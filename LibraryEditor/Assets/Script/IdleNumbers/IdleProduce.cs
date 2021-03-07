using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class IdleProduce : IMultiplier
{
    public Multiplier multiplier { get; }
    NUMBER targetResourse { get; }
    public IdleProduce(NumbersName Name, NUMBER targetResourse)
    {
        multiplier = new Multiplier();
        this.targetResourse = targetResourse;
        DataContainer<IdleProduce>.GetInstance().SetDataByName(this, Name);
        Observable.EveryUpdate().Subscribe(_ => ProducePerMinute());
    }
    void ProducePerMinute()
    {
        targetResourse.IncrementNumber(multiplier.CaluculatedNumber(0), true);
    }
}
