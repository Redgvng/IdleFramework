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
                var inventory_mono = subject as Inventory_Mono;
                int index = 0;
                foreach (var item in inventory_mono.inventory.GetItems())
                {
                    if (item.isSet)
                    {

                    }
                }
            }
        }
    }
}
