using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace InventoryLibrary
{
    public class CreateItemByOrder<T> : ISetItem<T> where T : IItem
    {
        readonly ISetItem<T>[] setItems;
        readonly Cal SlotNum;
        public bool CanSet => setItems.Where((item, index) => index < SlotNum.GetValue()).Any(x => x.CanSet);
        public CreateItemByOrder(ISetItem<T>[] setItems, Cal SlotNum)
        {
            this.setItems = setItems;
            this.SlotNum = SlotNum;
        }
        public void SetItem(T item)
        {
            Debug.Log(CanSet);
            if (!CanSet)
                return;

            for (int i = 0; i < setItems.Length; i++)
            {
                if (setItems[i].CanSet)
                {
                    var create = new CreateItem<T>(setItems[i]);
                    create.SetItem(item);
                    return;
                }
            }
        }
        public T GetItem()
        {
            return setItems[0].GetItem();
        }
    }
}
