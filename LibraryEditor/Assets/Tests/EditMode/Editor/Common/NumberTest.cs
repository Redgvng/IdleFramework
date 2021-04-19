using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using IdleLibrary.Upgrade;
using IdleLibrary;

namespace Tests
{
    public class NumberTest
    {
        [Test]
        public void CanGetNumber()
        {
            NUMBER number = new NUMBER();
            double temp = number.Number;
        }

        [Test]
        public void CanSetNumberWithSetClass()
        {
            NUMBER number = new NUMBER(3);
            number.IncrementNumber(1);
            Assert.AreEqual(4, number.Number);
        }
    }
}
