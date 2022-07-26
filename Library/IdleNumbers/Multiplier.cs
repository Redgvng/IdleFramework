﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using static IdleLibrary.UsefulMethod;
using TMPro;
using System.Numerics;

namespace IdleLibrary
{
    public interface IMultiplierInfo
    {
        public Func<double> multiplier { get; }
        public Func<bool> trigger { get; }
        public MultiplierType multiplierType { get; }
    }
    public enum MultiplierType
    {
        add,
        mul
    }

    //最も汎用性が高いタイプ
    public class MultiplierInfo : IMultiplierInfo
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

    //レベルの情報も保存する。アップグレードなどで使う
    //任意のレベルにおけるアップグレード量を取り出すことができる。
    public class MultiplierInfoWithLevel : IMultiplierInfo
    {
        private Func<long, double> multiplierWithLevel { get; } = (level) => 0;
        private readonly ILevel level;
        public Func<double> multiplier { get; }
        public Func<bool> trigger { get; }
        public MultiplierType multiplierType { get; }
        public MultiplierInfoWithLevel(Func<long, double> multiplierWithLevel, MultiplierType type, ILevel level, Func<bool> trigger = null)
        {
            this.multiplierWithLevel = multiplierWithLevel;
            this.trigger = trigger == null ? () => true : trigger;
            this.multiplierType = type;
            this.level = level;
            this.multiplier = () => level == null ? 0 : multiplierWithLevel(level.level);
        }

        //取り出す用の関数
        //任意のレベルにおけるmultiplierを取り出す
        private double GetMultiplierWithAnyLevel(long level) => multiplierWithLevel(level);
        //現在のmultiplierと任意のレベルにおけるmultiplierを取り出す。
        public double GetDiffOfMultiplierWithLevel(long targetLevel)
        {
            if (level == null) return 0;
            return multiplierWithLevel(targetLevel) - multiplierWithLevel(level.level);
        }
        public double CurrentValue()
        {
            return multiplier();
        }
        public double NextValue()
        {
            return GetMultiplierWithAnyLevel(level.level + 1);
        }
        public double NextIncrement()
        {
            return GetDiffOfMultiplierWithLevel(level.level + 1);
        }
    }

    public class Multiplier
    {
        public static void RegisterMultiplierAll(IMultiplierInfo multiplierInfo, string key, params Multiplier[] multipliers)
        {
            foreach (var multiplier in multipliers)
            {
                multiplier.RegisterMultiplier(multiplierInfo, key);
            }
        }
        public void RegisterMultiplier(IMultiplierInfo multiplierInfo)
        {
            var tempText = multiplierInfo.multiplierType == MultiplierType.add ?
                $"Key is not set yet so please be careful : id{AddMultiplier.Count}" :
                $"Key is not set yet so please be careful : id{MulMultiplier.Count}";
            RegisterMultiplier(multiplierInfo, tempText);
        }
        public void RegisterMultiplier(IMultiplierInfo multiplierInfo, string key)
        {
            if (multiplierInfo.multiplierType == MultiplierType.add)
            {
                AddMultiplier[key] = () =>
                {
                    if (!multiplierInfo.trigger())
                        return 0;

                    return multiplierInfo.multiplier();
                };
            }
            if (multiplierInfo.multiplierType == MultiplierType.mul)
            {
                MulMultiplier[key] = () =>
                {
                    if (!multiplierInfo.trigger())
                        return 1.0;

                    return multiplierInfo.multiplier();
                };
            }
        }

        public double CaluculatedNumber(double original)
        {
            return (original + add()) * mul();
        }

        double mul()
        {
            double temp = 1.0;
            foreach (var multiplier in MulMultiplier.Values)
            {
                temp *= multiplier();
            }
            return temp;
        }

        double add()
        {
            double temp = 0;
            foreach (var multiplier in AddMultiplier.Values)
            {
                temp += multiplier();
            }
            return temp;
        }

        private readonly Dictionary<string, Func<double>> AddMultiplier = new Better.Dictionary<string, Func<double>>();
        private readonly Dictionary<string, Func<double>> MulMultiplier = new Better.Dictionary<string, Func<double>>();

        public (double added, double multiplied) GetMultipliersFromKey(string startWith)
        {
            var _added = 0d;
            var _multiplied = 1.0d;
            foreach (var multiplier in AddMultiplier)
            {
                if (multiplier.Key.StartsWith(startWith)) _added += multiplier.Value();
            }
            foreach (var multiplier in MulMultiplier)
            {
                if (multiplier.Key.StartsWith(startWith)) _multiplied *= multiplier.Value();
            }
            return (_added, _multiplied);
        }
        public string DebugCurrentMultiplier()
        {
            var unko = "";
            unko += "----Additive---- \n";
            foreach (var multiplier in AddMultiplier)
            {
                unko += $"{multiplier.Key} : {multiplier.Value():F2}\n";
            }
            unko += "----Multiplicative---- \n";
            foreach (var multiplier in MulMultiplier)
            {
                unko += $"{multiplier.Key} : {multiplier.Value():F3} \n";
            }
            //Debug.Log(unko);
            return unko;
        }
    }
}
   