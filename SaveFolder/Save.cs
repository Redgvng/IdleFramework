using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace IdleLibrary
{
    [System.Serializable]
    public partial class Save
    {
        public string lastTime;
        public string birthDate;
        public bool isContinuePlay;
        public double allTime;

        public float SEVolume;
        public float BGMVolume;

        public long ascendPoint;
        //NUMBER
        public double[] numbers;

        public bool isEarlyAccess;
        public bool DEBUG_FORCED_REWARDED;
        public bool isScentific;
        public long refundedNum;
    }
}
