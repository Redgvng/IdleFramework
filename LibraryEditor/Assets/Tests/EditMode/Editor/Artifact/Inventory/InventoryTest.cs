using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using IdleLibrary.Inventory;
using System.Linq;
using IdleLibrary;
using IdleLibrary.Inventory;

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

            Assert.AreEqual(inventory.GetItem(1).id, 3);
            Assert.AreEqual(inventory.GetItem(2).id, -1);
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

            Assert.AreEqual(1, inventory.inputItem.index);
            Assert.IsTrue(inventory.inputItem.inputInventory == inventory);
        }

        [Test]
        public void CannotRegisterItemIfItemIsEmpty()
        {
            var inventory = new Inventory(input);
            var item = new Item(3);

            inventory.RegisterItem(1);

            Assert.AreEqual(-1, inventory.inputItem.index);
        }

        [Test]
        public void CanReleaseItem()
        {
            var inventory = new Inventory(input);
            var item = new Item(3);
            inventory.SetItem(item, 1);
            inventory.RegisterItem(1);

            inventory.ReleaseItem();

            Assert.AreEqual(-1, inventory.inputItem.inputItem.id);
        }

        [Test]
        public void CanSetItemFromOtherInventory()
        {
            var Input = new InputItem();
            var inventory = new Inventory(Input);
            var inventory2 = new Inventory(Input);
            inventory2.SetItemByOrder(new Item(5));
            inventory.SetItemByOrder(new Item(3));
            var swap = new SwapItemWithOtherInventory(inventory, inventory2);

            inventory.RegisterItem(0);
            inventory.SwapItemFromOtherInventory(inventory2, 0, Input);

            Assert.AreEqual(3,inventory2.GetItem(0).id);
            Assert.AreEqual(5,inventory.GetItem(0).id);
        }

        //inventory2を不要に代入しなければいけない問題
        [Test]
        public void CannotSwapItemWithinSameInventory()
        {
            var Input = new InputItem();
            var inventory = new Inventory(Input);
            var inventory2 = new Inventory(Input);
            inventory2.SetItemByOrder(new Item(5));
            inventory.SetItemByOrder(new Item(3));
            inventory.SetItemByOrder(new Item(4));

            inventory.RegisterItem(1);
            var swap = new SwapItemWithOtherInventory(inventory, inventory2);
            swap.Action(0);

            Assert.AreEqual(4,inventory.GetItem(0).id);
            Assert.AreEqual(3, inventory.GetItem(1).id);
        }

    }
}
