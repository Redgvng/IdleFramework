using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using static UsefulMethod;
using static Main;
using TMPro;
using UniRx;
using UniRx.Triggers;

namespace InventoryLibrary
{
	public class Item_Mono : MonoBehaviour, ISubject, ISetItem<Item>, ICreateItem<Item>,IDeleteItem<Item>
	{

        int index => transform.GetSiblingIndex();

        ItemContollerTestForMono<Item> controller;
        public void Attach(IObserver observer)
        {
            observers.Add(observer);
        }
        public void Detach(IObserver observer)
        {
            observers.Remove(observer);
        }
        public void Notify()
        {
            if (observers.Count == 0) return;
            foreach (var item in observers)
            {
                item._Update(this);
            }
        }
        List<IObserver> observers = new List<IObserver>();
        void Start()
        {
            controller = new ItemContollerTestForMono<Item>(index, main.S.items);
            this.ObserveEveryValueChanged(x => GetItem().id).Subscribe(_ => Notify());
            Notify();
        }

        public void Create(Item item)
        {
            controller.Create(item);
        }
        public void Delete()
        {
            controller.Delete();
        }
        public void SetItem(Item item)
        {
            controller.SetItem(item);
        }
        public Item GetItem()
        {
            return controller.GetItem();
        }
        public bool CanSet => controller.CanSet;
    }
    public class SetItemToSave<T> : ISetItem<T>
    {
        readonly int index;
        readonly T[] saveArray;
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
