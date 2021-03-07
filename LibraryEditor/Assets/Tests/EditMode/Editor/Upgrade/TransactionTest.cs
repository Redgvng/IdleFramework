using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UpgradeLibrary;

namespace Tests
{
    public class TransactionTest
    {
        NUMBER gold = new NUMBER();
        // A Test behaves as an ordinary method
        [Test]
        public void ShouldPayCost()
        {
            gold.Number = 100;
            Transaction transaction = new Transaction(gold, new LinearCost(1, 2, 1));
            //レベルに2を入れると1 + 2 * 1 = 3
            transaction.Pay();
            Assert.AreEqual(97, gold.Number);
        }

        [Test]
        public void CheckIfCannotPay()
        {
            gold.Number = 10;
            Transaction transaction = new Transaction(gold, new LinearCost(1, 2, 10));
            Assert.AreEqual(false, transaction.CanBuy());
        }

        [Test]
        public void CheckIfCanPay()
        {
            gold.Number = 10;
            Transaction transaction = new Transaction(gold, new LinearCost(1, 2, 0));
            Assert.AreEqual(true, transaction.CanBuy());
        }

        [Test]
        public void ShouldDoublePay()
        {
            NUMBER resourse1 = new NUMBER();
            resourse1.Number = 50;
            NUMBER resourse2 = new NUMBER();
            resourse2.Number = 50;
            ITransaction doubleTransaction = new MultipleTransaction(new Transaction[]
            {
                new Transaction(resourse1, new LinearCost(1,2, 0)),
                new Transaction(resourse2, new LinearCost(2,3, 0))
            });
            //１回買ったら減っているか？
            doubleTransaction.Pay();
            Assert.AreEqual(resourse1.Number, 49);
            Assert.AreEqual(resourse2.Number, 48);
        }

        [Test]
        public void DoubleCheckIfCannot()
        {
            NUMBER resourse1 = new NUMBER();
            resourse1.Number = 50;
            NUMBER resourse2 = new NUMBER();
            resourse2.Number = 1;
            ITransaction doubleTransaction = new MultipleTransaction(new Transaction[]
            {
                new Transaction(resourse1, new LinearCost(1,2, 0)),
                new Transaction(resourse2, new LinearCost(2,3, 0))
            }); 
            //一個も買えないはず
            Assert.AreEqual(doubleTransaction.CanBuy(), false);
        }
    }
}
