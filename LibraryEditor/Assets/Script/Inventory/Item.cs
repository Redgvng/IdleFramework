using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace InventoryLibrary
{
    public interface IItem
    {

    }
    [System.Serializable]
    public class Item : IItem
    {
        public int id;
        public int quality;
    }

    public class NullItem : IItem
    {

    }
}
