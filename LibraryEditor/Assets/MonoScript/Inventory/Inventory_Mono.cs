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
	public class Inventory_Mono : MonoBehaviour
	{

		static readonly int maxSize = 100;
		Cal slotNum;
		public Item_Mono[] items;

		[SerializeField]
		Button GenerateItemButton;
		// Use this for initialization
		void Awake()
		{

		}

		// Use this for initialization
		void Start()
		{

		}
	}
}