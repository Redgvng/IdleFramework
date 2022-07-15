using System;
using UnityEngine;

namespace IdleLibrary
{
    public interface ILevel
    {
        long level { get; set; }
        long maxLevel { get; }

        bool isMaxLevel => level >= maxLevel;
    }

    public class MockLevel : ILevel
    {
        public long level { get; set; }
        public long maxLevel => long.MaxValue;
    }
    //Value Object
    [Serializable]
    public class Level : ILevel
    {
        private long _level;

        public long maxLevel { get; private set; }
        public Level(long maxLevel = long.MaxValue)
        {
            this.maxLevel = maxLevel;
        }

        public virtual long level {
            get
            {
                return Math.Min(maxLevel, _level);
            }
            set
            {
                _level = value;
                if (maxLevel < _level) maxLevel = _level;
            }
        }
    }

    /*
     * ある条件のときに、他のレベルをあげるようなレベル。
     * 主に、チャレンジなどの特殊な制約を想定しています。
     */
    /*
    public class LevelWithOthers : ILevel
    {
        public virtual long level { get; set; }
        public void LevelUp(long level)
        {
            if (condition() && others != null)
            {
                others.LevelUp(level);
            }
            else
            {
                this.level += level;
            }
        }
        protected readonly ILevel others;
        protected Func<bool> condition;
        public LevelWithOthers(ILevel others, Func<bool> condition)
        {
            this.others = others;
            this.condition = condition;
        }
    }
    */
}
