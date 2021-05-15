using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace IdleLibrary.Inventory
{
    public class ItemFactory
    {
        public ITEM CreateRandomItem()
        {
            var item = new Artifact(-1);
            //Idを決めます。
            var id = UnityEngine.Random.Range(0,5);
            item.id = id;

            //クオリティを決めます
            var quality = UnityEngine.Random.Range(0, 100);
            item.quality = quality;

            //IdleActionの設定 (ここで成長率の設定等する？)
            var idleAction = new IdleAction(10);
            item.idleAction = idleAction;

            List<IEffect> effectList = new List<IEffect>();
            effectList.Add(new GoldGain(() => item.level + 1));
            effectList.Add(new ExpGain(() => item.level + 3));
            item.effects = effectList;

            return item;
        }
    }
}
