using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IdleLibrary.UI;

public class PopupSample : MonoBehaviour
{
    [SerializeField] WindowKind windowKind;
    // Start is called before the first frame update
    void Start()
    {
        switch (windowKind)
        {
            case WindowKind.TextOnly:
                Popup.popup.SetPopup(gameObject, WindowKind.TextOnly, "Popup Text Only Sample\nThis size is automatically fit \naccording to the description length.");
                break;
            case WindowKind.IconAndText:
                Popup.popup.SetPopup(gameObject, WindowKind.IconAndText, "Popup Icon And Text Sample\n- You can show any sprite.\n- The window size is automatically fit by this description length.", gameObject.GetComponent<Image>().sprite);
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
