using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IdleLibrary;
using IdleLibrary.Inventory;
using UniRx.Triggers;
using UniRx;
using System.Linq;

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
		_leftActions.Add((action, key));
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
		//inventoryのexpandNumが変化したら、それに応じてExpandを呼ぶ？
		this.ObserveEveryValueChanged(_ => inventory.totalInventoryNum).Subscribe(_ =>
		{
			int diff = inventory.totalInventoryNum - inventory.GetInventoryLength();
			if (diff <= 0) return;
			for (int i = 0; i < diff; i++)
			{
				inventory.items.Add(new NullItem(-1));
			}
		});
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
				inputItem.hoveredInventory = inventory;
			});
		item.GetOrAddComponent<ObservableEventTrigger>().OnPointerExitAsObservable()
			.Subscribe(_ => {
				inputItem.cursorId = -1;
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
