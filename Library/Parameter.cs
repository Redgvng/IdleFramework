using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Numerics;

namespace IdleLibrary
{
    public class Parameter : IProducableNumber
    {
        public double Number => Math.Pow(BaseAmount(), ExponentialAmount());
        public Multiplier multiplier { get; } = new Multiplier();
        public Multiplier exponentialMultiplier { get; } = new Multiplier();
        public Multiplier produceMultiplier { get; } = new Multiplier();
        public Multiplier independentMultiplier { get; } = new Multiplier();
        public double initialValue { get; }
        public Parameter(double initialValue)
        {
            this.initialValue = initialValue;
        }
        public double Produced { get; private set; }
        public void ProducePerTime(float time) => Produced += ProduceAmountPerSecond() * time;
        public double ProduceAmountPerSecond() => produceMultiplier.CaluculatedNumber(0);
        public double ExponentialAmount() => exponentialMultiplier.CaluculatedNumber(1);
        public double BaseAmount() => multiplier.CaluculatedNumber(initialValue) + Produced + independentMultiplier.CaluculatedNumber(0);
    }
}
