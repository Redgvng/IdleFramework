using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using IdleLibrary;
using IdleLibrary.UI;
using static IdleLibrary.UsefulMethod;

public class PopupConfirmSample : MonoBehaviour
{
    public PopupConfirm_UI popupConfirm;
    public void Confirm(IText descriptionText, IText buttonText = null, Action confirmAction = null)
    {
        setActive(popupConfirm.gameObject);
        popupConfirm.UpdateUI(descriptionText, buttonText, confirmAction);
    }
    public class Description : IText
    {
        string description;
        public Description(string description)
        {
            this.description = description;
        }
        public string Text()
        {
            return description;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(() => Confirm(new Description("Sample Description")));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
