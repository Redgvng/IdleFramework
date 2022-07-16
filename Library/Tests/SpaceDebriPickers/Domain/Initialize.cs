using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pickers.Domain {
    public class InitializeGame
    {
        private Test.Stage stage { get; set; } = new Test.Stage();
        private Test.DebriCollection debriCollection { get; set; } = new Test.DebriCollection();
        public CurrencyManager currencyManager { get; private set; } =  new CurrencyManager();

        public void Initialize()
        {
            var debriMultiplier = new ApplyDebriMultiplier(currencyManager, debriCollection);
            debriMultiplier.ApplyMultiplier();
        }

        public void StartGame()
        {
            stage.Initialize();
        }

        public void UpdateGame(float time = 1.0f)
        {
            stage.UpdateStage(time);
            currencyManager.UpdatePerTime(time);
        }
    }
}
