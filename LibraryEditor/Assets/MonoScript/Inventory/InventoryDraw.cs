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
            if (subject is Item_Mono)
            {
                var item = subject as Item_Mono;
                item.gameObject.GetComponent<Image>().sprite = sprites[item.GetItem().id];
            }
        }
    }
}
