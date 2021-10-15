using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace IdleLibrary.Upgrade
{
    public class DefaultUpgradeUI : MonoBehaviour, IUI
    {
        public Button upgradeButton => gameObject.GetComponent<Button>();
        public TextMeshProUGUI explainText;
        public TextMeshProUGUI costText;
        public TextMeshProUGUI levelText;
    }
}