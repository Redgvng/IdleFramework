using UnityEngine;
using System;
public interface IBasicStats
{
    IHP HP { get; }
    double ATK { get; }
    double DEF { get; }
}

public interface IHP
{
    double MaxHP { get; }
    double currentHp { get; set; }
}

//Sample
public class InverseStats : IBasicStats
{
    private readonly IBasicStats stats;
    public InverseStats(IBasicStats stats) { this.stats = stats; }

    public IHP HP => stats.HP;

    public double ATK => stats.DEF;

    public double DEF => stats.ATK;
}
