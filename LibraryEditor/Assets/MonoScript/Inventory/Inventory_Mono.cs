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
using Cysharp.Threading.Tasks;
using System.Linq;
namespace IdleLibrary.Inventory
{
	public class InventoryInfo
    {
		public Inventory inventory;
		public Transform canvas;
		public List<GameObject> items = new List<GameObject>();
		private GameObject itemPre;
		private List<(IInventoryAction action, KeyCode key)> _leftActions = new List<(IInventoryAction action, KeyCode key)>();
		private List<(IInventoryAction action, KeyCode key)> _rightActions = new List<(IInventoryAction action, KeyCode key)>();
		public void AddLeftAction(IInventoryAction action, KeyCode key = KeyCode.None)
		{
			_leftActions.Add((action,key));
		}
		public void AddRightaction(IInventoryAction action, KeyCode key = KeyCode.None)
        {
			_rightActions.Add((action, key));
        }
		public InventoryInfo(Inventory inventory, Transform canvas, List<GameObject> items, GameObject itemPre)
        {
			this.inventory = inventory;
			this.canvas = canvas;
			this.items = items;
			this.itemPre = itemPre;
			Initialize();
        }
		void Initialize()
        {
			Debug.Log(inventory.GetInventoryLength());
			for (int i = 0; i < inventory.GetInventoryLength(); i++)
			{
				InstantiateItem();
			}

			//inventoryLength()の値が変化したら、それに応じてInstantiateする.,
			this.ObserveEveryValueChanged(_ => inventory.GetInventoryLength()).Subscribe(_ =>
			{
				int diff = inventory.GetInventoryLength() - items.Count;
				if (diff <= 0) return;
                for (int i = 0; i < diff; i++)
                {
					InstantiateItem();
				}
			});
		}

		//Instantiateして、Observableをつける処理をする関数
		void InstantiateItem()
        {
			var item = GameObject.Instantiate(itemPre, canvas);
			item.GetOrAddComponent<ObservableEventTrigger>().OnPointerDownAsObservable()
				.Subscribe((UnityEngine.EventSystems.PointerEventData obj) =>
				{
					int index = items.IndexOf(item);
					bool hasKey = false;
					if (obj.pointerId == -1)
					{
						//_leftActions.ForEach((action) => action.Action(index));
						//keyが登録されているものから探索する。
						_leftActions.Where((pair) => pair.key != KeyCode.None).ToList().ForEach((pair) =>
						{
							if (Input.GetKey(pair.key))
							{
								hasKey = true;
								pair.action.Action(index);
								return;
							}
						});
						if (hasKey) return;
						_leftActions.Where((pair) => pair.key == KeyCode.None).ToList().ForEach((pair) =>
						{
							pair.action.Action(index);
							return;
						});
					}
					if (obj.pointerId == -2)
					{
						//_rightActions.ForEach((action) => action.Action(index));
						_rightActions.Where((pair) => pair.key != KeyCode.None).ToList().ForEach((pair) =>
						{
							if (Input.GetKey(pair.key))
							{
								hasKey = true;
								pair.action.Action(index);
								return;
							}
						});
						if (hasKey) return;
						_rightActions.Where((pair) => pair.key == KeyCode.None).ToList().ForEach((pair) =>
						{
							pair.action.Action(index);
							return;
						});
					}
				});
			items.Add(item);

        }
		
    }
	public class Inventory_Mono : Subject
	{
		[SerializeField]
		Button GenerateItemButton;
		[SerializeField]
		Button ExpandInventory;
		[SerializeField]
		Button ExpandEquipmentInventory;

		public GameObject item;

		public Transform canvas;
		[NonSerialized]
		public List<GameObject> items = new List<GameObject>();

		public Transform EquipmentCanvas;
		[NonSerialized]
		public List<GameObject> EquippedItems = new List<GameObject>();

		public List<InventoryInfo> UIInfoList = new List<InventoryInfo>();
		public InputItem inputItem = new InputItem();

		public InventoryInfo inventory;
		public InventoryInfo equipmentInventory;

		// Use this for initialization
		void Awake()
		{
			inventory = new InventoryInfo(new Inventory(inputItem, Main.main.S.inventory), canvas, items, item);
			equipmentInventory = new InventoryInfo(new Inventory(inputItem, Main.main.S.equipmentInventory), EquipmentCanvas, EquippedItems, item);

			//UIと紐づける
			UIInfoList.Add(inventory);
			UIInfoList.Add(equipmentInventory);

			//アクションを設定
			inventory.AddLeftAction(new SwapItem(inventory.inventory));
			inventory.AddLeftAction(new LockItem(inventory.inventory), KeyCode.L);
			inventory.AddRightaction(new DeleteItem(inventory.inventory));

			equipmentInventory.AddLeftAction(new SwapItem(equipmentInventory.inventory));
			equipmentInventory.AddRightaction(new RevertItemToOtherInventory(equipmentInventory.inventory, inventory.inventory));

			GenerateItemButton.OnClickAsObservable().Subscribe(_ => {
				inventory.inventory.SetItemByOrder(new Item(UnityEngine.Random.Range(0, 5)));
				equipmentInventory.inventory.SetItemByOrder(new Item(UnityEngine.Random.Range(0, 5)));
				});
			ExpandInventory.OnClickAsObservable().Subscribe(_ =>
			{
				inventory.inventory.ExpandInventory();
			});
			ExpandEquipmentInventory.OnClickAsObservable().Subscribe(_ =>
			{
				equipmentInventory.inventory.ExpandInventory();
			});

			SaveInventory();
			Notify();
		}

		private async void SaveInventory()
        {
			while (true)
			{
				Main.main.S.inventory = inventory.inventory.saveData;
				Main.main.S.equipmentInventory = equipmentInventory.inventory.saveData;
				await UniTask.DelayFrame(60);
			}
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
				Notify();
            }
        }
    }
}