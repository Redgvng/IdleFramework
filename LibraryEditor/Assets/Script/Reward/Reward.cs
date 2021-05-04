using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static UsefulMethod;

namespace IdleLibrary
{
    public interface IReward
    {
        void Reward();
        string Text();
    }
    //サンプルクラスとして、Numberを増やすリワードを作成する。
    public class NumberReward : IReward
    {
        IEnumerable<(NUMBER number, double increment)> info;
        public NumberReward(params (NUMBER number, double increment)[] info)
        {
            this.info = info;
        }
        public void Reward()
        {
            info.ToList().ForEach((x) => x.number.IncrementNumber(x.increment, true));
        }
        public string Text()
        {
            string str = "";
            info.ToList().ForEach((x) => str += "NUMBERを + " + tDigit(x.increment) + "します\n");
            return str;
        }
    }

    public class NullReward : IReward
    {
        public void Reward() { }
        public string Text() { return "No Reward"; }
    }
}
