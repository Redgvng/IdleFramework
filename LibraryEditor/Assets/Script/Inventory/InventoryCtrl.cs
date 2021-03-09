using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Main;

namespace InventoryLibrary
{
    public class InventoryCtrl : IItemController
    {
        public int MaxSize { get => 100; }
        Cal slotNum;
        IItem[] items;
        SetItemClass setItem;
        public InventoryCtrl()
        {
            slotNum = new Cal(10);
            items = new IItem[MaxSize];
            setItem = new SetItemClass(items, this);
        }
    }
    public class SetItemClass
    {
        IItem[] items;
        IItemController itemController;
        public SetItemClass(IItem[] items, IItemController itemController)
        {
            this.items = items;
            this.itemController = itemController;
        }
        //SetMethod 指定した番号にitemを入れる。番号を指定しない場合は順番に入れる。
        public void SetItem(IItem item, int index)
        {
            if (!items[index].isSet)
                items[index] = item;
            else
                SetItemInOrder(item);
        }
        void SetItemInOrder(IItem item)
        {
            for (int i = 0; i < itemController.MaxSize; i++)
            {
                if (i >= items.Length)
                {
                    Debug.Log("アイテムがいっぱいです");
                    return;
                }
                if (!items[i].isSet)
                {
                    items[i] = item;
                    return;
                }
            }
            Debug.Log("アイテムがいっぱいです");
        }
    }

    public class SwapItemClass : IItemStack
    {
        public void Stack(IItem[] items, int swapping, int swapped)
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

    public class DeleteItem
    {
        public void Delete(IItem[] items, int deleted) 
        {
            if(items.Length < deleted)
            {
                throw new System.Exception("インデックスがアイテムの長さよりも長いです");
            }

            items[deleted].id = -1;
            return;
        }
    }
}
