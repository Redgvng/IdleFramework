using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static IdleLibrary.UsefulMethod;
using static IdleLibrary.UsefulStatic;
using static IdleLibrary.Main;
using IdleLibrary;

/// <summary>
/// 主にsaveしたい配列の初期化を行うクラス
/// InitializeArray(ref main.SR.hoge, サイズ);
/// のようにして初期化する。アップデートなどで途中から変更することも可能。
/// 初期化はAwake()のAwakeBASE();のあとに書くことを推奨。
/// </summary>

namespace IdleLibrary
{
	public class SaveDeclare : MonoBehaviour
	{

		// Use this for initialization
		void Awake()
		{
			//NUMBER
			//InitializeArray(ref main.SR.upgradeLevels, Enum.GetValues(typeof(UpgradeKind)).Length);
		}
	}
}
