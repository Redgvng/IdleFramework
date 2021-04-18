using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IdleLibrary
{
    public enum NumbersName
    {
        gold,
        stone,
        exp
    }

    public enum CalsName
    {

    }


    public class DataInitialize : MonoBehaviour
    {
        public GameObject StoneButton;
        void Awake()
        {
            /*
            //ゲームで使われるリソースを宣言します。
            var gold = new NUMBER(NumbersName.gold);
            var stone = new NUMBER(NumbersName.stone);
            var exp = new NUMBER(NumbersName.exp);

            //クリックで得られるものを宣言します。
            new ClickProduce(NumbersName.stone, stone, StoneButton);

            //自動的に生産されるものを宣言します。
            new IdleProduce(NumbersName.gold, gold);
            new IdleProduce(NumbersName.stone, stone);
            */
        }
    }
}
