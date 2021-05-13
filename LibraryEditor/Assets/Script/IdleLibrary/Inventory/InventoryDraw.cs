using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Cysharp.Threading.Tasks;
using UniRx.Triggers;
using UniRx;
using System;
using IdleLibrary.UI;

namespace IdleLibrary.Inventory
{
    public class InventoryDraw : MonoBehaviour, IObserver
    {
        public Sprite[] sprites;
        //public Sprite defaultSprite;
        public Sprite lockedSprite;
        public Transform MouseImageCanvas;
        GameObject _itemIconWithMouse;
        Popup popUp;
        bool isPopUpInitialized;
        [SerializeField] Popup_UI pop;
        [SerializeField] GameObject mouseImagePre;

        void Awake()
        {
            GameObject.FindObjectsOfType<Subject>().ToList().ForEach(x => x.Attach(this)); 
        }

        //itemの状態を更新します。
        public void _Update(ISubject subject)
        {
            if(subject is IInventoryUIInfo)
            {
                var inventory_mono = subject as IInventoryUIInfo;
                var input = inventory_mono.input;

                //ポップアップの設定

                if(!isPopUpInitialized)
                {
                    Debug.Log("yobareteruyo");
                    popUp = new Popup(() => input.hoveredInventory != null && input.hoveredInventory.GetItem(input.cursorId).isSet, pop.gameObject);
                    pop.UpdateAsObservable().Where(_ => pop.gameObject.activeSelf).Subscribe(_ =>
                    {
                        if (!input.hoveredInventory.GetItem(input.cursorId).isSet) return;
                        pop.UpdateUI(
                            LocationKind.MouseFollow, 
                            input.hoveredInventory.GetItem(input.cursorId),
                            sprites[input.hoveredInventory.GetItem(input.cursorId).id]);
                        });
                    isPopUpInitialized = true;
                }

                //アイテム画像の更新
                foreach(var info in inventory_mono.UIInfo)
                {
                    int index = 0;
                    foreach (var item in info.inventory.GetItems())
                    {
                        if (index >= info.items.Count) continue;
                        //アイテム画像
                        info.items[index].transform.GetChild(0).GetComponent<Image>().sprite = item.isSet ? sprites[item.id] : lockedSprite;
                        info.items[index].transform.GetChild(1).gameObject.SetActive(item.isLocked);
                        index++;
                    }
                }

                //マウスにくっつくウインドウの設定(後々柔軟に変えられるようにしたい)
                if(_itemIconWithMouse == null)
                {
                    _itemIconWithMouse = Instantiate(mouseImagePre, MouseImageCanvas);
                    _itemIconWithMouse.GetOrAddComponent<ObservableEventTrigger>().OnPointerUpAsObservable().Subscribe(_ =>
                    {
                        _itemIconWithMouse.SetActive(false);
                    });
                    _itemIconWithMouse.GetComponent<Image>().raycastTarget = false;
                    _itemIconWithMouse.transform.GetChild(0).GetComponent<Image>().raycastTarget = false;
                }
                if (input.inputItem.id == -1)
                {
                    _itemIconWithMouse.SetActive(false);
                }
                else
                {
                    _itemIconWithMouse.SetActive(true);
                    _itemIconWithMouse.transform.GetChild(0).GetComponent<Image>().sprite = sprites[input.inputItem.id];
                    _itemIconWithMouse.transform.position = Input.mousePosition;
                    if(Input.GetMouseButtonUp(0)){
                        input.ReleaseItem();
                    }
                }
            }
        }
    }
}
