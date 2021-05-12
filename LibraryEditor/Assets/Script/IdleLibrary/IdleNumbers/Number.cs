using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;

namespace IdleLibrary
{
    public interface IName
    {
        string GetName();
    }
    public interface IMultiplier
    {
        Multiplier multiplier { get; }
    }
    //こいつら一旦統計量持てない
    [Serializable]
    public class NUMBER : IComparable<NUMBER>, IMultiplier
    {
        //Public
        public int NumberAsInt => (int)Number;
        [OdinSerialize] public virtual double Number { get; set; }
        [OdinSerialize] public virtual double TotalNumber { get; set; }
        public Multiplier multiplier {
            get
            {
                if (_multiplier == null) return new Multiplier();
                return _multiplier;
            }
            set => _multiplier = value;
        }
        private Multiplier _multiplier;
        public bool CanSet => true;

        //Private
        private ISetItem<double> setNumber;

        //ユニットテスト用コンストラクタ
        public NUMBER(double initialValue = 0)
        {
            Number = initialValue;
        }

        public virtual void IncrementNumber(double increment = 1, bool isNetValue = false)
        {
            Number += !isNetValue ? multiplier.CaluculatedNumber(increment) : increment;
            TotalNumber += !isNetValue ? multiplier.CaluculatedNumber(increment) : increment;
        }
        public virtual void DecrementNumber(double decrement = 1)
        {
            Number = Math.Max(Number - decrement, 0);
        }
        public int CompareTo(NUMBER other)
        {
            if (Number < other.Number)
            {
                return 1;
            }
            else if (Number > other.Number)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }

    /*
    public class CappedNumber : NUMBER
    {
        public Cap cap;
        public CappedNumber(Cap cap)
        {
            this.cap = cap;
        }

        //増やすときにチェック？
        public override void IncrementNumber(double increment, bool isNetValue = false)
        {
            Number += multiplier.CaluculatedNumber(increment);
            TotalNumber += multiplier.CaluculatedNumber(increment);
            cap.Check(this);
        }
    }
    */
}

