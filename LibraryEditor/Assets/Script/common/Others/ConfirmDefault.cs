using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

/// <summary>
/// 通常の方法でInstantiateせず、StartConfirmからでのみする
/// インスペクターからAddもしない
/// </summary>
public class ConfirmDefault : MonoBehaviour
{
    public TextMeshProUGUI explainText;
    public Text buttonText;
    public string explainStr { get => explainText.text; set => explainText.text = value; }
    public string buttonStr { get => buttonText.text; set => buttonText.text = value; }

    public Button closeButton, confirmButton;
    public Action confirmAction;

    void PrepareUI()
    {
        closeButton = transform.GetChild(1).GetComponent<Button>();
        explainText = transform.GetChild(2).GetComponentInChildren<TextMeshProUGUI>();
        buttonText = transform.GetChild(3).GetChild(0).GetComponentInChildren<Text>();
        confirmButton = transform.GetChild(3).GetComponentInChildren<Button>();
    }

    void PrepareFunc()
    {
        closeButton.onClick.AddListener(ClosePanel);
        confirmButton.onClick.AddListener(() => confirmAction());
        confirmButton.onClick.AddListener(ClosePanel);
    }

    public void StartConfirm(Action ConfirmAction, Transform Parent, string ExplainTxt, string ButtonTxt = "OK")
    {
        ConfirmDefault instaConfirm = Instantiate(this,Parent);
        instaConfirm.PrepareUI();
        instaConfirm.explainStr = ExplainTxt;
        instaConfirm.buttonStr = ButtonTxt;
        instaConfirm.confirmAction = ConfirmAction;

        instaConfirm.PrepareFunc();
    }

    void ClosePanel()
    {
        Destroy(this.gameObject);
    }
}
