using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace InventoryLibrary
{
    public class InventoryDraw : MonoBehaviour, IObserver
    {
        public Sprite[] sprites;
        public Sprite defaultSprite;
        public Sprite lockedSprite;
        ISubject[] items;
        ISubject inventoryController;

        void Awake()
        {
            items = gameObject.GetComponentsInChildren<ISubject>();
            inventoryController = gameObject.GetComponent<ISubject>();
            items.ToList().ForEach(x => x.Attach(this));
            inventoryController.Attach(this);
        }
        //itemの状態を更新します。
        public void _Update(ISubject subject)
        {
            Debug.Log(subject is Inventory_Mono);
            if (subject is Item_Mono)
            {
                var item = subject as Item_Mono;
                item.gameObject.GetComponent<Image>().sprite = sprites[item.GetItem().id];
            }
            if(subject is Inventory_Mono)
            {
                var item = subject as Inventory_Mono;
                var indexedItem = item.items.ToList().Select((c, i) => new { Content = c, index = i });
                indexedItem.ToList().ForEach(a =>
                {
                    if (item.SlotNum.GetValue() <= a.index)
                    {
                        a.Content.transform.GetChild(0).GetComponent<Image>().sprite = lockedSprite;
                        a.Content.gameObject.GetComponent<Image>().raycastTarget = false;
                        a.Content.transform.GetChild(0).GetComponent<Image>().raycastTarget = false;
                        a.Content.transform.GetChild(0).gameObject.SetActive(true);
                    }
                    else
                    {
                        a.Content.gameObject.GetComponent<Image>().raycastTarget = true;
                        a.Content.transform.GetChild(0).GetComponent<Image>().raycastTarget = true;
                        a.Content.transform.GetChild(0).gameObject.SetActive(false);
                    }
                });
            }
        }
    }
}
