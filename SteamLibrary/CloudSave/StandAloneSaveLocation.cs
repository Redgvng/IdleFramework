using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
#if UNITY_STANDALONE_WIN
using System.Windows.Forms;
#endif

//Steam Windows用です。
namespace IdleLibrary
{
    public class StandAloneSaveLocation : ISaveLocation<string>
    {
        public async UniTask<bool?> SetUserData(string save_str)
        {
#if UNITY_STANDALONE_WIN
        string saveTitle = gameTitle + "_" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
        //ディレクトリにセーブデータを書き出す
        //SaveFileDialogを生成する
        SaveFileDialog sa = new SaveFileDialog();
        sa.Title = "Save data";
        sa.InitialDirectory = @"C:\";
        sa.FileName = gameTitle;
        sa.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
        sa.FilterIndex = 1;

        //オープンファイルダイアログを表示する    
        DialogResult result = sa.ShowDialog();
        if (result == DialogResult.OK)
        {
            //「保存」ボタンが押された時の処理
            string fileName = sa.FileName;  //こんな感じで指定されたファイルのパスが取得できる
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.WriteLine(save_str);
                sw.Close();
            }
        }
        else if (result == DialogResult.Cancel)
        {
                return null;
        }
        await UniTask.DelayFrame(1);
#endif

            return null; //結果を受け取っていないため判定できない。ユーザーは見ればわかるはずだから問題ない
        }

        public async UniTask<string> GetUserData()
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

#if UNITY_STANDALONE_WIN
        using (OpenFileDialog openFileDialog = new OpenFileDialog())
        {
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                filePath = openFileDialog.FileName;

                //Read the contents of the file into a stream
                var fileStream = openFileDialog.OpenFile();

                using (StreamReader reader = new StreamReader(fileStream))
                {
                    fileContent = reader.ReadToEnd();
                }
            }
        }
#endif


            await UniTask.WaitWhile(() => fileContent == string.Empty);
            return fileContent;
        }

        public StandAloneSaveLocation(string gameTitle, string gameObjectName)
        {
            this.gameTitle = gameTitle;
            this.gameObjectName = gameObjectName;
        }
        string gameObjectName;
        string gameTitle;
    }
}
