using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace IdleLibrary.ProgressSlider
{
    public interface IUI
    {
        Slider GetSlider { get; }
    }
    public class UI : MonoBehaviour, IUI
    {
        public TextMeshProUGUI titleText;
        public Slider slider;

        public Slider GetSlider => slider;
    }
}
