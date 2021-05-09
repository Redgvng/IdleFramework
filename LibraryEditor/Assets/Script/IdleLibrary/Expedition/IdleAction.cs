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
         [OdinSerialize]  private NUMBER currentTime { get; set; }
         [SerializeField] private bool isStarted;
         [OdinSerialize]  public float initHour { get; private set; }

         public float CurrentTime => (float)currentTime.Number;
         public float RequiredTime => initHour;

         bool isStartedFirst;

         public IdleAction(float initHour)
         {
             this.initHour = initHour;
             currentTime = new NUMBER();
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
         public async void Initialize()
         {
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
             currentTime.IncrementNumber(timesec);
         }
         public float ProgressPercent()
         {
             return (float)(currentTime.Number / RequiredTime);
         }
     }

}
