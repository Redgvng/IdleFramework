namespace IdleLibrary
{
    public interface IPrestige
    {
        void OnPrestige();
    }

    public interface IPrestigeStats
    {
        long prestigeNum { get; }
    }
}