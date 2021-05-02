using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdleLibrary.Inventory
{
    //抽象クラスにするとシリアライズできない
    [System.Serializable]
    public class ITEM : IText
    {
        //InputInfoを持ちます。
        public InputInfo inputInfo;
        public int id;
        //public bool isSet => id >= 0;
        public ITEM(int id)
        {
            inputInfo = new InputInfo();
            inputInfo.inputItem = this;
            this.id = id;
        }
        public virtual string Text() { return $"----ITEM----\n\n- ID : {id}"; }
        public virtual ITEM CreateNullItem() { return new NullItem(-1); }
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
            return "Null Itemです。これは入ってちゃいけません";
        }
    }

    [System.Serializable]
    public class Item : ITEM
    {
        public Item(int id) : base(id) { }
        public override ITEM CreateNullItem()
        {
            return new Item(-1);
        }
    }

    //Itemを継承して自作のアイテムを作ります(セーブ関係上厳しい)
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
