using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IdleLibrary.Upgrade;
using System.Linq;

namespace IdleLibrary.IntegrationTest
{
    public class IntegrationUtiliity
    {
        public static void AutoBuy(IEnumerable<Upgrade.Upgrade> upgrades)
        {
            upgrades.Select((upgrade, index) => new { upgrade, index }).ToList().ForEach(x =>
            {
                if (x.upgrade.CanBuy())
                {
                    x.upgrade.Pay();
                }
            });
        }

        public static string UpgradeLevelsOnCSV(params Upgrade.Upgrade[] upgrades)
        {
            var text = "";
            foreach (var upgrade in upgrades)
            {
                text += $"{upgrade.level},";
            }
            return text;
        }
    }
}
