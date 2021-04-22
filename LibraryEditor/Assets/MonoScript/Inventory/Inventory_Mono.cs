﻿using System.Collections;
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
namespace IdleLibrary.Inventory
{
	public class Inventory_Mono : Subject
	{
		[SerializeField]
		Button GenerateItemButton;

		[SerializeField]
		private GameObject item;
		[SerializeField]
		private Transform canvas;

		public Inventory inventory = new Inventory();
		public GameObject[] items;

		// Use this for initialization
		void Awake()
		{
			//インスタンス化
			items = new GameObject[inventory.GetInventoryLength()];
            for (int i = 0; i < items.Length; i++)
            {
				items[i] = Instantiate(item, canvas);
			}

			GenerateItemButton.OnClickAsObservable().Subscribe(_ => inventory.SetItemByOrder(new Item(UnityEngine.Random.Range(0,5))));
			items.Select((game, index) => new { game, index})
				.ToList()
                .ForEach(x => x.game.GetOrAddComponent<ObservableEventTrigger>().OnPointerDownAsObservable()
			    .Subscribe((UnityEngine.EventSystems.PointerEventData obj) => {
                    if(obj.pointerId == -1)
                    {
						//clickAction.Click(x);
					} 
                    if (obj.pointerId == -2) inventory.DeleteItem(x.index);  }));
			Notify();
		}
        private void Update()
        {
            if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
            {
				Notify();
            }
        }
    }
}