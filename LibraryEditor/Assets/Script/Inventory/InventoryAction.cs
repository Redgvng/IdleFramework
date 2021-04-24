using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdleLibrary.Inventory
{
    public interface IInventoryAction
    {
        void Action(int index);
    }

    public class SwapItemWithSameInventory : IInventoryAction
    {
        private readonly Inventory inventory;
        public SwapItemWithSameInventory(Inventory inventory)
        {
            this.inventory = inventory;
        }
        public void Action(int index)
        {
            if (inventory.inputItem.inputItem.id != -1)
            {
                inventory.SwapItem(index, inventory.inputItem);
                inventory.inputItem.ReleaseItem();
                return;
            }
            inventory.RegisterItem(index);
        }
    }

    public class SwapItemWithOtherInventory : IInventoryAction
    {
        private readonly Inventory originalInventory;
        private readonly Inventory otherInventory;
        private InputItem input;
        public SwapItemWithOtherInventory(Inventory originalInventory, Inventory otherInventory)
        {
            this.originalInventory = originalInventory;
            this.otherInventory = otherInventory;
            input = originalInventory.inputItem;
        }
        public void Action(int index)
        {
            if (!input.inputItem.isSet)
            {
                originalInventory.RegisterItem(index);
                return;
            }

            Debug.Log("う");

            //もしinputアイテムとswap先が違うインベントリだったら
            if (input.inputInventory != originalInventory)
            {

                originalInventory.SwapItemFromOtherInventory(otherInventory, index, originalInventory.inputItem);
              　input.ReleaseItem();
               return;
            }
            //同じインベントリ内でやってるのであれば
            else
            {

                Debug.Log("こ");
                originalInventory.SwapItem(index, originalInventory.inputItem);
                input.ReleaseItem();
                return;
            }
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
            if (inventory.inputItem.inputItem.id == -1)
                inventory.DeleteItem(index);
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
}
