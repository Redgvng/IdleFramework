using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using static IdleLibrary.UsefulMethod;
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

    [Serializable]
    public class Cal
    {
        public Multiplier multiplier;
        private double initialValue;

        public Cal(double initialValue)
        {
            this.initialValue = initialValue;
            multiplier = new Multiplier();
        }
        public virtual double GetValue() => multiplier.CaluculatedNumber(initialValue);
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
