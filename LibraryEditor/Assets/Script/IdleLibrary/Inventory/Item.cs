using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdleLibrary.Inventory
{
    //���ۃN���X�ɂ���ƃV���A���C�Y�ł��Ȃ�
    [System.Serializable]
    public class ITEM : IText
    {
        public int id;
        public bool isSet => id >= 0;
        public bool isLocked;
        public ITEM(int id)
        {
            this.id = id;
        }
        public virtual string Text() { return $"----ITEM----\n\n- ID : {id}"; }
        public static ITEM CreateNullItem() { return new NullItem(-1); }

        //Item�̌��ʂ�...
        //Gold��������ǂ����邩�A

        //TargetNumber
        NUMBER targetNumber;
        MultiplierType multiplierType;

        void ItemEffect()
        {
            var number = new NUMBER();
            //number.multiplier.RegisterMultiplier(new MultiplierInfo())
        }
    }

    public class NullItem : ITEM
    {
        public NullItem(int id) : base(id) { }

        public override string Text()
        {
            return "Null Item�ł��B����͓����Ă��Ⴂ���܂���";
        }
    }

    [System.Serializable]
    public class Item : ITEM
    {
        public Item(int id) : base(id) { }
    }

    //Item���p�����Ď���̃A�C�e�������܂�(�Z�[�u�֌W�㌵����)
    /*
    [System.Serializable]
    public class Artifact : ITEM
    {
        public Artifact(int id) : base(id)
        {

        }

        public override string Text()
        {
            return $"----ITEM----\n\n- ID : {id}\n\n - Level : {level} \n- Quality : {quality} \n\n\n[Effects in Hidden Challenge]\n- Anti-Magid Power : {antimagicPower}";
        }

        public override ITEM CreateNullItem()
        {
            return new Artifact(-1);
        }

        public int level;
        public int quality;
        public double antimagicPower;
    }
    */
}
