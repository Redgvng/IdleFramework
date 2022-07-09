using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public interface IUIRenderer
{
    bool IsOutsideOfScreen();
}

public class OpenClose : MonoBehaviour, IUIRenderer
{
    public bool IsOutsideOfScreen() => !isOpen;
    RectTransform thisRect;
    public Button[] CloseButton;
    public Button[] OpenButton;
    public bool isOpen;
    public bool IsOpen => isOpen;
    public bool isOpenFirst;
    public Action openAction;
    public void Open()
    {
        if (isOpen)
            return;
        thisRect.anchoredPosition = Vector2.zero;
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
        for (int i = 0; i < OpenButton.Length; i++)
        {
            OpenButton[i].onClick.AddListener(Open);
        }
        for (int i = 0; i < CloseButton.Length; i++)
        {
            CloseButton[i].onClick.AddListener(Close);
        }
    }

    // Use this for initialization
    void Start()
    {
        if (isOpenFirst)
            isOpen = true;
    }
}