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
using Zenject;

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
		[Inject] GameSystem system;
		// Use this for initialization
		void Awake()
		{
			Debug.Log("Declareよばれてる");
			Main.main.dto.Initialize();
			InitializeArray(ref
                Main.main.SR.isSelected, Enum.GetValues(typeof(UpgradeContainer.UpgradeKind)).Length);
			InitializeArray(ref Main.main.SR.isUnlockCanvasLocked, Enum.GetValues(typeof(EquationKind)).Length);
		}
	}
}
