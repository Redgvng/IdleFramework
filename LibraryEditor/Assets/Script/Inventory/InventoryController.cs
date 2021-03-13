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
        public void Create(T item)
        {
            for (int i = 0; i < createItems.Length; i++)
            {
                 createItems[i].Create(item);
            }
        }
    }
}
