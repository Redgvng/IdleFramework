using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace IdleLibrary.Inventory
{
    //とりあえず適当に作ってみる...
    public class ItemFactory
    {
        //適当にゴールドを強化するという体のItemを作ってみる！
        public ITEM CreateRandomItem()
        {
            var item = new Artifact(-1);
            //Idを決めます。
            var id = UnityEngine.Random.Range(0, Enum.GetValues(typeof(ItemId)).Length);
            item.id = id;

            var quality = UnityEngine.Random.Range(0, 100);
            item.quality = quality;

            //IdleActionの設定
            var idleAction = new IdleAction();

            return item;
        }
    }
}
