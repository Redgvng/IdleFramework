using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScaleChanger : MonoBehaviour
{
    public ScrollRect mapScroll;
    public Button changeButton;
    public Image buttonImage;
    public Sprite[] buttonSprites;
    RectTransform content;
    [SerializeField]
    public int scaleId =2;
    [SerializeField]
    Vector2 smallVec2 = new Vector2(2.0f, 2.0f);
    [SerializeField]
    Vector2 mediumVec2 = new Vector2(1.0f, 1.0f);
    [SerializeField]
    Vector2 largeVec2 = new Vector2(0.74f, 0.74f);
    [SerializeField]
    Vector2 init = new Vector2(0.0f, 1.0f);

    void ChagneScale()
    {
        scaleId++;
        if (scaleId > 2)
        {
            scaleId = 0;
        }
        ApplyScale();
    }

    void ApplyScale()
    {
        switch (scaleId)
        {
            case 0:
                content.localScale = smallVec2;
                buttonImage.sprite = buttonSprites[0];
                break;
            case 1:
                buttonImage.sprite = buttonSprites[1];
                content.localScale = mediumVec2;
                break;
            case 2:
                buttonImage.sprite = buttonSprites[2];
                content.localScale = largeVec2;
                break;
            default:
                break;
        }
        mapScroll.horizontalScrollbar.value = init.x;
        mapScroll.verticalScrollbar.value = init.y;
    }

    void ActiveHorizontal()
    {
        mapScroll.horizontal = true;
    }

    void ActiveVertical()
    {
        mapScroll.vertical = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        content = mapScroll.content.GetComponent<RectTransform>();
        changeButton.onClick.AddListener(ChagneScale);
        buttonImage = changeButton.gameObject.GetComponent<Image>();

        ApplyScale();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
