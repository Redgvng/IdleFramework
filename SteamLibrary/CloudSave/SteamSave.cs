using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

//クラウドセーブします
namespace IdleLibrary
{
    public class SteamSave : ISaveLocation<string>
    {
        //Macは別にする必要あり？
        private const string FILENAME = "/SteamCloud_IdleSpiral.txt";
        public async UniTask<bool?> SetUserData(string save_str)
        {
            var filePath = UnityEngine.Application.persistentDataPath + FILENAME;
            var isNoSavedata = false;
            var time = new DateTime();
            //すでにセーブファイルがあるか？
            if (File.Exists(filePath))
            {
                var file = new System.IO.FileInfo(filePath);
                time = file.CreationTime;
            }
            else
            {
                isNoSavedata = true;
            }
            //ファイルをセーブする。
            using (StreamWriter sw = new StreamWriter(UnityEngine.Application.persistentDataPath + FILENAME))
            {
                Debug.Log(UnityEngine.Application.persistentDataPath + FILENAME);
                sw.WriteLine(save_str);
                sw.Close();
            }
            await UniTask.DelayFrame(1);
            if (isNoSavedata && !File.Exists(filePath)) return false;
            //更新日時が上書きされていたら
            var newFile = new System.IO.FileInfo(filePath);
            var newTime = newFile.CreationTime;
            if (!isNoSavedata && newTime <= time) return false;

            return true;
        }

        public async UniTask<string> GetUserData()
        {
            var fileContent = string.Empty;
            var filePath = UnityEngine.Application.persistentDataPath + FILENAME;
            //サーバーから取ってくる処理になります。
            if (File.Exists(filePath))
            {
                //セーブデータを取得します
                fileContent = File.ReadAllText(filePath);
            }
            else
            {
                //Debug.Log("Fileが存在しません。");
            }

            await UniTask.WaitWhile(() => fileContent == string.Empty);
            return fileContent;
        }

        public SteamSave(string gameTitle, string gameObjectName)
        {
            this.gameTitle = gameTitle;
            this.gameObjectName = gameObjectName;
        }
        string gameObjectName;
        string gameTitle;
    }
}