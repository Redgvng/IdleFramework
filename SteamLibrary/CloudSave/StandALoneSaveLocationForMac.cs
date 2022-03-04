using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using Cysharp.Threading.Tasks; 
using UnityEngine;

//Steam Windows用です。
namespace IdleLibrary
{
    public class StandAloneSaveLocationForMac : ISaveLocation<string>
    {
        static string filename = "localsave.txt";
        string FILENAME = "/" + filename;
        public async UniTask<bool?> SetUserData(string save_str)
        {
            Debug.Log(UnityEngine.Application.persistentDataPath + FILENAME);
            //ファイルをセーブする。
            using (StreamWriter sw = new StreamWriter(UnityEngine.Application.persistentDataPath + FILENAME))
            {
                sw.WriteLine(save_str);
                sw.Close();
            }
            await UniTask.DelayFrame(1);
            return null; //結果を受け取っていないため判定できない。ユーザーは見ればわかるはずだから問題ない
        }

        public async UniTask<string> GetUserData()
        {
            var fileContent = string.Empty;
            var filePath = UnityEngine.Application.persistentDataPath + FILENAME;
            Debug.Log(filePath);
            //サーバーから取ってくる処理になります。
            if (File.Exists(filePath))
            {
                //セーブデータを取得します
                fileContent = File.ReadAllText(filePath);
                //Debug.Log(filePath);
            }

            await UniTask.WaitWhile(() => fileContent == string.Empty);
            return fileContent;
        }

        public StandAloneSaveLocationForMac(string gameTitle, string gameObjectName)
        {
            this.gameTitle = gameTitle;
            this.gameObjectName = gameObjectName;
        }
        string gameObjectName;
        string gameTitle;
    }
}
