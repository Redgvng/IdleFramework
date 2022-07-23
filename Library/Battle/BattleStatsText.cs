using static IdleLibrary.UsefulMethod;
using UnityEngine;
using System;
using System.Linq;

public interface IBasicStatsText
{
    string HPText { get; }
    string ATKText { get; }
    string DEFText { get; }
}

public class BasicStatsText : IBasicStatsText
{
    private readonly IBasicStats stats;
    public BasicStatsText(IBasicStats stats)
    {
        this.stats = stats;
    }
    public string HPText => $"HP" + tDigit(stats.HP.MaxHP);
    public string ATKText => $"ATK {tDigit(stats.ATK)}";
    public string DEFText => $"DEF {tDigit(stats.DEF)}";
}


public class ColoredText : IBasicStatsText
{
    public enum TextKind
    {
        HPText, ATKText, DEFText
    }

    public struct RGB
    {
        public float r, g, b;
        public RGB(float r, float g, float b) { this.r = r; this.g = g; this.b = b; }
    }

    private readonly IBasicStatsText stats;
    private readonly (TextKind kind, RGB rgb)[] info;
    public ColoredText(IBasicStatsText stats, params (TextKind kind, RGB rgb)[] info)
    {
        this.stats = stats;
        this.info = info;
    }
    public string HPText => ColorText(TextKind.HPText) + stats.HPText + Reset;
    public string ATKText => ColorText(TextKind.ATKText) + stats.ATKText + Reset;
    public string DEFText => ColorText(TextKind.DEFText) + stats.DEFText + Reset;

    private string ColorText(TextKind kind)
    {
        if (info.Select(_ => _.kind).All(k => k != kind))
        {
            return "";
        }

        var rgb = info.FirstOrDefault(_ => _.kind == kind).rgb;
        var left = "{"; var right = "}";
        var result = $"\\color[rgb]{left}{rgb.r}, {rgb.g}, {rgb.b}{right}";

        return result;
    }

    private string Reset => "\\color[rgb]{1,1,1}";
}