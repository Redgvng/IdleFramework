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
}
