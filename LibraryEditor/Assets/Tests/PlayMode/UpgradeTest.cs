using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using UpgradeLibrary;
using TMPro;
using static UsefulMethod;

namespace Tests
{
    public class UpgradeTest : MonoBehaviour
    {
        public Button upgradeButton;
        public PopUp popUp;
        public Transform windowShowCanvas;
        public TextMeshProUGUI cookieText;

        void Start()
        {
            var pop = popUp.StartPopUp(upgradeButton.gameObject, windowShowCanvas.GetComponent<RectTransform>());
            //resourceを生成します。
            NUMBER cookie = new NUMBER(1000);
            NUMBER level = new NUMBER();
            Upgrade upgrade = new Upgrade(level, new Transaction(cookie,new LinearCost(1, 2, level)));
            upgradeButton.OnClickAsObservable().Subscribe(_ => upgrade.Pay());
            pop.UpdateAsObservable().Where(_ => pop.gameObject.activeSelf).Subscribe(_ => pop.text.text = "Upgrade Level " + level.Number) ;
            this.ObserveEveryValueChanged(_ => level.Number).Subscribe(_ => cookieText.text = tDigit(level.Number));
        }
    }
}
