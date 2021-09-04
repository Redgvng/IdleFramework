using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace IdleLibrary
{
    [System.Serializable]
    public class SaveR
    {
        public string ascendTime;
        public double distanceConsumed;
        public float currentTime;

        //UpgradeLevel
        public long[] upgradeLevels;
    }
}
