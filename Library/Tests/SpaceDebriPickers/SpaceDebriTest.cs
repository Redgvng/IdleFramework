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
using static IdleLibrary.UsefulMethod;
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


    InitializeGame game;
    [SetUp]
    public void SetUpTest()
    {
        game = new InitializeGame();
        game.Initialize();
        game.StartGame();
    }

    private void UpdatePerSecond()
    {
        game.UpdateGame(1);
    }


    private readonly static string title = "Space Debri Pickers";
    [Test]
    public void Integration()
    {
        ExportExcel.OutPutHeader(title, new string[] { "Time", "R"});
        for (int i = 0; i < 3600 * 6; i++)
        {
            UpdatePerSecond();
            if ((i + 60) % 60 == 0)
            {
                ExportExcel.OutPutData(title, new string[] {
                    i.ToString(), tDigit(game.currencyManager.GetCurrency(CurrencyKind.r).Number)});
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

