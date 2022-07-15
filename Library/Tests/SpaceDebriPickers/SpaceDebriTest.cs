using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using System.Linq;
using IdleLibrary;
using IdleLibrary.Upgrade;
using IdleLibrary.IntegrationTest;
using System;
using System.IO;
using static IdleLibrary.IntegrationTest.IntegrationUtiliity;
using Pickers.Domain;
using Pickers.Domain.Test;


public class SpaceDebriTest : MonoBehaviour
{
    //Modelを洗練させたい！
    public class Parameter : INumber
    {
        public double Number => BaseAmount();
        public Parameter(double initialValue)
        {
            this.initialValue = initialValue;
        }
        public void RegisterMultiplier(Func<double> multiplier, MultiplierType type, Func<bool> trigger = null)
        {
            this.multiplier.RegisterMultiplier(new MultiplierInfo(multiplier, type, trigger));
        }

        private double BaseAmount() => multiplier.CaluculatedNumber(initialValue);
        private Multiplier multiplier { get; } = new Multiplier();
        private double initialValue { get; }
    }

    public class Currency : IDecrementableNumber
    {
        public enum CurrencyKind { debri, exp}
        public Currency(CurrencyKind kind) { this.kind = kind; }
        public double Number { get; private set; }
        public void Decrement(double decrement)
        {
            Number -= decrement;
        }

        private readonly CurrencyKind kind;
    }

    InitializeGame initializeGame;
    [SetUp]
    public void SetUpTest()
    {
        initializeGame = new InitializeGame();
        initializeGame.Initialize();
        initializeGame.StartGame();
    }

    private void UpdatePerSecond()
    {
        initializeGame.UpdateGame(1);
    }


    [Test]
    public void Integration()
    {
        ExportExcel.OutPutHeader("Space Debri Pickers", new string[] { "Time"});
        for (int i = 0; i < 3600 * 6; i++)
        {
            UpdatePerSecond();
            if ((i + 60) % 60 == 0)
            {
                ExportExcel.OutPutData("Time", new string[] {
                    i.ToString()});
            }
        }
    }

    [Test]
    public void StageTest()
    {
        var stage = new Stage();
        stage.Initialize();

        for (int i = 0; i < 600; i++)
        {
            stage.UpdateStage();
        }
    }
}

