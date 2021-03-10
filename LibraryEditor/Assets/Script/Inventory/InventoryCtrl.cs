using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Main;

namespace InventoryLibrary
{
    //このクラスの役割は何？
    public class InventoryCtrl : IInventoryController
    {
        public int MaxSize { get => 100; }
        Cal slotNum;
        IItemContoroller<Item>[] items;
        public InventoryCtrl()
        {
            slotNum = new Cal(10);
            items = new IItemContoroller<Item>[MaxSize];
        }
    }
    //このクラスの役割は？アイテムの操作ができること。
    public class ItemContollerTest : ISetItem<ItemTest>
    {
        ItemTest _item { get; set; }
        public ItemTest GetItem() => _item;
        public void SetItem(ItemTest item) => _item = item;
        //constructor
        public ItemContollerTest()
        {
            if (_item == null) _item = new ItemTest(-1); 
        }
        public bool IsItemSet { get => _item.id >= 0; }
        public void CreateItem(ItemTest item)
        {
            if (!IsItemSet)
                _item = item;
        }
    }
    public class ItemTest
    {
        public int id { get; set; }
        public ItemTest(int id)
        {
            this.id = id;
        }
    }

    public class CreateItem<T>
    {
        ISetItem<T> set;
        public CreateItem(ISetItem<T> set)
        {
            this.set = set;
        }
        public void Create(T item)
        {
            if (set.IsItemSet)
                return;
            set.SetItem(item);
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
        public void DeleteThis(IItem item)
        {
            item.id = -1;
        }
    }
}
