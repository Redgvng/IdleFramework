namespace InventoryLibrary {
    public interface IGetItem<T> where T : struct
    {
        T GetItem();
    }
    public interface ISetItem<T> : IGetItem<T> where T : struct
    {
        void SetItem(T item);
        bool CanSet { get; }
    }
 
    public interface IDeleteItem
    {
        void Delete();
    }
    public interface IStackItem<T> where T : struct 
    {
        void Stack(ISetItem<T> item);
    }
    //Itemとは限らないので、ジェネリックにします。
    public interface IClickAction<T>
    {
        void Click(T clicked);
    }
}