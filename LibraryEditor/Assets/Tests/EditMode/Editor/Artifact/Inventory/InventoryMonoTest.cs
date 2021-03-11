using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using InventoryLibrary;

namespace Tests
{
    public class InventoryMonoTest : IPrebuildSetup
    {
        ItemTest[] saveArray = new ItemTest[itemNum];
        ItemTest nullItem;
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
    }
}
