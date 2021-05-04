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

    public class MultiplierInfo
    {
        public Func<double> addMultiplier { get; }
        public Func<double> mulMultiplier { get; }
        public Func<bool> trigger { get; }
        public MultiplierInfo(Func<bool> trigger = null, Func<double> addMultiplier = null, Func<double> mulMultiplier = null)
        {
            this.addMultiplier = addMultiplier;
            this.mulMultiplier = mulMultiplier;
            this.trigger = trigger == null ? () => true : trigger;
        }
    }

    public class Multiplier
    {
        public void RegisterMultiplier(MultiplierInfo multiplierInfo)
        {
            if (multiplierInfo.addMultiplier != null)
            {
                AddMultiplier.Add(() =>
                {
                    if (!multiplierInfo.trigger())
                        return 0;

                    return multiplierInfo.addMultiplier();
                });
            }
            if (multiplierInfo.mulMultiplier != null)
            {
                MulMultiplier.Add(() =>
                {
                    if (!multiplierInfo.trigger())
                        return 1.0;

                    return multiplierInfo.mulMultiplier();
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
