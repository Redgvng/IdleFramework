using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UniRx;
using TMPro;
using System;

namespace IdleLibrary.ProgressSlider.ResourceDistribution {

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
        public long level { get => progressSlider.level; set => progressSlider.level = value; }
        private readonly INumber number;
        public double stored;
        private readonly ProgressSlider progressSlider;
        public Element(INumber number, ILevel level, Func<double> RequiredProgress, Func<double, double> ProgressSpeedPerFrame)
        {
            this.number = number;
            this.progressSlider = new ProgressSlider(RequiredProgress, () => ProgressSpeedPerFrame(this.stored), level);
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
            if(progressSlider != null)
                progressSlider.Update();
        }
        public float CurrentProgressRatio()
        {
            if (progressSlider != null)
                return progressSlider.CurrentProgressRatio();
            else
                return 0;
        }
        public double CurrentProgress() => progressSlider.currentProgress;
    }
}
