using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdleLibrary.Inventory
{
    //とりあえず適当に作ってみる...
    public class ItemFactory
    {
        public static ITEM CreateRandomItem()
        {
            //まず空のアイテムを作る
            var item = new ITEM(-1);
            return item;
        }
    }
}
