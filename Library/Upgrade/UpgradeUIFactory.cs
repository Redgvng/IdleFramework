using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UniRx;

//簡単なもの
namespace IdleLibrary.Upgrade
{
    public interface IUI
    {
        Button upgradeButton { get; }
    }

    public class UpgradeUIFactory<T> : MonoBehaviour where T : MonoBehaviour, IUI
    {
        public T Create(IUpgrade upgrade, T ui, Transform canvas)
        {
            var instantiatedUi = Instantiate(ui, canvas);
            instantiatedUi.upgradeButton.onClick.AddListener(() => upgrade.Pay());
            Observable.EveryFixedUpdate().Subscribe(_ => instantiatedUi.upgradeButton.interactable = upgrade.CanBuy());
            return instantiatedUi;
        }
    }
}
