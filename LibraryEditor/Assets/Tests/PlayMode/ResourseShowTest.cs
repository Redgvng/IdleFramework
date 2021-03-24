using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
using UniRx.Triggers;

namespace Tests
{
    public class ResourseShowTest : MonoBehaviour
    {
        public TextMeshProUGUI gold, stone, exp;
        // Start is called before the first frame update
        void Start()
        {
            this.ObserveEveryValueChanged(_ => DataContainer<NUMBER>.GetInstance().GetDataByName(NumbersName.gold).Number)
                .Subscribe(_ => gold.text = "Gold : " + DataContainer<NUMBER>.GetInstance().GetDataByName(NumbersName.gold).Number.ToString("F0")) ;
            this.ObserveEveryValueChanged(_ => DataContainer<NUMBER>.GetInstance().GetDataByName(NumbersName.stone).Number)
                .Subscribe(_ => stone.text = "Stone : " + DataContainer<NUMBER>.GetInstance().GetDataByName(NumbersName.stone).Number.ToString("F0"));
            this.ObserveEveryValueChanged(_ => DataContainer<NUMBER>.GetInstance().GetDataByName(NumbersName.exp).Number)
                .Subscribe(_ => exp.text = "Exp : " + DataContainer<NUMBER>.GetInstance().GetDataByName(NumbersName.exp).Number.ToString("F0"));
        }
    }
}
