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
        [Test]
        public void CanSetItem()
        {
            var inventory = new Inventory();

            inventory.SetItem(new Item(3), 2);

            Assert.IsTrue(inventory.GetItem(2).id != -1);
            Assert.AreEqual(inventory.GetItem(2).id ,3);
        }

        [Test]
        public void CanSetItemByOrder()
        {
            var inventory = new Inventory();
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
            var inventory = new Inventory();
            var item = new Item(3);
            inventory.SetItem(item, 2);

            inventory.SwapItem(1, 2);

            Assert.AreEqual(inventory.GetItem(1).id, 3);
            Assert.AreEqual(inventory.GetItem(2).id, -1);
        }
        [Test]
        public void CanSwapItemToNotEmpty()
        {
            var inventory = new Inventory();
            var item = new Item(3);
            var item2 = new Item(1);
            inventory.SetItem(item, 2);
            inventory.SetItem(item2, 1);

            inventory.SwapItem(1, 2);

            Assert.AreEqual(inventory.GetItem(1).id, 3);
            Assert.AreEqual(inventory.GetItem(2).id, 1);
        }

        [Test]
        public void CanDeleteItem()
        {
            var inventory = new Inventory();
            var item = new Item(3);
            inventory.SetItem(item, 1);

            inventory.DeleteItem(1);

            Assert.IsFalse(inventory.GetItem(1).isSet);
        }

    }
}
