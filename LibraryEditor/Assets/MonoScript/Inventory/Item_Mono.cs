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
	public class Item_Mono : MonoBehaviour, IItem, ISubject
	{
		//idでアイテムを識別します。
        public int id { get; set; }
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
            foreach (var item in observers)
            {
                item.Update(this);
            }
        }
        List<IObserver> observers = new List<IObserver>();
        // Use this for initialization
        void Awake()
		{

		}

		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}
	}
}
