using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IdleLibrary;
using IdleLibrary.Upgrade;
using System.Linq;

namespace IdleLibrary.IntegrationTest
{
    public interface IPrestigeAgent
    {
        public bool CanPrestige();
    }
    //Prestigeのアルゴリズムを決める
    //とりあえず、アップグレードが買えるならprestigeする。
    public class AsSoonAsYouCanBuy : IPrestigeAgent
    {
        private readonly IPrestigePoint prestigePoint;
        private readonly IEnumerable<Upgrade.Upgrade> upgrades;
        public AsSoonAsYouCanBuy(IPrestigePoint prestigePoint, IEnumerable<Upgrade.Upgrade> upgrades)
        {
            this.prestigePoint = prestigePoint;
            this.upgrades = upgrades;
        }
        public bool CanPrestige()
        {
            //プレスティージ後のポイントを計算する。
            var totalPoint = prestigePoint.TempNumber + prestigePoint.Number;
            //アップグレードの中で、最もコストの低いものを計算する。
            var lowest = upgrades.Min(upgrade => upgrade.cost.Cost);
            //Debug.Log($"lowest : {lowest:F1}, totalPoint : {totalPoint:F1}");
            if (totalPoint >= lowest)
            {
                return true;
            }

            return false;
        }
    }
}
