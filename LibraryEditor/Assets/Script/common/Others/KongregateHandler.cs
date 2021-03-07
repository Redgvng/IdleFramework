
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KongregateHandler : MonoBehaviour
{
    public TextMeshProUGUI idText;

    // Standard unity callback, executed once for the script to start
    public void Start()
    {
        // Utility: show feedback
        SetMessage("Loading kongregate api...");

        /*
         * Important: execute Javascript in the external context to initialize
         * Kongregate API, Unity Support and set up callback GameObject and Method.
         * In this case, callback is set to a GameObject called Kongregate and a
         * method called OnKongregateAPILoaded, which we define later in this class.
         * Once Kongregate API is initialized, Unity will locate this object by name
         * ("Kongregate") and execute a method "OnKongregateAPILoaded" passing in user
         * info string as an argument.
         */
        Application.ExternalEval(
          "if(typeof(kongregateUnitySupport) != 'undefined'){" +
          " kongregateUnitySupport.initAPI('Kongregate', 'OnKongregateAPILoaded');" +
          "} else {" +
          " console.error('No unity support!');" +
          "};"
        );
    }

    /*
     * Executed once Kongregate API is ready. This method is invoked by KongregateAPI
     * and receives a structured text with multiple pieces of data you must parse manually.
     * The userInfo string parameter has the following structure: 'user_identifier|user_name|auth_token'
     */
    public void OnKongregateAPILoaded(string userInfo)
    {
        SetMessage("Received user info! Logging though playfab...");

        // We split userInfo string using '|' character to acquire auth token and Kongregate ID.
        var userInfoArray = userInfo.Split('|');
        var authTicket = userInfoArray[2];
        var kongregateId = userInfoArray[0];

        LogToBrowser("Auth Token: " + authTicket);
        LogToBrowser("Kongregate Id: " + kongregateId);

        //idText.text = "Id : " +  authTicket + "\n" + kongregateId;

        /*
         * We then execute PlayFab API call called LoginWithKongregate.
         * LoginWithKongregate requires KongregateID and AuthTicket.
         * We also pass CreateAccount flag, to automatically create player account.
         */
    }


    
    private void SetMessage(string message)
    {
        Debug.Log(message);
    }

    private void LogToBrowser(string message)
    {
        Application.ExternalEval(string.Format("console.log('{0}')", message));
    }

    public Text InfoLabel;

}