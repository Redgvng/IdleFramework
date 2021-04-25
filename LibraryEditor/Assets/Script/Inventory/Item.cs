using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdleLibrary.Inventory
{
    [System.Serializable]
    public class Item
    {
        public int id;
        public bool isLocked;
        public bool isSet => id >= 0;
        public Item(int id)
        {
            this.id = id;
            this.isLocked = false;
        }
        public virtual string Text()
        {
            return $"----ITEM----\n\n- ID : {id}";
        }
    }

    //Item‚ğŒp³‚µ‚Ä©ì‚ÌƒAƒCƒeƒ€‚ğì‚è‚Ü‚·
    [System.Serializable]
    public class Artifact : Item
    {
        public Artifact(int id) : base(id)
        {

        }

        public override string Text()
        {
            return $"----ITEM----\n\n- ID : {id}\n\n - Level : {level} \n- Quality : {quality} \n\n\n[Effects in Hidden Challenge]\n- Anti-Magid Power : {antimagicPower}";
        }

        public int level;
        public int quality;
        public double antimagicPower;
    }
}
