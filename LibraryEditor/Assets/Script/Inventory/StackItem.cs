using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace InventoryLibrary {
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

    public class SwapItemFromInventory<T, U> : IClickAction<T> where U : struct, IItem
    {
        ISetItem<U> inputItem;

        public void Click(T stackItem)
        {
            if (!(stackItem is SwapItem<U> && stackItem is ISetItem<U>))
                return;


            var swap = stackItem as SwapItem<U>;

            if (inputItem != null && inputItem.GetItem().id != 0)
            {
                Debug.Log("ひっくり返したよ");
                swap.Stack(inputItem);
                inputItem = default;
            }
            else
            {
                Debug.Log("登録したよ");
                inputItem = swap as ISetItem<U>;
            }
        }
    }
}