using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UniRx;
using UniRx.Triggers;
//using static Popup_UI;
using IdleLibrary.UI;
using IdleLibrary;

namespace IdleLibrary.UI
{
    public class PopupSample : MonoBehaviour
    {
        [SerializeField] private bool isWithIcon;
        [SerializeField] private LocationKind locationKind;
        [SerializeField] private Popup_UI popup_ui;
        private bool isOver;

        void Start()
        {
            var Popup = new Popup(ShowCondition, popup_ui.gameObject);

            //Popup_UIを使った例
            Sprite iconSprite = isWithIcon ? gameObject.GetComponent<Image>().sprite : null;
            var text = new SampleText();
            SetUI(gameObject, popup_ui, locationKind, text, iconSprite);
        }
        void SetUI(GameObject targetObject, Popup_UI popup_ui, LocationKind locationKind, IText description, Sprite iconSprite = null)
        {
            var eventTrigger = targetObject.AddComponent<ObservableEventTrigger>();
            eventTrigger.OnPointerEnterAsObservable().Subscribe(data => { isOver = true; popup_ui.UpdateUI(locationKind, description, iconSprite); });
            eventTrigger.OnPointerExitAsObservable().Subscribe(data => { isOver = false; });
        }
        bool ShowCondition()
        {
            return isOver;
        }

    }

    public class SampleText : IText
    {
        public string Text()
        {
            return "Sample Description\n- Sample 1\n- Sample 2\n- Sample 3";
        }
    }
}
