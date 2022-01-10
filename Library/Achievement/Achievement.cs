using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace IdleLibrary
{
    public interface IAchievementCondition
    {
        bool UnlockCondition();
        float CurrentProgressRatio();
    }

    public abstract class Achievement
    {
        private readonly IAchievementCondition unlockCondition;
        public Achievement(IAchievementCondition unlockCondition)
        {
            this.unlockCondition = unlockCondition;
        }
        public void OnClaim()
        {
            if (!CanCalim()) return;
                isUnlocked = true;
        }
        public float CurrentProgressRatio() => unlockCondition.CurrentProgressRatio();
        public abstract bool isUnlocked { get; set; }
        public bool CanCalim() => unlockCondition.UnlockCondition();

    }

    //ある量に達したかどうか
    public class NumberAchievement : IAchievementCondition
    {
        private readonly INumber number;
        private readonly double required;
        public NumberAchievement(INumber number, double required)
        {
            this.number = number;
            this.required = required;
        }
        public bool UnlockCondition()
        {
            return number.Number >= required;
        }
        public float CurrentProgressRatio() => (float)(number.Number / required);
    }

    //Prestige関係
    public class PrestigeNumberAchievement : IAchievementCondition
    {
        private readonly IPrestigeStats stats;
        private readonly double required;
        public PrestigeNumberAchievement(IPrestigeStats stats, double required)
        {
            this.stats = stats;
            this.required = required;
        }
        public bool UnlockCondition()
        {
            return stats.prestigeNum >= required;
        }
        public float CurrentProgressRatio() => (float)(stats.prestigeNum / required);
    }
}
