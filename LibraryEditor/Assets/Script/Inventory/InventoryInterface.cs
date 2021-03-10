namespace InventoryLibrary {
    public interface IItemStack
    {
        void Stack(IItem[] items, int original, int stacked);
    }
    public interface IINventoryController
    {
        int MaxSize { get; }
    }
    public interface IItemContoroller<T>
    {
        T item { get; }
        bool IsItemSet { get; }
        void SetItem(T item);
    }
}