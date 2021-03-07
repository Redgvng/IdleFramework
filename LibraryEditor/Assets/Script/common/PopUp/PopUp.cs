using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// PopUPを表示させるためのスクリプト。
/// 使い方
/// ①このスクリプトをプレハブにアタッチする。
/// ②そのプレハブをスクリプトから読み込む。
/// ③スクリプトから「Popup obj = プレハブ.StartPopUp(gameObject, 親のcanvas);」と呼ぶ
/// ④スクリプトのUpdateでobjのtextsとtextProsを更新する。
/// </summary>
public class PopUp : MonoBehaviour
{
    public bool on_GetComponents = true;
    public TextMeshProUGUI text;
    RectTransform parent_rect;
    [NonSerialized]
    public Vector3 Distance = new Vector3(200.0f, 100.0f);

    public PopUp StartPopUp(GameObject hoverObject, RectTransform parent)
    {
        var tmp_PopUp = Instantiate(this, parent);
        tmp_PopUp.parent_rect = parent;
        text = GetComponentInChildren<TextMeshProUGUI>();
        var eventtrigger = hoverObject.AddComponent<ObservableEventTrigger>();
        eventtrigger.OnPointerEnterAsObservable().Subscribe(data => { tmp_PopUp.gameObject.SetActive(true);});
        eventtrigger.OnPointerExitAsObservable().Subscribe(data => tmp_PopUp.gameObject.SetActive(false));
        tmp_PopUp.UpdateAsObservable().Where(_ => tmp_PopUp.gameObject.activeSelf).Subscribe(_ => {
            tmp_PopUp.ApplyPosition();
            });
        return tmp_PopUp;
    }

    Vector3 parent_size;
    public void ApplyPosition()
    {
        parent_size = new Vector3(parent_rect.rect.width, parent_rect.rect.height);
        var magnification = parent_rect.rect.width / Screen.width;
        if (Input.mousePosition.y >= Screen.height / 2 && Input.mousePosition.x >= Screen.width / 2)//第一象限
        {
            gameObject.transform.localPosition = Input.mousePosition * magnification - (parent_size / 2) + new Vector3(-Distance.x, -Distance.y);
        }
        else if (Input.mousePosition.y >= Screen.height / 2 && Input.mousePosition.x < Screen.width / 2)//第二象限
        {
            gameObject.transform.localPosition = Input.mousePosition * magnification - (parent_size / 2) + new Vector3(Distance.x, -Distance.y);
        }
        else if (Input.mousePosition.y < Screen.height / 2 && Input.mousePosition.x > Screen.width / 2)//第四象限
        {
            gameObject.transform.localPosition = Input.mousePosition * magnification - (parent_size / 2) + new Vector3(-Distance.x, Distance.y);
        }
        else if (Input.mousePosition.y < Screen.height / 2 && Input.mousePosition.x < Screen.width / 2)//第三象限
        {
            gameObject.transform.localPosition = Input.mousePosition * magnification - (parent_size / 2) + new Vector3(Distance.x, Distance.y);
        }
    }

}
