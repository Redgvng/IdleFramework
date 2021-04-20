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
        private float requiredHour;
        private bool isStarted;
        private float currentTime;

        public Expedition(ITransaction transaction, float initHour)
        {
            this.transaction = transaction;
            this.requiredHour = initHour;
        }
        public bool CanClaim()
        {
            return isStarted && currentTime >= RequiredTimesec();
        }
        public bool CanStart()
        {
            return !isExpedition && transaction.CanBuy();
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
            //Claimした時の報酬
        }        
        async void Progress()
        {
            while (true)
            {
                IncreaseCurrentTime(1);
                await UniTask.Delay(1000);
            }
        }
        public void IncreaseCurrentTime(float timesec) 
        {
            if (isStarted && currentTime < RequiredTimesec())
                currentTime += timesec;
        }
    }
}
