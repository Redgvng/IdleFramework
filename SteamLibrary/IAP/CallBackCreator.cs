#if UNITY_ANDROID || UNITY_IOS || UNITY_TIZEN || UNITY_TVOS || UNITY_WEBGL || UNITY_WSA || UNITY_PS4 || UNITY_WII || UNITY_XBOXONE || UNITY_SWITCH
#define DISABLESTEAMWORKS
#endif

#if !DISABLESTEAMWORKS
using Steamworks;
using System.Net.Http;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;

public class CallBackCreator : MonoBehaviour
{
    //Steam�̏��F�R�[���o�b�N�ł��BOnUserRespondedToTxn��ǂ����ň��o�^���Ă��������B
    private Callback<MicroTxnAuthorizationResponse_t> responseCallback;
    async void OnUserRespondedToTxn(MicroTxnAuthorizationResponse_t txn_callback)
    {
        //BASE.main.debugText.text = "Callback�͌Ă΂�Ă��";
        if (txn_callback.m_bAuthorized == 1)
        {
            //�Q�[����ʂɖ߂�܂ő҂��܂��B
            var parameters_purshaceComplete = new Dictionary<string, string>()
             {
            {"key", SteamIAP.SteamWebAPIKey },
            {"orderid", txn_callback.m_ulOrderID.ToString() },
            {"appid", SteamIAP.appId },
              };
            var result_purchaseComplete = await SteamIAP.httpClient.PostAsync($"https://partner.steam-api.com/ISteamMicroTxn/FinalizeTxn/v2/"
                , new FormUrlEncodedContent(parameters_purshaceComplete));
            //result = OK�ł���΍w�������������̂ŁA�A�C�e����t�^���܂��B
            string str_purshaceComplete = await result_purchaseComplete.Content.ReadAsStringAsync();
            JObject jObject_purchaseComplete = JObject.Parse(str_purshaceComplete);
            string isPurchaseCompletedStr = jObject_purchaseComplete["response"]["result"].ToString();
            if (isPurchaseCompletedStr == "OK")
            {
                SteamIAP.isPurchaseApprovedBySteam = 1;
            }
            else
            {
                SteamIAP.isPurchaseApprovedBySteam = -1;
            }
        }
        else
        {
            SteamIAP.isPurchaseApprovedBySteam = -1;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        responseCallback = Callback<MicroTxnAuthorizationResponse_t>.Create(OnUserRespondedToTxn);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
#endif
