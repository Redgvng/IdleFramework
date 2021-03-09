using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Main;

namespace InventoryLibrary
{
    public class InventoryCtrl
    {
        public static readonly int maxSize = 100;
        Cal slotNum;
        IItem[] items;
        SetItemClass setItem;
        public InventoryCtrl()
        {
            slotNum = new Cal(10);
            items = new IItem[maxSize];
            setItem = new SetItemClass(items);
        }
    }
    public class SetItemClass
    {
        IItem[] items;
        public SetItemClass(IItem[] items)
        {
            this.items = items;
        }
        //SetMethod 指定した番号にitemを入れる。番号を指定しない場合は順番に入れる。
        public void SetItem(Item item, int index)
        {
            if (items[index] is NullItem)
                items[index] = item;
            else
                SetItemInOrder(item);
        }
        void SetItemInOrder(Item item)
        {
            for (int i = 0; i < InventoryCtrl.maxSize; i++)
            {
                if (i >= items.Length)
                {
                    Debug.Log("アイテムがいっぱいです");
                    return;
                }
                if (items[i] is NullItem)
                {
                    items[i] = item;
                    return;
                }
            }
            Debug.Log("アイテムがいっぱいです");
        }
    }

    public class SwapItemClass
    {
        public void SwapItem(IItem[] items, int swapping, int swapped)
        {
            if(items.Length < swapping || items.Length < swapped)
            {
                throw new System.Exception("インデックスがアイテムの長さよりも長いです");
            }

            var tempItem = items[swapped];
            items[swapped] = items[swapping];
            items[swapping] = tempItem;
        }
    }
}
