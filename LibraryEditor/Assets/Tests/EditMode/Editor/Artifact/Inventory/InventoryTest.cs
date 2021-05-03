﻿using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using IdleLibrary.Inventory;
using System.Linq;
using IdleLibrary;

namespace Tests
{
    public class InventoryTest
    {
        InputItem input = new InputItem();
        [Test]
        public void CanSetItem()
        {
            var inventory = new Inventory(input);

            inventory.SetItem(new Item(3), 2);

            Assert.IsTrue(inventory.GetItem(2).id != -1);
            Assert.AreEqual(inventory.GetItem(2).id ,3);
        }

        [Test]
        public void CanSetItemAfterExpandInventory()
        {
            var inventory = new Inventory(input);
            inventory.ExpandInventory();
            Debug.Log(inventory.GetInventoryLength());

            inventory.SetItem(new Item(0), 10);

            Assert.AreEqual(inventory.GetItem(10).id, 0);
        }

        [Test]
        public void CanSetItemByOrder()
        {
            var inventory = new Inventory(input);
            var item = new Item(1);

            inventory.SetItem(item, 0);
            inventory.SetItem(item, 3);
            inventory.SetItem(item, 4);
            inventory.SetItem(item, 6);
            inventory.SetItemByOrder(item);
            inventory.SetItemByOrder(item);
            inventory.SetItemByOrder(item);
            inventory.SetItemByOrder(item);

            Assert.AreEqual(inventory.GetItem(1).id, 1);
            Assert.AreEqual(inventory.GetItem(2).id, 1);
            Assert.AreEqual(inventory.GetItem(5).id, 1);
            Assert.AreEqual(inventory.GetItem(7).id, 1);
        }

        [Test]
        public void CanSwapItemToEmpty()
        {
            var inventory = new Inventory(input);
            var item = new Item(3);
            inventory.SetItem(item, 2);

            inventory.SwapItem(1, 2);

            Assert.AreEqual(3,inventory.GetItem(1).id);
            Assert.AreEqual(-1,inventory.GetItem(2).id);
        }
        [Test]
        public void CanSwapItemAfterExpand()
        {
            var Input = new InputItem();
            var inventory = new Inventory(Input);
            var item = new Item(3);
            inventory.ExpandInventory();
            inventory.SetItem(item, 2);
            var swap = new SwapItem(inventory,Input);

            inventory.RegisterItem(2);
            swap.Action(10);

            Assert.AreEqual(-1,inventory.GetItem(2).id);
            Assert.AreEqual(3,inventory.GetItem(10).id);
        }
        [Test]
        public void CanSwapItemToNotEmpty()
        {
            var inventory = new Inventory(input);
            var item = new Item(3);
            var item2 = new Item(1);
            inventory.SetItem(item, 2);
            inventory.SetItem(item2, 1);

            inventory.SwapItem(1, 2);

            Assert.AreEqual(inventory.GetItem(1).id, 3);
            Assert.AreEqual(inventory.GetItem(2).id, 1);
        }
        [Test]
        public void CanSwapItemFromInputItem()
        {
            var Input = new InputItem();
            var inventory = new Inventory(Input);
            var item = new Item(3);
            var item2 = new Item(1);
            inventory.SetItem(item, 2);
            inventory.SetItem(item2, 1);
            inventory.RegisterItem(2);

            inventory.SwapItem(1, Input);

            Assert.AreEqual(inventory.GetItem(1).id, 3);
            Assert.AreEqual(inventory.GetItem(2).id, 1);
        }

        [Test]
        public void CanDeleteItem()
        {
            var inventory = new Inventory(input);
            var item = new Item(3);
            inventory.SetItem(item, 1);

            inventory.DeleteItem(1);

            Assert.IsFalse(inventory.GetItem(1).isSet);
        }

        [Test]
        public void CanRegisterItem()
        {
            var inventory = new Inventory(input);
            var item = new Item(3);
            inventory.SetItem(item, 1);

            inventory.RegisterItem(1);

            Assert.AreEqual(1, inventory.input.inputItem.inputInfo.index);
            Assert.IsTrue(inventory.input.inputItem.inputInfo.inputInventory == inventory);
        }

        [Test]
        public void CannotRegisterItemIfItemIsEmpty()
        {
            var inventory = new Inventory(input);
            var item = new Item(3);

            inventory.RegisterItem(1);

            Assert.AreEqual(-1, inventory.input.inputItem.id);
        }

        [Test]
        public void CanReleaseItem()
        {
            var Input = new InputItem();
            var inventory = new Inventory(Input);
            var item = new Item(3);
            inventory.SetItem(item, 1);
            inventory.RegisterItem(1);
            Assert.AreEqual(3, Input.inputItem.id);

            Input.ReleaseItem();

            Assert.AreEqual(-1, Input.inputItem.id);
        }

        [Test]
        public void CanSetItemFromOtherInventory()
        {
            var Input = new InputItem();
            var inventory = new Inventory(Input);
            var inventory2 = new Inventory(Input);
            inventory2.SetItemByOrder(new Item(5));
            inventory.SetItemByOrder(new Item(3));
            var swap = new SwapItem(inventory,Input);

            inventory2.RegisterItem(0);
            swap.Action(0);

            Assert.AreEqual(3,inventory2.GetItem(0).id);
            Assert.AreEqual(5,inventory.GetItem(0).id);
        }

        [Test]
        public void CannotSwapItemWithinSameInventory()
        {
            var Input = new InputItem();
            var inventory = new Inventory(Input);
            inventory.SetItemByOrder(new Item(3));
            inventory.SetItemByOrder(new Item(4));
            var swap = new SwapItem(inventory, Input);

            inventory.RegisterItem(1);
            swap.Action(0);

            Assert.AreEqual(4,inventory.GetItem(0).id);
            Assert.AreEqual(3, inventory.GetItem(1).id);
        }

        [Test]
        public void CanRevertItemToOtherInventory()
        {
            var inventory = new Inventory(input);
            var inventory2 = new Inventory(input);
            inventory2.SetItemByOrder(new Item(1));
            var revert = new RevertItemToOtherInventory(inventory2, inventory);

            revert.Action(0);

            Assert.AreEqual(-1, inventory2.GetItem(0).id);
            Assert.AreEqual(1, inventory.GetItem(0).id);    
        }

        [Test]
        public void CannotDeleteWhenItemIsLocked()
        {
            var inventory = new Inventory(input);
            var item = new Item(0);
            var lockItem = new LockItem(inventory);
            inventory.SetItemByOrder(item);
            var deleteItem = new DeleteItem(inventory);

            lockItem.Action(0);
            deleteItem.Action(0);

            Assert.AreEqual(0, inventory.GetItem(0).id);
        }

        [Test]
        public void CannotLockWithNullItem()
        {
            var inventory = new Inventory(input);
            var lockItem = new LockItem(inventory);

            lockItem.Action(0);

            Assert.IsFalse(inventory.GetItem(0).inputInfo.isLocked);
        }

    }
}
