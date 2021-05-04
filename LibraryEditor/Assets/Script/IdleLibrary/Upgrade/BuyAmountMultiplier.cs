using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Linq;

namespace IdleLibrary
{
    public class BuyAmountMultiplier : Subject
    {
        //ゲーム開始時に、自分の子要素にあるIObserverを全て取得します。
        private void Awake()
        {
            var obs = GetAllChildren.GetAllByObservers(this.gameObject);
            obs.ForEach((x) => Attach(x));

            Debug.Log(observers.Count);

            buy1.OnClickAsObservable().Subscribe(_ => { multiplierNum = 1; Notify(); });
            buy10.OnClickAsObservable().Subscribe(_ => { multiplierNum = 10; Notify(); });
            buy25.OnClickAsObservable().Subscribe(_ => { multiplierNum = 25; Notify(); });
            buyMax.OnClickAsObservable().Subscribe(_ => { multiplierNum = -1; Notify(); });
        }

        //Maxの場合は-1を入れます。
        public int multiplierNum = 1;

        public Button buy1, buy10, buy25, buyMax;
    }
}
