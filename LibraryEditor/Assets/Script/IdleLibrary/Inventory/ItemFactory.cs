using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace IdleLibrary.Inventory
{
    //�Ƃ肠�����K���ɍ���Ă݂�...
    public class ItemFactory
    {
        //�K���ɃS�[���h����������Ƃ����̂�Item������Ă݂�I
        public ITEM CreateRandomItem()
        {
            var item = new Artifact(-1);
            //Id�����߂܂��B
            var id = UnityEngine.Random.Range(0, Enum.GetValues(typeof(ItemId)).Length);
            item.id = id;

            var quality = UnityEngine.Random.Range(0, 100);
            item.quality = quality;

            //IdleAction�̐ݒ�
            var idleAction = new IdleAction();

            return item;
        }
    }
}
