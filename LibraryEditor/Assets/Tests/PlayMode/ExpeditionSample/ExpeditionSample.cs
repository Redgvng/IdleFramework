using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UsefulMethod;
using static UsefulStatic;
using IdleLibrary;
using System.Linq;

//‚±‚±‚ÉUIŠÖŒW‚Ìˆ—‚ÍˆêØ‘‚©‚È‚¢
//‚±‚ÌƒXƒNƒŠƒvƒg‚¾‚¯‚ÅExpedition‚Ìˆ—‚ÍŠ®Œ‹‚·‚é
public class ExpeditionSample : MonoBehaviour
{
    float[] requiredHours = new float[] { 0.5f, 1.0f, 2.0f, 4.0f, 8.0f, 24.0f };
    int hourId;
    [SerializeField]
    private Expedition_UI[] expeditions;

    // Start is called before the first frame update
    void Start()
    {
        //’Ê‰Ý
        var gold = new NUMBER(100);
        //Expedition‚ðì‚é
        var expedition1 = MakeSomeExpedition(gold, new FixedCost(1), 0.5f);
        var expedition2 = MakeSomeExpedition(gold, new FixedCost(1), 0.5f);
        var expedition3 = MakeSomeExpedition(gold, new FixedCost(1), 0.5f);
        var expedition4 = MakeSomeExpedition(gold, new FixedCost(1), 0.5f);

        //UI‚Æ•R‚Ã‚¯‚é(Instantiate‚·‚é‚â‚è•û‚É•Ï‚¦‚½‚Ù‚¤‚ª‚¢‚¢‚©‚à‚µ‚ê‚È‚¢)
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
