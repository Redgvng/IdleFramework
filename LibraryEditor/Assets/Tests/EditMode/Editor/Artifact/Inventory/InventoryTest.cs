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
            var creates  = Enumerable.Range(0, 100).Select(_ => new CreateItem<ItemTest>(new NullSetItem<ItemTest>())).ToArray();
            var create = new CreateItemByOrder<ItemTest>(creates);
            creates[0].Create(new ItemTest(1));
            creates[1].Create(new ItemTest(1));
            create.Create(new ItemTest(2));
            Assert.IsTrue(creates[2] != null);
            Assert.IsFalse(creates[2].CanSet);
            Assert.IsFalse(creates.ToList().All(x => x != null));
        }
        [Test]
        public void CanCreateItemFromInventoryByOrderWithSaveArray()
        {
            var saveArray = new Item[100];
            var creates = Enumerable.Range(0, 100).Select(id => new CreateItem<Item>(new SetItemToSave<Item>(id,saveArray))).ToArray();
            var create = new CreateItemByOrder<Item>(creates);
            creates[0].Create(new Item(1));
            creates[1].Create(new Item(1));
            create.Create(new Item(2));
            Assert.AreEqual(1, saveArray[0].id);
            Assert.AreEqual(1, saveArray[1].id);
            Assert.AreEqual(2, saveArray[2].id);
        }
        [Test]
        public void CanSwapItem()
        {
            var items = Enumerable.Range(0, 100).Select(id => new NullSetItem<ItemTest>()).ToArray();
            items[3].SetItem(new ItemTest(1));
            items[5].SetItem(new ItemTest(2));
            //3と5を入れ替えます。
            var swap = new SwapItem<ItemTest>(items[3]);
            swap.Stack(items[5]);
            Assert.AreEqual(1, items[5].GetItem().id);
            Assert.AreEqual(2, items[3].GetItem().id);
        }
        class dummy : IStackItem<ItemTest>, ISetItem<ItemTest>
        {
            IStackItem<ItemTest> stack;
            ISetItem<ItemTest> set;
            public dummy(IStackItem<ItemTest> stack , ISetItem<ItemTest> set)
            {
                this.stack = stack;
                this.set = set;
            }
            public void Stack(ISetItem<ItemTest> item)
            {
                stack.Stack(item);
            }

            public void SetItem(ItemTest item)
            {
                set.SetItem(item);
            }

            public ItemTest GetItem()
            {
                return set.GetItem();
            }
        }
        [Test]
        public void CanSwapItemFromInventory()
        {
            var swap = new SwapItemFromInventory<dummy, ItemTest>();
            var set1 = new NullSetItem<ItemTest>();
            var set2 = new NullSetItem<ItemTest>();
            var item1 = new dummy(new SwapItem<ItemTest>(set1), set1);
            var item2 = new dummy(new SwapItem<ItemTest>(set2), set2);
            item1.SetItem(new ItemTest(1));
            item2.SetItem(new ItemTest(2));
            swap.Click(item1);
            //これで1が登録された。
            swap.Click(item2);
            //1と2がひっくり返るはず
            Assert.AreEqual(1, item2.GetItem().id);
            Assert.AreEqual(2, item1.GetItem().id);
        }
        [Test]
        public void CanSwapItemWithNullItem()
        {
            var items = Enumerable.Range(0, 100).Select(id => new NullSetItem<ItemTest>()).ToArray();
            items[3].SetItem(new ItemTest(1));
            //3と5を入れ替えます。ただし、5は何も入ってません。
            var swap = new SwapItem<ItemTest>(items[3]);
            swap.Stack(items[5]);
            Assert.AreEqual(1, items[5].GetItem().id);
            Assert.AreEqual(0, items[3].GetItem().id);
        }
        [Test]
        public void CanExpandSlot()
        {

        }
    }
}
