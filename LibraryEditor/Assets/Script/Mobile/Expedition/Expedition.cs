using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;
//UI
using UnityEngine.UI;
using TMPro;
using static UsefulMethod;
using static UsefulStatic;

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
        private float currentTimesec;
        private bool isStarted;

        public Expedition(ITransaction transaction, float initHour, IReward reward = null)
        {
            this.transaction = transaction;
            this.requiredHour = initHour;
            this.reward = reward == null ? new NullReward() : reward;
            Progress();
        }
        public bool CanClaim()
        {
            return currentTimesec >= RequiredTimesec();
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
            currentTimesec = 0;
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
                if (isStarted && currentTimesec < RequiredTimesec())
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
            return currentTimesec / RequiredTimesec();
        }

        //以下UI
        public Button startClaimButton;
        public TextMeshProUGUI startClaimText, requiredHourText, progressPercentText, rewardText;
        public Slider progressBar;

        public void UpdateStartClaimButton()
        {
            if (isStarted)
            {
                startClaimText.text = "Claim";
                startClaimButton.interactable = CanClaim();
            }
            else
            {
                startClaimText.text = "Start";
                startClaimButton.interactable = CanStart();
            }
        }
        public void UpdateProgress()
        {
            progressPercentText.text = percent(ProgressPercent());
            progressBar.value = ProgressPercent();
        }
        public void UpdateRequiredHour()
        {
            requiredHourText.text = requiredHour.ToString("F1") + " h";
        }

        //使用例
        public Button rightButton, leftButton;
        float[] requiredHours = new float[] { 0.5f, 1.0f, 2.0f, 4.0f, 8.0f, 24.0f };
        int hourId;
        public void SwitchRequiredHour(bool isRight)
        {
            if (isStarted)
                return;
            if (isRight)
                hourId = hourId < requiredHours.Length - 1 ? hourId + 1 : 0;
            else
                hourId = hourId > 0 ? hourId - 1 : requiredHours.Length - 1;
            SelectTime(requiredHours[hourId]);
            UpdateRequiredHour();
        }
        
    }
}
