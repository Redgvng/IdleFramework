using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UsefulMethod;
using static UsefulStatic;
using IdleLibrary;

public class ExpeditionSample : MonoBehaviour
{
    Expedition expedition;
    public Button startClaimButton, rightButton, leftButton;
    public TextMeshProUGUI startClaimText, requiredHourText, progressPercentText, rewardText;
    public Slider progressBar;
    float[] requiredHours = new float[] { 0.5f, 1.0f, 2.0f, 4.0f, 8.0f, 24.0f };
    int hourId;

    void UpdateUI()
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
        if (expedition.IsStarted())
            return;
        if (isRight)
            hourId = hourId < requiredHours.Length - 1 ? hourId + 1 : 0;
        else
            hourId = hourId > 0 ? hourId - 1 : requiredHours.Length - 1;
        expedition.SelectTime(requiredHours[hourId]);
        UpdateRequiredHour();
    }

    // Start is called before the first frame update
    void Start()
    {
        var number = new NUMBER(100);
        var cost = new FixedCost(30);
        var transaction = new Transaction(number, cost);
        expedition = new Expedition(0.5f, transaction);

        UpdateUI();
        startClaimButton.onClick.AddListener(() => { expedition.StartOrClaim(); UpdateUI(); });
        rightButton.onClick.AddListener(() => SwitchRequiredHour(true));
        leftButton.onClick.AddListener(() => SwitchRequiredHour(false));
    }

    // Update is called once per frame
    void Update()
    {
        UpdateProgress();
    }
}
