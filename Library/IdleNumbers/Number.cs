using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;

namespace IdleLibrary
{
    public interface IMultiplier
    {
        Multiplier multiplier { get; }
    }

    public interface INumber
    {
        double Number { get; }
        void Increment(double increment);
        void Decrement(double decrement);
    }

    //こいつら一旦統計量持てない
    [Serializable]
    public class NUMBER : IMultiplier,INumber
    {
        public double Number { get => _Number; private set => _Number = value; }
        [SerializeField] double _Number;
        public double TotalNumber;
        public Multiplier multiplier {
            get
            {
                if (_multiplier == null) return new Multiplier();
                return _multiplier;
            }
            set => _multiplier = value;
        }
        public NUMBER(double initialNumber) { Number = initialNumber; }
        public NUMBER() { }
        private Multiplier _multiplier;
        public virtual void Increment(double increment = 1)
        {
            Number += multiplier.CaluculatedNumber(increment);
            TotalNumber += multiplier.CaluculatedNumber(increment);
        }
        public void IncrementFixNumber(double fixIncrement)
        {
            Number += fixIncrement;
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
    public class AsyncNumber : IMultiplier, INumber
    {
        public double Number { get => GainedNumber - ConsumedNumber; }
        protected virtual double GainedNumber { get; }
        protected virtual double ConsumedNumber { get; set; }
        public Multiplier multiplier
        {
            get
            {
                if (_multiplier == null) return new Multiplier();
                return _multiplier;
            }
            set => _multiplier = value;
        }
        private Multiplier _multiplier;
        public virtual void Increment(double increment = 1)
        {
            
        }
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

