using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdleLibrary.Inventory
{
    //�Ƃ肠�����K���ɍ���Ă݂�...
    public class ItemFactory
    {
        public static ITEM CreateRandomItem()
        {
            //�܂���̃A�C�e�������
            var item = new ITEM(-1);
            return item;
        }
    }
}
