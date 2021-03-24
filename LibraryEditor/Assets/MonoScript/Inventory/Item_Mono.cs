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
using CommonLibrary;

namespace InventoryLibrary
{
	public class Item_Mono : Subject, ISetItem<Item>, IDeleteItem<Item>,IStackItem<Item>
	{

        int index => transform.GetSiblingIndex();
        ItemContollerTestForMono<Item> controller;
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
            controller = new ItemContollerTestForMono<Item>(index, saveArray);
            this.ObserveEveryValueChanged(x => GetItem().id).Subscribe(_ => Notify());
            Notify();
        }

        public void Delete()
        {
            controller.Delete();
        }
        public void SetItem(Item item)
        {
            controller.SetItem(item);
        }
        public Item GetItem()
        {
            return controller.GetItem();
        }
        public void Stack(ISetItem<Item> stack)
        {
            controller.Stack(stack);
        }
        public bool CanSet => controller.CanSet;
    }
    public class SetItemToSave<T> : ISetItem<T>
    {
        readonly int index;
        readonly T[] saveArray;
        public bool CanSet => true;
        public SetItemToSave(int index, T[] saveArray)
        {
            this.index = index;
            this.saveArray = saveArray;
        }
        public T GetItem()
        {
            return saveArray[index];
        }

        public void SetItem(T item)
        {
            saveArray[index] = item;
        }
    }
    public class NullSetItem<T> : ISetItem<T> 
    {
        T item { get; set; }
        public bool CanSet => true;
        public T GetItem()
        {
            return item;
        }
        public void SetItem(T item)
        {
            this.item = item;
        }
    }
}
