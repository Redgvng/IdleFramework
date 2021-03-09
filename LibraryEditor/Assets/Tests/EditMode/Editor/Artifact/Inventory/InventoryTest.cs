using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using InventoryLibrary;

namespace Tests
{
    public class InventoryTest
    {
        //class DummyItem : IItem { }
        [Test]
        public void CanSetItem()
        {
            IItem[] items = new IItem[10];
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = new NullItem();
            }
            SetItemClass set = new SetItemClass(items);
            set.SetItem(new Item(), 3);
            Assert.IsFalse(items[3] is NullItem || items[3] == null);
        }
        [Test]
        public void CannotSetItemIfAlreadyHave()
        {
            IItem[] items = new IItem[10];
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = new NullItem();
            }
            items[3] = new Item();
            SetItemClass set = new SetItemClass(items);
            set.SetItem(new Item(), 3);
            Assert.IsFalse(items[0] is NullItem);
            Assert.IsTrue(items[1] is NullItem);
        }
        [Test]
        public void CanSwapItem()
        {
            IItem[] items = new IItem[10];
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = new Item();
                items[i].id = i;
            }
            var arrangeItemClass = new SwapItemClass();
            arrangeItemClass.Stack(items, 3, 5);
            Assert.AreEqual(items[3].id, 5);
            Assert.AreEqual(items[5].id, 3);
        }
        [Test]
        public void CanSwapItemWithNullItem()
        {
            IItem[] items = new IItem[10];
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = new Item();
                items[i].id = i;
            }
            items[7] = new NullItem();
            var arrangeItemClass = new SwapItemClass();
            arrangeItemClass.Stack(items, 3, 7);
            Assert.AreEqual(items[3].id, -1);
            Assert.AreEqual(items[7].id, 3);
        }

        [Test]
        public void CanItemDeleted()
        {
            IItem[] items = new IItem[10];
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = new Item();
                items[i].id = i;
            }
            var deleteItem = new DeleteItem();
            deleteItem.Delete(items, 3);
            Assert.IsTrue(items[3] is NullItem);
        }
    }
}
