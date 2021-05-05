using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using static IdleLibrary.UsefulMethod;
using TMPro;

namespace IdleLibrary
{
    public class IMultipliable
    {
        Multiplier multiplier { get; }
    }

    public enum MultiplierType
    {
        add,
        mul
    }

    public class MultiplierInfo
    {
        public Func<double> multiplier { get; }
        public Func<bool> trigger { get; }
        public MultiplierType multiplierType { get; }
        public MultiplierInfo(Func<double> multiplier, MultiplierType type, Func<bool> trigger = null)
        {
            this.multiplier = multiplier;
            this.trigger = trigger == null ? () => true : trigger;
            this.multiplierType = type;
        }
    }

    public class Multiplier
    {
        public void RegisterMultiplier(MultiplierInfo multiplierInfo)
        {
            if (multiplierInfo.multiplierType == MultiplierType.add)
            {
                AddMultiplier.Add(() =>
                {
                    if (!multiplierInfo.trigger())
                        return 0;

                    return multiplierInfo.multiplier();
                });
            }
            if (multiplierInfo.multiplierType == MultiplierType.mul)
            {
                MulMultiplier.Add(() =>
                {
                    if (!multiplierInfo.trigger())
                        return 1.0;

                    return multiplierInfo.multiplier();
                });
            }
        }

        public double CaluculatedNumber(double original)
        {
            return (original + add()) * mul();
        }

        double mul()
        {
            double temp = 1.0;
            for (int i = 0; i < MulMultiplier.Count; i++)
            {
                temp *= MulMultiplier[i]();
            }
            return temp;
        }

        double add()
        {
            double temp = 0;
            for (int i = 0; i < AddMultiplier.Count; i++)
            {
                temp += AddMultiplier[i]();
            }
            return temp;
        }

        private readonly List<Func<double>> AddMultiplier = new List<Func<double>>();
        private readonly List<Func<double>> MulMultiplier = new List<Func<double>>();
    }
}
