using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using System.Linq;

namespace IdleLibrary.ProgressSlider
{
    public interface IProgressSlider
    {
        void Update();
        float CurrentProgressRatio();
    }
    [Serializable]
    public class ProgressSlider : ILevel, IProgressSlider
    {
        public long level { get => _level.level; set => _level.level = value; }
        [SerializeField] double _currentProgress;
        public double currentProgress { get => _currentProgress; private set => _currentProgress = value; }
        private readonly Func<double> RequiredProgress = () => 0;
        private readonly Func<double> ProgressSpeedPerFrame = () => 0;
        private readonly ILevel _level;
        private void Validate()
        {
            if (currentProgress >= RequiredProgress())
            {
                currentProgress -= RequiredProgress();
                level++;
            }
        }
        public ProgressSlider(Func<double> RequiredProgress, Func<double> ProgressSpeedPerFrame, ILevel _level)
        {
            this.RequiredProgress = RequiredProgress;
            this.ProgressSpeedPerFrame = ProgressSpeedPerFrame;
            this._level = _level;
        }
        public void Update()
        {
            currentProgress += ProgressSpeedPerFrame();
            Validate();
        }
        public float CurrentProgressRatio() => (float)(currentProgress / RequiredProgress());
        public float TimeToLevelUpForSecond() => (float)((RequiredProgress() - currentProgress) / (ProgressSpeedPerFrame() / Time.fixedDeltaTime))-1;
        public static void Load(IEnumerable<ProgressSlider> sliders, double[] loadProgress)
        {
            sliders
            .Select((slider, index) => new { slider, index })
            .ToList()
            .ForEach(pair => pair.slider.currentProgress = loadProgress[pair.index]);
        }
        public static void Save(IEnumerable<ProgressSlider> sliders, double[] loadProgress)
        {
            sliders
            .Select((slider, index) => new { slider, index })
            .ToList()
            .ForEach(pair => pair.slider.ObserveEveryValueChanged(x => x.currentProgress)
            .Subscribe(_ => loadProgress[pair.index] = pair.slider.currentProgress));
        }

        private bool isOfflineBonusGot = false;
        private long diffLevel;
        private double totalProgress;
        public void GetOfflineBonus(double offlineTime)
        {
            if (isOfflineBonusGot) return;
            var previous = level;
            totalProgress = ProgressSpeedPerFrame() / Time.fixedDeltaTime * offlineTime;
            currentProgress += ProgressSpeedPerFrame() / Time.fixedDeltaTime * offlineTime;
            Validate();
            diffLevel = level - previous;
            isOfflineBonusGot = true;
        }
        public (long diffLevel, double totalProgress) OfflineBonusInfo()
        {
            return (diffLevel, totalProgress);
        }
    }

    [Serializable]
    public class AsyncProgressSlider : ILevel, IProgressSlider
    {
        public long level { get => _level.level; set => _level.level = value; }
        [SerializeField] double numberConsumed;
        public double currentProgress { get => number.Number - numberConsumed; }
        private readonly Func<double> RequiredProgress = () => 0;
        private readonly IGetNumber number;
        private readonly ILevel _level;
        public AsyncProgressSlider(Func<double> RequiredProgress, IGetNumber number, ILevel _level)
        {
            this.RequiredProgress = RequiredProgress;
            this.number = number;
            this._level = _level;
        }
        public void Update()
        {
            if (currentProgress >= RequiredProgress())
            {
                numberConsumed += RequiredProgress();
                level++;
            }
        }
        public float CurrentProgressRatio() => (float)(currentProgress / RequiredProgress());
    }
}
