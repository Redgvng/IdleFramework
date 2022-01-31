using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace IdleLibrary
{
    public interface IPrestigePoint : INumber
    {
        double TempNumber { get; }
        void OnPrestige();
        void Reset();
    }
    public class ProducedPrestigePoint : IProducableNumber, IDecrementableNumber, IPrestigePoint, IStatsNumber
    {
        public virtual double Number { get; protected set; }
        public virtual double TempNumber { get; protected set; }
        public virtual double MaxNumber { get; protected set; }
        public virtual double TotalNumber { get; protected set; }
        protected Func<double> func;
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
            TotalNumber += TempNumber;
            if (MaxNumber < Number) MaxNumber = Number;
            TempNumber = 0;
        }
        public void ProduceBySecond(long second) => TempNumber += _produceAmount() * second;
        public void Reset()
        {
            Number = 0;
            TempNumber = 0;
        }
        public void ProducePerSecond() => TempNumber += _produceAmount();
        public void ProducePerFrame() => TempNumber += _produceAmount() * Time.fixedDeltaTime;
        private double _produceAmount() => func();
    }

    public class NormalPrestigePoint : IIncrementableNumber, IDecrementableNumber, IPrestigePoint, IStatsNumber
    {
        public virtual double Number { get; protected set; }
        public virtual double TempNumber { get; protected set; }
        public virtual double MaxNumber { get; protected set; }
        public virtual double TotalNumber { get; protected set; }
        public NormalPrestigePoint()
        {
         
        }
        public void Increment(double increment)
        {
            TempNumber += increment;
        }
        public void Decrement(double decrement = 1)
        {
            Number -= decrement;
        }
        public void OnPrestige()
        {
            Number += TempNumber;
            TotalNumber += TempNumber;
            if (MaxNumber < Number) MaxNumber = Number;
            TempNumber = 0;
        }
        public void Reset()
        {
            Number = 0;
            TempNumber = 0;
        }
    }
}
