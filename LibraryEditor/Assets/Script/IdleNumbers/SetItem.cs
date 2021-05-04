namespace IdleLibrary
{
    public class SetItemToSave<T> : ISetItem<T>
    {
        readonly int index;
        readonly T[] saveArray;
        public bool CanSet => true;
        public SetItemToSave(int index, T[] saveArray)
        {
            this.index = index;
            this.saveArray = saveArray;
        }
        public T GetItem()
        {
            return saveArray[index];
        }

        public void SetItem(T item)
        {
            saveArray[index] = item;
        }
    }
    public class NullSetItem<T> : ISetItem<T>
    {
        T item { get; set; }
        public bool CanSet => true;
        public T GetItem()
        {
            return item;
        }
        public void SetItem(T item)
        {
            this.item = item;
        }
    }
}
