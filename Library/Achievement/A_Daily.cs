using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Linq;
using UniRx;

namespace IdleLibrary
{
    //日付を跨いだらリセットする。
    public class A_Daily : MonoBehaviour
    {
        [Inject] IGetAchievement getAchievement;
        private double timeLapse;
        // Start is called before the first frame update
        void Start()
        {
            this.ObserveEveryValueChanged(_ => _.timeLapse).Subscribe(_ =>
            {
                getAchievement.achievements.Where(_ => _.unlockCondition is NoClickAchievement).ToList().ForEach(_ =>
                {
                    var condition = _.unlockCondition as NoClickAchievement;
                    condition.Notify(timeLapse);
                });
            });
        }1
    }
}
