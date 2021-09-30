using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace IdleLibrary
{
    public class ProgressSlider : ILevel
    {
        public long level { get => _level.level; set => _level.level = value; }
        [SerializeField] double currentProgress;
        private readonly Func<double> RequiredProgress = () => 0;
        private readonly Func<double> ProgressSpeedPerFrame = () => 0;
        private readonly ILevel _level;
        public ProgressSlider(Func<double> RequiredProgress, Func<double> ProgressSpeedPerFrame, ILevel _level)
        {
            this.RequiredProgress = RequiredProgress;
            this.ProgressSpeedPerFrame = ProgressSpeedPerFrame;
            this._level = _level;
        }
        public void Update()
        {
            currentProgress += ProgressSpeedPerFrame();
            if (currentProgress >= RequiredProgress())
            {
                currentProgress -= RequiredProgress();
                level++;
            }
        }
        public void IncrementProgress(double increment)
        {
            currentProgress += increment;
        }
        public float CurrentProgressRatio() => (float)(currentProgress / RequiredProgress());
    }
}
