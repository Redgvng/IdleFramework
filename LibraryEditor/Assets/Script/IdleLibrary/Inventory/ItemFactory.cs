using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace IdleLibrary.Inventory
{
    //とりあえず適当に作ってみる...
    public class ItemFactory
    {
        //使うインベントリ
        /*
        private readonly Inventory targetInventory;
        public ItemFactory(Inventory targetInventory)
        {
            this.targetInventory = targetInventory;
        }
        */
        //適当にゴールドを強化するという体のItemを作ってみる！
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

            //ステータスの増加率を決定する。リストで値を格納しよう、stringと値のペアでやることにする。
            Dictionary<string, Func<double>> itemEffectDic = new Dictionary<string, Func<double>>();
            //ゴールド増加量を増やす.デリゲートも保存できるかな？
            itemEffectDic.Add("Gold Cap+", () => item.level + 1);
            itemEffectDic.Add("EXP+", () => item.level * 2 + 1);
            item.effect = itemEffectDic;

            return item;
        }
    }
}
