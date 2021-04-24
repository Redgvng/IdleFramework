using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdleLibrary.Inventory
{
    public class InputItem
    {
        public Item inputItem { get; set; }
        public int index { get { if (inputItem.id == -1) return -1; else return _index; } set => _index = value; }
        public Inventory inputInventory { get; set; }
        int _index;
        public InputItem()
        {
            inputItem = new Item(-1);
        }
    }

    public class Inventory 
    {
        private List<Item> items = new List<Item>();
        private Cal maxNum = new Cal(10);
        public InputItem inputItem;

        public Inventory(InputItem inputItem)
        {
            for (int i = 0; i < maxNum.GetValue(); i++)
            {
                items.Add(new Item(-1));
            }
            this.inputItem = inputItem;
        }

        //function
        public Item GetItem(int index)
        {
            if (index < 0 || index >= maxNum.GetValue())
            {
                Debug.LogError("その場所からgetできません");
                return new Item(-1);
            }

            return items[index];
        }
        public IEnumerable<Item> GetItems()
        {
            return items;
        }
        public int GetInventoryLength()
        {
            return (int)maxNum.GetValue();
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
        public void SwapItem(int swapped, int swapping)
        {
            var item = GetItem(swapped);
            SetItem(GetItem(swapping), swapped);
            SetItem(item, swapping);
        }
        public void SwapItem(int swapped, InputItem input)
        {
            var item = GetItem(swapped);
            SetItem(input.inputItem, swapped);
            SetItem(item, input.index);
        }
        public void SwapItemFromOtherInventory(Inventory otherInventory, int otherId, InputItem input)
        {
            var item = otherInventory.GetItem(otherId);
            otherInventory.SetItem(GetItem(input.index), otherId);
            SetItem(item, input.index);
        } 

        public void DeleteItem(int index)
        {
            var nullItem = new Item(-1);
            SetItem(nullItem, index);
        }
        public void RegisterItem(int index)
        {
            if (GetItem(index).isSet)
            {
                inputItem.inputItem = GetItem(index);
                inputItem.index = index;
                inputItem.inputInventory = this;
            }
        }
        public void ReleaseItem()
        {
            inputItem.inputItem = new Item(-1);

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
