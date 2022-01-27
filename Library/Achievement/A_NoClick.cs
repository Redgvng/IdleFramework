using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Linq;
using UniRx;

namespace IdleLibrary {
    //やりたいことは、とにかく条件を達成したら通知をすることだけ。
    public class A_NoClick : MonoBehaviour
    {
        [Inject] IGetAchievement getAchievement;
        private double timeLapse;
        // Start is called before the first frame update
        void Start()
        {
            Observable.EveryFixedUpdate().Subscribe(_ => timeLapse += Time.fixedDeltaTime);
            var stream = Observable.EveryUpdate().Where(_ => Input.GetMouseButton(0) || Input.GetMouseButton(1));
            stream.Subscribe(_ => timeLapse = 0).AddTo(gameObject);
            this.ObserveEveryValueChanged(_ => _.timeLapse).Subscribe(_ =>
            {
                getAchievement.achievements.Where(_ => _.unlockCondition is NoClickAchievement).ToList().ForEach(_ =>
                {
                    var condition = _.unlockCondition as NoClickAchievement;
                    condition.Notify(timeLapse);
                });
            });
        }
    }
}
