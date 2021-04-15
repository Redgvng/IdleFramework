using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

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
    public class NUMBER : IComparable<NUMBER>, IMultiplier
    {
        //Public
        public int NumberAsInt => (int)Number;
        public virtual double Number { get => setNumber.GetItem(); set => setNumber.SetItem(value); }
        public virtual double TotalNumber { get; set; }
        public Multiplier multiplier { get; }
        public bool CanSet => true;

        //Private
        private ISetItem<double> setNumber;

        //ユニットテスト用コンストラクタ
        public NUMBER(double initialValue = 0)
        {
            Number = initialValue;
            multiplier = new Multiplier();
            this.setNumber = new NullSetItem<double>();
        }

        //本番用コンストラクタ
        public NUMBER(NumbersName Name, IDataContainer<NUMBER> dataContainer, ISetItem<double> setNumber = null)
        {
            dataContainer.SetDataByName(this, Name);  
            multiplier = new Multiplier();
            this.setNumber = setNumber == null ? new NullSetItem<double>() : setNumber;
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

