using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using System.Linq;
using IdleLibrary;
using IdleLibrary.Upgrade;
using IdleLibrary.IntegrationTest;
using System;
using System.IO;
using static IdleLibrary.IntegrationTest.IntegrationUtiliity;

public class ExponentialIdle : MonoBehaviour
{
    Parameter dt = new Parameter(1.0);
    Parameter t = new Parameter(0);
    Parameter f = new Parameter(1.0);
    Parameter db = new Parameter(0);
    Parameter b = new Parameter(0);
    Parameter x = new Parameter(0);

    [Test]
    public void Integration()
    {

    }
}
