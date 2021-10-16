using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace IdleLibrary.ProgressSlider.ResourceDistribution
{
    public class ElementUIFactory<T> : MonoBehaviour where T : MonoBehaviour, IUI
    {
        public T CreateElement(Element element, T ui, Transform canvas)
        {
            var instantiatedUi = Instantiate(ui, canvas);
            instantiatedUi.storeButton.onClick.AddListener(() => element.Store());
            instantiatedUi.retrieveButton.onClick.AddListener(() => element.Retrieve());
            this.ObserveEveryValueChanged(_ => element.CurrentProgressRatio()).Subscribe(_ => instantiatedUi.progressSlider.value = element.CurrentProgressRatio());
            Observable.EveryFixedUpdate().Subscribe(_ => element.Update());
            return instantiatedUi;
        }
    }
}