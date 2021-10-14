using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static IdleLibrary.UsefulMethod;
using UniRx;

namespace IdleLibrary
{
    public class Main : MonoBehaviour
    {
        public double allTime { get => S.allTime; set => S.allTime = value; }
        public DateTime birthTime
        {
            get { return DateTime.FromBinary(Convert.ToInt64(S.birthDate)); }
            set { S.birthDate = value.ToBinary().ToString(); }
        }
        [NonSerialized]
        public DateTime ReleaseTime = DateTime.Parse("8/21/2019 7:00:00 AM");
        public DateTime lastTime//最後にプレイした時間。
        {
            get { return DateTime.FromBinary(Convert.ToInt64(S.lastTime)); }
            set { S.lastTime = value.ToBinary().ToString(); }
        }
        [Range(0.05f, 20.0f)]
        public float tick = 1.0f;
        //
        [SerializeField]
        public SaveR SR;
        [SerializeField] public Save S;
        public SaveDeclare SD;
        public saveCtrl saveCtrl;

        public AudioSource SoundEffectSource;
        //public Sound sound;
        public GameObject plainPopText;


        public static Main main;
        private void Awake()
        {
            main = this;
            SoundEffectSource = gameObject.GetOrAddComponent<AudioSource>();
        }

        // Start is called before the first frame update
        void Start()
        {
            //初めてのプレイだったら現在の値を代入
            if (!S.isContinuePlay)
            {
                birthTime = DateTime.Now;
                lastTime = DateTime.Now;
                S.isContinuePlay = true;
            }
            //不正な時間が入っていたら現在の値を代入
            if (lastTime < ReleaseTime || lastTime > DateTime.Now)
            {
                lastTime = DateTime.Now;
            }
            StartCoroutine(plusTime());
            //this.ObserveEveryValueChanged(_ => tick).Subscribe(_ => Time.fixedDeltaTime = 1f /tick / 10);
        }


        IEnumerator plusTime()
        {
            while (true)
            {
                allTime++;
                yield return new WaitForSeconds(1.0f);
            }
        }

        [ContextMenu("Validate")]
        private void Validate()
        {
            S.tempTornadoPoint = 0;
            S.tempPrestigePoint = 0;
        }
    }
}
