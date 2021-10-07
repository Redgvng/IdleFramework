using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace IdleLibrary.ProgressSlider
{
    public class UIFactory<T> : MonoBehaviour where T : MonoBehaviour, IUI
    {
        private IProgressSlider slider;
        public T CreateSliderUI(IProgressSlider slider, T ui, Transform canvas)
        {
            this.slider = slider;
            var instantiatedUi = Instantiate(ui, canvas);
            this.ObserveEveryValueChanged(_ => slider.CurrentProgressRatio()).Subscribe(_ => instantiatedUi.GetSlider.value = this.slider.CurrentProgressRatio());
            Observable.EveryFixedUpdate().Subscribe(_ => slider.Update());
            return instantiatedUi;
        }
    }
}