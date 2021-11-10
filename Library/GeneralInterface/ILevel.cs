using System;
using UnityEngine;

namespace IdleLibrary
{
    public interface ILevel
    {
        long level { get; set; }
    }
    public class MockLevel : ILevel
    {
        public long level { get; set; }
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
        public long maxLevel { get; }
    }
}
