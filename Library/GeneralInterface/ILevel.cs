using System;
using UnityEngine;

namespace IdleLibrary
{
    public interface ILevel
    {
        long level { get; set; }
        void LevelUp(long level);
    }
    public class MockLevel : ILevel
    {
        public long level { get; set; }
        public void LevelUp(long level) { this.level += level; }
    }
    //Value Object
    [Serializable]
    public class Level : ILevel
    {
        public long level {
            get => _level;
            set {
                _level = value;
                if (tempMaxLevel < _level) tempMaxLevel = _level;
            }
        }
        [SerializeField] private long _level;

        private long tempMaxLevel;
        public long maxLevel { get => tempMaxLevel; }
        public void LevelUp(long level) => this.level += level;
    }

    /*
     * ある条件のときに、他のレベルをあげるようなレベル。
     * 主に、チャレンジなどの特殊な制約を想定しています。
     */
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
}
