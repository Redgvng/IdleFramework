using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace IdleLibrary.Inventory
{
    public class InputItem
    {
        public ITEM inputItem { get; set; }
        public int index { get { if (!inputItem.isSet) return -1; else return _index; } set => _index = value; }
        public Inventory inputInventory { get { if (!inputItem.isSet) return null; else return _inventory; } set => _inventory = value; }
        public int cursorId;
        private int _index;
        private Inventory _inventory;
        public void ReleaseItem()
        {
            inputItem = inputItem.CreateNullItem();
        }
        public InputItem()
        {
            inputItem = new NullItem(-1);
            cursorId = -1;
        }
    }

    //基底クラスはセーブできないので...派生するテクニックを使ってみるか
    [System.Serializable]
    public class InventoryForSave
    {
        public List<ITEM> items = new List<ITEM>();
        public int expandNum;
    }

    public class Inventory 
    {
        //Needed to Save
        public List<ITEM> items { get => saveData.items; set => saveData.items = value; }
        public int expandNum { get => saveData.expandNum; set => saveData.expandNum = value; }
        public InventoryForSave saveData;

        public InputItem inputItem;
        public bool isFull => items.All((item) => item.isSet);
        public readonly int initialInventoryNum = 10;
        int totalInventoryNum => expandNum + initialInventoryNum;
        private ITEM originalItem;
        //Saveすべき変数を注入する
        //使うアイテムのインスタンスを何でもいいので渡します。
        public Inventory(InputItem inputItem, InventoryForSave saveData = null, ITEM item = null)
        {
            this.saveData = saveData == null ? new InventoryForSave() : saveData;
            originalItem = item == null ? new Item(-1) : item;
            if (items.Count == 0)
            {
                for (int i = 0; i < totalInventoryNum; i++)
                {
                    if (item == null)
                    {
                        items.Add(originalItem);
                    }
                    else
                    {
                        items.Add(item.CreateNullItem());
                    }
                }
            }
            this.inputItem = inputItem;
        }

        //とりあえず何も考えずに...
        public void ExpandInventory()
        {
            items.Add(new Item(-1));
            expandNum++;
        }

        public void SortById()
        {
            var tempItems = items.Select(item => { if (item.id == -1) item.id = 9999; return item; }).OrderBy((x) => x.id).ToList();
            tempItems.ForEach((x) => { if (x.id == 9999) x.id = -1; });
            items = tempItems;
        }

        public void GenerateItemRandomly()
        {
            var item = originalItem.CreateNullItem();
            item.id = UnityEngine.Random.Range(0, 5);
            SetItemByOrder(item);
        }

        //function
        public ITEM GetItem(int index)
        {
            if (index < 0 || index >= totalInventoryNum)
            {
                return originalItem.CreateNullItem();
            }

            return items[index];
        }
        public IEnumerable<ITEM> GetItems()
        {
            return items;
        }
        public int GetInventoryLength()
        {
            return totalInventoryNum;
        }

        public void SetItem(ITEM item, int index)
        {
            if(index < 0 || index >= totalInventoryNum)
            {
                Debug.LogError("セットできません");
                return;
            }

            items[index] = item;
        }
        public void SetItemByOrder(ITEM item)
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
            var nullItem = originalItem.CreateNullItem();
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
}
