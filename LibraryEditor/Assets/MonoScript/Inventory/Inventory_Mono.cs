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
namespace IdleLibrary.Inventory
{
	public class Inventory_Mono : Subject
	{
		[SerializeField]
		Button GenerateItemButton;

		public GameObject item;

		public Inventory inventory = new Inventory();
		public Transform canvas;
		[NonSerialized]
		public GameObject[] items;

		public Inventory EquipmentInventory = new Inventory();
		public Transform EquipmentCanvas;
		[NonSerialized]
		public GameObject[] EquippedItems;

		public List<(Inventory inventory, GameObject[] items)> UIInfoList = new List<(Inventory inventory, GameObject[] items)>();

		void InitializeInventory(Inventory inventory, GameObject[] items, Transform canvas)
        {
			//インスタンス化
			items = new GameObject[inventory.GetInventoryLength()];
			for (int i = 0; i < items.Length; i++)
			{
				items[i] = Instantiate(item, canvas);
			}
			//InventoryActionの登録
			var swap = new SwapItem(inventory);
			var delete = new DeleteItem(inventory);

			items.Select((game, index) => new { game, index })
				.ToList()
				.ForEach(x => x.game.GetOrAddComponent<ObservableEventTrigger>().OnPointerDownAsObservable()
				.Subscribe((UnityEngine.EventSystems.PointerEventData obj) => {
					if (obj.pointerId == -1)
					{
						swap.Action(x.index);
					}
					if (obj.pointerId == -2)
					{
						delete.Action(x.index);
					}
				}));
			UIInfoList.Add((inventory, items));
		}

		// Use this for initialization
		void Awake()
		{
			InitializeInventory(inventory, items, canvas);
			InitializeInventory(EquipmentInventory, EquippedItems, EquipmentCanvas);

			GenerateItemButton.OnClickAsObservable().Subscribe(_ => {
				inventory.SetItemByOrder(new Item(UnityEngine.Random.Range(0, 5)));
				EquipmentInventory.SetItemByOrder(new Item(UnityEngine.Random.Range(0, 5)));
				});
			Notify();
		}
        private void Update()
        {
			Notify();
            if (Input.GetMouseButtonDown(1))
            {
				inventory.ReleaseItem();
            }
			if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
            {
				//Notify();
            }
        }
    }
}