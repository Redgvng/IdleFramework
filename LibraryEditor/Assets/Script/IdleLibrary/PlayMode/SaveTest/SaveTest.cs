using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx.Triggers;
using UniRx;
using TMPro;
using Cysharp.Threading.Tasks;

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
    }
    public class SaveTest : MonoBehaviour
    {
        public Button Cookie;
        public TextMeshProUGUI cookieText;
        Dictionary<Resource, int> dic { get => Main.main.S.dic; set => Main.main.S.dic = value; }
        // Start is called before the first frame update
        void Start()
        {
            if (dic == null) dic = new Dictionary<Resource, int>();
            Cookie.onClick.AddListener(() =>
            {
                IdleLibrary.Main.main.S.cookie++;
            });
            this.ObserveEveryValueChanged(_ => Main.main.S.cookie).Subscribe(_ => cookieText.text = UsefulMethod.tDigit(Main.main.S.cookie));
            //AddItemToDictionary();
        }

        async void AddItemToDictionary()
        {
            await UniTask.Delay(1000);
            dic[Resource.cookie] = 3;
            await UniTask.Delay(1000);
            dic[Resource.gold] = 10;
            await UniTask.Delay(1000);
            dic[Resource.stone] = 100;
        }
    }
}

