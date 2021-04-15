using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace IdleLibrary
{
    public class ClickProduce : IMultiplier
    {
        public Multiplier multiplier { get; }
        NUMBER targetResourse { get; }
        public ClickProduce(NumbersName Name, NUMBER targetResourse, GameObject game, IDataContainer<ClickProduce> dataContainer)
        {
            multiplier = new Multiplier();
            this.targetResourse = targetResourse;
            var trigger = game.AddComponent<ObservableEventTrigger>();
            trigger.OnPointerDownAsObservable().Subscribe(_ => Click());
            dataContainer.SetDataByName(this, Name);
        }
        void Click()
        {
            targetResourse.IncrementNumber(multiplier.CaluculatedNumber(1), true);
            Debug.Log(targetResourse.Number);
            Debug.Log(DataContainer<NUMBER>.GetInstance().GetDataByName(NumbersName.stone).Number);
        }
    }
}
