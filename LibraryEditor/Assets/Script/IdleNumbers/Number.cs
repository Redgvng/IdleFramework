using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

namespace CommonLibrary
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

        //テスト用コンストラクタ
        public NUMBER(double initialNumber = 0)
        {
            multiplier = new Multiplier();
            this.Number = initialNumber;
        }
        //基本的にはこのコンストラクタを使うこと。Mainに大きく依存
        public NUMBER(NumbersName Name, double initialNumber = 0)
        {
            DataContainer<IMultiplier>.GetInstance().SetDataByName(this, Name);
            DataContainer<NUMBER>.GetInstance().SetDataByName(this, Name);  
            multiplier = new Multiplier();
            setNumber = new SetItemToSave<double>((int)Name, Main.main.S.numbers);
            if(!Main.main.S.isContinuePlay)
                this.Number = initialNumber;
        }
        public virtual void IncrementNumber(double increment = 1, bool isNetValue = false)
        {
            Number += !isNetValue ? multiplier.CaluculatedNumber(increment) : increment;
            TotalNumber += !isNetValue ? multiplier.CaluculatedNumber(increment) : increment;
            Debug.Log(Main.main.S.numbers[(int)NumbersName.stone]);
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

    public class Money : NUMBER
    {
        public Money(double initialValue)
        {
            IncrementNumber(initialValue);
        }
    }
}

