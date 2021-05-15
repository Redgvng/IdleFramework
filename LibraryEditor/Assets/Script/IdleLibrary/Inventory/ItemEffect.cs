using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using System;
using static IdleLibrary.UsefulMethod;

namespace IdleLibrary.Inventory
{
    //ˆê’UAbasic effect‚µ‚©‘¶İ‚µ‚È‚¢‚à‚Ì‚Æ‚µ‚ÄƒNƒ‰ƒX‚ğì‚Á‚Ä‚İ‚é
    //GoldGain‚ÆExpGain‚ğì‚Á‚Ä‚İ‚é
    public interface IValueText
    {
        string ValueText(double value);
    }
    public interface IEffect : IText
    {

    }
    public class GoldGain : IEffect
    {
        [OdinSerialize] public readonly Func<double> value;
        public GoldGain(Func<double> value)
        {
            this.value = value;
        }
        public string Text()
        {
            return $"Gold Gain + {tDigit(value())}";
        }
    }

    public class ExpGain : IEffect
    {
        [OdinSerialize] public readonly Func<double> value;
        public ExpGain(Func<double> value)
        {
            this.value = value;
        }
        public string Text()
        {
            return $"Exp Gain + {tDigit(value())}";
        }
    }
    /*
    public interface IEffectValue
    {
        double Value();
        string TotalValueText(double value);
    }

    public interface IEffect
    {
        string EffectText();
    }
    */
}
