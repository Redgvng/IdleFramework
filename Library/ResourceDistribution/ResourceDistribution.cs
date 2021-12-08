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

    public interface IElement
    {
        void Store();
        void Retrieve();
        float CurrentProgressRatio();
        void Update();
    }
}
