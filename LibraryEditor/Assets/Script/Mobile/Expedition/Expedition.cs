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
        public NUMBER number;
        public ICost cost;
        private float requiredHour;
        private bool isStarted;
        private float currentTime;

        public Expedition(NUMBER number, ICost cost = null, float initHour = 1.0f)
        {
            this.number = number;
            if (cost == null)
                this.cost = new NullCost();
            else
                this.cost = cost;
            SelectTime(initHour);
            Progress();
        }
        public bool CanClaim()
        {
            return isStarted && currentTime >= RequiredTimesec();
        }
        public bool CanStart()
        {
            return !isStarted && number.Number >= cost.Cost;
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
            number.DecrementNumber(cost.Cost);
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
