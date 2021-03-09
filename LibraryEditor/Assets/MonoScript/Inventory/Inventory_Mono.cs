using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using static UsefulMethod;
using static Main;
using TMPro;

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
	public class Inventory_Mono : MonoBehaviour, IItemController
	{
		public int MaxSize { get => 100; }
		Cal slotNum;
	    public Item_Mono[] items;

		[SerializeField]
		Button GenerateItemButton;
		// Use this for initialization
		void Awake()
		{
			items = gameObject.GetComponentsInChildren<Item_Mono>();
		}

		// Use this for initialization
		void Start()
		{

		}
	}
}