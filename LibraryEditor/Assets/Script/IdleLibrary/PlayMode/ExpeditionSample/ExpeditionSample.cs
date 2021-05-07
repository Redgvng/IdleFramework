using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using IdleLibrary;
using System.Linq;

//������UI�֌W�̏����͈�؏����Ȃ�
//���̃X�N���v�g������Expedition�̏����͊�������
public class ExpeditionSample : MonoBehaviour
{
    [SerializeField]
    private Expedition_UI[] expeditions;

    // Start is called before the first frame update
    void Start()
    {
        //�ʉ�
        var gold = new NUMBER(100);
        //Expedition�����
        var expedition1 = MakeSomeExpedition(1, gold, new FixedCost(1));
        var expedition2 = MakeSomeExpedition(2, gold, new FixedCost(1));
        var expedition3 = MakeSomeExpedition(3, gold, new FixedCost(1));
        var expedition4 = MakeSomeExpedition(4, gold, new FixedCost(1));

        //UI�ƕR�Â���(Instantiate��������ɕς����ق���������������Ȃ�)
        expeditions[0].LinkExpedition(expedition1);
        expeditions[1].LinkExpedition(expedition2);
        expeditions[2].LinkExpedition(expedition3);
        expeditions[3].LinkExpedition(expedition4);
    }

    Expedition MakeSomeExpedition(int id, NUMBER number, ICost cost)
    {
        var transaction = new Transaction(number, cost);
        return new Expedition(id, new ExpeditionForSave[0], null, null, new float[] { 0.5f, 1.0f, 2.0f, 4.0f, 8.0f, 24.0f});
    }

    // Update is called once per frame
    void Update()
    {
        expeditions.ToList().ForEach((x) => x.UpdateUI());
    }
}
