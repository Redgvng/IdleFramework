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
	public class InventoryInfo
    {
		public Inventory inventory;
		public Transform canvas;
		public GameObject[] items;
		private GameObject itemPre;
		private List<IInventoryAction> _leftActions = new List<IInventoryAction>();
		private List<IInventoryAction> _rightActions = new List<IInventoryAction>();
		public void AddLeftAction(IInventoryAction action)
		{
			_leftActions.Add(action);
		}
		public void AddRightaction(IInventoryAction action)
        {
			_rightActions.Add(action);
        }
		public InventoryInfo(Inventory inventory, Transform canvas, GameObject[] items, GameObject itemPre)
        {
			this.inventory = inventory;
			this.canvas = canvas;
			this.items = items;
			this.itemPre = itemPre;
			Initialize();
        }
		void Initialize()
        {
			//インスタンス化
			items = new GameObject[inventory.GetInventoryLength()];
			for (int i = 0; i < items.Length; i++)
			{
				items[i] = GameObject.Instantiate(itemPre, canvas);
			}

			AddRightaction(new DeleteItem(inventory));

			items.Select((game, index) => new { game, index })
				.ToList()
				.ForEach(x => x.game.GetOrAddComponent<ObservableEventTrigger>().OnPointerDownAsObservable()
				.Subscribe((UnityEngine.EventSystems.PointerEventData obj) => {
					if (obj.pointerId == -1)
					{
						_leftActions.ForEach((action) => action.Action(x.index));
					}
					if (obj.pointerId == -2)
					{
						_rightActions.ForEach((action) => action.Action(x.index));
					}
				}));
		}
    }
	public class Inventory_Mono : Subject
	{
		[SerializeField]
		Button GenerateItemButton;

		public GameObject item;

		public Transform canvas;
		[NonSerialized]
		public GameObject[] items;

		public Transform EquipmentCanvas;
		[NonSerialized]
		public GameObject[] EquippedItems;

		public List<InventoryInfo> UIInfoList = new List<InventoryInfo>();
		public InputItem inputItem = new InputItem();

		public InventoryInfo inventory;
		public InventoryInfo equipmentInventory;

		/*
		void InitializeInventory(Inventory inventory, GameObject[] items, Transform canvas)
        {
			//インスタンス化
			items = new GameObject[inventory.GetInventoryLength()];
			for (int i = 0; i < items.Length; i++)
			{
				items[i] = Instantiate(item, canvas);
			}
			//InventoryActionの登録
			var swap = new SwapItemWithSameInventory(inventory);
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
		*/

		// Use this for initialization
		void Awake()
		{
			inventory = new InventoryInfo(new Inventory(inputItem), canvas, items, item);
			equipmentInventory = new InventoryInfo(new Inventory(inputItem), EquipmentCanvas, EquippedItems, item);

			//UIと紐づける
			UIInfoList.Add(inventory);
			UIInfoList.Add(equipmentInventory);

			//アクションを設定
			inventory.AddLeftAction(new SwapItemWithOtherInventory(inventory.inventory, equipmentInventory.inventory));
			equipmentInventory.AddLeftAction(new SwapItemWithOtherInventory(equipmentInventory.inventory,inventory.inventory));

			GenerateItemButton.OnClickAsObservable().Subscribe(_ => {
				inventory.inventory.SetItemByOrder(new Item(UnityEngine.Random.Range(0, 5)));
				equipmentInventory.inventory.SetItemByOrder(new Item(UnityEngine.Random.Range(0, 5)));
				});
			Notify();
		}
        private void Update()
        {
			Notify();
            if (Input.GetMouseButtonDown(1))
            {
				inputItem.ReleaseItem();
            }
			if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
            {
				//Notify();
            }
        }
    }
}