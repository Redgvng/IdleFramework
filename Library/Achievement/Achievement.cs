using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace IdleLibrary
{
    public interface IGetAchievement
    {
        IEnumerable<Achievement> achievements { get; }
    }

    public interface IAchievementCondition
    {
        bool UnlockCondition();
        float CurrentProgressRatio();
    }

    public abstract class Achievement
    {
        public IAchievementCondition unlockCondition { get; private set; }
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
        public bool CanCalim() => unlockCondition.UnlockCondition() && !isUnlocked;
    }

    public class MultipleAchievementCondition : IAchievementCondition
    {
        private readonly IAchievementCondition[] achievementConditions;
        public MultipleAchievementCondition(params IAchievementCondition[] achievementConditions)
        {
            this.achievementConditions = achievementConditions;
        }
        public bool UnlockCondition()
        {
            foreach (var achievement in achievementConditions)
            {
                if (!achievement.UnlockCondition()) return false;
            }
            return true;
        }
        public float CurrentProgressRatio()
        {
            return achievementConditions.Select(_ => Mathf.Min(1.0f, _.CurrentProgressRatio())).Average();
        }
    }

    //汎用Achievement
    public class NormalAchievement : IAchievementCondition
    {
        private Func<bool> unlockCondition;
        public NormalAchievement(Func<bool> unlockCondition)
        {
            this.unlockCondition = unlockCondition;
        }
        public bool UnlockCondition()
        {
            return unlockCondition();
        }
        public float CurrentProgressRatio() => unlockCondition() ? 1.0f : 0f;
    }

    //Monoで処理する必要のあるAchievement
    //一度でもクリアしたら常にクリアの状態にする。
    public class NoClickAchievement : IAchievementCondition
    {
        public double progress { get; set; }
        public double required;
        private bool isClearedOnce;
        public NoClickAchievement(double required)
        {
            this.required = required;
        }
        public bool UnlockCondition()
        {
            return progress >= required;
        }
        public float CurrentProgressRatio()
        {
            if (required == 0) return 0;
            return (float)(progress / required);
        }
        public void Notify(double progress)
        {
            if (isClearedOnce) return;
            this.progress = progress;
            if (UnlockCondition()) isClearedOnce = true;
        }
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

    public class LevelAchievement : IAchievementCondition
    {
        private readonly ILevel level;
        private readonly long required;
        public LevelAchievement(ILevel level, long required)
        {
            this.level = level;
            this.required = required;
        }
        public bool UnlockCondition()
        {
            return level.level >= required;
        }
        public float CurrentProgressRatio() => (float)level.level / required;
    }
}
