using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;

namespace IdleLibrary.Inventory {

    public class InventoryTest : MonoBehaviour
    {
        [SerializeField] Button cookieButton;
        [SerializeField] TextMeshProUGUI cookieText;
        public NUMBER cookie;
        public NUMBER cookieAutoProduce;
        private void Awake()
        {
            cookie = new NUMBER();
            cookieAutoProduce = new NUMBER();
        }
        // Start is called before the first frame update
        void Start()
        {
            cookieButton.onClick.AddListener(() => cookie.IncrementNumber(1));
            CookieProduce();
        }

        async void CookieProduce()
        {
            while (true)
            {
                cookie.IncrementNumber(cookieAutoProduce.Number);
                await UniTask.Delay(1000);
            }
        }

        // Update is called once per frame
        void Update()
        {
            cookieText.text = UsefulMethod.tDigit(cookie.Number);
        }
    }
}
