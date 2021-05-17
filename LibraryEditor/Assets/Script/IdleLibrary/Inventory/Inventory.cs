using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.Serialization;
using System;

namespace IdleLibrary.Inventory
{
    public class InputItem
    {
        public ITEM inputItem;
        public int index;
        //�N���b�N���ꂽ���ɓo�^����܂�
        public Inventory inputInventory;
        //�z�o�[�����Ƃ��ɓo�^����܂�.�z�o�[��o�^���Ă��Ȃ��Ƃ��ɁA���̕ϐ��������Ă���̂͗ǂ��Ȃ��B
        public Inventory hoveredInventory;
        public int cursorId;
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

    //���N���X�̓Z�[�u�ł��Ȃ��̂�...�h������e�N�j�b�N���g���Ă݂邩
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
        internal int totalInventoryNum => expandNum + initialInventoryNum;

        private ISetItem setItem;
        //Save���ׂ��ϐ��𒍓�����
        //�e�X�g�p�R���X�g���N�^�ł�
        public Inventory()
        {
            saveData = new InventoryForSave();
            var originalItem = new Item(-1);
            for (int i = 0; i < totalInventoryNum; i++)
            {
                items.Add(originalItem);
            }
            this.input = new InputItem();
        }
        public Inventory(InputItem input, ref InventoryForSave saveData,int initialInventoryNum)
        {
            if (saveData == null)
                saveData = new InventoryForSave();
            this.saveData = saveData;
            this.initialInventoryNum = initialInventoryNum;
            if (items.Count == 0)
            {
                for (int i = 0; i < totalInventoryNum; i++)
                {
                    items.Add(new NullItem(-1));
                }
            }
            this.input = input;
        }

        //�Ƃ肠���������l������...
        public void ExpandInventory()
        {
            items.Add(new NullItem(-1));
            expandNum++;
        }

        public void GenerateItemRandomly()
        {
            var item = new Item(-1);
            item.id = UnityEngine.Random.Range(0, 5);
            SetItemByOrder(item);
        }

        //function
        public ITEM GetItem(int index)
        {
            if (index < 0 || index >= totalInventoryNum)
            {
                return new NullItem(-1);
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
            if(setItem == null)
            {
                setItem = new SimpleSetItem(this);
            }
            if (index < 0 || index >= totalInventoryNum)
            {
                Debug.LogError("�Z�b�g�ł��܂���");
                return;
            }
            setItem.SetItem(item, index);
        }
        public void RegisterSetItem(ISetItem setItem)
        {
            this.setItem = setItem;
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

            Debug.LogError("�Z�b�g�ł��܂���");
            return;
        }
        public void SwapItem(int swapped, int swapping)
        {
            var item = GetItem(swapped);
            SetItem(GetItem(swapping), swapped);
            SetItem(item, swapping);
        }
        //�����ɁA���̃C���x���g����swap���C�����邩�H
        public void SwapItem(int swapped, InputItem input)
        {
            if (this == input.inputInventory)
            {
                var item = GetItem(swapped);
                SetItem(input.inputItem, swapped);
                SetItem(item, input.index);
            }
            //inventory��������ꍇ
            else
            {
                var item = GetItem(swapped);
                SetItem(input.inputItem, swapped);
                input.inputInventory.SetItem(item, input.index);
            }
        }

        public void DeleteItem(int index)
        {
            var nullItem = new NullItem(-1);
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
