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
        public Func<double> multiplier => () => level == null ? 0 : multiplierWithLevel(level.level);
        public Func<bool> trigger { get; }
        public MultiplierType multiplierType { get; }
        public MultiplierInfoWithLevel(Func<long, double> multiplierWithLevel, MultiplierType type, ILevel level, Func<bool> trigger = null)
        {
            this.multiplierWithLevel = multiplierWithLevel;
            this.trigger = trigger == null ? () => true : trigger;
            this.multiplierType = type;
            this.level = level;
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
        public static void RegisterMultiplierAll(IMultiplierInfo multiplierInfo, params Multiplier[] multipliers)
        {
            foreach (var multiplier in multipliers)
            {
                multiplier.RegisterMultiplier(multiplierInfo);
            }
        }
        public void RegisterMultiplier(IMultiplierInfo multiplierInfo)
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
                if (MulMultiplier[i]() == 0) Debug.LogError($"0が入っています");
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
   