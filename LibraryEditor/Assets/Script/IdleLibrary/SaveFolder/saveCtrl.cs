﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using static IdleLibrary.Main;

namespace IdleLibrary
{
    //[DefaultExecutionOrder(-2)]
    public class saveCtrl : MonoBehaviour
    {
        //ロードの処理
        void getSaveKey()
        {
            //SaveR
            if (saveClass.GetObject<SaveR>(keyList.resetSaveKey) == null)
            {
                main.SR = new SaveR();
            }
            else
            {
                main.SR = saveClass.GetObject<SaveR>(keyList.resetSaveKey);
            }

            //Save
            if (saveClass.GetObject<Save>(keyList.permanentSaveKey) == null)
            {
                main.S = new Save();
            }
            else
            {
                main.S = saveClass.GetObject<Save>(keyList.permanentSaveKey);
            }

            //SaveOのロード
            /*
            if(Save_Odin.Load<SaveO>() == null)
            {
                main.SO = new SaveO();
            }
            else
            {
                main.SO = Save_Odin.Load<SaveO>();
            }
            */

        }

        //セーブの処理
        public void setSaveKey()
        {
            saveClass.SetObject(keyList.resetSaveKey, main.SR);
            saveClass.SetObject(keyList.permanentSaveKey, main.S);
            //SaveOのセーブ
            //saveClass.SetObject(keyList.odinSaveKey, main.SO);
        }


        private void Awake()
        {
            getSaveKey();
        }

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(doSave());
        }

        IEnumerator doSave()
        {
            while (true)
            {
                yield return new WaitForSeconds(1.0f);
                main.lastTime = DateTime.Now;
                setSaveKey();
            }
        }
    }
}
