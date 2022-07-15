using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using IdleLibrary;

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

        }
        private void MakeCurrency()
        {
            currencies[CurrencyKind.r] = new Currency(CurrencyKind.r);
            currencies[CurrencyKind.s] = new Currency(CurrencyKind.s);
        }

        public Currency GetCurrency(CurrencyKind kind) => currencies[kind];
    }
}

