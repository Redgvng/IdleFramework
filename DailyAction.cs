using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace IdleLibrary {

    public class DailyAction : MonoBehaviour, IRegisterDailyAction
    {
        [Inject] ITime currentTime;
        private List<Action> DailyActions = new List<Action>();
        private DateTime lastTime;

        //オフラインから復帰したときに、日を跨いでいたら行う
        void DoDailyAction() => DailyActions.ForEach(_ => _());
        void Start()
        {
            DateTime now = currentTime.currentTime;//端末の現在時刻の取得
            var todayDate = now.Year * 10000 + now.Month * 100 + now.Day;//日付を数値化　2020年9月1日だと20200901になる
            this.lastTime = Main.main.lastTime;
            var lastTime = Main.main.lastTime.Year * 10000 + Main.main.lastTime.Month * 100 + Main.main.lastTime.Day;

            if (lastTime < todayDate)//日付が進んでいる場合
            {
                DoDailyAction();
            }
        }

        //ゲーム中に日を跨いだ場合
        private async void JudgeDailyWhilePlaying()
        {
            while (true)
            {
                DateTime now = currentTime.currentTime;
                var todayDate = now.Year * 10000 + now.Month * 100 + now.Day;
                var lastTime = this.lastTime.Year * 10000 + this.lastTime.Month * 100 + this.lastTime.Day;
                if (lastTime < todayDate)
                {
                    this.lastTime = Main.main.lastTime;
                    DoDailyAction();
                }
                await UniTask.Delay(1000);
            }
        }
        public void AddDailyAction(Action action)
        {
            DailyActions.Add(action);
        }
    }
}
