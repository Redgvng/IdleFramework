using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using static UsefulMethod;
using TMPro;

public class Multiplier
{
    public void AddAddtiveMultiplier(Func<double> multiplier)
    {
        AddMultiplier.Add(multiplier);
    }
    public double CaluculatedNumber(double original)
    {
        return (original + add()) * mul();
    }
    public void AddMultiplicativeMultiplier(Func<double> multiplier)
    {
        MulMultiplier.Add(multiplier);
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

    List<Func<double>> AddMultiplier = new List<Func<double>>();
    List<Func<double>> MulMultiplier = new List<Func<double>>();
}
