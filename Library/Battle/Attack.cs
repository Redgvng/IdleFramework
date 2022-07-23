using System;
using UnityEngine;

public interface IOnAttack
{
    double damage(double gotDamage);
    void OnAttack(double dmg, IBasicStats stats);
}

public class NormalOnAttack : IOnAttack
{
    private readonly IBasicStats stats;
    public double damage(double gotDamage) => gotDamage - stats.DEF;
    public NormalOnAttack(IBasicStats stats)
    {
        this.stats = stats;    }
    public void OnAttack(double dmg, IBasicStats stats)
    {
        stats.HP.currentHp = Math.Max(0, stats.HP.currentHp - damage(dmg));
    }
}

//Sample
public class DamageCutOnAttack : IOnAttack
{
    private readonly IBasicStats stats;
    private readonly IOnAttack onAttack;
    public double damage(double gotDamage) => Math.Min(1, stats.DEF) * onAttack.damage(gotDamage);
    public DamageCutOnAttack(IOnAttack onAttack, IBasicStats stats)
    {
        this.stats = stats;
        this.onAttack = onAttack;
    }
    public void OnAttack(double dmg, IBasicStats stats)
    {
        onAttack.OnAttack(damage(dmg), stats);
    }
}

public class OnAttackEvent : IOnAttack
{
    private readonly Action<double> action;
    private readonly IOnAttack iOnAttack;
    public OnAttackEvent(IOnAttack iOnAttack, Action<double> action)
    {
        this.action = action;
        this.iOnAttack = iOnAttack;
    }
    public double damage(double gotDamage) => iOnAttack.damage(gotDamage);

    public void OnAttack(double dmg, IBasicStats stats)
    {
        iOnAttack.OnAttack(damage(dmg), stats);
        action(damage(dmg));
    }
}