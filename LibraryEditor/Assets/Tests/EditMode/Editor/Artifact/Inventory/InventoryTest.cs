using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using InventoryLibrary;
using System.Linq;

namespace Tests
{
    public class InventoryTest
    {
        [Test]
        public void CanCreateItemFromInventoryByOrder()
        {
            var creates  = Enumerable.Range(0, 100).Select(_ => new CreateItem<ItemTest>(new NullSetItem<ItemTest>(new ItemTest(-1)))).ToArray();
            var create = new CreateItemByOrder<ItemTest>(creates);
            creates[0].Create(new ItemTest(1));
            creates[1].Create(new ItemTest(1));
            create.Create(new ItemTest(2));
            Assert.IsTrue(creates[2] != null);
            Assert.IsFalse(creates[2].CanSet);
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

    }
}
