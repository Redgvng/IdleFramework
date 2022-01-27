using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewOpenClose : MonoBehaviour
{

    RectTransform thisRect;
    public Button[] CloseButton;
    public Button[] OpenButton;
    public bool isOpen;
    public bool IsOpen => isOpen;
    public bool isOpenFirst;
    public Action openAction;
    private Vector2 initialPosition;
    public void Open()
    {
        if (isOpen)
            return;
        thisRect.anchoredPosition = initialPosition;
        isOpen = true;
        if (openAction != null) openAction();
    }
    public void Close()
    {
        if (!isOpen)
            return;
        thisRect.anchoredPosition = Vector2.left * 8000f;
        isOpen = false;
    }
    // Use this for initialization
    void Awake()
    {
        thisRect = gameObject.GetComponent<RectTransform>();
        initialPosition = thisRect.anchoredPosition;
        for (int i = 0; i < OpenButton.Length; i++)
        {
            OpenButton[i].onClick.AddListener(Open);
        }
        for (int i = 0; i < CloseButton.Length; i++)
        {
            CloseButton[i].onClick.AddListener(Close);
        }
        thisRect.anchoredPosition = Vector2.left * 8000f;
    }

    // Use this for initialization
    void Start()
    {
        if (isOpenFirst)
        {
            Open();
        }
    }
}