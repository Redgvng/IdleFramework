using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using System.IO;
using System.Text;
using IdleLibrary.Inventory;

namespace IdleLibrary
{
    //Odin Save
    [System.Serializable]
    public partial class SaveO
    {

    }

    public class Save_Odin : MonoBehaviour
    {

        public static string GetJsonFromOdinSave<T>(T obj)
        {
            List<UnityEngine.Object> unityObjectReferences = new List<UnityEngine.Object>();
            DataFormat dataFormat = DataFormat.JSON;
            var bytes = SerializationUtility.SerializeValue(obj, dataFormat, out unityObjectReferences);
            string jsonStr = Encoding.UTF8.GetString(bytes);
            return jsonStr;
        }

        //Jsonを受け取って、バイト列にする。
        public static T Load<T>(string json)
        {
            List<UnityEngine.Object> unityObjectReferences = new List<UnityEngine.Object>();
            DataFormat dataFormat = DataFormat.JSON;
            var bytes = Encoding.UTF8.GetBytes(json);
            var data = SerializationUtility.DeserializeValue<T>(bytes, dataFormat, unityObjectReferences);

            return data;
        }
        /*
        // Somewhere, a method to serialize data to json might look something like this
        private void SerializeData()
        {
            // Save to Assets folder
            string path = Application.dataPath + "/data.json";

            List<UnityEngine.Object> unityObjectReferences = new List<UnityEngine.Object>();

            DataFormat dataFormat = DataFormat.JSON;

            // Serialization
            {
                var bytes = SerializationUtility.SerializeValue(originalData, dataFormat, out unityObjectReferences);
                File.WriteAllBytes(path, bytes);

                // If you want the json string, use UTF8 encoding
                // var jsonString = System.Text.Encoding.UTF8.GetString(bytes);
            }

            // Deserialization
            {
                var bytes = File.ReadAllBytes(path);

                // If you have a string to deserialize, get the bytes using UTF8 encoding
                // var bytes = System.Text.Encoding.UTF8.GetBytes(jsonString);

                var data = SerializationUtility.DeserializeValue<MyData>(bytes, dataFormat, unityObjectReferences);
            }
        }
        */
    }
}
