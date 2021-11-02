namespace IdleLibrary
{
    public interface IOfflineBonus<T>
    {
        T GetOfflineBonus(double offlineTime);
    }
}