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
            //Id�����߂܂��B
            var id = UnityEngine.Random.Range(0,5);
            item.id = id;

            var quality = UnityEngine.Random.Range(0, 100);
            item.quality = quality;

            //IdleAction�̐ݒ� (�����Ő������̐ݒ蓙����H)
            var idleAction = new IdleAction(10);
            item.idleAction = idleAction;

            /*
            List<ItemEffect> effectList = new List<ItemEffect>();
            //�S�[���h�����ʂ𑝂₷.�f���Q�[�g���ۑ��ł��邩�ȁH
            itemEffectDic.Add("Gold Cap+", () => item.level + 1);
            itemEffectDic.Add("EXP+", () => item.level * 2 + 1);
            */
            //item.effect = itemEffectDic;


            return item;
        }
    }
}
