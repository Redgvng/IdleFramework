using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

namespace IdleLibrary
{
    public class TestTimer : MonoBehaviour, ITime
    {
        [SerializeField] DateTime timer;
        [SerializeField] int second;
        public DateTime currentTime => timer;
        // Start is called before the first frame update
        void Start()
        {
            timer = DateTime.Now + new TimeSpan(4,0,0,0);
            Observable.Interval(System.TimeSpan.FromSeconds(1)).Subscribe(_ =>
            {
                timer += new TimeSpan(0, 0, 0, second);
            });
        }
    }
}
