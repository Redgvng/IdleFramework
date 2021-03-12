using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace InventoryLibrary
{
    public class CreateItemByOrder<T> : ICreateItem<T> where T : IItem
    {
        readonly ICreateItem<T>[] createItems;
        public CreateItemByOrder(ICreateItem<T>[] createItems)
        {
            this.createItems = createItems;
        }
        public bool CanSet => createItems.Any(x => x.CanSet);
        public void Create(T item)
        {
            if (!CanSet)
                return;

            for (int i = 0; i < createItems.Length; i++)
            {
                if (createItems[i].CanSet)
                {
                    createItems[i].Create(item);
                    return;
                }
            }
        }
    }

    public class SwapItem<T> where T : IItem
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
}
