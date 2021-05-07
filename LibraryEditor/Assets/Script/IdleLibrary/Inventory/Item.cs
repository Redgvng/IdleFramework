using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;

namespace IdleLibrary.Inventory
{
    public enum ItemId
    {
        sample1,
        sample2,
        sample3,
        sample4,
        sample5,

    }
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
            return "This is the null item. Text should not be shown.";
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
    public class Artifact : ITEM, ILevel
    {
        public Artifact(int id) : base(id)
        {

        }
        public override string Text()
        {
            return $"----ITEM----\n- ID : {id}\n\n - Level : {level} \n- Quality : {quality} \n[Effects in Hidden Challenge]\n- Anti-Magid Power : {antimagicPower}";
        }
        public override ITEM CreateNullItem()
        {
            return new Artifact(-1);
        }

        public IdleAction idleAction { get; set; }
        public long level { get; set; }
        public int quality { get; set; }
        public double antimagicPower { get; set; }
    }
    
}
