using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UsefulMethod;
using static UsefulStatic;
using static Main;
using CommonLibrary;

/// <summary>
/// 主にsaveしたい配列の初期化を行うクラス
/// InitializeArray(ref main.SR.hoge, サイズ);
/// のようにして初期化する。アップデートなどで途中から変更することも可能。
/// 初期化はAwake()のAwakeBASE();のあとに書くことを推奨。
/// </summary>
public class SaveDeclare :MonoBehaviour {

	// Use this for initialization
	void Awake () {
		//NUMBER
		InitializeArray(ref main.S.numbers, Enum.GetValues(typeof(NumbersName)).Length);
		//Inventory
		InitializeArray(ref main.S.items, 100);
		InitializeArray(ref main.S.equippedItems, 100);

	}

	// Use this for initialization
	void Start () {

	}

	void Update()
    {
		Debug.Log(main.S.numbers.Length);
		Debug.Log(main.S.numbers.Length);
		Debug.Log(main.S.items.Length);
	}
}
