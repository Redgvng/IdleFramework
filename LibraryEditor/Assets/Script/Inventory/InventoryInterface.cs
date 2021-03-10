namespace InventoryLibrary {
    public interface IItemStack
    {
        void Stack(IItem[] items, int original, int stacked);
    }
    public interface IGetItem<T>
    {
        T GetItem();
    }
    public interface ISetItem<T> : IGetItem<T>
    {
        void SetItem(T item);
    }
    public interface ICreateitem<T>
    {
        void Create(T item);
    }
    public interface IDeleteItem<T>
    {
        void Delete();
    }
}