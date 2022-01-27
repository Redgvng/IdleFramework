using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace IdleLibrary
{
    public class LocalAndEditorLocation : ISaveLocation<string>
    {

        public UniTask<string> GetUserData()
        {
#if UNITY_STANDALONE_WIN
            return windows_standalone.GetUserData();
#elif UNITY_STANDALONE_OSX
            return mac_standalone.GetUserData();
#endif

#if UNITY_EDITOR
            return editor_location.GetUserData();
#elif UNITY_WEBGL
                return local_location.GetUserData();
#else
                return local_location.GetUserData();
#endif
        }

        public UniTask<bool?> SetUserData(string value)
        {
#if UNITY_STANDALONE_WIN
            return windows_standalone.SetUserData(value);
#elif UNITY_STANDALONE_OSX
            return mac_standalone.SetUserData(value);
#endif
#if UNITY_EDITOR
            return editor_location.SetUserData(value);
#elif UNITY_WEBGL
                return local_location.SetUserData(value);
#else
                return local_location.SetUserData(value);;
#endif
        }

        public LocalAndEditorLocation(string gameTitle, string gameObjectName, TextAsset saveFile_textAsset)
        {
            editor_location = new EditorSaveLocation(gameTitle, saveFile_textAsset);
            local_location = new LocalSaveLocation(gameTitle, gameObjectName);
            windows_standalone = new StandAloneSaveLocation(gameTitle, gameObjectName);
            mac_standalone = new StandAloneSaveLocationForMac(gameTitle, gameObjectName);
        }
        ISaveLocation<string> editor_location;
        ISaveLocation<string> local_location;
        ISaveLocation<string> windows_standalone;
        ISaveLocation<string> mac_standalone;
    }
}
