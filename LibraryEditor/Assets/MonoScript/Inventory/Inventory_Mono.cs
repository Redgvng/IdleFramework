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
using System.Linq;
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
	public class Inventory_Mono : Subject
	{
		//Readonly
		public const int inventoryNum = 100;
		//Private
		ISetItem<Item> inputItem = default;
		IClickAction<Item_Mono> clickAction;

		//Public
		public int InventoryId;
		[NonSerialized]
		public Item_Mono[] items;
		public Cal SlotNum = new Cal(10);

		[SerializeField]
		Button GenerateItemButton;

		// Use this for initialization
		void Awake()
		{
			clickAction = new SwapItemFromInventory<Item_Mono, Item>();
			var create = new CreateItemByOrder<Item>(items, SlotNum);
			GenerateItemButton.OnClickAsObservable().Subscribe(_ => create.SetItem(new Item(UnityEngine.Random.Range(1,6))));
			items.ToList()
                .ForEach(x => x.gameObject.GetOrAddComponent<ObservableEventTrigger>().OnPointerDownAsObservable()
			    .Subscribe((UnityEngine.EventSystems.PointerEventData obj) => {
                    if(obj.pointerId == -1)
                    {
						clickAction.Click(x);
                    } 
                    if (obj.pointerId == -2) x.Delete(); }));
			this.ObserveEveryValueChanged(_ => inputItem?.GetItem().id).Subscribe(_ => Notify());
			this.ObserveEveryValueChanged(_ => SlotNum.GetValue()).Subscribe(_ => Notify());
		}      
    }
}