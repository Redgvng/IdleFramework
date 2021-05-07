using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx.Triggers;
using UniRx;
using TMPro;

namespace IdleLibrary
{
    public partial class Save
    {
        public double cookie;
    }
    public class SaveTest : MonoBehaviour
    {
        public Button Cookie;
        public TextMeshProUGUI cookieText;
        // Start is called before the first frame update
        void Start()
        {
            Cookie.onClick.AddListener(() =>
            {
                IdleLibrary.Main.main.S.cookie++;
            });
            this.ObserveEveryValueChanged(_ => Main.main.S.cookie).Subscribe(_ => cookieText.text = UsefulMethod.tDigit(Main.main.S.cookie));
        }
    }
}

