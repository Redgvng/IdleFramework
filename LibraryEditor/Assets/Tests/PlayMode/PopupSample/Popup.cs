using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UniRx;
using UniRx.Triggers;
using static UsefulMethod;
using static UsefulStatic;

namespace IdleLibrary.UI
{
    public interface IPopup
    {

    }
    public enum WindowKind
    {
        TextOnly,
        IconAndText,
    }

    public enum LocationKind
    {
        MouseFollow,//マウスに追従
        Corner,//キャンバスの四隅
    }

    public class Popup : MonoBehaviour, IPopup
    {
        Func<bool> showCondition;
        GameObject windowObject;
        public Popup(Func<bool> showCondition, GameObject windowObject)
        {
            this.showCondition = showCondition;
            this.windowObject = windowObject;
            ShowWindow();
        }
        async void ShowWindow()
        {
            bool tempBool = false;
            while (true)
            {
                if (showCondition() && !tempBool)
                {
                    setActive(windowObject);
                    tempBool = true;
                }
                else if (!showCondition() && tempBool)
                {
                    setFalse(windowObject);
                    tempBool = false;
                }
                await UniTask.DelayFrame(1);
            }
        }
    }
}
