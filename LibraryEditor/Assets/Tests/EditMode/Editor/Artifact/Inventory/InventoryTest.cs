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
            var itemCtrl = new ItemContollerTest();
            var item = new ItemTest(1);
            itemCtrl.SetItem(item);
            Assert.IsTrue(itemCtrl.GetItem().id == 1);
            Assert.IsTrue(itemCtrl.IsItemSet);
        }
        [Test]
        public void CannotSetItemIfAlreadyHave()
        {
            var itemCtrl = new ItemContollerTest();
            var item = new ItemTest(1);
            itemCtrl.SetItem(item);
            var item2 = new ItemTest(2);
            itemCtrl.SetItem(item2);
            Assert.IsFalse(itemCtrl.GetItem().id == 2);
        }
        [Test]
        public void CanSwapItem()
        {
            IItem[] items = new IItem[10];
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = new Item(i);
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
                items[i] = new Item(i);
            }
            items[7].id = -1;
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
                items[i] = new Item(i);
            }
            var deleteItem = new DeleteItem();
            deleteItem.Delete(items, 3);
            Assert.IsTrue(items[3].id == -1);
        }
    }
}
