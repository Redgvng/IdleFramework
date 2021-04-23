using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace IdleLibrary {

    public interface IExpedition
    {
        bool CanStart();
        bool CanClaim();
        void SelectTime(float hour);
        void StartExpedition();
        void Claim();
        void Reward();
    }

    public class Expedition : IExpedition
    {
        private readonly ITransaction transaction;
        private readonly IReward reward;
        private float requiredHour;
        private bool isStarted;
        private float currentTime;

        public Expedition(ITransaction transaction, float initHour, IReward reward)
        {
            this.transaction = transaction;
            this.requiredHour = initHour;
            Progress();
        }
        public bool CanClaim()
        {
            return currentTime >= RequiredTimesec();
        }
        public bool CanStart()
        {
            return !isStarted && transaction.CanBuy();
        }
        public void SelectTime(float hour)
        {
            requiredHour = hour;
        }
        public float RequiredTimesec()
        {
            return requiredHour * 3600f;
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
            currentTime = 0;
            Reward();
        }
        public void Reward()
        {
            reward.Reward();
        }        
        async void Progress()
        {
            while (true)
            {
                if (isStarted && currentTime < RequiredTimesec())
                    IncreaseCurrentTime(1);
                await UniTask.Delay(1000);
            }
        }
        public void IncreaseCurrentTime(float timesec) 
        {
            currentTime += timesec;
        }
    }
}
