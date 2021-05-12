using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace IdleLibrary.Inventory
{
    //�Ƃ肠�����K���ɍ���Ă݂�...
    public class ItemFactory
    {
        //�g���C���x���g��
        /*
        private readonly Inventory targetInventory;
        public ItemFactory(Inventory targetInventory)
        {
            this.targetInventory = targetInventory;
        }
        */
        //�K���ɃS�[���h����������Ƃ����̂�Item������Ă݂�I
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

            //�X�e�[�^�X�̑����������肷��B���X�g�Œl���i�[���悤�Astring�ƒl�̃y�A�ł�邱�Ƃɂ���B
            Dictionary<string, Func<double>> itemEffectDic = new Dictionary<string, Func<double>>();
            //�S�[���h�����ʂ𑝂₷.�f���Q�[�g���ۑ��ł��邩�ȁH
            itemEffectDic.Add("Gold Cap+", () => item.level + 1);
            itemEffectDic.Add("EXP+", () => item.level * 2 + 1);
            item.effect = itemEffectDic;

            return item;
        }
    }
}
