﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;
using System.Linq;
using IdleLibrary.UI;

namespace IdleLibrary
{
	public partial class Save
	{
		public Inventory.InventoryForSave inventory, equipmentInventory;
	}
}
namespace IdleLibrary.Inventory
{
	public interface IInventoryUIInfo
    {
		InputItem input { get; }
		IEnumerable<InventoryInfo> UIInfo { get; }
    }
	public class Inventory_Mono : Subject, IInventoryUIInfo
	{
		
		[SerializeField]
		Button GenerateItemButton;
		[SerializeField]
		Button ExpandInventory;
		[SerializeField]
		Button ExpandEquipmentInventory;
		

		public GameObject item;
		public TextMeshProUGUI inventoryItemInfoText;

		public Transform canvas;
		[NonSerialized]
		public List<GameObject> items = new List<GameObject>();

		public Transform EquipmentCanvas;
		[NonSerialized]
		public List<GameObject> EquippedItems = new List<GameObject>();

	    List<InventoryInfo> UIInfoList = new List<InventoryInfo>();
	    InputItem inputItem = new InputItem();
		public InputItem input => inputItem;
		public IEnumerable<InventoryInfo> UIInfo => UIInfoList;

		[NonSerialized] public InventoryInfo inventory;
		[NonSerialized] public InventoryInfo equipmentInventory;

		// Use this for initialization
		void Awake()
		{
			inventory = new InventoryInfo(new Inventory(inputItem, ref Main.main.S.inventory, 10), canvas, items, item, inputItem);
			equipmentInventory = new InventoryInfo(new Inventory(inputItem, ref Main.main.S.equipmentInventory, 10), EquipmentCanvas, EquippedItems, item, inputItem);

			//UIと紐づける
			UIInfoList.Add(inventory);
			UIInfoList.Add(equipmentInventory);

			//アクションを設定
			//inventory.AddLeftAction(new SwapItem(inventory.inventory));
			//クリックじゃなくてドラッグアンドドロップにもできる
			var swap = new StackAndSwapItem(inventory.inventory);
			inventory.RegisterHoldAction(swap, new Releaseitem(inputItem), swap);
			inventory.AddLeftAction(new ShowInfoToTextField(inventory.inventory, inventoryItemInfoText));
			inventory.AddLeftAction(new LockItem(inventory.inventory), KeyCode.L);
			inventory.AddLeftAction(new DeleteItem(inventory.inventory), KeyCode.D);
			inventory.AddRightaction(new RevertItemToOtherInventory(inventory.inventory,equipmentInventory.inventory));

			//equipmentInventory.AddLeftAction(new SwapItem(equipmentInventory.inventory));
			var swap2 = new StackAndSwapItem(equipmentInventory.inventory);
			equipmentInventory.inventory.RegisterSetItem(new StartIdleActionWithSet(new SimpleSetItem(equipmentInventory.inventory)));
			equipmentInventory.RegisterHoldAction(swap2, new Releaseitem(inputItem), swap2);
			equipmentInventory.AddRightaction(new RevertItemToOtherInventory(equipmentInventory.inventory, inventory.inventory));
			equipmentInventory.AddLeftAction(new ShowInfoToTextField(equipmentInventory.inventory, inventoryItemInfoText));

			//ItemのIdle Actionの設定をします.
			equipmentInventory.inventory.GetItems().ToList().ForEach((x) =>
			{
				Artifact arti = null;
				if (x is Artifact) arti = x as Artifact;
				if (arti != null)
                {
					arti.idleAction.Initialize();
					arti.idleAction.Start();
                }
			});

			//ItemFactoryを作ります
			var itemFactory = new ItemFactory();
			GenerateItemButton.OnClickAsObservable().Subscribe(_ => {
				var item = itemFactory.CreateRandomItem();
				//var item = new StackableItem(UnityEngine.Random.Range(0,5));
				inventory.inventory.SetItemByOrder(item);
				});
			ExpandInventory.OnClickAsObservable().Subscribe(_ =>
			{
				inventory.inventory.ExpandInventory();
			});
			ExpandEquipmentInventory.OnClickAsObservable().Subscribe(_ =>
			{
				equipmentInventory.inventory.ExpandInventory();
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
				Notify();
            }
        }
    }
}