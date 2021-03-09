namespace InventoryLibrary {
    public interface IItemStack
    {
        void Stack(IItem[] items, int original, int stacked);
    }
    public interface IItemController
    {
        int MaxSize { get; }
    }
    public interface IGetItem
    {
        IItem item { get; }
    }
}