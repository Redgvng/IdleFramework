using System;
using System.Collections.Generic;
using UnityEngine;

namespace IdleLibrary
{
    public interface IMultiplier
    {
        Multiplier multiplier { get; }
    }

    public interface INumber
    {
        double Number { get; }
    }

    public interface IIncrementableNumber : INumber
    {
        void Increment(double increment);
    }

    public interface IDecrementableNumber : INumber
    {
        void Decrement(double decrement);
    }

    public interface IProducableNumber : INumber
    {
        void ProducePerSecond();
        void ProducePerFrame();
    }

    [Serializable]
    public class NUMBER : IMultiplier, IIncrementableNumber, IDecrementableNumber
    {
        public virtual double Number { get; set; }
        public double TotalNumber;
        public double MaxNumber;
        public Multiplier multiplier { get; } = new Multiplier();
        public virtual void Increment(double increment = 1)
        {
            Number += multiplier.CaluculatedNumber(increment);
            TotalNumber += multiplier.CaluculatedNumber(increment);
            if (MaxNumber <= TotalNumber) MaxNumber = TotalNumber;
        }
        public void IncrementFixNumber(double fixIncrement)
        {
            var increment = fixIncrement;
            Number += increment;
            TotalNumber += increment;
            if (MaxNumber <= TotalNumber) MaxNumber = TotalNumber;
        }
        public virtual void Decrement(double decrement = 1)
        {
            Number = Math.Max(Number - decrement, 0);
        }
        public void ResetNumberToZero()
        {
            Number = 0;
        }
    }

    //これを使う場合、通常GainedNumberが関数となるはず。それ以外の場合はクラスを作り直した方がいいかも
    [Serializable]
    public class AsyncNumber : IMultiplier, IDecrementableNumber
    {
        public double Number { get => GainedNumber - ConsumedNumber; }
        protected virtual double GainedNumber { get; }
        protected virtual double ConsumedNumber { get; set; }
        public Multiplier multiplier { get; } = new Multiplier();
        public virtual void Decrement(double decrement = 1)
        {
            if(Number < decrement)
            {
                throw new Exception("DecrementがNumberより大きいです");
            }
            ConsumedNumber += decrement;
        }
    }

}

