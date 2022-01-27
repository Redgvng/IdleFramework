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
        [Inject] IRegisterDailyAction registerDailyAction;
        private void Awake()
        {
            getAchievement.achievements.Where(_ => _.unlockCondition is DAILY_ACHIEVEMENT).ToList().ForEach(_ =>
            {
                var condition = _.unlockCondition as DAILY_ACHIEVEMENT;
                registerDailyAction.AddDailyAction(() => condition.OnDayPassed());
                registerDailyAction.AddDailyAction(() => condition.CreateNewAchievement());
            });
        }
    }
}
