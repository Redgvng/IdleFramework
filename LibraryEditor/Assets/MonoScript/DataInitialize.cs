using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataInitialize : MonoBehaviour
{
    void Awake()
    {
        //ゲームで使われるリソースを宣言します。
        new NUMBER("Gold");
        new NUMBER("Stone");
        new NUMBER("Crystal");
        new NUMBER("Leaf");
        new NUMBER("Exp");
    }
}
