using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;

namespace IdleLibrary.Inventory
{
    public interface IEffect
    {

    }

    public class BasicEffect
    {
        [OdinSerialize] private readonly Multiplier multiplier;
    }
}
