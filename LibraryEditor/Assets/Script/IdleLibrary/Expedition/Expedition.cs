using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;
using System;
using Sirenix.Serialization;

namespace IdleLibrary {

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
    */

    /*
     * IdleActionの要件は何か？
     * - 開始という概念がある
     * - Calimという概念がある
     * - 開始するための条件がある
     * - Claimするための条件がある？
     * これくらい？これをinterfaceに反映
     */
    public interface IIdleAction
    {
        bool CanStart();
        bool CanClaim();
        void Start();
        void Claim();
        float currentTime { get; }
        float requiredTime { get; }
    }
    //Expeditionから、純粋にアイドル時間で何かをする処理を抜き出します。
    [Serializable]
    public class IdleAction : IIdleAction
    {
        [OdinSerialize] private NUMBER currentTime { get; set; }
        [SerializeField] private bool isStarted;
        [OdinSerialize] private readonly Action OnClaim;
        [OdinSerialize] private readonly Func<bool> canClaim;
        [OdinSerialize] public float initHour { get; private set; }

        float IIdleAction.currentTime => (float)currentTime.Number;
        public float requiredTime => initHour;

        public IdleAction(float initHour, Action OnClaim = null, Func<bool> canClaim = null)
        {
            this.initHour = initHour;
            currentTime = new NUMBER();
            this.OnClaim = OnClaim == null ? () => { } : OnClaim;
            this.canClaim = canClaim == null ? () => true : canClaim;
            Progress();
        }
        public bool CanClaim()
        {
            return currentTime.Number >= requiredTime;
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
            OnClaimCallback();
        }
        private void OnClaimCallback()
        {
            OnClaim();
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
            return (float)(currentTime.Number / requiredTime);
        }
    }

    //Levelを持つIdleActionです。
    [Serializable]
    public class IdleActionWithlevel : IIdleAction, ILevel
    {
        [OdinSerialize]
        private readonly IIdleAction idleAction;
        public long level { get; set; }

        public float currentTime => idleAction.currentTime;
        public float requiredTime => idleAction.requiredTime;
        public IdleActionWithlevel(IIdleAction idleAction)
        {
            if(this.idleAction == null) this.idleAction = idleAction;
        }
        public bool CanClaim() => idleAction.CanClaim();
        public bool CanStart() => idleAction.CanStart();
        public void Start() => idleAction.Start();
        public void Claim()
        {
            idleAction.Claim();
            level++;
        }

    }

    //IdleActionとの違いは？
    //- transactionを持つ
    //- rewardを持つ
    //- 時間が選べる
    //↑これらは別々のクラスとして実装すべき
    public class Expedition
    {
        private readonly ITransaction transaction;
        private readonly IReward reward;
        private float requiredHour;
        private float currentTimesec;
        private bool isStarted;
        float[] requiredHours = new float[] { 0.5f, 1.0f, 2.0f, 4.0f, 8.0f, 24.0f };
        int hourId;

        public Expedition(float initHour, ITransaction transaction = null, IReward reward = null)
        {
            this.transaction = transaction == null ? new NullTransaction() : transaction;
            this.requiredHour = initHour;
            this.reward = reward == null ? new NullReward() : reward;
            Progress();
        }
        public bool IsStarted()
        {
            return isStarted;
        }
        public void StartOrClaim()
        {
            if (IsStarted())
                Claim();
            else
                StartExpedition();
        }
        public bool CanClaim()
        {
            return currentTimesec >= RequiredTime(true);
        }
        public bool CanStart()
        {
            return !isStarted && transaction.CanBuy();
        }
        public void SelectTime(float hour)
        {
            requiredHour = hour;
        }
        public float CurrentTimesec()
        {
            return currentTimesec;
        }
        public float RequiredTime(bool isSec)
        {
            if (isSec)
                return requiredHour * 3600f;
            else
                return requiredHour;
        }
        public void StartExpedition()
        {
            if (!CanStart())
                return;
            transaction.Pay();
            isStarted = true;
        }
        public void Claim()
        {
            if(!CanClaim())
                return;
            isStarted = false;
            currentTimesec = 0;
            Reward();
        }
        private void Reward()
        {
            reward.Reward();
        }        
        async void Progress()
        {
            while (true)
            {
                if (isStarted && currentTimesec < RequiredTime(true))
                    IncreaseCurrentTime(1);
                await UniTask.Delay(1000);
            }
        }
        public void SwitchRequiredHour(bool isRight)
        {
            if (IsStarted())
                return;
            if (isRight)
                hourId = hourId < requiredHours.Length - 1 ? hourId + 1 : 0;
            else
                hourId = hourId > 0 ? hourId - 1 : requiredHours.Length - 1;
            SelectTime(requiredHours[hourId]);
        }
        public void IncreaseCurrentTime(float timesec) 
        {
            currentTimesec += timesec;
        }
        public float ProgressPercent()
        {
            return currentTimesec / RequiredTime(true);
        }

    }
}
