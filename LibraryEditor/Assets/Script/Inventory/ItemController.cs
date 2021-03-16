using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Main;

namespace InventoryLibrary
{
    public class ItemContollerTestForMono<T> : ISetItem<T>, ICreateItem<T>, IDeleteItem<T> where T : IItem
    {
        public T GetItem() => setItem.GetItem();
        public void SetItem(T item) => setItem.SetItem(item);
        public bool CanSet { get => GetItem().id == -1; }
        //constructor
        public ItemContollerTestForMono(int index, T[] saveArray, T nullItem)
        {
            setItem = new SetItemToSave<T>(index, saveArray, nullItem);
            createItem = new CreateItem<T>(this);
            deleteItem = new DeleteItem<T>(setItem, nullItem);
        }
        //PrivateMember
        readonly ISetItem<T> setItem;
        readonly ICreateItem<T> createItem;
        readonly IDeleteItem<T> deleteItem;
        public void Create(T item)
        {
            createItem.Create(item);
        }
        public void Delete()
        {
            deleteItem.Delete();
        }
    }
    public class ItemTest : IItem
    {
        public int id { get; set; }
        public ItemTest(int id)
        {
            this.id = id;
        }
    }

    public class CreateItem<T> : ICreateItem<T> where T : IItem
    {
        ISetItem<T> set;

        public bool CanSet => set.GetItem().id < 0;

        public CreateItem(ISetItem<T> set)
        {
            this.set = set;
        }
        public void Create(T item)
        {
            if(set.GetItem() == null || set.GetItem().id < 0)
                set.SetItem(item);
        }
    }

    public class DeleteItem<T> : IDeleteItem<T>
    {
        ISetItem<T> set;
        T nullItem;
        public DeleteItem(ISetItem<T> set, T nullItem)
        {
            this.set = set;
            this.nullItem = nullItem;
        }
        public void Delete()
        {
            set.SetItem(nullItem);
        }
    }

}
