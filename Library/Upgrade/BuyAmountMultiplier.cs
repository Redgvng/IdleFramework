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
        //�Q�[���J�n���ɁA�����̎q�v�f�ɂ���IObserver��S�Ď擾���܂��B
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

        //Max�̏ꍇ��-1�����܂��B
        public int multiplierNum = 1;

        public Button buy1, buy10, buy25, buyMax;
    }
}
