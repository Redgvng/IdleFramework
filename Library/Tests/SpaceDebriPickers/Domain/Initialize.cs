using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pickers.Domain {
    //唯一のエントリーポイントである
    public class InitializeGame
    {
        Test.Stage stage;
        public void Initialize()
        {
            stage = new Test.Stage();
        }

        public void StartGame()
        {
            stage.Initialize();
        }

        public void UpdateGame(float time = 1.0f)
        {
            stage.UpdateStage(time);
        }
    }
}
