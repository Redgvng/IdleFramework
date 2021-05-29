using System;
using Sirenix.Serialization;
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
    public long level { get; set; }
}
//MaxCapありレベル
[Serializable]
public class TimeBasedLevel : ILevel
{
    public long level { get => _levelCap; set => _levelCap = value; }
    [OdinSerialize] private long _levelCap;
    public long maxLevelCap;
    public TimeBasedLevel(long maxLevelCap)
    {
        this.level = 1;
        this.maxLevelCap = maxLevelCap;
    }
}
