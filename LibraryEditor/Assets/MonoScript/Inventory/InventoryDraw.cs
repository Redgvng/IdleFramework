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
                Debug.Log(inventory_mono.inventory.InputId);
                int index = 0;
                //スロットの反映
                foreach (var item in inventory_mono.inventory.GetItems())
                {
                    if (item.isSet)
                    {
                        inventory_mono.items[index].transform.GetChild(0).GetComponent<Image>().sprite = sprites[item.id];
                    }
                    else
                    {
                        inventory_mono.items[index].transform.GetChild(0).GetComponent<Image>().sprite = lockedSprite;
                        //inventory_mono.items[index].gameObject.GetComponent<Image>().raycastTarget = false;
                        //inventory_mono.items[index].transform.GetChild(0).GetComponent<Image>().raycastTarget = false;
                        //inventory_mono.items[index].transform.GetChild(0).gameObject.SetActive(true);
                    }
                    index++;
                }
                //マウスにくっつくウインドウの設定(後々柔軟に変えられるようにしたい)
                if(_itemIconWithMouse == null)
                {
                    _itemIconWithMouse = Instantiate(inventory_mono.item, inventory_mono.canvas);
                    _itemIconWithMouse.GetComponent<Image>().raycastTarget = false;
                    _itemIconWithMouse.transform.GetChild(0).GetComponent<Image>().raycastTarget = false;
                }
                if(inventory_mono.inventory.InputId == -1)
                {
                    _itemIconWithMouse.SetActive(false);
                }
                else
                {
                    _itemIconWithMouse.SetActive(true);
                    _itemIconWithMouse.transform.GetChild(0).GetComponent<Image>().sprite = sprites[inventory_mono.inventory.GetItem(inventory_mono.inventory.InputId).id];
                    _itemIconWithMouse.transform.position = Input.mousePosition;
                }
            }
        }
    }
}
