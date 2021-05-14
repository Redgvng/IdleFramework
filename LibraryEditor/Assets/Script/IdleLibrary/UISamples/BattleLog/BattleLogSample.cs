using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using IdleLibrary.UI;

public class BattleLogSample : MonoBehaviour
{
    public LogController logCtrl;
    public Button makeSampleLogButton;
    void SampleLog()
    {
        logCtrl.Log("<size=20>Sample Log From Button", 2);
    }
    async void SampleLogMaker()
    {
        int count = 0;
        while (true)
        {
            count++;
            logCtrl.Log("<size=20>Sample Log " + count.ToString(), 2);
            await UniTask.Delay(1000);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        SampleLogMaker();
        makeSampleLogButton.onClick.AddListener(SampleLog);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
