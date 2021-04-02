namespace IdleLibrary.Inventory {
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
        void Click();
    }
}