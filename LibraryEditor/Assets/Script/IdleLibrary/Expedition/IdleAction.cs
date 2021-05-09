using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;
using System;
using Sirenix.Serialization;

namespace IdleLibrary
{
        /*
       public interface IExpedition
       {
           bool IsStarted();
           float CurrentTimesec();
           float RequiredTime(bool isSec);
           void SelectTime(float hour);
           void StartOrClaim();
           void StartExpedition();
           void Claim();
       }
       [System.Serializable]
       public class ExpeditionForSave
       {
           public long completedNum;
           public float currentTimeSec;
           public bool isStarted;
           public int hourId;
       }
       /*
        * IdleActionの要件は何か？
        * - 開始という概念がある
        * - Calimという概念がある
        * - 開始するための条件がある
        * - Claimするための条件がある？
        * これくらい？これをinterfaceに反映
        */
        public interface IIdleAction : ITimeInterval
        {
            void Start();
            bool CanStart();

            bool CanClaim();
            void Claim();
        }

        //↓これはいろんなところで使うはず
        public interface ITimeInterval
        {
            float CurrentTime { get; }
            float RequiredTime { get; }
        }
        //Expeditionから、純粋にアイドル時間で何かをする処理を抜き出します。
        [Serializable]
        public class IdleAction : IIdleAction
        {
            [OdinSerialize] private NUMBER currentTime { get; set; }
            [SerializeField] private bool isStarted;
            [OdinSerialize] public float initHour { get; private set; }

            public float CurrentTime => (float)currentTime.Number;
            public float RequiredTime => initHour;

            public IdleAction(float initHour)
            {
                this.initHour = initHour;
                currentTime = new NUMBER();
                Progress();
            }
            public bool CanClaim()
            {
                return currentTime.Number >= RequiredTime;
            }
            public bool CanStart()
            {
                return !isStarted;
            }
            public void Start()
            {
                if (!CanStart())
                    return;

                isStarted = true;
            }
            public void Claim()
            {
                if (!CanClaim())
                    return;
                isStarted = false;
                currentTime.Number = 0;
            }
            async void Progress()
            {
                while (true)
                {
                    if (isStarted && !CanClaim())
                        IncreaseCurrentTime(1);
                    await UniTask.Delay(1000);
                }
            }

            public void IncreaseCurrentTime(float timesec)
            {
                currentTime.IncrementNumber(timesec);
            }
            public float ProgressPercent()
            {
                return (float)(currentTime.Number / RequiredTime);
            }
        }

        //Levelを持つIdleActionです。
        /*
        [Serializable]
        public class IdleActionWithlevel : IIdleAction, ILevel
        {
            [OdinSerialize]
            private readonly IIdleAction idleAction;
            [OdinSerialize]
            public long level { get; set; }

            public float CurrentTime => idleAction.CurrentTime;
            public float RequiredTime => idleAction.RequiredTime;
            public bool CanClaim() => idleAction.CanClaim();
            public bool CanStart() => idleAction.CanStart();
            public void Start() => idleAction.Start();

            public IdleActionWithlevel(IIdleAction idleAction)
            {
                if(this.idleAction == null) this.idleAction = idleAction;
            }
            public void Claim()
            {
                idleAction.Claim();
                level++;
            }
        }
        */
}
