using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using static Main;

public class Load : MonoBehaviour, IPointerDownHandler
{
    public Button saveButton;
    string saveTitle, saveContent;
    bool isOver;
    AES aes = new AES();

    string[] saveStrArray = new string[2];
    string[] jsonArray = new string[2];

    void Start()
    {
#if UNITY_EDITOR
#elif UNITY_WEBGL
        saveButton.onClick.AddListener(()=>StartCoroutine(saveText()));


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
        saveTitle = "IdleWine_" + DateTime.Now.ToString();

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


    public void FileDialogResult(string fileUrl)
    {
        StartCoroutine(ReadTxt(fileUrl));
    }

    IEnumerator ReadTxt(string url)
    {
        var www = new WWW(url);
        yield return www;
        jsonArray = www.text.Split('#');
        //復号化

        for (int i = 0; i < jsonArray.Length; i++)
        {
            saveStrArray[i] = null;
            saveStrArray[i] = System.Text.Encoding.UTF8.GetString(aes.dencrypt(Convert.FromBase64String(jsonArray[i])));
            yield return saveStrArray[i];
        }
        SaveR SRdata = JsonUtility.FromJson<SaveR>(saveStrArray[0]);
        Save Sdata = JsonUtility.FromJson<Save>(saveStrArray[1]);
        yield return SRdata;
        yield return Sdata;
        main.SR = SRdata;
        main.S = Sdata;
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("wine");
    }

    //string debugData = "";
    //public void LoadOnEditor()
    //{
    //    StartCoroutine(LoadOnEditorCor(debugData));
    //}

    //IEnumerator LoadOnEditorCor(string data)
    //{
    //    yield return data;
    //    jsonArray = data.Split('#');
    //    //復号化
    //    for (int i = 0; i < jsonArray.Length; i++)
    //    {
    //        saveStrArray[i] = null;
    //        saveStrArray[i] = System.Text.Encoding.UTF8.GetString(aes.dencrypt(Convert.FromBase64String(jsonArray[i])));
    //        yield return saveStrArray[i];
    //    }
    //    SaveR SRdata = JsonUtility.FromJson<SaveR>(saveStrArray[0]);
    //    Save Sdata = JsonUtility.FromJson<Save>(saveStrArray[1]);
    //    yield return SRdata;
    //    yield return Sdata;
    //    main.SR = SRdata;
    //    main.S = Sdata;
    //    yield return new WaitForSeconds(1.0f);
    //    SceneManager.LoadScene("wine");
    //}
}
