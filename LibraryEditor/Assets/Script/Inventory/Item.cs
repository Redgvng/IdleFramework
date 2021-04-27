using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdleLibrary.Inventory
{
    //���ۃN���X
    [System.Serializable]
    public class ITEM
    {
        public int id;
        public bool isLocked;
        public bool isSet => id >= 0;
        public ITEM(int id)
        {
            this.id = id;
            this.isLocked = false;
        }
        public virtual string Text() { return ""; }
        public virtual ITEM CreateNullItem() { return null; }
    }
    public class NullItem : ITEM
    {
        public NullItem(int id) : base(id) { }
        public override ITEM CreateNullItem()
        {
            var nullItem =  new NullItem(-1);
            return nullItem;
        }

        public override string Text()
        {
            return "Null Item�ł��B����͓����Ă��Ⴂ���܂���";
        }
    }
    [System.Serializable]
    public class Item : ITEM
    {
        public Item(int id) : base(id) { }
        public override string Text()
        {
            return $"----ITEM----\n\n- ID : {id}";
        }
        public override ITEM CreateNullItem()
        {
            return new Item(-1);
        }
    }

    //Item���p�����Ď���̃A�C�e�������܂�
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
}
