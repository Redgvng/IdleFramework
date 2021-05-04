using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using static IdleLibrary.Main;

namespace IdleLibrary
{
    public class Load : MonoBehaviour, IPointerDownHandler
    {
        public Button saveButton;
        public Button saveButtonOnCrazygame;
        string saveTitle, saveContent;
        [SerializeField]
        string gameTitle = "";
        [SerializeField]
        string sceneName = "";
        bool isOver;
        public static bool isLoaded;
        AES aes = new AES();
        public static bool isLoading;

        string[] saveStrArray = new string[6];
        string[] jsonArray = new string[6];

        void Start()
        {
            saveButton.onClick.AddListener(() => StartCoroutine(saveText()));
            saveButtonOnCrazygame.onClick.AddListener(() => StartCoroutine(saveText()));

#if UNITY_EDITOR
#elif UNITY_WEBGL
        


        Application.ExternalEval(
            @"
document.addEventListener('click', function() {

    var fileuploader = document.getElementById('fileuploader');
    if (!fileuploader) {
        fileuploader = document.createElement('input');
        fileuploader.setAttribute('style','display:none;');
        fileuploader.setAttribute('type', 'file');
        fileuploader.setAttribute('id', 'fileuploader');
//        fileuploader.setAttribute('class', 'focused');
        document.getElementsByTagName('body')[0].appendChild(fileuploader);

        fileuploader.onchange = function(e) {
        var files = e.target.files;
            for (var i = 0, f; f = files[i]; i++) {
                //window.alert(URL.createObjectURL(f));

				//var reader = new FileReader();
				//reader.readAsText(f);

                SendMessage('" + gameObject.name + @"', 'FileDialogResult', URL.createObjectURL(f));
            }
        };
    }else{
	    if (fileuploader.getAttribute('class') == 'focused') {
	        fileuploader.setAttribute('class', '');
	        fileuploader.click();

	    }
	}
});
            ");
#endif
        }

        //Incentivized Ads 初回Bonusから呼ぶ
        public IEnumerator saveText()
        {
            //main.saveCtrl.setSaveKey();
            yield return new WaitForSeconds(0.3f);

            saveTitle = gameTitle + "_" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");

            saveStrArray[0] = PlayerPrefs.GetString(keyList.resetSaveKey);
            saveStrArray[1] = PlayerPrefs.GetString(keyList.permanentSaveKey);
            //暗号化
            for (int i = 0; i < saveStrArray.Length; i++)
            {
                jsonArray[i] = null;
                jsonArray[i] = Convert.ToBase64String(aes.encrypt(System.Text.Encoding.UTF8.GetBytes(saveStrArray[i])));
                yield return jsonArray[i];
            }
            //結合
            saveContent = null;
            saveContent = string.Join("#", jsonArray);
            yield return saveContent;

#if UNITY_EDITOR
            //EditorのAssets/SaveData/にセーブ
            FileInfo file = new FileInfo(Application.dataPath + "/SaveData/" + gameTitle + "_OnEditor.txt");
            file.Directory.Create();
            File.WriteAllText(file.FullName, saveContent);

#elif UNITY_WEBGL
        Application.ExternalEval(
                    @"
		const a = document.createElement('a');
		a.href = URL.createObjectURL(new Blob(['"+ saveContent + @"'], {type: 'text/plain'}));
		a.download = '" + saveTitle + @"';

		a.style.display = 'none';
		document.body.appendChild(a);
		a.click();
		document.body.removeChild(a);
		");
#endif
        }

        public void OnPointerDown(PointerEventData eventData)
        {
#if UNITY_EDITOR
            LoadOnEditor();
#elif UNITY_WEBGL
        Application.ExternalEval(
            @"
var fileuploader = document.getElementById('fileuploader');
if (!fileuploader) {
        fileuploader = document.createElement('input');
        fileuploader.setAttribute('style','display:none;');
        fileuploader.setAttribute('type', 'file');
        fileuploader.setAttribute('id', 'fileuploader');
		fileuploader.setAttribute('class', 'focused');
        document.getElementsByTagName('body')[0].appendChild(fileuploader);

        fileuploader.onchange = function(e) {
        var files = e.target.files;
            for (var i = 0, f; f = files[i]; i++) {
                //window.alert(URL.createObjectURL(f));

				//var reader = new FileReader();
				//reader.readAsText(f);

                SendMessage('" + gameObject.name + @"', 'FileDialogResult', URL.createObjectURL(f));
            }
        };
    }
if (fileuploader) {
    fileuploader.setAttribute('class', 'focused');
}
            ");
#endif
        }

        WWW www;

        public void FileDialogResult(string fileUrl)
        {
            // isLoading = true;
            // preventError();
            // //StartCoroutine(ReadTxt(fileUrl));
            // ReadTxt(fileUrl);
            StartCoroutine(preDownLoad(fileUrl));
        }

        IEnumerator preDownLoad(string url)
        {
            isLoaded = true;
            www = new WWW(url);
            yield return www;
            preventError();
            ReadTxt(url);
        }

        void ReadTxt(string url)
        {
            //var www = new WWW(url);
            //yield return www;
            jsonArray = www.text.Split('#');
            //復号化

            for (int i = 0; i < jsonArray.Length; i++)
            {
                saveStrArray[i] = null;
                saveStrArray[i] = System.Text.Encoding.UTF8.GetString(aes.dencrypt(Convert.FromBase64String(jsonArray[i])));
                //yield return saveStrArray[i];
            }
            SaveR SRdata = JsonUtility.FromJson<SaveR>(saveStrArray[0]);
            Save Sdata = JsonUtility.FromJson<Save>(saveStrArray[1]);

            main.SR = SRdata;
            main.S = Sdata;
            //yield return new WaitForSeconds(1.0f);
            SceneManager.LoadScene(sceneName);
        }

        public TextAsset saveFile_Debug;
        [ContextMenu("Editorからロード")]
        void LoadOnEditor()
        {
            if (saveFile_Debug == null)
            {
                return;
            }
            preventError();
            //StartCoroutine(LoadOnEditorCor(saveFile_Debug.text));
            LoadOnEditorCor(saveFile_Debug.text);
        }

        void preventError()
        {
            StopAllCoroutines();
            Time.timeScale = 0;
        }

        void LoadOnEditorCor(string data)
        {
            // yield return new WaitForSeconds(0.1f);
            // yield return data;
            jsonArray = data.Split('#');
            //復号化
            for (int i = 0; i < jsonArray.Length; i++)
            {
                saveStrArray[i] = null;
                saveStrArray[i] = System.Text.Encoding.UTF8.GetString(aes.dencrypt(Convert.FromBase64String(jsonArray[i])));
                // yield return saveStrArray[i];
            }
            SaveR SRdata = JsonUtility.FromJson<SaveR>(saveStrArray[0]);
            Save Sdata = JsonUtility.FromJson<Save>(saveStrArray[1]);

            main.SR = SRdata;
            main.S = Sdata;

            //yield return new WaitForSeconds(1.0f);
            SceneManager.LoadScene("main");
        }
    }
}
