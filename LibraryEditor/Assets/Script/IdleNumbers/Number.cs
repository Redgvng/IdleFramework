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
public class NUMBER : IComparable<NUMBER>
{
    public int NumberAsInt => (int)Number;
    public virtual double Number { get; set; }
    public virtual double TotalNumber { get; set; }

    public NUMBER(double initialNumber = 0)
    {
        this.Number = initialNumber;
    }
    //基本的にはこのコンストラクタを使うこと。
    public NUMBER(string Name, double initialNumber = 0)
    {
        this.Number = initialNumber;
        DataContainer<NUMBER>.GetInstance().SetDataByName(this, Name);
    }
    public virtual void IncrementNumber(double increment = 1)
    {
        Number += increment;
        TotalNumber += increment;
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
    public override void IncrementNumber(double increment)
    {
        Number += increment;
        TotalNumber += increment;
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


