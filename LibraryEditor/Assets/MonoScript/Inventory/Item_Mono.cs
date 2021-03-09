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
    //Save.Sに、自作のIItemクラスを宣言してください。ここではサンプルとしてItemクラスになっています。
	public class Item_Mono : MonoBehaviour, ISubject, IGetItem
	{
        IItem _item { 
            get 
            {
                if (main.S.items[index] == null)
                {
                    main.S.items[index] = new Item(-1);
                    return main.S.items[index];
                }
                else 
                return main.S.items[index];
            } 
            set => main.S.items[index] = (Item)value; 
        }
        public IItem item => _item;
        int index => transform.GetSiblingIndex();
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
            foreach (var _item in observers)
            {   
                Debug.Log(item.id);
                _item._Update(this);
            }
        }
        void Start()
        {
            this.ObserveEveryValueChanged(x => x._item.id).Subscribe(_ => Notify());
            Notify();
        }
        List<IObserver> observers = new List<IObserver>();
	}
}
