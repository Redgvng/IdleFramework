#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH||!DEBUG
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS
using Steamworks;
using Newtonsoft.Json.Linq;
#endif
using System.Net.Http;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class SteamIAP : MonoBehaviour
{

    string itemId = "";
    string amount = "";
    string description = "";
    Action OnApproved = () => { };
    Action OnDenied = () => { };
    public void Initialize(string itemId, string amount, string description, Action OnApproved, Action OnDenied)
    {
        this.itemId = itemId;
        this.amount = amount;
        this.description = description;
        this.OnApproved = OnApproved;
        this.OnDenied = OnDenied;
        gameObject.GetComponent<Button>().onClick.AddListener(Buy);
    }

#if !DISABLESTEAMWORKS
    //ここにSteamWebAPIKeyを入力します。
    public static readonly string SteamWebAPIKey = "27EF08F4646592A8FFE6ED771150F2DE";
    //デポのIDを入力します。
    public static readonly string appId = "1690730";
    //オーバーレイからゲームに戻ったときのコールバックです。
    private Callback<GameOverlayActivated_t> overlayCallback;
    void OnOverlayAction(GameOverlayActivated_t overlay_callback)
    {
        isOverlay = overlay_callback.m_bActive == 1 ? true : false;
    }
    static bool isOverlay;
    public static bool isTxn;
    //1 ... 許可されました。 2... 拒否されました。
    public static int isPurchaseApprovedBySteam = 0;
    //決済中にパネルを表示してクリックできないようにします。
    public GameObject ClickBlockPanel;

    //HttpClient
    public static HttpClient httpClient = new HttpClient();
    //ボタンを押すたびに呼ばれます。
    async void Buy()
    {
        isTxn = true;
        isPurchaseApprovedBySteam = 0;
        //SteamIdです
        CSteamID userId = SteamUser.GetSteamID();
        ulong id = userId.m_SteamID;
        //言語を取得します。
        string language = SteamApps.GetCurrentGameLanguage();
        var result = await httpClient.GetAsync(@"https://partner.steam-api.com/ISteamMicroTxn/GetUserInfo/v2/?key=" + SteamWebAPIKey
            + "&steamid=" + id).AsUniTask();
        string s = await result.Content.ReadAsStringAsync();
        JObject jObject = JObject.Parse(s);
        //通貨です。
        string currency = jObject["response"]["params"]["currency"].ToString();
        int orderId = UnityEngine.Random.Range(1, int.MaxValue);
        var parameters = new Dictionary<string, string>()
            {
            {"key", SteamWebAPIKey },
            {"orderid", orderId.ToString() },
            {"steamid", id.ToString() },
            {"appid", appId },
            {"itemcount", 1.ToString() },
            {"language", "en" },
            {"currency", "USD" },
            {"itemid[0]", itemId },
            {"qty[0]", 1.ToString() },
            {"amount[0]", amount },
            {"description[0]", description },
            };
        var content = new FormUrlEncodedContent(parameters);
        var result2 = await httpClient.PostAsync($"https://partner.steam-api.com/ISteamMicroTxn/InitTxn/v3/"
            , content);

        await result2.Content.ReadAsStringAsync();

        await UniTask.WaitUntil(() => isPurchaseApprovedBySteam != 0);
        if (isPurchaseApprovedBySteam == -1)
        {
            isTxn = false;
            return;
        }
        else if (isPurchaseApprovedBySteam == 1)
        {
            //ここに購入の処理を書きます。
            OnApproved();
        }
        isTxn = false;
    }
#else
    void Buy() { }
#endif

}

