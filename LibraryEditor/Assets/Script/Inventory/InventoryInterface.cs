namespace InventoryLibrary {
    public interface IItemStack
    {
        void Stack(IItem[] items, int original, int stacked);
    }
    public interface IInventoryController
    {
        int MaxSize { get; }
    }
    public interface IGetItem<T>
    {
        T GetItem();
    }
    public interface ISetItem<T> : IGetItem<T>
    {
        void SetItem(T item);
        bool IsItemSet { get; }
    }
}