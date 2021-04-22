using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdleLibrary.Inventory
{
    public interface IInventoryAction
    {
        void Action(int index);
    }

    public class SwapItem : IInventoryAction
    {
        private readonly Inventory inventory;
        public SwapItem(Inventory inventory)
        {
            this.inventory = inventory;
        }
        public void Action(int index)
        {
            if (inventory.InputId != -1)
            {
                inventory.SwapItem(index, inventory.InputId);
                inventory.ReleaseItem();
                return;
            }
            inventory.RegisterItem(index);
        }
    }

    public class SwapItemWithOtherInventory : IInventoryAction
    {
        private readonly Inventory originalInventory;
        private readonly Inventory otherInventory;
        public SwapItemWithOtherInventory(Inventory originalInventory, Inventory otherInventory)
        {
            this.originalInventory = originalInventory;
            this.otherInventory = otherInventory;
        }
        public void Action(int index)
        {
            if(originalInventory.InputId != -1)
            {
                originalInventory.SwapItemFromOtherInventory(otherInventory, originalInventory.InputId, index);
                originalInventory.ReleaseItem();
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
            if (inventory.InputId == -1)
                inventory.DeleteItem(index);
        }
    }

    public class Releaseitem : IInventoryAction
    {
        private readonly Inventory inventory;
        public Releaseitem(Inventory inventory)
        {
            this.inventory = inventory;
        }
        public void Action(int index)
        {
            if(inventory.InputId != -1)
            {
                inventory.ReleaseItem();
            }
        } 
    }
}
