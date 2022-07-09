/*
#if !(UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX)
#define DISABLESTEAMWORKS
#endif
#if !DISABLESTEAMWORKS
using Steamworks;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Cysharp.Threading.Tasks;
using System;
using System.Net.Http;

namespace IdleLibrary
{
    public class ServerTime : MonoBehaviour, ITime
    {

#if !DISABLESTEAMWORKS
        private DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        private DateTime defaultTime = new DateTime();
        public DateTime currentTime { get; private set; }
        public DateTime GetDateTime(long unixTime)
        {
            return UnixEpoch.AddSeconds(unixTime);
        }
        JObject j;
        private async UniTask<DateTime> GetServerDateTime()
        {
            var time = await SteamIAP.httpClient.GetAsync(@"https://api.steampowered.com/ISteamWebAPIUtil/GetServerInfo/v1/").AsUniTask();
            string s = await time.Content.ReadAsStringAsync();
            JObject j = JObject.Parse(s);
            var unixTime = j["servertime"].ToString();
            var result = GetDateTime(long.Parse(unixTime));
            if (result == defaultTime) return DateTime.Now;
            return GetDateTime(long.Parse(unixTime));
        }
        //5ふんおきに取ろう！
        private async void UpdateServerTime()
        {
            await UniTask.Delay(2000);
            currentTime = await GetServerDateTime();
            while (true)
            {
                currentTime = await GetServerDateTime();
                await UniTask.Delay(1000 * 360 *3);
            }
        }
        private void Awake()
        {
            UpdateServerTime();
        }
#else
        public DateTime currentTime => new DateTime();
#endif
    }
}
*/
