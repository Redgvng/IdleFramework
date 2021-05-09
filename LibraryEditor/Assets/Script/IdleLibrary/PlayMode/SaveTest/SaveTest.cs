using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx.Triggers;
using UniRx;
using TMPro;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

namespace IdleLibrary
{
    public enum Resource
    {
        cookie,
        gold,
        stone
    }
    public partial class Save
    {
        public double cookie;
        public Dictionary<Resource, int> dic;
        public GameObject ball;
    }
    public class SaveTest : SerializedMonoBehaviour
    {
        public GameObject sampleObject;
        public Button Cookie;
        public TextMeshProUGUI cookieText;
        Dictionary<Resource, int> dic { get => Main.main.S.dic; set => Main.main.S.dic = value; }
        GameObject ball { get => Main.main.S.ball; set => Main.main.S.ball = value; }
        public GameObject ballPre;
        public Transform window;
        // Start is called before the first frame update
        void Start()
        {
            if (dic == null) dic = new Dictionary<Resource, int>();
            if (ball == null) ball = Instantiate(ballPre, window);
            Cookie.onClick.AddListener(() =>
            {
                IdleLibrary.Main.main.S.cookie++;
            });
            this.ObserveEveryValueChanged(_ => Main.main.S.cookie).Subscribe(_ => cookieText.text = UsefulMethod.tDigit(Main.main.S.cookie));
            Move();
        }

        async void Move()
        {
            while (true)
            {
                ball.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, 1);
                await UniTask.Delay(20);
            }
        }
    }
}

