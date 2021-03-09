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
		Cal slotNum;
	    IItem[] items;
		SetItemClass setItem;

		public int MaxSize { get => 100; }
		public Sprite[] sprites;
		public Sprite defaultSprite;


		[SerializeField]
		Button GenerateItemButton;
		// Use this for initialization
		void Awake()
		{
			var getitems = gameObject.GetComponentsInChildren<IGetItem>();
			items = new IItem[getitems.Length];
			Debug.Log(getitems.Length);
            for (int i = 0; i < items.Length; i++)
            {
				items[i] = getitems[i].item;
				if (getitems[i] is ISubject)
				{
					var subject = getitems[i] as ISubject;
					subject.Attach(this);
				}
			}
			setItem = new SetItemClass(items, this);
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

		void GenerateItem()
        {
			var item = new Item(UnityEngine.Random.Range(0, 5));
			setItem.SetItem(item, 0);
        }

    }
}