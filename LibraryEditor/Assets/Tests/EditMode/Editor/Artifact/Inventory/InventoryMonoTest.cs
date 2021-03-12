using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using InventoryLibrary;

namespace Tests
{
    public class ItemMonoTest
    {
        ItemTest[] saveArray = new ItemTest[itemNum];
        const int itemNum = 100;
        [Test]
        public void CanSetItem()
        {
            int index = 3;

            var itemController = new ItemContollerTestForMono<ItemTest>(index, saveArray);
            var item = new ItemTest(5);
            itemController.Create(item);
            Assert.IsTrue(saveArray[3].id != 0);
            Assert.AreEqual(5, saveArray[3].id);
        }

        [Test]
        public void CanDeleteItem()
        {
            int index = 3;

            var itemController = new ItemContollerTestForMono<ItemTest>(index, saveArray);
            itemController.Delete();
            Debug.Log(itemController.GetItem().id != 0);
            Assert.AreEqual(0, itemController.GetItem().id);
        }
    }
}
