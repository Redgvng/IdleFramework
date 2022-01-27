using System;
namespace IdleLibrary
{
    public interface ITime
    {
        DateTime currentTime { get; }
    }
    public interface IRegisterDailyAction
    {
        void AddDailyAction(Action action);
    }
}