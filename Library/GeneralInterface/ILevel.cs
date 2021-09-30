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
    public class Level : ILevel
    {
        public long level { get => _level; set => _level = value; }
        [SerializeField] private long _level;
    }
}
