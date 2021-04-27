using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IdleLibrary.UI;

public class PopupSample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Popup.popup.SetPopup(gameObject, WindowKind.TextOnly, "HeyHeyTest");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
