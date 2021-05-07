using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx.Triggers;
using UniRx;
using TMPro;
using System;
using Sirenix.OdinInspector;

namespace IdleLibrary
{
    public partial class Save
    {
        public double cookie;
        public Action action = () => { };
        public Dictionary<double, int> dic;
    }
    public class SaveTest : SerializedMonoBehaviour
    {
        public Button Cookie, EffectButton;
        public TextMeshProUGUI cookieText;
        public Dictionary<double, int> dic { get => Main.main.S.dic; set => Main.main.S.dic = value; }
        // Start is called before the first frame update
        void Start()
        {
            //dic = new Dictionary<double, int>();
            //dic.Add(2d, 0);
            //dic.Add(3d, 1);
            Cookie.onClick.AddListener(() =>
            {
                IdleLibrary.Main.main.S.cookie++;
            });
            this.ObserveEveryValueChanged(_ => Main.main.S.cookie).Subscribe(_ => cookieText.text = UsefulMethod.tDigit(Main.main.S.cookie));
            Main.main.S.action();
            EffectButton.onClick.AddListener(() => { Main.main.S.action = () => { Debug.Log("saved"); Cookie.onClick.Invoke(); Cookie.onClick.Invoke(); Cookie.onClick.Invoke(); }; }) ;
        }
    }
}

