namespace CommonLibrary
{
    public interface ISetItem<T> : IGetItem<T>
    {
        void SetItem(T item);
        bool CanSet { get; }
    }
    public interface IGetItem<T>
    {
        T GetItem();
    }
}