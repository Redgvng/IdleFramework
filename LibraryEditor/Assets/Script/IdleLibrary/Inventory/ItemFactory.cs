using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace IdleLibrary.Inventory
{
    public class ItemFactory
    {
        public ITEM CreateSomeItem()
        {
            var item = new Item(-1);
            //Id‚ğŒˆ‚ß‚Ü‚·B
            var id = UnityEngine.Random.Range(0,5);
            item.id = id;

            return item;
        }
    }
}
