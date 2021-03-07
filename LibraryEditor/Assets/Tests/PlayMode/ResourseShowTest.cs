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
            this.ObserveEveryValueChanged(_ => DataContainer<NUMBER>.GetInstance().GetDataByName("Gold").Number)
                .Subscribe(_ => gold.text = DataContainer<NUMBER>.GetInstance().GetDataByName("Gold").Number.ToString("F0"));
            this.ObserveEveryValueChanged(_ => DataContainer<NUMBER>.GetInstance().GetDataByName("Stone").Number)
                .Subscribe(_ => stone.text = DataContainer<NUMBER>.GetInstance().GetDataByName("Stone").Number.ToString("F0"));
            this.ObserveEveryValueChanged(_ => DataContainer<NUMBER>.GetInstance().GetDataByName("Exp").Number)
                .Subscribe(_ => exp.text = DataContainer<NUMBER>.GetInstance().GetDataByName("Exp").Number.ToString("F0"));
        }
    }
}
