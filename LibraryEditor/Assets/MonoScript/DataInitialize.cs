using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Resourse
{
    gold,
    stone
}

public class DataInitialize : MonoBehaviour
{
    public GameObject StoneButton;
    void Awake()
    {
        //ゲームで使われるリソースを宣言します。
        var gold = new NUMBER("Gold");
        var stone = new NUMBER("Stone");
        var exp = new NUMBER("Exp");

        //クリックで得られるものを宣言します。
        new ClickProduce("Stone", stone, StoneButton);

        //自動的に生産されるものを宣言します。
        new IdleProduce("Gold IdleProduce", DataContainer<NUMBER>.GetInstance().GetDataByName("Gold"));
        new IdleProduce("Stone IdleProduce", DataContainer<NUMBER>.GetInstance().GetDataByName("Stone"));
    }
}
