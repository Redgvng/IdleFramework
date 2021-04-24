using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace IdleLibrary.Inventory
{
    public class InputItem
    {
        public Item inputItem { get; set; }
        public int index { get { if (!inputItem.isSet) return -1; else return _index; } set => _index = value; }
        public Inventory inputInventory { get { if (!inputItem.isSet) return null; else return _inventory; } set => _inventory = value; }
        private int _index;
        private Inventory _inventory;
        public void ReleaseItem()
        {
            inputItem = new Item(-1);
        }
        public InputItem()
        {
            inputItem = new Item(-1);
        }
    }

    [System.Serializable]
    public class InventoryForSave
    {
        public List<Item> items = new List<Item>();
        public int expandNum;
    }

    public class Inventory 
    {
        //Needed to Save
        public List<Item> items => saveData.items;
        public int expandNum { get => saveData.expandNum; set => saveData.expandNum = value; }
        public InventoryForSave saveData;

        public InputItem inputItem;
        public bool isFull => items.All((item) => item.isSet);
        public readonly int initialInventoryNum = 10;
        int totalInventoryNum => expandNum + initialInventoryNum;

        //Saveすべき変数を注入する
        public Inventory(InputItem inputItem, InventoryForSave saveData = null)
        {
            this.saveData = saveData == null ? new InventoryForSave() : saveData;
            for (int i = 0; i < totalInventoryNum; i++)
            {
                items.Add(new Item(-1));
            }
            this.inputItem = inputItem;
        }

        //とりあえず何も考えずに...
        public void ExpandInventory()
        {
            items.Add(new Item(-1));
            expandNum++;
        }

        //function
        public Item GetItem(int index)
        {
            if (index < 0 || index >= totalInventoryNum)
            {
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
            return totalInventoryNum;
        }

        public void SetItem(Item item, int index)
        {
            if(index < 0 || index >= totalInventoryNum)
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
        //こいつに、他のインベントリのswapも任せられるか？
        public void SwapItem(int swapped, InputItem input)
        {
            if (this == input.inputInventory)
            {
                var item = GetItem(swapped);
                SetItem(input.inputItem, swapped);
                SetItem(item, input.index);
            }
            //inventoryが違った場合
            else
            {
                var item = GetItem(swapped);
                SetItem(input.inputItem, swapped);
                input.inputInventory.SetItem(item, input.index);
            }
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

    }

    [System.Serializable]
    public class Item
    {
        public int id;
        public bool isLocked;
        public bool isSet => id >= 0;
        public Item(int id)
        {
            this.id = id;
            this.isLocked = false;
        }
    }
}
