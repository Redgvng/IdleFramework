using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace IdleLibrary.ResourceDistribution
{
    public class ElementUIFactory<T> : MonoBehaviour where T : MonoBehaviour, IUI
    {
        private List<T> uis = new List<T>();
        private List<Element> elements = new List<Element>();
        public void CreateElement(Element element, T ui, Transform canvas)
        {
            elements.Add(element);
            var instantiatedUi = Instantiate(ui, canvas);
            uis.Add(instantiatedUi);
            instantiatedUi.storeButton.onClick.AddListener(() => element.Store());
            instantiatedUi.retrieveButton.onClick.AddListener(() => element.Retrieve());
            this.ObserveEveryValueChanged(_ => element.CurrentProgressRatio()).Subscribe(_ => instantiatedUi.progressSlider.value = element.CurrentProgressRatio());
            Observable.EveryFixedUpdate().Subscribe(_ => element.Update());
        }
        public Element GetElement(int index) => elements[index];
        public IEnumerable<Element> GetElements => elements;

        public T GetUI(int index) => uis[index];
        public IEnumerable<T> GetUIs => uis;
    }
}