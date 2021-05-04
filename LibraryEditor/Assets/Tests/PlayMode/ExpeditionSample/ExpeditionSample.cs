using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using IdleLibrary;
using System.Linq;

//ここにUI関係の処理は一切書かない
//このスクリプトだけでExpeditionの処理は完結する
public class ExpeditionSample : MonoBehaviour
{
    [SerializeField]
    private Expedition_UI[] expeditions;

    // Start is called before the first frame update
    void Start()
    {
        //通貨
        var gold = new NUMBER(100);
        //Expeditionを作る
        var expedition1 = MakeSomeExpedition(gold, new FixedCost(1), 0.5f);
        var expedition2 = MakeSomeExpedition(gold, new FixedCost(1), 0.5f);
        var expedition3 = MakeSomeExpedition(gold, new FixedCost(1), 0.5f);
        var expedition4 = MakeSomeExpedition(gold, new FixedCost(1), 0.5f);

        //UIと紐づける(Instantiateするやり方に変えたほうがいいかもしれない)
        expeditions[0].LinkExpedition(expedition1);
        expeditions[1].LinkExpedition(expedition2);
        expeditions[2].LinkExpedition(expedition3);
        expeditions[3].LinkExpedition(expedition4);
    }

    Expedition MakeSomeExpedition(NUMBER number, ICost cost, float initHour)
    {
        var transaction = new Transaction(number, cost);
        return new Expedition(initHour, transaction);
    }

    // Update is called once per frame
    void Update()
    {
        expeditions.ToList().ForEach((x) => x.UpdateUI());
    }
}
