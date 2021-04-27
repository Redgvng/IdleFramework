using System.Collections;
using System.Collections.Generic;
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
        void UpdateText(WindowKind windowKind, string description, Sprite iconSprite);//popupのテキストを更新したいときによぶ
    }
    public enum ShowWay
    {
        Hover,
        Click,
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
        public void SetPopup(GameObject targetObject, WindowKind windowKind, string description, Sprite iconSprite = null)
        {
            SetShowWay(targetObject, windowKind, description, iconSprite);
        }

        public void UpdateText(WindowKind windowKind, string description, Sprite iconSprite = null)
        {
            switch (windowKind)
            {
                case WindowKind.TextOnly:
                    popupTextOnly.UpdateUI(description);
                    break;
                case WindowKind.IconAndText:
                    popupIconAndText.UpdateUI(description, iconSprite);
                    break;
            }
        }
        [NonSerialized] public static Popup popup;
        [SerializeField] ShowWay showWay;
        [SerializeField] LocationKind locationKind;
        public Popup_UI popupTextOnly, popupIconAndText;

        private void Awake()
        {
            popup = this;
            SetLocationKind();
        }
        private Popup_UI PopupWindow(WindowKind windowKind)
        {
            switch (windowKind)
            {
                case WindowKind.TextOnly:
                    return popupTextOnly;
                case WindowKind.IconAndText:
                    return popupIconAndText;
            }
            return popupTextOnly;
        }
        private void SetLocationKind()
        {
            for (int i = 0; i < Enum.GetNames(typeof(WindowKind)).Length; i++)
            {
                PopupWindow((WindowKind)i).locationKind = locationKind;
            }
        }
        private void SetShowWay(GameObject targetObject, WindowKind windowKind, string description, Sprite iconSprite)
        {
            var eventTrigger = targetObject.AddComponent<ObservableEventTrigger>();
            switch (showWay)
            {
                case ShowWay.Hover:
                    eventTrigger.OnPointerEnterAsObservable().Subscribe(data => { PopupWindow(windowKind).Show(); UpdateText(windowKind, description, iconSprite); });
                    eventTrigger.OnPointerExitAsObservable().Subscribe(data => PopupWindow(windowKind).Hide());
                    break;
                case ShowWay.Click:
                    eventTrigger.OnPointerDownAsObservable().Subscribe(data => { PopupWindow(windowKind).SwitchShowAndHide(); UpdateText(windowKind, description, iconSprite); });
                    break;
            }

        }
    }

}
