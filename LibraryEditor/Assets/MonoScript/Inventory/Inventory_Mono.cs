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
	    public Item_Mono[] items;
		ISetItem<Item> inputItem = default;
		public Cal SlotNum = new Cal(10);
		[SerializeField]
		Button GenerateItemButton;
		//クリック関係
		IClickAction<Item_Mono> clickAction;
		// Use this for initialization
		void Awake()
		{
			items = gameObject.GetComponentsInChildren<Item_Mono>();
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