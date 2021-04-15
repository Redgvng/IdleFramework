using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using static UsefulMethod;
using TMPro;

namespace IdleLibrary
{
    public interface ICapped
    {
        bool CappedAction(NUMBER number, Cal calculator);
    }

    public class NullCapped : ICapped
    {
        public bool CappedAction(NUMBER number, Cal calculator) { return false; }
    }

    public class Cap_Truncate : ICapped
    {
        public bool CappedAction(NUMBER number, Cal calculator)
        {
            if (number.Number > calculator.GetValue())
            {
                number.Number = calculator.GetValue();
                return true;
            }
            return false;
        }
    }

    public class Cal
    {
        public Multiplier multiplier = new Multiplier();
        private double initialValue;

        public Cal(double initialValue)
        {
            this.initialValue = initialValue;
        }
        public Cal(double initialValue, CalsName Name)
        {
            this.initialValue = initialValue;
            DataContainer<Cal>.GetInstance().SetDataByName(this, Name);
        }
        public virtual double GetValue() => multiplier.CaluculatedNumber(initialValue);
    }

    //レベルに依存したCal
    public class CalDL : Cal
    {
        private Func<long, double> initialFunc;
        private ILevel level;
        public CalDL(Func<long, double> initialFunc, ILevel level) : base(0)
        {
            this.initialFunc = initialFunc;
            this.level = level;
        }
        public CalDL(Func<long, double> initialFunc, ILevel level, CalsName Name) : base(0,Name)
        {
            this.initialFunc = initialFunc;
            this.level = level;
        }
        public override double GetValue() => multiplier.CaluculatedNumber(initialFunc(level.level));
        public double GetValue(long level) => multiplier.CaluculatedNumber(initialFunc(level));
    }

    public class Cap : Cal
    {
        ICapped capped;
        public Cap(double initialValue, ICapped capped = null) : base(initialValue)
        {
            if (capped == null)
                this.capped = new Cap_Truncate();
            else
                this.capped = capped;
        }
        public void Check(NUMBER number)
        {
            capped.CappedAction(number, this);
        }
    }
}
