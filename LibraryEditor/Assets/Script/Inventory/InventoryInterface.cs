namespace InventoryLibrary {
    public interface IGetItem<T>
    {
        T GetItem();
    }
    public interface ISetItem<T> : IGetItem<T> 
    {
        void SetItem(T item);
        bool CanSet { get; }
    }
 
    public interface IDeleteItem<T> 
    {
        void Delete();
    }
    public interface IStackItem<T>
    {
        void Stack(ISetItem<T> item);
    }
    public interface IClickAction<T>
    {
        void Click(T clicked);
    }
}