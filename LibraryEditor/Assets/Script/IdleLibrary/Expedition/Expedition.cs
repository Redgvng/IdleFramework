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
        private float[] requiredHours;

        private readonly int id;

        //Save
        [SerializeField] private long completedNum { get => saveData[id].completedNum; set => saveData[id].completedNum = value; }
        [SerializeField] private float currentTimesec { get => saveData[id].currentTimeSec; set => saveData[id].currentTimeSec = value; }
        [SerializeField] private bool isStarted { get => saveData[id].isStarted; set => saveData[id].isStarted = value; }
        [SerializeField] private int hourId { get => saveData[id].hourId; set => saveData[id].hourId = value; }
        [SerializeField] private ExpeditionForSave[] saveData;

        public Expedition(int id, ExpeditionForSave[] saveData, ITransaction transaction = null, IReward reward = null, params float[] requiredHoursArray)
        {
            this.id = id;
            this.saveData = saveData;
            this.transaction = transaction == null ? new NullTransaction() : transaction;
            this.requiredHours = requiredHoursArray;
            requiredHour = requiredHours[hourId];
            this.reward = reward == null ? new NullReward() : reward;
            Progress();
        }
        //Test用
        public Expedition(int id, ITransaction transaction = null, IReward reward = null, params float[] requiredHoursArray)
        {
            saveData = new ExpeditionForSave[1]
            {
                new ExpeditionForSave()
            };
            this.id = id;
            this.transaction = transaction == null ? new NullTransaction() : transaction;
            this.requiredHours = requiredHoursArray;
            if (requiredHours.Length != 0) requiredHour = requiredHours[hourId];
            this.reward = reward == null ? new NullReward() : reward;
            Progress();
        }

        public bool IsStarted()
        {
            return isStarted;
        }
        public long CompletedNum()
        {
            return completedNum;
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
            completedNum++;
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
            if (currentTimesec > RequiredTime(true))
                currentTimesec = RequiredTime(true);
        }
        public float ProgressPercent()
        {
            return currentTimesec / RequiredTime(true);
        }

    }
}
