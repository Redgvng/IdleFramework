using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class ClickProduce : IMultiplier
{
    public Multiplier multiplier { get; }
    NUMBER targetResourse { get; }
    public ClickProduce(NumbersName Name, NUMBER targetResourse, GameObject game)
    {
        multiplier = new Multiplier();
        this.targetResourse = targetResourse;
        var trigger = game.AddComponent<ObservableEventTrigger>();
        trigger.OnPointerDownAsObservable().Subscribe(_ => Click());
        DataContainer<ClickProduce>.GetInstance().SetDataByName(this, Name);
    }
    void Click()
    {
        targetResourse.IncrementNumber(multiplier.CaluculatedNumber(1), true);
    }
}
