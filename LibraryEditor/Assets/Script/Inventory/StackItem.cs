using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace IdleLibrary.Inventory {
    public class SwapItem<T> : IStackItem<T> where T : IItem
    {
        readonly ISetItem<T> swappedItem;
        public SwapItem(ISetItem<T> setItem = null)
        {
            this.swappedItem = setItem == null ? new NullSetItem<T>() : setItem;
        }
        public void Stack(ISetItem<T> swapping)
        {
            var tempItem = swappedItem.GetItem();
            swappedItem.SetItem(swapping.GetItem());
            swapping.SetItem(tempItem);
        }
    }

    //T ... Inventory Slot
    //U ... Entity of Item
    public class SwapItemFromInventory<T> : IClickAction<T> where T : struct, IItem 
    {
        ISetItem<T> inputItem;
        ISetItem<T> originalItem;
        public SwapItemFromInventory(ISetItem<T> originalItem)
        {
            this.originalItem = originalItem;
        }
        public void Click()
        {
            var swap = new SwapItem<T>(originalItem);
            if (inputItem != null && inputItem.GetItem().id != 0)
            {
                Debug.Log("ひっくり返したよ");
                swap.Stack(inputItem);
                inputItem = default;
            }
            else
            {
                Debug.Log("登録したよ");
                inputItem = swap as ISetItem<T>;
            }
        }
    }
}