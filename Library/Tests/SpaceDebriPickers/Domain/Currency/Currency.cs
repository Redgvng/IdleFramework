using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using IdleLibrary;
using System.Linq;

namespace Pickers.Domain
{
    public enum CurrencyKind { r, s }
    public class Currency : IDecrementableNumber, IProducableNumber
    {
        public Currency(CurrencyKind kind) { this.kind = kind; }
        public double Number { get; private set; }
        public void Decrement(double decrement)
        {
            Number -= decrement;
        }

        public void ProducePerTime(float time)
        {
            Number += time * IncrementPerSecond();
        }

        public Multiplier producePerSecondMultiplier { get; } = new Multiplier();
        private double IncrementPerSecond()
        {
            return producePerSecondMultiplier.CaluculatedNumber(0);
        }
        private readonly CurrencyKind kind;
    }

    public class CurrencyManager
    {
        private Better.Dictionary<CurrencyKind, Currency> currencies = new Better.Dictionary<CurrencyKind, Currency>();
        public CurrencyManager()
        {
            MakeCurrency();
        }
        public void UpdatePerTime(float time = 1.0f)
        {
            currencies.Select(_ => _.Value).ToList().ForEach(_ => _.ProducePerTime(time));
        }
        private void MakeCurrency()
        {
            currencies[CurrencyKind.r] = new Currency(CurrencyKind.r);
            currencies[CurrencyKind.s] = new Currency(CurrencyKind.s);
        }

        public Currency GetCurrency(CurrencyKind kind) => currencies[kind];
    }

    public class ApplyDebriMultiplier
    {
        private readonly CurrencyManager currencyManager;
        private readonly IDebliCollection debriCollecition;
        public ApplyDebriMultiplier(CurrencyManager currencyManager, IDebliCollection collection)
        {
            this.currencyManager = currencyManager;
            this.debriCollecition = collection;
        }
        public void ApplyMultiplier()
        {
            currencyManager.GetCurrency(CurrencyKind.r).producePerSecondMultiplier.RegisterMultiplier(new MultiplierInfo(() =>
            debriCollecition.GetCollectedDebri(1) * (1 + debriCollecition.GetCollectedDebri(2)) * (1 + debriCollecition.GetCollectedDebri(3)),
            MultiplierType.add
            ), "Base_debriMultiplier");
        }
    }
}

