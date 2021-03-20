using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace InventoryLibrary {
    public class SwapItem : IStackItem
    {
        readonly ISetItem swappedItem;
        public SwapItem(ISetItem setItem = null)
        {
            this.swappedItem = setItem == null ? new NullSetItem() : setItem;
        }
        public void Stack(ISetItem swapping)
        {
            var tempItem = swappedItem.GetItem();
            swappedItem.SetItem(swapping.GetItem());
            swapping.SetItem(tempItem);
        }
    }

    public class SwapItemFromInventory<T> : IClickAction<T>
    {
        ISetItem inputItem;

        public void Click(T stackItem)
        {
            Debug.Log(stackItem is SwapItem);
            Debug.Log(stackItem is ISetItem);
            if (!(stackItem is SwapItem && stackItem is ISetItem))
                return;

            var swap = stackItem as SwapItem;

            if (inputItem != null && inputItem.GetItem().id != 0)
            {
                swap.Stack(inputItem);
                inputItem = default;
            }
            else
            {
                inputItem = swap as ISetItem;
            }
        }
    }
}