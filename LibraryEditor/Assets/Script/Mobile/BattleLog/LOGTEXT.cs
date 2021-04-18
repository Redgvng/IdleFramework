using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using static Main;
using static UsefulMethod;
using Cysharp.Threading.Tasks;

public class LOGTEXT : MonoBehaviour
{
    //フェードアウトの滑らかさ（デフォルト：1秒間に10コマ）
    static int fadeoutsmoothness = 10;
    static float showTime = 3.0f;
    TextMeshProUGUI thisText;
    [NonSerialized] public bool isActive;
    
    private void Awake()
    {
        thisText = gameObject.GetComponent<TextMeshProUGUI>();
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
                thisText.color -= Color.black / showTime / fadeoutsmoothness * 0.75f;
                if (thisText.color.a <= 0.25)
                {
                    thisText.color = Color.clear;
                    isActive = false;
                }
            }
            else
                await UniTask.DelayFrame(1);
        }
    }
}
