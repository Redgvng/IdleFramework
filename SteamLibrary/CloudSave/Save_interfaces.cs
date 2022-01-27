using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

namespace IdleLibrary
{
    /// <summary>
    /// PlayerPrefs, Local, Editor, PlayFabなどの保存先のバリエーション
    /// </summary>
    public interface ISaveLocation<T>
    {
        UniTask<bool?> SetUserData(T value); //戻り値は成功かどうか
        UniTask<T> GetUserData();
    }

    /* SaveElement. 実際にセーブをするクラス */
    public interface ISaveElement
    {
        object GetToBeSavedObject();                // object型でもJsonUtility.ToJson()は使える。
        Action<string> LoadAction { get; set; }     // ロードの処理には型の情報が必要だから。
        bool CanParse(string data_str);
        bool CanLoad(object data_obj);
        bool IsOdinData { get; set; }
    }

    /// <summary>
    /// SaveExecutor. 実際に処理を行うクラス
    /// </summary>
    public class SaveElement<T> : ISaveElement
        where T : class
    {
        public object GetToBeSavedObject() { return ToBeSavedObject; }
        public Action<string> LoadAction { get; set; }
        public bool IsOdinData { get; set; }
        public bool CanParse(string data_str)
        {
            try { return JsonUtility.FromJson<T>(data_str) != null; }
            catch { return false; }
        }
        public bool CanLoad(object data_obj)
        {
            if (_CanLoad == null) { return true; }
            return _CanLoad(data_obj);
        }
        public SaveElement(T ToBeSavedObject, Action<string> LoadAction)
        {
            this.ToBeSavedObject = ToBeSavedObject;
            this.LoadAction = LoadAction;
        }
        T ToBeSavedObject;
        public Predicate<object> _CanLoad { get; set; }
    }
    /*  */

    /* SaveExecutor. 実際に処理を行うインターフェイス */
    public interface ISaveExecutor
    {
        void Save();
        void Load();
    }

    public class SaveExecutor : ISaveExecutor
    {
        public void Save()
        {
            var task = _SaveTask();
        }

        async UniTask _SaveTask()
        {
            List<string> dataStrList = new List<string>();
            foreach (var data in saveData)
            {
                dataStrList.Add(ToJson(data.GetToBeSavedObject())); ;
            }
            // 暗号化
            if (toEncrypt)
            {
                for (int i = 0; i < dataStrList.Count; i++)
                {
                    dataStrList[i] = Encrypt(dataStrList[i]);
                }
            }

            // 結合
            string data_str = string.Join(Separator.ToString(), dataStrList);

            var task = saveLocation.SetUserData(data_str);
            //await task;
            bool? successed = await task;

            if (successed == true)
            {
                SaveSuccessAction?.Invoke();
            }
            else if (successed == false)
            {
                SaveFailureAction?.Invoke();
            }
        }

        public void Load()
        {
            var task = LoadTask();
        }

        // NOTE : Dictionaryにすればリリース後でも追加できるのでは?
        public async UniTask LoadTask()
        {
            // ロードする間に、新たなデータがセーブされるのを防ぐために時間を止める
            //Time.timeScale = 0;

            // 文字列を呼び出す
            var dataTask = saveLocation.GetUserData();
            //await dataTask;
            // 分割
            //var dataStrList = dataTask.Result.Split(Separator);
            var result = await dataTask;
            var dataStrList = result.Split(Separator);
            if (dataStrList.Length == 0) { LoadFailureAction?.Invoke(); return; }
            // 復号化
            if (toEncrypt)
            {
                for (int i = 0; i < dataStrList.Length; i++)
                {
                    dataStrList[i] = Dencrypt(dataStrList[i]);
                }
            }
            //　オブジェクトに変換可能かチェック
            /*
            for (int i = 0; i < dataStrList.Length; i++)
            {
                if (saveData[i].CanParse(dataStrList[i]) == false)
                {
                    Debug.Log("オブジェクトに変換できません");
                    LoadFailureAction?.Invoke();
                    return;
                }
            }
            */

            // ロード可能かチェック
            for (int i = 0; i < dataStrList.Length; i++)
            {
                if (saveData[i].CanLoad(dataStrList[i]) == false)
                {
                    Debug.Log("ロードできないファイルです");
                    LoadFailureAction?.Invoke();
                    return;
                }
            }

            //リリース日よりも前にロードした場合は読み込めないようにする。
            if (Main.main.ReleaseTime >= Main.main.birthTime)
            {
                Debug.Log("リリース前のセーブデータなのでロードできません");
                return;
            }

            // ゲーム内のセーブデータに代入
            for (int i = 0; i < dataStrList.Length; i++)
            {
                saveData[i].LoadAction(dataStrList[i]);
            }

            LoadAction?.Invoke();
        }

        string ToJson(object obj)
        {
            return JsonUtility.ToJson(obj);
        }

        string Encrypt(string original_str)
        {
            return Convert.ToBase64String(aes.encrypt(System.Text.Encoding.UTF8.GetBytes(original_str)));
        }

        string Dencrypt(string encrypted)
        {
            try { return System.Text.Encoding.UTF8.GetString(aes.dencrypt(Convert.FromBase64String(encrypted))); }
            catch { return ""; }
        }

        void DoLoadFailureAction()
        {
            LoadFailureAction?.Invoke();
        }

        public SaveExecutor(IEnumerable<ISaveElement> saveData, ISaveLocation<string> saveLocation, bool toEncrypt)
        {
            this.saveData = saveData.ToList();
            this.saveLocation = saveLocation;
            this.toEncrypt = toEncrypt;

            this.aes = new AES();
            this.Separator = '#';
        }
        public char Separator { get; set; }
        public Action LoadAction { get; set; }
        public Action LoadFailureAction { get; set; }
        public Action SaveSuccessAction { get; set; }
        public Action SaveFailureAction { get; set; }
        List<ISaveElement> saveData;
        ISaveLocation<string> saveLocation;
        bool toEncrypt;
        AES aes;
    }
    /*  */
}
