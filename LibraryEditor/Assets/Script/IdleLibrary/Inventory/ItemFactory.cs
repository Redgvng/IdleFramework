using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace IdleLibrary.Inventory
{
    public enum EffectKind
    {
        gold,
        stone,
        
    }
    public class ItemFactory
    {
        public ITEM CreateRandomItem()
        {
            var item = new Artifact(-1);
            //Idを決めます。
            var id = UnityEngine.Random.Range(0,5);
            item.id = id;

            var quality = UnityEngine.Random.Range(0, 100);
            item.quality = quality;

            //IdleActionの設定 (ここで成長率の設定等する？)
            var idleAction = new IdleAction(10);
            item.idleAction = idleAction;

            /*
            List<ItemEffect> effectList = new List<ItemEffect>();
            //ゴールド増加量を増やす.デリゲートも保存できるかな？
            itemEffectDic.Add("Gold Cap+", () => item.level + 1);
            itemEffectDic.Add("EXP+", () => item.level * 2 + 1);
            */
            //item.effect = itemEffectDic;


            return item;
        }
    }
}
