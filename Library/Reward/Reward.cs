using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static IdleLibrary.UsefulMethod;

namespace IdleLibrary
{
    public interface IReward
    {
        void Reward();
        string Text();
    }
    //�T���v���N���X�Ƃ��āANumber�𑝂₷�����[�h���쐬����B
    public class NumberReward : IReward
    {
        IEnumerable<(NUMBER number, double increment)> info;
        public NumberReward(params (NUMBER number, double increment)[] info)
        {
            this.info = info;
        }
        public void Reward()
        {
            info.ToList().ForEach((x) => x.number.IncrementFixNumber(x.increment));
        }
        public string Text()
        {
            string str = "";
            info.ToList().ForEach((x) => str += "NUMBER�� + " + tDigit(x.increment) + "���܂�\n");
            return str;
        }
    }

    public class NullReward : IReward
    {
        public void Reward() { }
        public string Text() { return "No Reward"; }
    }
}
