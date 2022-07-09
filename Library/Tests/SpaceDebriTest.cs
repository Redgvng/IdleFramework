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

    public class Picker
    {
        public Parameter capacity, speed, inhalePower, fuel, fuelSpeed;
        public Picker(double initial_capacity, double initial_speed, double initial_inhalePower, double initial_fuel, double initial_fuelSpeed)
        {
            capacity = new Parameter(initial_capacity);
            speed = new Parameter(initial_speed);
            inhalePower = new Parameter(initial_inhalePower);
            fuel = new Parameter(initial_fuel);
            fuelSpeed = new Parameter(initial_fuelSpeed);
        }

        public double Capacity() => capacity.Number;
        public double Speed() => speed.Number;
        public double InhalePower() => inhalePower.Number;
        public double Fuel() => fuel.Number;
        public double FuelSpeed() => fuelSpeed.Number;
    }

    public class StageManager
    {
        public long currentStage { get; private set; }
        public StageManager(Func<bool> canProgress, Action<long> onProgress)
        {
            this.canProgress = canProgress;
            this.onProgress = onProgress;

            onProgress(currentStage);
        }
        public void OnProgress()
        {
            if (!canProgress()) return;

            currentStage++;
            onProgress(currentStage);
        }

        private readonly Func<bool> canProgress;
        private readonly Action<long> onProgress;
    }

    public interface IDebriGenerator { long GenerateDebri(); }
    public class DebriGenerator
    {
        public Func<long> growthFactor;
        public DebriGenerator(Func<long> growthFactor) { this.growthFactor = growthFactor; }
        public long GenerateDebri()
        {
            return (long)(10 * Math.Pow(1.5, growthFactor()));
        }
    }
    public class DebriManager
    {
        private readonly IDebriGenerator debriGenerator;
        public DebriManager(IDebriGenerator debriGenerator)
        {
            this.debriGenerator = debriGenerator;
        }
        public void GenerateDebri()
        {
            debriGenerator.GenerateDebri();
        }
    }

    private void UpdatePerSecond()
    {
        
    }

    private Picker mainPicker;
    private StageManager stageManager;
    [SetUp]
    public void Initialize()
    {
        mainPicker = new Picker(100, 10, 10, 50, 1);
        //stageManager = new StageManager(() => )
    }

    private void Reset()
    {
        
    }

    [Test]
    public void Integration()
    {
        ExportExcel.OutPutHeader("Antimatter", new string[] { "Time"});
        for (int i = 0; i < 3600 * 6; i++)
        {
            UpdatePerSecond();
            if ((i + 60) % 60 == 0)
            {
                ExportExcel.OutPutData("Antimatter", new string[] {
                    i.ToString()});
            }
        }
    }

}

