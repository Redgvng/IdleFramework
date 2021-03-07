using System;
using System.Collections.Generic;
using UnityEngine;

/*
public interface INumber : IMultiplier
{
    double Number { get; set; }
    void IncrementNumber(double increment);
    void DecrementNumber(double decrement);
}

public class DummyNumber : INumber
{
    public double Number { get; set; }
    public Multiplier multiplier { get; }

    public DummyNumber(double initialNumber)
    {
        Number = initialNumber;
        multiplier = new Multiplier();
    }
    public void DecrementNumber(double decrement)
    {
        Number -= decrement;
    }

    public void IncrementNumber(double increment)
    {
        Number += increment;
    }
}
*/

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
    public int NumberAsInt => (int)Number;
    public virtual double Number { get; set; }
    public virtual double TotalNumber { get; set; }

    public Multiplier multiplier { get; }
    public NUMBER(double initialNumber = 0)
    {
        multiplier = new Multiplier();
        this.Number = initialNumber;
    }
    //基本的にはこのコンストラクタを使うこと。
    public NUMBER(NumbersName Name, double initialNumber = 0)
    {
        multiplier = new Multiplier();
        this.Number = initialNumber;
        DataContainer<IMultiplier>.GetInstance().SetDataByName(this, Name);
        DataContainer<NUMBER>.GetInstance().SetDataByName(this, Name);
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


