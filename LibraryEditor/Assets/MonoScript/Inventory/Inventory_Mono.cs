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


public partial class Save
{
	public IdleLibrary.Inventory.InventoryForSave inventory, equipmentInventory;
}


namespace IdleLibrary.Inventory
{
	public class InventoryInfo
    {
		public Inventory inventory;
		public Transform canvas;
		public List<GameObject> items = new List<GameObject>();
		private GameObject itemPre;
		private InputItem inputItem;
		private List<(IInventoryAction action, KeyCode key)> _leftActions = new List<(IInventoryAction action, KeyCode key)>();
		private List<(IInventoryAction action, KeyCode key)> _rightActions = new List<(IInventoryAction action, KeyCode key)>();
		private IInventoryAction[] _holdAction = new IInventoryAction[3];
		public void AddLeftAction(IInventoryAction action, KeyCode key = KeyCode.None)
		{
			_leftActions.Add((action,key));
		}
		public void AddRightaction(IInventoryAction action, KeyCode key = KeyCode.None)
        {
			_rightActions.Add((action, key));
        }
		public void RegisterHoldAction(IInventoryAction holdAction, IInventoryAction cancelAction, IInventoryAction releaseAction)
        {
			_holdAction[0] = holdAction;
			_holdAction[1] = cancelAction;
			_holdAction[2] = releaseAction;
        }
		public InventoryInfo(Inventory inventory, Transform canvas, List<GameObject> items, GameObject itemPre, InputItem inputItem)
        {
			this.inventory = inventory;
			this.canvas = canvas;
			this.items = items;
			this.itemPre = itemPre;
			this.inputItem = inputItem;
			Initialize();

		}
		void Initialize()
        {
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
		//最終的にはこの中身もカプセル化すべきか？
		void InstantiateItem()
        {
			var item = GameObject.Instantiate(itemPre, canvas);
			item.GetOrAddComponent<ObservableEventTrigger>().OnPointerDownAsObservable()
				.Subscribe((UnityEngine.EventSystems.PointerEventData obj) =>
				{
					int index = items.IndexOf(item);
					bool hasKey = false;
					void DoAction(List<(IInventoryAction action, KeyCode key)> list)
                    {
						//keyが登録されているものから探索する。
						list.Where((pair) => pair.key != KeyCode.None).ToList().ForEach((pair) =>
						{
							if (Input.GetKey(pair.key))
							{
								hasKey = true;
								pair.action.Action(index);
								return;
							}
						});
						if (hasKey) return;
						list.Where((pair) => pair.key == KeyCode.None).ToList().ForEach((pair) =>
						{
							pair.action.Action(index);
							return;
						});
					}
					if (obj.pointerId == -1)
					{
						DoAction(_leftActions);
					}
					if (obj.pointerId == -2)
					{
						DoAction(_rightActions);
					}
				});
			item.GetOrAddComponent<ObservableEventTrigger>().OnPointerEnterAsObservable()
				.Subscribe(_ => {
					inputItem.cursorId = items.IndexOf(item);
					});
			item.GetOrAddComponent<ObservableEventTrigger>().OnBeginDragAsObservable()
				.Subscribe(_ => _holdAction[0].Action(items.IndexOf(item)));
			item.GetOrAddComponent<ObservableEventTrigger>().OnDropAsObservable()
				.Subscribe(_ => {
					if (inputItem.cursorId == -1) 
					{
						_holdAction[1].Action(items.IndexOf(item));
                    }
                    else
                    {
						_holdAction[2].Action(items.IndexOf(item));
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
		[SerializeField]
		Button SortByIdButton;

		public GameObject item;
		public TextMeshProUGUI inventoryItemInfoText;

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
			inventory = new InventoryInfo(new Inventory(inputItem.info, Main.main.S.inventory), canvas, items, item, inputItem);
			equipmentInventory = new InventoryInfo(new Inventory(inputItem.info, Main.main.S.equipmentInventory), EquipmentCanvas, EquippedItems, item, inputItem);

			//UIと紐づける
			UIInfoList.Add(inventory);
			UIInfoList.Add(equipmentInventory);

			//アクションを設定
			//inventory.AddLeftAction(new SwapItem(inventory.inventory));
			//クリックじゃなくてドラッグアンドドロップにもできる
			var swap = new SwapItem(inventory.inventory, inputItem);
			inventory.RegisterHoldAction(swap, new Releaseitem(inputItem), swap);
			inventory.AddLeftAction(new ShowInfoToTextField(inventory.inventory, inventoryItemInfoText));
			inventory.AddLeftAction(new LockItem(inventory.inventory), KeyCode.L);
			inventory.AddLeftAction(new DeleteItem(inventory.inventory), KeyCode.D);
			inventory.AddRightaction(new RevertItemToOtherInventory(inventory.inventory,equipmentInventory.inventory));

			//equipmentInventory.AddLeftAction(new SwapItem(equipmentInventory.inventory));
			var swap2 = new SwapItem(equipmentInventory.inventory, inputItem);
			equipmentInventory.RegisterHoldAction(swap2, new Releaseitem(inputItem), swap2);
			equipmentInventory.AddRightaction(new RevertItemToOtherInventory(equipmentInventory.inventory, inventory.inventory));

			//Canvasの外に出たらcursorIdを-1にする
			//canvas.gameObject.GetOrAddComponent<ObservableEventTrigger>().OnPointerExitAsObservable()
			//	.Subscribe(_ => inputItem.cursorId = -1);


			GenerateItemButton.OnClickAsObservable().Subscribe(_ => {
				inventory.inventory.GenerateItemRandomly();
				equipmentInventory.inventory.GenerateItemRandomly();
				});
			ExpandInventory.OnClickAsObservable().Subscribe(_ =>
			{
				inventory.inventory.ExpandInventory();
			});
			ExpandEquipmentInventory.OnClickAsObservable().Subscribe(_ =>
			{
				equipmentInventory.inventory.ExpandInventory();
			});
			SortByIdButton.OnClickAsObservable().Subscribe(_ =>
			{
				inventory.inventory.SortById();
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