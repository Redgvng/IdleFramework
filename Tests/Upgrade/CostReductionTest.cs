using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IdleLibrary;
using IdleLibrary.Upgrade;
using NUnit.Framework;

namespace Tests
{
    public class CostReductionTest
    {
        [Test]
        public void CanCostReduction()
        {
            var cost = new LinearCost(100, 0, new Level());
            cost.multiplier.RegisterMultiplier(new MultiplierInfo(() => 0.9, MultiplierType.mul));

            Assert.AreEqual(90, cost.Cost);
        }
    }
}