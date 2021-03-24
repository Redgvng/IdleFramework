using CommonLibrary;
namespace InventoryLibrary {
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