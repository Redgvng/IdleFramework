using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using System;
using static IdleLibrary.UsefulMethod;

namespace IdleLibrary.Inventory
{
    public enum Calway
    {
        add,
        mul
    }
    //����statsbreakdown�Ŏg���\���̂���C���^�[�t�F�[�X
    //�e�L�X�g�̂݃f�R���[�g�\�ɂ��Ă����K�v���邩�H
    public interface IStatsBreakdown : IStatsBreakdownText
    {
        //�e�X�̒l
        double Value();
        //���������Ƃ�bool (�N���X�������Ƃ��Aid�������Ƃ�...)
        bool IsEqual(object obj);
    }
    //�f�R���[�g�p�C���^�[�t�F�[�X
    public interface IStatsBreakdownText
    {
        //���͂̃t�H�[�}�b�g(�l���Ȃ�����ň�����g���΂���)
        string StatsBreakdownText(double value);
    }
    public interface IEffect : IText
    {
        
    }
    public class BasicEffect : IEffect, IStatsBreakdown
    {
        [OdinSerialize] public readonly Func<double> value;
        public string effectText;
        public Calway calway;
        public BasicEffect(string effectText, Func<double> value, Calway calway)
        {
            this.value = value;
            this.effectText = effectText;
            this.calway = calway;
        }
        public string Text()
        {
            string text = calway switch
            {
                Calway.add => effectText + " + " + tDigit(Value()),
                Calway.mul => effectText + " + " + percent(Value()),
                _ => ""
            };
            return text;
        }
        public string StatsBreakdownText(double value)
        {
            string text = calway switch
            {
                Calway.add => effectText + " + " + tDigit(value),
                Calway.mul => effectText + " + " + percent(value),
                _ => ""
            };
            return text;
        }
        public double Value() => value();
        public bool IsEqual(object obj)
        {
            if (obj is BasicEffect)
            {
                var effect = obj as BasicEffect;
                if(effect.effectText == this.effectText)
                {
                    return true;
                }
            }
            return false;
        }
    }

    //stats breakdown��\�������鏈������낤�B
    public class StatsBreakdownMaker
    {
        private readonly IEnumerable<IStatsBreakdown> statsBreakdowns;
        public StatsBreakdownMaker(IEnumerable<IStatsBreakdown> statsBreakdowns)
        {
            this.statsBreakdowns = statsBreakdowns;
        }
    }
}
