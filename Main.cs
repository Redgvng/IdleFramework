using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static IdleLibrary.UsefulMethod;
using UniRx;
using Cysharp.Threading.Tasks;

namespace IdleLibrary
{
    public class Main : MonoBehaviour
    {
        //[Inject] ITime _currentTime;
        public DateTime currentTime => DateTime.Now;
        public double allTime { get => S.allTime; set => S.allTime = value; }
        public DateTime birthTime
        {
            get { return DateTime.FromBinary(Convert.ToInt64(S.birthDate)); }
            set { S.birthDate = value.ToBinary().ToString(); }
        }
        [NonSerialized]
        public DateTime ReleaseTime = DateTime.Parse("04/29/2022 7:00:00 AM");
        public DateTime lowerTime = DateTime.Parse("01/01/2020 7:00:00 AM");
        public DateTime lastTime//最後にプレイした時間。
        {
            get 
            {
                if (S.lastTime == "0" || S.lastTime == "") return DateTime.Now;
                var result = DateTime.FromBinary(Convert.ToInt64(S.lastTime));
                if (result < ReleaseTime) return DateTime.Now;
                return DateTime.FromBinary(Convert.ToInt64(S.lastTime));
            }
            set { S.lastTime = value.ToBinary().ToString(); }
        }
        [Range(0.05f, 20.0f)]
        public float tick = 1.0f;
        //
        [SerializeField]
        public SaveR SR;
        [SerializeField] public Save S;
        //[SerializeField] public DTO dto;
        public SaveDeclare SD;
        public saveCtrl saveCtrl;

        public AudioSource SoundEffectSource;
        //public Sound sound;
        public GameObject plainPopText;


        public static Main main;
        private void Awake()
        {
            System.Globalization.CultureInfo.CurrentCulture = new System.Globalization.CultureInfo("en-us");
            main = this;
            SoundEffectSource = gameObject.GetOrAddComponent<AudioSource>();
            Initialize();
        }

        private void Initialize()
        {
            if (!S.isContinuePlay)
            {
                birthTime = currentTime;
                lastTime = currentTime;
                S.isContinuePlay = true;
            }
            if (lastTime < ReleaseTime || lastTime > currentTime)
            {
                lastTime = currentTime;
            }
            plusTime();
            //this.ObserveEveryValueChanged(_ => tick).Subscribe(_ => Time.fixedDeltaTime = 1f /tick / 10);
        }

        private async void  plusTime()
        {
            while (true)
            {
                allTime++;
                await UniTask.Delay(1000);
            }
        }

    }
}
