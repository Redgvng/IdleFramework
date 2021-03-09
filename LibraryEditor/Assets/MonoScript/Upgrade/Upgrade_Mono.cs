using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectKind
{
    Number,
    Cal,
    Click,
    Produce
}
public enum CalculateWay
{
    additive,
    multiplicative
}
public class Upgrade_Mono : MonoBehaviour, ILevel
{
    public long level { get; set; }
    [SerializeField]
    CostKind costkind;
    [SerializeField]
    NumbersName[] resourseNames;
    [SerializeField]
    double linear_initialValue, linear_steep;
    [SerializeField]
    CalculateWay calway;
    [SerializeField]
    NumbersName targetNumber;
    [SerializeField]
    CalsName targetCal;

    [SerializeField]
    EffectKind effectKind;
    // アップグレードを作成します。
    void Awake()
    {

    }

}
