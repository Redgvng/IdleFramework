using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace IdleLibrary {

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

    public class Expedition : IExpedition
    {
        private readonly ITransaction transaction;
        private readonly IReward reward;
        private float requiredHour;
        private float currentTimesec;
        private bool isStarted;

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
