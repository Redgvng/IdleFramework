using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using System;
using static IdleLibrary.UsefulMethod;

namespace IdleLibrary.Inventory
{
    public enum Calway
    {
        add,
        mul
    }
    //将来statsbreakdownで使う可能性のあるインターフェース
    //テキストのみデコレート可能にしておく必要あるか？
    public interface IStatsBreakdown : IStatsBreakdownText
    {
        //各々の値
        double Value();
        //等しいことのbool (クラスが同じとか、idが同じとか...)
        bool IsEqual(object obj);
    }
    //デコレート用インターフェース
    public interface IStatsBreakdownText
    {
        //文章のフォーマット(値がないやつも最悪これ使えばいい)
        string StatsBreakdownText(double value);
    }
    public interface IEffect : IText
    {
        
    }
    public class BasicEffect : IEffect, IStatsBreakdown
    {
        [OdinSerialize] public readonly Func<double> value;
        public string effectText;
        public Calway calway;
        public BasicEffect(string effectText, Func<double> value, Calway calway)
        {
            this.value = value;
            this.effectText = effectText;
            this.calway = calway;
        }
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
        public double Value() => value();
        public bool IsEqual(object obj)
        {
            if (obj is BasicEffect)
            {
                var effect = obj as BasicEffect;
                if(effect.effectText == this.effectText)
                {
                    return true;
                }
            }
            return false;
        }
    }

    //stats breakdownを表示させる処理を作ろう。
    public class StatsBreakdownMaker
    {
        private readonly IEnumerable<IStatsBreakdown> statsBreakdowns;
        public StatsBreakdownMaker(IEnumerable<IStatsBreakdown> statsBreakdowns)
        {
            this.statsBreakdowns = statsBreakdowns;
        }
    }
}
