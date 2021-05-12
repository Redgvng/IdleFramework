using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using Cysharp.Threading.Tasks;
using System;

namespace IdleLibrary.Inventory
{
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
        public StackableItem(int id) : base(id) { stackedNumber = -1; }
        [OdinSerialize] public int stackedNumber { get; set; }

        public bool CanStack(ITEM item)
        {
            if (!(item is StackableItem)) return false;
            if (item.id == this.id) return true;
            else return false;
        }
        public void Stack(ITEM item)
        {
            stackedNumber++;
        }

        public override ITEM CreateNullItem()
        {
            return new StackableItem(-1);
        }
        public override string Text()
        {
            return $"Test用Stackableなアイテムです。現在{stackedNumber}個スタックしています";
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
            return $"----ITEM----\n- ID : {id}\n\n - Level : {level} \n- Quality : {quality} \n- Anti-Magid Power : {antimagicPower}"
                + $""
                + $"- Time to Level Up : {(idleAction.CurrentTime / idleAction.RequiredTime).ToString("F2")}";
        }
        public string EffectText()
        {
            return "";
        }
        public override ITEM CreateNullItem()
        {
            return new Artifact(-1);
        }

        async void DelayedInitialize()
        {
            await UniTask.WaitUntil(() => idleAction != null);
            idleAction.Start();
            UpdateIdleAction();
        }
        async void UpdateIdleAction()
        {
            while (true)
            {
                if (idleAction.CanClaim())
                {
                    idleAction.Claim();
                    idleAction.Start();
                }
                await UniTask.Delay(1000);
            }
        }
        public Dictionary<string, Func<double>> effect = new Dictionary<string, Func<double>>();
        public long level;
        public Action StartIdleAction => DelayedInitialize;
        [OdinSerialize] public IdleAction idleAction { get; set; }
        public int quality;
        public double antimagicPower;
    }
    
}
