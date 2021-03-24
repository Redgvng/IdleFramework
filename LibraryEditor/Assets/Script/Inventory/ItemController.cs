using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Main;
using CommonLibrary;

namespace InventoryLibrary
{
    public class ItemContollerTestForMono<T> : ISetItem<T>, IDeleteItem<T>, IStackItem<T> where T : struct, IItem
    {
        public T GetItem() => setItem.GetItem();
        public void SetItem(T item) => setItem.SetItem(item);
        public bool CanSet { get => GetItem().id == 0; }
        //constructor
        public ItemContollerTestForMono(int index, T[] saveArray)
        {
            var setSave = new SetItemToSave<T>(index, saveArray);
            setItem = setSave;
            deleteItem = new DeleteItem<T>(setSave);
            swapItem = new SwapItem<T>(setSave);
        }
        //PrivateMember
        readonly ISetItem<T> setItem;
        readonly IDeleteItem<T> deleteItem;
        IStackItem<T> swapItem;
        public void Delete()
        {
            deleteItem.Delete();
        }
        public void Stack(ISetItem<T> item)
        {
            swapItem.Stack(item);
        }
    }


    public class CreateItem<T> : ISetItem<T> where T : IItem
    {
        ISetItem<T> set;
        public bool CanSet => set.GetItem().id == 0;

        public CreateItem(ISetItem<T> set)
        {
            this.set = set;
        }
        public void SetItem(T item)
        {
            if (CanSet)
                set.SetItem(item);
        }

        public T GetItem()
        {
            return set.GetItem();
        }
    }

    public class DeleteItem<T> : IDeleteItem<T>
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
