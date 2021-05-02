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
using IdleLibrary.UI;

public class PopupSample : MonoBehaviour
{
    [SerializeField] Popup_UI popup_ui;
    bool isShowCondition;
    // Start is called before the first frame update

    void Start()
    {
        var Popup = new Popup(IsShowCondition, popup_ui.gameObject);
        SetShowWay(gameObject, ShowWay.Hover, "Sample Text");
    }
    bool IsShowCondition()
    {
        return isShowCondition;
    }
    //HoverOrClick
    void SetShowWay(GameObject targetObject, ShowWay showWay, string description, Sprite iconSprite = null)
    {
        var eventTrigger = targetObject.AddComponent<ObservableEventTrigger>();
        switch (showWay)
        {
            case ShowWay.Hover:
                eventTrigger.OnPointerEnterAsObservable().Subscribe(data => { isShowCondition = true; popup_ui.UpdateUI(description, iconSprite); });
                eventTrigger.OnPointerExitAsObservable().Subscribe(data => { isShowCondition = false; }) ;
                break;
            case ShowWay.Click:
                eventTrigger.OnPointerDownAsObservable().Subscribe(data => { isShowCondition = !isShowCondition; popup_ui.UpdateUI(description, iconSprite); });
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
