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
        public Sprite defaultSprite;
        public Sprite lockedSprite;

        void Awake()
        {
            GameObject.FindObjectsOfType<Subject>().ToList().ForEach(x => x.Attach(this));
        }

        //itemの状態を更新します。
        public void _Update(ISubject subject)
        {
            if (subject is Item_Mono)
            {
                Debug.Log(subject);
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
