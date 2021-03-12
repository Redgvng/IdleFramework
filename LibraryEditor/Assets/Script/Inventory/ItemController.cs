using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Main;

namespace InventoryLibrary
{
    public class ItemContollerTestForMono<T> : ISetItem<T>, ICreateItem<T>, IDeleteItem<T> where T : struct, IItem
    {
        public T GetItem() => setItem.GetItem();
        public void SetItem(T item) => setItem.SetItem(item);
        public bool CanSet { get => GetItem().id == 0; }
        //constructor
        public ItemContollerTestForMono(int index, T[] saveArray)
        {
            setItem = new SetItemToSave<T>(index, saveArray);
            createItem = new CreateItem<T>(this);
            deleteItem = new DeleteItem<T>(setItem);
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


    public class CreateItem<T> : ICreateItem<T> where T : IItem
    {
        ISetItem<T> set;
        public bool CanSet => set.GetItem().id == 0;

        public CreateItem(ISetItem<T> set)
        {
            this.set = set;
        }
        public void Create(T item)
        {
            if (CanSet) 
                set.SetItem(item);
        }
    }

    public class DeleteItem<T> : IDeleteItem<T> where T : IItem
    {
        ISetItem<T> set;
        public DeleteItem(ISetItem<T> set)
        {
            this.set = set;
        }
        public void Delete()
        {
            set.SetItem(default);
        }
    }

}
