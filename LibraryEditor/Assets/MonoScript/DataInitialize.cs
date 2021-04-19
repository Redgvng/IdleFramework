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

        NUMBER gold, stone, exp;
        void Awake()
        {
            /*
            //ゲームで使われるリソースを宣言します。
            gold = new NUMBER(NumbersName.gold, DataContainer<NUMBER>.GetInstance());
            stone = new NUMBER(NumbersName.stone, DataContainer<NUMBER>.GetInstance());
            exp = new NUMBER(NumbersName.exp, DataContainer<NUMBER>.GetInstance());

            //クリックで得られるものを宣言します。
            new ClickProduce(NumbersName.stone, stone, StoneButton, DataContainer<ClickProduce>.GetInstance());

            //自動的に生産されるものを宣言します。
            new IdleProduce(NumbersName.gold, gold);
            new IdleProduce(NumbersName.stone, stone);
            */
        }
    }
}
