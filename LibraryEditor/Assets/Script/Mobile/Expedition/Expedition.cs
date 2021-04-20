using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace IdleLibrary.Upgrade {

    public interface IExpedition
    {
        bool CanStart();
        bool CanClaim();
        void SelectTime(float hour);
        void StartExplore();
        void Claim();
        void Reward();
    }
    public class Expedition : IExpedition
    {
        public NUMBER number;
        public ICost cost;
        private float requiredHour;
        //private float[] hour = new float[] { 0.5f, 1.0f, 2.0f, 4.0f, 8.0f, 24.0f};
        private bool isExpedition;
        private float currentTime;

        public Expedition(NUMBER number, ICost cost, float initHour)
        {
            this.number = number;
            this.cost = cost;
            this.requiredHour = initHour;
        }
        public bool CanClaim()
        {
            return isExpedition && currentTime >= requiredHour;
        }
        public bool CanStart()
        {
            return !isExpedition && number.Number >= cost.Cost;
        }
        public void SelectTime(float hour)
        {
            this.requiredHour = hour;
        }
        public void StartExplore()
        {
            if (!CanStart())
                return;
            number.DecrementNumber(cost.Cost);
            isExpedition = true;
        }
        public void Claim()
        {
            if(!CanClaim())
                return;
            isExpedition = false;
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
                if(isExpedition && currentTime < requiredHour)
                    currentTime++;
                await UniTask.Delay(1000);
            }
        }
    }
}
