using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ToggleButton : MonoBehaviour
{
    public enum Change
    {
        ActiveObject,
        ChangeColor,
        ChangeSprite
    }

    public Change change;

    public GameObject targetObject;
    public Color selectedColor;
    public Sprite selectedSprite;

    Color initColor;
    Sprite initSprite;

    Image thisImage;

    public bool initChecked;

    ToggleButton[] buttons;

    public UnityEvent PositiveEvent;
    public UnityEvent NegativeEvent;

    void ActiveObject()
    {
        targetObject.SetActive(true);
        PositiveEvent?.Invoke();
        foreach(ToggleButton btn in buttons)
        {
            if(btn == this) { continue; }
            btn.targetObject.gameObject.SetActive(false);
            btn.NegativeEvent?.Invoke();
        }
    }

    void ChangeColor()
    {
        thisImage.color = selectedColor;
        PositiveEvent?.Invoke();
        foreach (ToggleButton btn in buttons)
        {
            if (btn == this) { continue; }
            btn.thisImage.color = btn.initColor;
            btn.NegativeEvent?.Invoke();
        }
    }

    void ChangeSprite()
    {
        thisImage.sprite = null;
        thisImage.sprite = selectedSprite;
        PositiveEvent?.Invoke();
        foreach (ToggleButton btn in buttons)
        {
            if (btn == this) { continue; }
            btn.thisImage.sprite = null;
            btn.thisImage.sprite = btn.initSprite;
            btn.NegativeEvent?.Invoke();
        }
    }

    private void Awake()
    {
        thisImage = gameObject.GetComponent<Image>();
        initColor = thisImage.color;
        initSprite = thisImage.sprite;
        buttons = gameObject.transform.parent.GetComponentsInChildren<ToggleButton>();

        switch (change)
        {
            case Change.ActiveObject:
                gameObject.GetComponent<Button>().onClick.AddListener(ActiveObject);
                break;
            case Change.ChangeColor:
                gameObject.GetComponent<Button>().onClick.AddListener(ChangeColor);
                break;
            case Change.ChangeSprite:
                gameObject.GetComponent<Button>().onClick.AddListener(ChangeSprite);
                break;
            default:
                break;
        }
    }

    private void Start()
    {
        if (initChecked)
        {
            gameObject.GetComponent<Button>().onClick.Invoke();
        }
    }
}
