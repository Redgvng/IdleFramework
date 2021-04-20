using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdleLibrary.Inventory
{
    public class Inventory 
    {
        private List<Item> items = new List<Item>();
        private Cal maxNum = new Cal(10);
        public Inventory()
        {
            for (int i = 0; i < maxNum.GetValue(); i++)
            {
                items.Add(new Item(-1));
            }
        }
        public Item GetItem(int index)
        {
            if (index < 0 || index >= maxNum.GetValue())
            {
                Debug.LogError("その場所からgetできません");
                return new Item(-1);
            }

            return items[index];
        }
        public void SetItem(Item item, int index)
        {
            if(index < 0 || index >= maxNum.GetValue())
            {
                Debug.LogError("セットできません");
                return;
            }

            items[index] = item;
        }
        public void SetItemByOrder(Item item)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (!items[i].isSet)
                {
                    SetItem(item, i);
                    return;
                }
            }

            Debug.LogError("セットできません");
            return;
        }
        public void SwapItem()
        {

        }
    }

    public struct Item
    {
        public int id { get; }
        public bool isSet => id >= 0;
        public Item(int id)
        {
            this.id = id;
        }
    }
}
