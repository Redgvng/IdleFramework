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
