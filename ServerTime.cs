using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Cysharp.Threading.Tasks;
using System;
using Steamworks;

public class ServerTime : MonoBehaviour
{
    private DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);
    private DateTime currentTime;
    private static 
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
