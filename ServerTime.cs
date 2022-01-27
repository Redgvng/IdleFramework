using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Cysharp.Threading.Tasks;
using System;
using Steamworks;
using System.Net.Http;

namespace IdleLibrary
{
    public class ServerTime : MonoBehaviour, ITime
    {
        private DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        public DateTime currentTime { get; private set; }
        public DateTime GetDateTime(long unixTime)
        {
            return UnixEpoch.AddSeconds(unixTime);
        }
        JObject j;
        private async UniTask<DateTime> GetServerDateTime()
        {
            await UniTask.Delay(1000);
            var time = await SteamIAP.httpClient.GetAsync(@"https://api.steampowered.com/ISteamWebAPIUtil/GetServerInfo/v1/").AsUniTask();
            string s = await time.Content.ReadAsStringAsync();
            JObject j = JObject.Parse(s);
            var unixTime = j["servertime"].ToString();
            return GetDateTime(long.Parse(unixTime));
        }
        private async void UpdateServerTime()
        {
            while (true)
            {
                currentTime = await GetServerDateTime();
                await UniTask.Delay(1000);
            }
        }
        private void Start()
        {
            UpdateServerTime();
        }
    }
}
