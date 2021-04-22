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

        void Awake()
        {
            GameObject.FindObjectsOfType<Subject>().ToList().ForEach(x => x.Attach(this));
        }

        //itemの状態を更新します。
        public void _Update(ISubject subject)
        {
            if(subject is Inventory_Mono)
            {
                Debug.Log("よばれてるよ");
                var inventory_mono = subject as Inventory_Mono;
                int index = 0;
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
            }
        }
    }
}
