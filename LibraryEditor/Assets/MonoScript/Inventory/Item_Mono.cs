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

namespace InventoryLibrary
{
	public class Item_Mono : MonoBehaviour, IItem
	{
		//idでアイテムを識別します。
        public int id { get; set; }

        // Use this for initialization
        void Awake()
		{

		}

		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}
	}
}
