using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;

namespace IdleLibrary.Inventory
{
    //抽象クラスにするとシリアライズできない
    [System.Serializable]
    public abstract class ITEM : IText
    {
        public int id;
        public bool isSet => id >= 0;
        public bool isLocked;
        public ITEM(int id)
        {
            this.id = id;
        }
        public abstract string Text();
        public abstract ITEM CreateNullItem();
    }

    public class NullItem : ITEM
    {
        public NullItem(int id) : base(id) { }
        public override ITEM CreateNullItem()
        {
            return new NullItem(-1);
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
        public override string Text()
        {
            return "Test用itemです。";
        }
    }


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
