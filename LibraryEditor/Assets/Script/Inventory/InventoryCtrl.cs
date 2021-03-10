using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Main;

namespace InventoryLibrary
{
    //このクラスの役割は何？リポジトリの役割をする？いらんくね？
    /*
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
    */
    //このクラスの役割は？アイテムの操作ができること。
    public class ItemContollerTest : ISetItem<ItemTest>, ICreateitem<ItemTest>, IDeleteItem<ItemTest>
    {
        ItemTest _item { get; set; }
        public ItemTest GetItem() => _item;
        public void SetItem(ItemTest item) => _item = item;
        public bool IsItemSet { get => _item.id >= 0; }
        //constructor
        public ItemContollerTest()
        {
            if (_item == null) _item = new ItemTest(-1);
            createItem = new CreateItem<ItemTest>(this);
        }
        public ItemContollerTest(ICreateitem<ItemTest> createItem, IDeleteItem<ItemTest> deleteItem = null)
        {
            this.createItem = createItem;
            this.deleteItem = deleteItem;
        }

        //PrivateMember
        readonly ICreateitem<ItemTest> createItem;
        readonly IDeleteItem<ItemTest> deleteItem;
        public void Create(ItemTest item)
        {
            createItem.Create(item);
        }
        public void Delete()
        {
            if (deleteItem == null)
                _item.id = -1;
            else
                deleteItem.Delete();
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

    public class CreateItem<T> : ICreateitem<T>
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
