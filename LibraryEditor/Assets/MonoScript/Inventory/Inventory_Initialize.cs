using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdleLibrary.Inventory {
    public class Inventory_Initialize : MonoBehaviour
    {
        Inventory_Mono inventory;
        [SerializeField]
        Item_Mono itemPre;
        void Awake()
        {
            inventory = gameObject.GetComponent<Inventory_Mono>();
            inventory.items = new Item_Mono[Inventory_Mono.inventoryNum];
            for (int i = 0; i < Inventory_Mono.inventoryNum; i++)
            {
                inventory.items[i] = Instantiate(itemPre, gameObject.transform);
            }
        }
    }
}
