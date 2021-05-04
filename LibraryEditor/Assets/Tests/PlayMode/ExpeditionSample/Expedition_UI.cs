using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UsefulMethod;
using static UsefulStatic;
using IdleLibrary;

//‚±‚ê‚ÍExpedition‚ÌŽÀ‘•‚ÌÚ×‚ÉˆË‘¶‚µ‚È‚¢
public class Expedition_UI : MonoBehaviour
{
    Expedition expedition;
    public Button startClaimButton, rightButton, leftButton;
    public TextMeshProUGUI startClaimText, requiredHourText, progressPercentText, rewardText;
    public Slider progressBar;
    //«‚±‚Ì2‚Â‚ÍŽÀ‘•‚ÌÚ×‚È‚Ì‚Å‘‚©‚È‚¢
    //float[] requiredHours = new float[] { 0.5f, 1.0f, 2.0f, 4.0f, 8.0f, 24.0f };
    //int hourId;

    //«ÅI“I‚É‚Íˆø”‚ðinterface‚É‚·‚é
    public void LinkExpedition(Expedition expedition)
    {
        this.expedition = expedition;
    }

    public void UpdateUI()
    {
        UpdateStartClaimButton();
        UpdateProgress();
        UpdateRequiredHour();
        UpdateRightLeftButton();
    }
    void UpdateStartClaimButton()
    {
        if (expedition.IsStarted())
        {
            startClaimText.text = "Claim";
            startClaimButton.interactable = expedition.CanClaim();
        }
        else
        {
            startClaimText.text = "Start";
            startClaimButton.interactable = expedition.CanStart();
        }
    }
    void UpdateRightLeftButton()
    {
        rightButton.interactable = !expedition.IsStarted();
        leftButton.interactable = !expedition.IsStarted();
    }
    void UpdateProgress()
    {
        progressPercentText.text = DoubleTimeToDate(expedition.CurrentTimesec()) + " ( " + percent(expedition.ProgressPercent()) + " )";
        progressBar.value = expedition.ProgressPercent();
    }
    void UpdateRequiredHour()
    {
        requiredHourText.text = expedition.RequiredTime(false).ToString("F1") + " h";
    }
    void SwitchRequiredHour(bool isRight)
    {
        expedition.SwitchRequiredHour(isRight);
    }

    // Start is called before the first frame update
    void Start()
    { 
        startClaimButton.onClick.AddListener(() => { expedition.StartOrClaim(); });
        rightButton.onClick.AddListener(() => SwitchRequiredHour(true));
        leftButton.onClick.AddListener(() => SwitchRequiredHour(false));
    }

}
