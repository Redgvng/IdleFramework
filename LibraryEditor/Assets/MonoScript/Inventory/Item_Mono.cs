using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using static UsefulMethod;
using static Main;
using TMPro;
using UniRx;
using UniRx.Triggers;
using System.Runtime.Serialization;

namespace InventoryLibrary
{
	public class Item_Mono : Subject, ISetItem<Item>, IDeleteItem,IStackItem<Item>
	{

        int index => transform.GetSiblingIndex();
        ItemContollerTestForMono controller;
        //セーブする配列の設定
        Item[] saveArray { get
            {
                switch (transform.parent.GetComponent<Inventory_Mono>().InventoryId)
                {
                    case 0:
                        return main.S.items;
                    case 1:
                        return main.S.equippedItems;
                    default:
                        return null;
                }
            } }

        void Start()
        {
            controller = new ItemContollerTestForMono(index, saveArray);
            this.ObserveEveryValueChanged(x => GetItem().id).Subscribe(_ => Notify());
            Notify();
        }

        public void Delete()
        {
            controller.Delete();
        }
        public void SetItem(IItem item)
        {
            controller.SetItem(item);
        }
        public IItem GetItem()
        {
            return controller.GetItem();
        }
        public void Stack(ISetItem stack)
        {
            controller.Stack(stack);
        }
        public bool CanSet => controller.CanSet;
    }
    public class SetItemToSave<T> : ISetItem where T : IItem
    {
        readonly int index;
        readonly T[] saveArray;
        public bool CanSet => true;
        public SetItemToSave(int index, T[] saveArray)
        {
            this.index = index;
            this.saveArray = saveArray;
        }
        public IItem GetItem()
        {
            return saveArray[index];
        }

        public void SetItem(IItem item)
        {
            saveArray[index] = item as T;
        }
    }

    //テストでのみ使われるやつ？です
    public class NullSetItem : ISetItem
    {
        IItem item { get; set; }
        public bool CanSet => true;
        public IItem GetItem()
        {
            if(item == null)
            {
                item = new ItemTest(0);
                return GetItem();
            }
            return item;
        }
        public void SetItem(IItem item)
        {
            this.item = item;
        }
    }
}
