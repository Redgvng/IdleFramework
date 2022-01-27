using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace IdleLibrary
{
    public class LocalSaveLocation : ISaveLocation<string>
    {
        public async UniTask<bool?> SetUserData(string save_str)
        {
            string saveTitle = gameTitle + "_" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");

            Application.ExternalEval(
                        @"
		const a = document.createElement('a');
		a.href = URL.createObjectURL(new Blob(['" + save_str + @"'], {type: 'text/plain'}));
		a.download = '" + saveTitle + @"';
		a.style.display = 'none';
		document.body.appendChild(a);
		a.click();
		document.body.removeChild(a);
		");

            await UniTask.DelayFrame(1);
            return null; //結果を受け取っていないため判定できない。ユーザーは見ればわかるはずだから問題ない
        }

        public async UniTask<string> GetUserData()
        {
            localText = "";
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
                SendMessage('" + gameObjectName + @"', 'GetTextFromLocal', URL.createObjectURL(f));
            }
        };
    }
if (fileuploader) {
    fileuploader.setAttribute('class', 'focused');
}
            ");

            await UniTask.WaitWhile(() => localText == "");
            return localText;
        }

        string localText;
        void GetTextFromLocal(string fileUrl)
        {
            var task = GetTextFromFileUrl(fileUrl);
        }

        async UniTask GetTextFromFileUrl(string fileUrl)
        {
            string text = "";
            text = new WWW(fileUrl).text;
            await UniTask.WaitUntil(() => text == "");
            localText = text;
        }

        void Initialize()
        {
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
                SendMessage('" + gameObjectName + @"', 'GetTextFromLocal', URL.createObjectURL(f));
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
        }

        public LocalSaveLocation(string gameTitle, string gameObjectName)
        {
            this.gameTitle = gameTitle;
            this.gameObjectName = gameObjectName;
            Initialize();
        }
        string gameObjectName;
        string gameTitle;
    }
}
