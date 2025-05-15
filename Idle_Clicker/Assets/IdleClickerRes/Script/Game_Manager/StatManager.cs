using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace IdleClicker
{

    public class StatManager : IButtonCommand
    {
        private TextMeshProUGUI statDisplay;
        private StatContainer statData;

        public StatManager(TextMeshProUGUI statDisplay, StatContainer statData)
        {
            this.statDisplay = statDisplay;
            this.statData = statData;
        }

        public void Execute(string name)
        {
            if (statData.buttonName == name)
            {
                statDisplay.text =
                string.Format("{0,-18}:   {1}\n\n", "Coins Collected", statData.currentCollectedCoin) +
                string.Format("{0,-18}:   {1}\n\n", "Coins/sec", statData.coinPerSecond) +
                string.Format("{0,-18}:   {1}", "Upgrades", statData.totalNumberOfUpgrades);
            }

        }

    }
}

