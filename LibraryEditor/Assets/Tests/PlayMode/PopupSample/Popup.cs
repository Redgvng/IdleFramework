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
        void UpdateText(string description);//popupのテキストを更新したいときによぶ
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
            this.windowKind = windowKind;
            this.description = description;
            this.iconSprite = iconSprite;
            SetLocationKind();
            SetShowWay(targetObject);
        }

        public void UpdateText(string description)
        {
            switch (windowKind)
            {
                case WindowKind.TextOnly:
                    popupTextOnly.UpdateUI(description);
                    break;
                case WindowKind.IconAndText:
                    popupTextOnly.UpdateUI(description, iconSprite);
                    break;
            }
        }
        [NonSerialized] public static Popup popup;
        [SerializeField] ShowWay showWay;
        [SerializeField] LocationKind locationKind;
        public Popup_UI popupTextOnly, popupIconAndText;
        private WindowKind windowKind;
        private string description;
        private Sprite iconSprite;

        private void Awake()
        {
            popup = this;
        }
        private Popup_UI PopupWindow()
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
            PopupWindow().locationKind = locationKind;
        }
        private void SetShowWay(GameObject targetObject)
        {
            var eventTrigger = targetObject.AddComponent<ObservableEventTrigger>();
            switch (showWay)
            {
                case ShowWay.Hover:
                    eventTrigger.OnPointerEnterAsObservable().Subscribe(data => { PopupWindow().Show(); UpdateText(description); });
                    eventTrigger.OnPointerExitAsObservable().Subscribe(data => PopupWindow().Hide());
                    break;
                case ShowWay.Click:
                    eventTrigger.OnPointerDownAsObservable().Subscribe(data => PopupWindow().SwitchShowAndHide());
                    break;
            }

        }
    }

}
