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
    public interface ICreateItem<T> 
    {
        void Create(T item);
        bool CanSet { get; }
    }
    public interface IDeleteItem<T> 
    {
        void Delete();
    }
}