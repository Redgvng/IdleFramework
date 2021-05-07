using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.Serialization;

namespace IdleLibrary.Inventory
{
    public class InputItem
    {
        public ITEM inputItem;
        public int index;
        public Inventory inputInventory;
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

        public InputItem input;
        public bool isFull => items.All((item) => item.isSet);
        public readonly int initialInventoryNum = 10;
        int totalInventoryNum => expandNum + initialInventoryNum;
        private ITEM originalItem;
        //Saveすべき変数を注入する
        //使うアイテムのインスタンスを何でもいいので渡します。
        public Inventory(InputItem input, InventoryForSave saveData = null, ITEM originalItem = null)
        {
            this.saveData = saveData == null ? new InventoryForSave() : saveData;
            originalItem = originalItem == null ? new ITEM(-1) : originalItem;
            if (items.Count == 0)
            {
                for (int i = 0; i < totalInventoryNum; i++)
                {
                    items.Add(originalItem);
                }
            }
            this.input = input;
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
            var item = new Artifact(-1);
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
            var nullItem = new ITEM(-1);
            SetItem(nullItem, index);
        }
        public void RegisterItem(int index)
        {
            if (GetItem(index).isSet)
            {
                input.inputItem = GetItem(index);
                input.index = index;
                input.inputInventory = this;
            }
        }

    }
}
