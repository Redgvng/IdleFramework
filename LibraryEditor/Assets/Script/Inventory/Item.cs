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
    public class Item : IItem
    {
        public int id { get; set; }
        public int quality;
    }

    public class NullItem : IItem
    {
        public int id { get => -1; set { } }
    }
}
