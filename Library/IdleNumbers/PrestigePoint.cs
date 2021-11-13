using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace IdleLibrary
{
    public interface IPrestigePoint : INumber
    {
        double TempNumber { get; }
    }
    public class ProducedPrestigePoint : IProducableNumber, IDecrementableNumber, IPrestigePoint
    {
        public double Number { get; private set; }
        public double TempNumber { get; private set; }
        private readonly Func<double> func;
        public ProducedPrestigePoint(Func<double> func)
        {
            this.func = func;
        }
        public void Decrement(double decrement = 1)
        {
            Number -= decrement;
        }
        public void OnPrestige()
        {
            Number += TempNumber;
            TempNumber = 0;
        }
        public void ProducePerSecond() => TempNumber += _produceAmount();
        private double _produceAmount() => func();
    }
}
