using UnityEngine;
using IdleLibrary.UI;

namespace IdleLibrary.UI
{
    public class LogController : MonoBehaviour
    {
        public LogText[] logTexts;
        public void Log(string text, float showtimesec)
        {
            for (int i = 0; i < logTexts.Length; i++)
            {
                if (logTexts[i] != null && !logTexts[i].isActive)
                {
                    logTexts[i].SetInfo(text, showtimesec);
                    return;
                }
            }
            gameObject.transform.GetChild(logTexts.Length - 1).gameObject.GetComponent<LogText>().SetInfo(text, showtimesec);
        }
    }
}
