using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using static IdleLibrary.Main;
using UnityEngine.SceneManagement;

namespace IdleLibrary
{
    //[DefaultExecutionOrder(-2)]
    public class saveCtrl : MonoBehaviour
    {
        //ロードの処理
        void getSaveKey()
        {
            //Save
            if (saveClass.GetObject<Save>(keyList.permanentSaveKey) == null)
            {
                main.S = new Save();
            }
            else
            {
                main.S = saveClass.GetObject<Save>(keyList.permanentSaveKey);
            }

            //SaveR
            if (saveClass.GetObject<SaveR>(keyList.resetSaveKey) == null)
            {
                main.SR = new SaveR();
            }
            else
            {
                main.SR = saveClass.GetObject<SaveR>(keyList.resetSaveKey);
            }

            //DTOのロード
            /*
            if(saveClass.GetObject<DTO>("dto") == null)
            {
                main.dto = new DTO();
            }
            else
            {
                main.dto = saveClass.GetObject<DTO>("dto");
            }
            */
        }

        //セーブの処理
        public void setSaveKey()
        {
            saveClass.SetObject(keyList.resetSaveKey, main.SR);
            saveClass.SetObject(keyList.permanentSaveKey, main.S);
            //saveClass.SetObject("dto", gameSystem.idleSystem.dto);
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
            var wait = new WaitForSeconds(1.0f);
            while (true)
            {
                yield return wait;
                main.lastTime = DateTime.Now;
                setSaveKey();
            }
        }
    }
}
