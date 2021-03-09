using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using static UsefulMethod;
using static Main;
using TMPro;

namespace InventoryLibrary
{
	public enum Id
    {
		item1,
		item2,
		item3,
		item4,
		item5
    }
	public class Inventory_Mono : MonoBehaviour, IItemController, IObserver
	{
		public int MaxSize { get => 100; }
		Cal slotNum;
	    ISubject[] items;
		public Sprite[] sprites;

		[SerializeField]
		Button GenerateItemButton;
		// Use this for initialization
		void Awake()
		{
			items = gameObject.GetComponentsInChildren<ISubject>();
		}

		//itemの状態を更新します。
        public void Update(ISubject subject)
        {
            for (int i = 0; i < items.Length; i++)
            {
				if(ReferenceEquals(subject, items[i])&&subject is Item_Mono)
                {
					var item = subject as Item_Mono;
					item.gameObject.GetComponent<Image>().sprite = sprites[i];
                }
            }
        }
    }
}