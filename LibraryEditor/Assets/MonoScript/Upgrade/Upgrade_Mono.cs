using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade_Mono : MonoBehaviour, ILevel
{
    public long level { get; set; }
    [SerializeField]
    CostKind costkind;
    [SerializeField]
    double linear_initialValue, linear_steep;
    // アップグレードを作成します。
    void Awake()
    {

    }

}
