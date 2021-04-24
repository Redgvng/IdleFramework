using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace IdleLibrary.Inventory
{
    public class InventoryDraw : MonoBehaviour, IObserver
    {
        public Sprite[] sprites;
        //public Sprite defaultSprite;
        public Sprite lockedSprite;
        public Transform MouseImageCanvas;
        GameObject _itemIconWithMouse;

        void Awake()
        {
            GameObject.FindObjectsOfType<Subject>().ToList().ForEach(x => x.Attach(this)); 
        }

        //itemの状態を更新します。
        public void _Update(ISubject subject)
        {
            if(subject is Inventory_Mono)
            {
                var inventory_mono = subject as Inventory_Mono;
                foreach(var info in inventory_mono.UIInfoList)
                {
                    int index = 0;
                    foreach (var item in info.inventory.GetItems())
                    {
                        var it = info.items[index];
                        if (it == null) continue;
                        if (item.isSet)
                        {
                            info.items[index].transform.GetChild(0).GetComponent<Image>().sprite = sprites[item.id];
                        }
                        else
                        {
                            info.items[index].transform.GetChild(0).GetComponent<Image>().sprite = lockedSprite;
                        }
                        index++;
                    }
                }

                //マウスにくっつくウインドウの設定(後々柔軟に変えられるようにしたい)
                if(_itemIconWithMouse == null)
                {
                    _itemIconWithMouse = Instantiate(inventory_mono.item, MouseImageCanvas);
                    _itemIconWithMouse.GetComponent<Image>().raycastTarget = false;
                    _itemIconWithMouse.transform.GetChild(0).GetComponent<Image>().raycastTarget = false;
                }
                if(inventory_mono.inputItem.inputItem.id == -1)
                {
                    _itemIconWithMouse.SetActive(false);
                }
                else
                {
                    _itemIconWithMouse.SetActive(true);
                    _itemIconWithMouse.transform.GetChild(0).GetComponent<Image>().sprite = sprites[inventory_mono.inputItem.inputItem.id];
                    _itemIconWithMouse.transform.position = Input.mousePosition;
                }
            }
        }
    }
}
