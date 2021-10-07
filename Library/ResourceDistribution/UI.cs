using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace IdleLibrary.ProgressSlider.ResourceDistribution
{
    public class UI : MonoBehaviour, IUI
    {
        public TextMeshProUGUI titleText;
        public Slider slider;
        public Button plusButton;
        public Button minusButton;

        //Interface
        public Button storeButton => plusButton;
        public Button retrieveButton => minusButton;
        public Slider progressSlider => slider;
    }
}
