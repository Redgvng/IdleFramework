using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using System;
using static IdleLibrary.UsefulMethod;
using IdleLibrary;

namespace IdleLibrary.Inventory
{
    public enum Calway
    {
        add,
        mul
    }
    //将来statsbreakdownで使う可能性のあるインターフェース
    //テキストのみデコレート可能にしておく必要あるか？
    public interface IStatsBreakdown
    {
        //各々の値
        double Value();
        string StatsBreakdownText(double value);
    }

    public interface IEffect : IText
    {
        IEffect Clone();
        Enum effectType { get; }
    }
    public class ValueEffect
    {
        public Func<double> value = () => 0;
        public string effectText;
        public Calway calway;
        public string Text()
        {
            string text = calway switch
            {
                Calway.add => effectText + " + " + tDigit(Value()),
                Calway.mul => effectText + " + " + percent(Value()),
                _ => ""
            };
            return text;
        }
        public string StatsBreakdownText(double value)
        {
            string text = calway switch
            {
                Calway.add => effectText + " + " + tDigit(value),
                Calway.mul => effectText + " + " + percent(value),
                _ => ""
            };
            return text;
        }
        public double Value()
        {
            if (value == null) return 0;
            return value();
        }
    }
    [Serializable]
    public class BasicEffect : ValueEffect , IEffect, IStatsBreakdown
    {
        [OdinSerialize] private Enum _effectType;
        public Enum effectType => _effectType;
        public BasicEffect(Enum type, string effectText, Calway calway)
        {
            this.effectText = effectText;
            this.calway = calway;
            this._effectType = type;
        }
        //コピーコンストラクタ
        public BasicEffect(BasicEffect basicEffect)
        {
            this.effectText = basicEffect.effectText;
            this.calway = basicEffect.calway;
            this._effectType = basicEffect._effectType;
            this.value = basicEffect.value;
        }
        
        public IEffect Clone()
        {
            var clonedEffect = new BasicEffect(this.effectType, this.effectText, this.calway);
            return clonedEffect;
        }
    }

    [Serializable]
    public class OptionBasicEffect : ValueEffect,  IEffect, IStatsBreakdown, ILevel
    {
        public long level { get; set; }
        public long maxLevel;
        [OdinSerialize] private Enum _effectType;
        public Enum effectType => _effectType;
        public OptionBasicEffect(Enum type, string effectText, Calway calway)
        {
            this.effectText = effectText;
            this.calway = calway;
            this._effectType = type;
        }

        public IEffect Clone()
        {
            var clonedEffect = new BasicEffect(this.effectType, this.effectText, this.calway);
            return clonedEffect;
        }
        public void SetMaxLevel(long maxLevel)
        {
            level = 1;
            this.maxLevel = maxLevel;
        }
    }

}


