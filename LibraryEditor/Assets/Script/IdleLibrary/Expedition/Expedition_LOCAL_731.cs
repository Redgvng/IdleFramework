using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Cysharp.Threading.Tasks;

namespace IdleLibrary {

    public interface IExpedition
    {
        bool IsStarted();
        float CurrentTimesec();
        float RequiredTime(bool isSec);
        float RealRequiredTimesec();
        void SelectTime(float hour);
        void StartOrClaim();
        void StartExpedition();
        void Claim();
    }
    public interface IExpeditionAction
    {
        void OnStart(ILevel level, int hourId);
        void OnClaim();
    }
    public class NullExpeditionAction : IExpeditionAction
    {
        public void OnStart(ILevel level, int hourId) { }
        public void OnClaim() { }
    }
    [System.Serializable]
    public class ExpeditionForSave
    {
        public long completedNum;
        public float currentTimeSec;
        public bool isStarted;
        public int hourId;
        public IExpeditionAction action;
    }
    public class Expedition : IExpedition, ILevel
    {
        private ITransaction transaction;
        public IExpeditionAction action { get => saveData[id].action; set => saveData[id].action = value; }
        private float requiredHour;
        private float[] requiredHours;
        private readonly int id;
        private ILevel ilevel;

        //Save
        [SerializeField] private long completedNum { get => saveData[id].completedNum; set => saveData[id].completedNum = value; }
        [SerializeField] private float currentTimesec { get => saveData[id].currentTimeSec; set => saveData[id].currentTimeSec = value; }
        [SerializeField] private bool isStarted { get => saveData[id].isStarted; set => saveData[id].isStarted = value; }
        [SerializeField] public int hourId { get => saveData[id].hourId; set => saveData[id].hourId = value; }
        public long level { get => completedNum; set => completedNum = value; }
        private Func<float> timeSpeedFactor = () => 1;
        [SerializeField] private ExpeditionForSave[] saveData;

        public Expedition(int id, ExpeditionForSave[] saveData, ITransaction transaction = null, IExpeditionAction action = null, params float[] requiredHoursArray)
        {
            this.id = id;
            this.saveData = saveData;
            //初期化が必要
            for (int i = 0; i < saveData.Length; i++)
            {
                saveData[i] = saveData[i] ?? new ExpeditionForSave();
            }
            this.transaction = transaction == null ? new NullTransaction() : transaction;
            this.requiredHours = requiredHoursArray;
            requiredHour = requiredHours[hourId];
            if(this.action == null) { this.action = action == null ? new NullExpeditionAction() : action; }
            Progress();
        }
        public void SetTransaction(ITransaction transaction)
        {
            this.transaction = transaction;
        }
        public void SetTimeSpeedFactor(Func<float> timeSpeedFactor)
        {
            this.timeSpeedFactor = timeSpeedFactor;
        }
        public void SetILevel(ILevel ilevel)
        {
            this.ilevel = ilevel;
        }
        //Test用
        public Expedition(int id, ITransaction transaction = null, IExpeditionAction action = null, params float[] requiredHoursArray)
        {
            saveData = new ExpeditionForSave[1]
            {
                new ExpeditionForSave()
            };
            this.id = id;
            this.transaction = transaction == null ? new NullTransaction() : transaction;
            this.requiredHours = requiredHoursArray;
            if (requiredHours.Length != 0) requiredHour = requiredHours[hourId];
            this.action = action == null ? new NullExpeditionAction() : action;
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
            return currentTimesec >= RealRequiredTimesec();
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
        public float RealRequiredTimesec()
        {
            return RequiredTime(true) / Math.Max(1, timeSpeedFactor());
        }
        public void StartExpedition()
        {
            if (!CanStart())
                return;
            transaction.Pay();
            isStarted = true;
            action.OnStart(ilevel, hourId);
        }
        public void Claim()
        {
            if(!CanClaim())
                return;
            isStarted = false;
            currentTimesec = 0;
            completedNum++;
            action.OnClaim();
        }
        async void Progress()
        {
            while (true)
            {
                if (isStarted && currentTimesec < RequiredTime(true))
                    IncreaseCurrentTime(1);
                await UniTask.Delay(1000, delayType: DelayType.Realtime);
            }
        }
        public void SwitchRequiredHour(bool isRight)
        {
            if (IsStarted())
                return;
            if (isRight)
                hourId = hourId < requiredHours.Length - 1 && requiredHours[hourId + 1] != 0 ? hourId + 1 : 0;
            else
                hourId = hourId > 0 && requiredHours[hourId - 1] != 0 ? hourId - 1 : requiredHours.Length - 1;
            SelectTime(requiredHours[hourId]);
        }
        public void IncreaseCurrentTime(float timesec) 
        {
            currentTimesec += timesec;
            if (currentTimesec > RealRequiredTimesec())
                currentTimesec = RealRequiredTimesec();
        }
        public float ProgressPercent()
        {
            return currentTimesec / RealRequiredTimesec();
        }
        public string LeftTimesecString()
        {
            return UsefulMethod.DoubleTimeToDate(RealRequiredTimesec() - CurrentTimesec());
        }
    }
}
