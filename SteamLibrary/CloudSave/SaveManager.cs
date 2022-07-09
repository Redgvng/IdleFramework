using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static IdleLibrary.Main;
using static IdleLibrary.UsefulMethod;
namespace IdleLibrary
{
    /// <summary>
    /// NOTE:セーブの前にセーブの処理を止める必要がある
    /// </summary>
    public class SaveManager : MonoBehaviour
    {
        [SerializeField] string sceneName = "IdleSpiral";
        //UI
        [SerializeField] Button loadButton;
        [SerializeField] Button saveButton;
        [SerializeField] Button steam_loadButton;
        [SerializeField] Button steam_saveButton;
        [SerializeField] TextAsset editorSaveData;

        //SaveData
        public List<ISaveElement> saveDataList = new List<ISaveElement>();

        //SaveExecutor
        SaveExecutor local_executor;

        private void Start()
        {

            //セーブデータの設定
            saveDataList.Add(new SaveElement<SaveR>(main.SR, (x) => LoadFunc(ref main.SR, x)));
            saveDataList.Add(new SaveElement<Save>(main.S, (x) => LoadFunc(ref main.S, x)));
            //saveDataList.Add(new SaveElement<DTO>(gameSystem.idleSystem.dto, (x) => LoadFunc(ref gameSystem.idleSystem.dto, x)));

            // Local
            ISaveLocation<string> local_location = new LocalAndEditorLocation("Idle Spiral", gameObject.name, editorSaveData);
            local_executor = new SaveExecutor(saveDataList, local_location, true)
            { LoadAction = AfterOverload };
            loadButton.onClick.AddListener(BeforeLoadAction);
            loadButton.onClick.AddListener(local_executor.Load);
            saveButton.onClick.AddListener(local_executor.Save);

            //STEAM
            ISaveLocation<string> cloud_location = new SteamSave("Idle Spiral", gameObject.name);
            ISaveExecutor steam_executor = new SaveExecutor(saveDataList, cloud_location, true)
            { LoadAction = AfterOverload };
            steam_loadButton.onClick.AddListener(BeforeLoadAction);
            steam_loadButton.onClick.AddListener(steam_executor.Load);
            steam_saveButton.onClick.AddListener(steam_executor.Save);
            StartCoroutine(PersistCloudSave());

        }


        IEnumerator PersistCloudSave()
        {
            WaitForSecondsRealtime wait = new WaitForSecondsRealtime(300f);
            while (true)
            {
                yield return wait;
                steam_saveButton.onClick.Invoke();
            }
        }

        void BeforeLoadAction()
        {

        }

        // オブジェクトに代入した後に呼ぶ関数
        void AfterOverload()
        {
            //DTOも必ずセーブしなければいけない
            Main.main.saveCtrl.setSaveKey(); //NOTE:saveCtrlに依存させないようにする
            SceneManager.LoadScene(sceneName);
        }


        void LoadFunc<T>(ref T obj, string save_data)
        {
            obj = JsonUtility.FromJson<T>(save_data);
        }
    }
}