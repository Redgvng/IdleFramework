using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UniRx;
using TMPro;
using System;

namespace IdleLibrary.ResourceDistribution {

    public interface IInitialize
    {
        void Initialize();
    }

    public interface IUI
    {
        Button storeButton { get; }
        Button retrieveButton { get; }
        Slider progressSlider { get; }
    }

    public class Element : ILevel
    {
        public long level { get; set; }
        private readonly INumber number;
        private double stored;
        private readonly ProgressSlider progressSlider;
        public Element(INumber number, ProgressSlider progressSlider)
        {
            this.number = number;
            this.progressSlider = progressSlider;
        }
        public void Store()
        {
            var storing = number.Number;
            stored += storing;
            number.Decrement(storing);
        }
        public void Retrieve()
        {
            number.Increment(stored);
            stored = 0;
        }
        public void Update()
        {
            progressSlider.IncrementProgress(stored);
        }
        public float CurrentProgressRatio() => progressSlider.CurrentProgressRatio();
    }
}
