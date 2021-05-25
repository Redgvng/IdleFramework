using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UniRx;
using UniRx.Triggers;
using static IdleLibrary.UsefulMethod;
namespace IdleLibrary.UI
{
    public interface IPopup
    {

    }
    public class Popup : MonoBehaviour, IPopup
    {
        Func<bool> showCondition;
        GameObject windowObject;
        bool isOver;
        public Popup(Func<bool> showCondition, GameObject windowObject)
        {
            this.showCondition = showCondition;
            this.windowObject = windowObject;
            ShowWindow();
        }
        //Funcを入れなければ自動的にホバートリガーにします
        public Popup(GameObject hoveredObject, GameObject windowObject)
        {
            var trigger = hoveredObject.GetOrAddComponent<ObservableEventTrigger>();
            trigger.OnPointerEnterAsObservable().Subscribe(_ => isOver = true);
            trigger.OnPointerExitAsObservable().Subscribe(_ => isOver = false);
            this.showCondition = () => isOver;
            this.windowObject = windowObject;
            ShowWindow();
        }
        async void ShowWindow()
        {
            //To resolve the bug of ContentSizeFitter
            setActive(windowObject);
            setFalse(windowObject);
            bool tempBool = false;
            while (true)
            {
                if (showCondition() && !tempBool)
                {
                    setActive(windowObject);
                    tempBool = true;
                    await UniTask.DelayFrame(1);
                    setFalse(windowObject);
                    setActive(windowObject);
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
