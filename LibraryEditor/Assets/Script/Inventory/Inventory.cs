using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace IdleLibrary.Inventory
{
    public class Inventory 
    {
        public Inventory()
        {
            for (int i = 0; i < Capacity.GetValue(); i++)
            {
                items.Add(new Item(-1));
            }
        }
        
        //private
        private List<Item> items = new List<Item>();
        private Cal Capacity = new Cal(10);

        //public
        public void SetItem(Item item, int index)
        {
            if(items[index].id != -1)
            {
                Debug.LogError("‚»‚±‚É‚Í‚·‚Å‚ÉƒAƒCƒeƒ€‚ª‚ ‚è‚Ü‚·");
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
                    items[i] = item;
                }
            }
        }
    }

    public struct Item
    {
        public int id { get; }
        public bool isSet => id != -1;
        public Item(int id) { this.id = id; }
    }
}
