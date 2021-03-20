using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Main;

namespace InventoryLibrary
{
    public class ItemContollerTestForMono : ISetItem, IDeleteItem, IStackItem
    {
        public IItem GetItem() => setItem.GetItem();
        public void SetItem(IItem item) => setItem.SetItem(item);
        public bool CanSet { get => GetItem().id == 0; }
        //constructor
        public ItemContollerTestForMono(int index, Item[] saveArray)
        {
            var setSave = new SetItemToSave(index, saveArray);
            setItem = setSave;
            deleteItem = new DeleteItem(setSave);
            swapItem = new SwapItem(setSave);
        }
        //PrivateMember
        readonly ISetItem setItem;
        readonly IDeleteItem deleteItem;
        IStackItem swapItem;
        public void Delete()
        {
            deleteItem.Delete();
        }
        public void Stack(ISetItem item)
        {
            swapItem.Stack(item);
        }
    }


    public class CreateItem : ISetItem
    {
        ISetItem set;
        public bool CanSet => set.GetItem() == null ? false : set.GetItem().id == 0;
        public CreateItem(ISetItem set)
        {
            this.set = set;
        }
        public void SetItem(IItem item)
        {
            if (CanSet)
                set.SetItem(item);
        }

        public IItem GetItem()
        {
            return set.GetItem();
        }
    }

    public class DeleteItem : IDeleteItem
    {
        ISetItem set;
        public DeleteItem(ISetItem set)
        {
            this.set = set;
        }
        public void Delete()
        {
            set.SetItem(default);
        }
    }

}
