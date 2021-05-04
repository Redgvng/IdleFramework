using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using static IdleLibrary.Main;
using static IdleLibrary.UsefulMethod;
using Cysharp.Threading.Tasks;


public class LogController : MonoBehaviour
{
    public LOGTEXT[] logTexts;

    public void Log(string text, float timesec = 3.0f)
    {        
        for (int i = 0; i < logTexts.Length; i++)
        {
            if (!logTexts[i].isActive)
            {
                logTexts[i].SetInfo(text, timesec);
                return;
            }
        }
        gameObject.transform.GetChild(logTexts.Length - 1).gameObject.GetComponent<LOGTEXT>().SetInfo(text, timesec);
    }
}
