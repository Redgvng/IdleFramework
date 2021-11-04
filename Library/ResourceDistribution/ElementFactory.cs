using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using System.Linq;

namespace IdleLibrary.ProgressSlider.ResourceDistribution
{
    public class ElementUIFactory<T> : MonoBehaviour where T : MonoBehaviour, IUI
    {
        public T CreateElement(Element element, T ui, Transform canvas, Func<bool> IsUpdateTrigger = null)
        {
            var trigger = IsUpdateTrigger == null ? () => true : IsUpdateTrigger;
            var instantiatedUi = Instantiate(ui, canvas);
            instantiatedUi.storeButton.onClick.AddListener(() => element.Store());
            instantiatedUi.retrieveButton.onClick.AddListener(() => element.Retrieve());
            this.ObserveEveryValueChanged(_ => element.CurrentProgressRatio())
                .Where(_ => trigger())
                .Subscribe(_ => instantiatedUi.progressSlider.value = element.CurrentProgressRatio())
                .AddTo(this);
            Observable.EveryFixedUpdate().Where(_ => trigger()).Subscribe(_ => element.Update()).AddTo(this);
            return instantiatedUi;
        }
    }
}