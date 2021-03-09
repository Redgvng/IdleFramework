using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

namespace Tests
{
    public class PopUpTest : MonoBehaviour
    {
        public Button targetButton;
        public PopUp popUp;
        public Transform windowShowCanvas;
        // Start is called before the first frame update
        void Start()
        {
            var pop = popUp.StartPopUp(targetButton.gameObject, windowShowCanvas.GetComponent<RectTransform>());
            pop.UpdateAsObservable().Where(_ => pop.gameObject.activeSelf).Subscribe(_ => pop.text.text = "unko");
        }
    }
}
