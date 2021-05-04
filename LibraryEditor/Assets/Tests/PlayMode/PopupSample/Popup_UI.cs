using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Cysharp.Threading.Tasks;
using static UsefulMethod;
using IdleLibrary.UI;

public class Popup_UI : MonoBehaviour
{
    [SerializeField] private Image windowImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [NonSerialized] public LocationKind locationKind;
    public enum LocationKind
    {
        MouseFollow,//マウスに追従
        Corner,//キャンバスの四隅
    }
    private RectTransform thisRect;
    public void SwitchShowAndHide()
    {
        if (gameObject.activeSelf) Hide();
        else Show();
    }
    public void Show()
    {
        SetWindowLocation();
        setActive(gameObject);
    }
    public void Hide()
    {
        setFalse(gameObject);
    }
    public void UpdateUI(LocationKind locationKind, Func<string> descriptionString, Sprite iconSprite = null)
    {
        this.locationKind = locationKind;
        UpdateText(descriptionString);
        if (iconSprite != null) UpdateIcon(iconSprite);
    }
    private void UpdateText(Func<string> str)
    {
        descriptionText.text = str();
    }
    private void UpdateIcon(Sprite spr)
    {
        iconImage.sprite = spr;
    }

    void Awake()
    {
        thisRect = gameObject.GetComponent<RectTransform>();
        setFalse(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf) SetWindowLocation();
    }
    private void SetWindowLocation()
    {
        switch (locationKind)
        {
            case LocationKind.MouseFollow:
                SetMouseFollow();
                break;
            case LocationKind.Corner:
                SetCorner();
                break;
        }
    }
    Vector3 screenSize = new Vector2(Screen.width, Screen.height);
    private void SetMouseFollow()
    {
        thisRect.anchorMin = Vector2.one * 0.5f;
        thisRect.anchorMax = Vector2.one * 0.5f;
        thisRect.pivot = Vector2.one * 0.5f;
        Vector3 position = Input.mousePosition - screenSize * 0.5f;
        if (position.y >= 0 && position.x >= 0)//第一象限
            thisRect.anchoredPosition = position + new Vector3(-thisRect.sizeDelta.x, -thisRect.sizeDelta.y) * 0.6f;
        else if (position.y >= 0 && position.x < 0)//第二象限
            thisRect.anchoredPosition = position + new Vector3(thisRect.sizeDelta.x, -thisRect.sizeDelta.y) * 0.6f;
        else if (position.y < 0 && position.x >= 0)//第四象限
            thisRect.anchoredPosition = position + new Vector3(-thisRect.sizeDelta.x, thisRect.sizeDelta.y) * 0.6f;
        else if (position.y < 0 && position.x < 0)//第三象限
            thisRect.anchoredPosition = position + new Vector3(thisRect.sizeDelta.x, thisRect.sizeDelta.y) * 0.6f;
    }
    private void SetCorner()
    {
        Vector2 position = Input.mousePosition;
        if (position.x < Screen.width / 2)
        {
            if (position.y < Screen.height * (1 / 3f))
            {
                thisRect.anchorMin = Vector2.right;
                thisRect.anchorMax = Vector2.right;
                thisRect.pivot = Vector2.right;
                thisRect.anchoredPosition = -Vector2.right * 80f + Vector2.up * 80f;
            }
            else
            {
                thisRect.anchorMin = Vector2.one;
                thisRect.anchorMax = Vector2.one;
                thisRect.pivot = Vector2.one;
                thisRect.anchoredPosition = -Vector2.one * 80f;
            }
        }
        else
        {
            if (position.y < Screen.height * (1 / 3f))
            {
                thisRect.anchorMin = Vector2.zero;
                thisRect.anchorMax = Vector2.zero;
                thisRect.pivot = Vector2.zero;
                thisRect.anchoredPosition = Vector2.one * 80f;
            }
            else
            {
                thisRect.anchorMin = Vector2.up;
                thisRect.anchorMax = Vector2.up;
                thisRect.pivot = Vector2.up;
                thisRect.anchoredPosition = Vector2.right * 80f + Vector2.down * 80f;
            }
        }
    }

}
