using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using Cysharp.Threading.Tasks;
using System;
using System.Linq;

namespace IdleLibrary.Inventory
{
    //スタックすることのできるアイテムか？
    public interface IStackableItem
    {
        bool CanStack(ITEM item);
        void Stack(ITEM item);
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
        public Item(int id) : base(id) {}

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
    public class StackableItem : ITEM, IStackableItem
    {
        public StackableItem(int id) : base(id) { stackedNumber = 1; }
        [OdinSerialize] public int stackedNumber { get; set; }

        public bool CanStack(ITEM item)
        {
            if (!(item is StackableItem)) return false;
            if (item.id == this.id) return true;
            else return false;
        }
        public void Stack(ITEM item)
        {
            var it = item as StackableItem;
            stackedNumber += it.stackedNumber;
        }

        public override ITEM CreateNullItem()
        {
            return new StackableItem(-1);
        }
        public override string Text()
        {
            return $"This is a stackable item for a test purpose. \n Currently {stackedNumber} items are stacked.";
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
            return $"----ARTIFACT----\n- ID : {id}\n\n- Level : {level} \n- Quality : {quality} \n- Anti-Magid Power : {antimagicPower}"
                + $"\n- Time to Level Up : {(idleAction.CurrentTime / idleAction.RequiredTime).ToString("F2")}"
                + "\n" + EffectText();
        }
        public override ITEM CreateNullItem()
        {
            return new Artifact(-1);
        }
        string EffectText()
        {
            string text = "[Effect]\n\n";
            effects.ForEach((x) => text += x.Text() + "\n");
            return text;
        }

        [OdinSerialize] public List<IEffect> effects = new List<IEffect>(); 
        public long level;
        public long levelCap;
        public long maxLevelCap;//LevelCap､ﾎﾉﾏﾏﾞ
        [OdinSerialize] public IdleAction idleAction { get; set; }
        public int quality;
        public double antimagicPower;
    }
    
}
