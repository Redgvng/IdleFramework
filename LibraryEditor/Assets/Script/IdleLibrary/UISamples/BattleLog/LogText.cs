using UnityEngine;
using TMPro;
using System;
using Cysharp.Threading.Tasks;

namespace IdleLibrary.UI
{
    public class LogText : MonoBehaviour
    {
        private readonly int fadeoutsmoothness = 10;//= 10 frame/sec
        private float showTime = 3.0f;//Default Showing Time
        private TextMeshProUGUI thisText;
        [NonSerialized] public bool isActive;

        private void Awake()
        {
            thisText = gameObject.GetComponent<TextMeshProUGUI>();
            thisText.color = Color.clear;
        }
        public void SetInfo(string text, float timesec)
        {
            isActive = true;
            gameObject.transform.SetSiblingIndex(0);
            thisText.color = Color.white;
            thisText.text = text;
            showTime = timesec;
            FadeOut();
        }
        async void FadeOut()
        {
            while (true)
            {
                if (isActive)
                {
                    await UniTask.Delay(1000 / fadeoutsmoothness);
                    thisText.color -= Color.black / showTime / (float)fadeoutsmoothness;
                    if (thisText.color.a <= 0)
                    {
                        thisText.color = Color.clear;
                        isActive = false;
                    }
                }
                else
                    break;
            }
        }
    }
}
