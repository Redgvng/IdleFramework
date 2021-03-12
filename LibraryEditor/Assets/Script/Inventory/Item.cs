using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace InventoryLibrary
{
    public interface IItem
    {
        int id { get; set; }
    }
    [System.Serializable]
    public struct Item : IItem
    {
        [SerializeField]
        int _id;
        public int id { get => _id; set => _id = value; }
        public bool isSet => id == 0;
        public Item(int id)
        {
            _id = id;
        }
    }
    public struct ItemTest : IItem
    {
        public int id { get; set; }
        public ItemTest(int id)
        {
            this.id = id;
        }
    }
}
