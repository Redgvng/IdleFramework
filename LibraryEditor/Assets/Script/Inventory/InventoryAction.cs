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

    public class SwapItem : IInventoryAction
    {
        private readonly Inventory inventory;
        private readonly InputItem inputItem;
        public SwapItem(Inventory inventory, InputItem inputItem)
        {
            this.inventory = inventory;
            this.inputItem = inputItem;
        }
        public void Action(int index)
        {
            if (inventory.input.inputItem.id != -1)
            {
                inventory.SwapItem(index, inventory.input);
                inputItem.ReleaseItem();
                return;
            }
            inventory.RegisterItem(index);
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
                Debug.LogError("ƒAƒCƒeƒ€‚ª‚¢‚Á‚Ï‚¢‚Å‚·");
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
        public void Action(int index)
        {
            if (inventory.GetItem(index).inputInfo.isLocked)
                return;

            if (inventory.input.inputItem.id == -1)
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
                inventory.GetItem(index).inputInfo.isLocked = true;
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

}
