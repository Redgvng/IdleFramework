using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 一緒についているImageを取得して点滅させるコンポーネント
/// canBlinkを変更して制御する
/// </summary>
public class Blink : MonoBehaviour
{
    Image thisImage;
    public float deltaValue = 0.05f;
    public bool canBlink = true;
    /// <summary>
    /// true→1.0f false→0.0f
    /// デフォルトはfalse
    /// </summary>
    public bool initAlfa = false;
    bool isPlus;
    public bool isMap;
    Color thisColor;

    private void Awake()
    {
        thisImage = gameObject.GetComponent<Image>();
        thisColor = thisImage.color;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMap)
        {
            if (canBlink)
            {
                if (isPlus)
                {
                    thisColor.a += deltaValue;
                }
                else
                {
                    thisColor.a -= deltaValue;
                }
                if (thisColor.a >= 1.0f)
                {
                    isPlus = false;
                }
                if (thisColor.a <= 0.0f)
                {
                    isPlus = true;
                }
            }
            else
            {
                thisColor.a = initAlfa ? 1.0f : 0.0f;
            }
            thisImage.color = thisColor;
        }
        else if(isMap){

            if (canBlink)
            {
                if (isPlus)
                {
                    thisColor.a += deltaValue/4;
                }
                else
                {
                    thisColor.a -= deltaValue/4;
                }
                if (thisColor.a >= 1.0f)
                {
                    isPlus = false;
                }
                if (thisColor.a <= 0.5f)
                {
                    isPlus = true;
                }
            }
            else
            {
                thisColor.a = initAlfa ? 1.0f : 0.0f;
            }
            thisImage.color = thisColor;
        }
    }
}
