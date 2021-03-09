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
        [SerializeField]
        int _id;
        public int id { get => _id; set => _id = value; }
        public bool isSet => id >= 0;
        public Item(int id)
        {
            this.id = id;
        }
    }

}
