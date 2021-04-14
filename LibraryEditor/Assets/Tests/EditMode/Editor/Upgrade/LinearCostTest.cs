
using NUnit.Framework;
using System;
using UnityEngine.TestTools;
using IdleLibrary.Upgrade;
using IdleLibrary;

[TestFixture]
public class LinearCostTest : IPrebuildSetup
{
    Upgrade upgrade;
    ICost cost;
    NUMBER money;
    NUMBER dummy;
    class l : ILevel
    {
        public long level { get; set; }
        public l(long i) { level = i; }
    }

    [OneTimeSetUp]
    public void Setup()
    {

    }


}
