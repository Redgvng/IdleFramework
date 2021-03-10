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
//
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
	//状態の更新のみに使うべき・・・？
	public class Inventory_Mono : MonoBehaviour, IInventoryController, IObserver
	{
		Cal slotNum;
	    Item_Mono[] items;

		public int MaxSize { get => 100; }
		public Sprite[] sprites;
		public Sprite defaultSprite;


		[SerializeField]
		Button GenerateItemButton;
		// Use this for initialization
		void Awake()
		{
			items = gameObject.GetComponentsInChildren<Item_Mono>();
            for (int i = 0; i < items.Length; i++)
            {
				if (items[i] is ISubject)
				{
					var subject = items[i] as ISubject;
					subject.Attach(this);
				}
			}
			GenerateItemButton.OnClickAsObservable().Subscribe(_ => GenerateItem());
		}

		//itemの状態を更新します。
        public void _Update(ISubject subject)
        {
			 if(subject is Item_Mono)
			 {
		   		var item = subject as Item_Mono;
				Debug.Log(item.item.isSet);
				if (!item.item.isSet) item.gameObject.GetComponent<Image>().sprite = defaultSprite;
				else item.gameObject.GetComponent<Image>().sprite = sprites[item.item.id];
			 }
        }

		//test用のメソッドです。
		void GenerateItem()
        {
			var item = new Item(UnityEngine.Random.Range(0, 5));
            for (int i = 0; i < items.Length; i++)
            {
                if (!items[i].item.isSet)
                {
					items[i].Set(item);
					return;
                }
            }
        }

    }
}