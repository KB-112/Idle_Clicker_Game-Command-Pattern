using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IdleClicker
{
    public class StatHolderFunction : MonoBehaviour
    {

        public void StatInfo(StatInputConfig statInputConfig, ShopConfig shopInputConfig, string buttonPressedName)
        {
            if (buttonPressedName == statInputConfig.buttonName)
            {
                statInputConfig.statInfo.text =
         $"<mspace=25>Total    :</mspace> {statInputConfig.totalBalance} coins\n" +
         $"<mspace=25>Current  :</mspace> {(int.TryParse(statInputConfig.currentScoreText.text, out int val) ? val : 0)} coins\n" +
         $"<mspace=25>Per Tap  :</mspace> {shopInputConfig.tapUpgrades[0].tapPerIncrement} coins\n" +
         $"<mspace=25>Per Idle :</mspace> {shopInputConfig.idleUpgrades[0].idlePerIncrement} coins";
            }
        }
    }

    [System.Serializable]
    public class StatInputConfig
    {
        public string buttonName;
        public TextMeshProUGUI statInfo;
        public int totalBalance;
        public TextMeshProUGUI currentScoreText;
    
    }

    

}
