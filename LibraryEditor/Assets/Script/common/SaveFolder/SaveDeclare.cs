using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UsefulMethod;
using static UsefulStatic;
using static Main;

/// <summary>
/// 主にsaveしたい配列の初期化を行うクラス
/// InitializeArray(ref main.SR.hoge, サイズ);
/// のようにして初期化する。アップデートなどで途中から変更することも可能。
/// 初期化はAwake()のAwakeBASE();のあとに書くことを推奨。
/// </summary>
public class SaveDeclare :MonoBehaviour {

	// Use this for initialization
	void Awake () {
		//Upgrade 
		//InitializeArray(ref main.S.UpgradeLevels, Enum.GetValues(typeof(Upgrade.ID)).Length);
		InitializeArray(ref main.S.items, 100);
	}

	// Use this for initialization
	void Start () {

	}
}
