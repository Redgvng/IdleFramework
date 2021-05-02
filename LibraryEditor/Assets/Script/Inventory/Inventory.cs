using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace IdleLibrary.Inventory
{
    [System.Serializable]
    public class InputInfo
    {
        //どこに入っているか？
        public int index;
        //どのインベントリに入っているか？
        public Inventory inputInventory;
        public bool isLocked;
    }
    public class InputItem
    {
        public ITEM inputItem;
        //public InputInfo info;
        public int cursorId;
        public void ReleaseItem()
        {
            inputItem = ITEM.CreateNullItem();
        }
        public InputItem()
        {
            inputItem = new ITEM(-1);
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
        //Saveすべき変数を注入する
        //使うアイテムのインスタンスを何でもいいので渡します。
        public Inventory(InputItem inputItem, InventoryForSave saveData = null)
        {
            this.saveData = saveData == null ? new InventoryForSave() : saveData;
            var originalItem = new ITEM(-1);
            if (items.Count == 0)
            {
                for (int i = 0; i < totalInventoryNum; i++)
                {
                    items.Add(originalItem);
                }
            }
            this.inputItem = inputItem;
        }

        //とりあえず何も考えずに...
        public void ExpandInventory()
        {
            items.Add(new ITEM(-1));
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
            var item = new ITEM(-1);
            item.id = UnityEngine.Random.Range(0, 5);
            SetItemByOrder(item);
        }

        //function
        public ITEM GetItem(int index)
        {
            if (index < 0 || index >= totalInventoryNum)
            {
                return new ITEM(-1);
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
            if (this == input.inputItem.inputInfo.inputInventory)
            {
                var item = GetItem(swapped);
                SetItem(input.inputItem, swapped);
                SetItem(item, input.inputItem.inputInfo.index);
            }
            //inventoryが違った場合
            else
            {
                var item = GetItem(swapped);
                SetItem(input.inputItem, swapped);
                input.inputItem.inputInfo.inputInventory.SetItem(item, input.inputItem.inputInfo.index);
            }
        }

        public void DeleteItem(int index)
        {
            var nullItem = new ITEM(-1);
            SetItem(nullItem, index);
        }
        public void RegisterItem(int index)
        {
            if (GetItem(index).isSet)
            {
                inputItem.inputItem = GetItem(index);
                inputItem.inputItem.inputInfo.index = index;
                inputItem.inputItem.inputInfo.inputInventory = this;
            }
        }

    }
}
