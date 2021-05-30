using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace IdleLibrary.Inventory
{
    public interface IInventoryAction
    {
        void Action(int index);
    }

    public class DoActionWithSomeKey : IInventoryAction
    {
        private readonly IInventoryAction action;
        private readonly KeyCode key;
        public DoActionWithSomeKey(IInventoryAction action, KeyCode key)
        {
            this.action = action;
            this.key = key;
        }
        public void Action(int index)
        {
            if (!Input.GetKey(key))
                return;

            action.Action(index);
        }
    }

    //あらゆるアイテムをスワップします
    public class SwapItem : IInventoryAction
    {
        private readonly Inventory inventory;
        public SwapItem(Inventory inventory)
        {
            this.inventory = inventory;
        }
        public void Action(int index)
        {
            if (inventory.input.inputItem.id != -1)
            {
                inventory.SwapItem(index, inventory.input);
                inventory.input.ReleaseItem();
                return;
            }
            inventory.RegisterItem(index);
        }
    }
    public class StackItem : IInventoryAction
    {
        private readonly Inventory inventory;
        public StackItem(Inventory inventory)
        {
            this.inventory = inventory;
        }
        public void Action(int index)
        {
            isActionScceeded = false;
            if (inventory.input.inputItem is IStackableItem && inventory.GetItem(index) is IStackableItem)
            {
                var stackable = inventory.GetItem(index) as IStackableItem;
                if (stackable.CanStack(inventory.input.inputItem))
                {
                    stackable.Stack(inventory.input.inputItem);
                    inventory.input.ReleaseItem();
                    inventory.input.inputInventory.DeleteItem(inventory.input.index);
                    isActionScceeded = true;
                }
            }
        }
        public bool isActionScceeded;
    }
    //まずstack判定を行い、その後スワップを行います。インベントリの標準搭載機能？
    public class StackAndSwapItem : IInventoryAction
    {
        private readonly Inventory inventory;
        private readonly SwapItem swap;
        private readonly StackItem stack;
        public StackAndSwapItem(Inventory inventory)
        {
            this.inventory = inventory;
            swap = new SwapItem(inventory);
            stack = new StackItem(inventory);
        }
        public void Action(int index)
        {
            stack.Action(index);
            if(!stack.isActionScceeded)
                swap.Action(index);
        }
    }

    public class RevertItemToOtherInventory : IInventoryAction
    {
        private readonly Inventory inventory;
        private readonly Inventory revertedInventory;
        public RevertItemToOtherInventory(Inventory inventory, Inventory revertedInventory)
        {
            this.inventory = inventory;
            this.revertedInventory = revertedInventory;
        }
        public void Action(int index)
        {
            if (revertedInventory.isFull)
            {
                Debug.LogError("アイテムがいっぱいです");
                return;
            }
            var item = inventory.GetItem(index);
            revertedInventory.SetItemByOrder(item);
            inventory.DeleteItem(index);
        }
    }

    public class DeleteItem : IInventoryAction
    {
        private readonly Inventory inventory;
        public DeleteItem(Inventory inventory)
        {
            this.inventory = inventory;
        }
        public bool CanDelete(int index)
        {
            return !inventory.GetItem(index).isLocked && inventory.input.inputItem.id == -1;
        }
        public void Action(int index)
        {
            if (CanDelete(index))
                inventory.DeleteItem(index);
        }
    }

    public class LockItem : IInventoryAction
    {
        private readonly Inventory inventory;
        public LockItem(Inventory inventory)
        {
            this.inventory = inventory;
        }
        public void Action(int index)
        {
            if (!inventory.GetItem(index).isSet)
            {
                return;
            }

            if (inventory.input.inputItem.id == -1)
            {
                inventory.GetItem(index).isLocked = true;
            }
        }
    }

    public class Releaseitem : IInventoryAction
    {
        private readonly InputItem input;
        public Releaseitem(InputItem input)
        {
            this.input = input;
        }
        public void Action(int index)
        {
            if(input.inputItem.id != -1)
            {
                input.ReleaseItem();
            }
        } 
    }

    public class ShowInfoToTextField : IInventoryAction
    {
        private readonly Inventory inventory;
        private readonly TextMeshProUGUI textField;
        public ShowInfoToTextField(Inventory inventory, TextMeshProUGUI textField)
        {
            this.inventory = inventory;
            this.textField = textField;
            textField.text = "";
        }
        public void Action(int index)
        {
            textField.text = inventory.GetItem(index).Text();
        }
    }

    public class ShowItemEffectToTextField : IInventoryAction
    {
        private readonly Inventory inventory;
        private readonly TextMeshProUGUI textField;
        public ShowItemEffectToTextField(Inventory inventory, TextMeshProUGUI textField)
        {
            this.inventory = inventory;
            this.textField = textField;
            textField.text = "";
        }
        public void Action(int index)
        {
            textField.text = inventory.GetItem(index).Text();
        }
        
        //アイテムのeffectを表示します
        string Text()
        {
            string text = "";
            return text;
        }
    }

}
