using UnityEngine;
using Sirenix.Serialization;


namespace IdleLibrary
{
    public static class saveClass
    {
        /// <summary>
        /// 指定されたオブジェクトの情報を保存します
        /// </summary>
        public static void SetObject<T>(string key, T obj)
        {
            var json = JsonUtility.ToJson(obj);
            //var json = Save_Odin.GetJsonFromOdinSave<T>(obj);
            PlayerPrefs.SetString(key, json);
            Debug.Log(json);
        }

        /// <summary>
        /// 指定されたオブジェクトの情報を読み込みます
        /// </summary>
        public static T GetObject<T>(string key)
        {
            var json = PlayerPrefs.GetString(key);
            var obj = JsonUtility.FromJson<T>(json);
            Debug.Log(json);
            return obj;
        }

    }
}