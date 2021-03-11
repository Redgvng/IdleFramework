using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using InventoryLibrary;

namespace Tests
{
    public class ItemMonoTest : IPrebuildSetup
    {
        ItemTest[] saveArray = new ItemTest[itemNum];
        ItemTest nullItem = new ItemTest(-1);
        const int itemNum = 100;

        public void Setup()
        {
            nullItem = new ItemTest(-1);
        }

        [Test]
        public void CanSetItem()
        {
            int index = 3;

            var itemController = new ItemContollerTestForMono<ItemTest>(index, saveArray, nullItem);
            var item = new ItemTest(5);
            itemController.Create(item);
            Assert.IsTrue(saveArray[3] != null);
            Assert.AreEqual(5, saveArray[3].id);
        }
        [Test]
        public void CanDeleteItem()
        {
            int index = 3;

            var itemController = new ItemContollerTestForMono<ItemTest>(index, saveArray, nullItem);
            itemController.Delete();
            Debug.Log(itemController.GetItem() == null);
            Assert.AreEqual(-1, itemController.GetItem().id);
        }
    }
}
