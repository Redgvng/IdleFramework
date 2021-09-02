using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;
using System;
using Sirenix.Serialization;

namespace IdleLibrary
{
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
        [OdinSerialize]  private NUMBER currentTime { 
            get
            {
                if (_currentTime == null) return new NUMBER();
                return _currentTime;
            } 
            set => _currentTime = value;
        }
        private NUMBER _currentTime;
        [SerializeField] private bool isStarted;
        [OdinSerialize]  public float initHour { get; private set; }

        public float CurrentTime => (float)currentTime.Number;
        public float RequiredTime => initHour;

        bool isStartedFirst;

        public IdleAction(float initHour)
        {
            this.initHour = initHour;
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
            currentTime.ResetNumberToZero();
        }
        public async void Initialize()
        {
            if (isStartedFirst) return;
            isStartedFirst = true; 
            while (true)
            {
                if (isStarted && !CanClaim())
                    IncreaseCurrentTime(1);
                await UniTask.Delay(1000);
            }
        }

        public void IncreaseCurrentTime(float timesec)
        {
            currentTime.Increment(timesec);
        }
        public float ProgressPercent()
        {
            return (float)(currentTime.Number / RequiredTime);
        }
     }

}
