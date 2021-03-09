using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace InventoryLibrary
{
    public interface IItem
    {
        int id { get; set; }
        bool isSet { get; }
    }
    [System.Serializable]
    public class Item : IItem
    {
        public int id { get; set; }
        public bool isSet => id >= 0;
        public int quality;
        public Item(int id)
        {
            this.id = id;
        }
    }

}
