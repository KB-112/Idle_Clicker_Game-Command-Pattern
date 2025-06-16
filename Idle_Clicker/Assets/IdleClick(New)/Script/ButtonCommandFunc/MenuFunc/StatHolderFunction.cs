using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IdleClicker
{
    public class StatHolderFunction : MonoBehaviour
    {

        public void StatInfo(StatInputConfig statInputConfig, ShopConfig shopInputConfig,string buttonPressedName)
        {
            if (buttonPressedName == statInputConfig.buttonName)
            {
               


                statInputConfig.statInfo.text =
         $"<mspace=25>Total    :</mspace> {statInputConfig.playerData.TotalBalance} coins\n" +
         $"<mspace=25>Current  :</mspace> {statInputConfig.playerData.Score} coins\n" +
         $"<mspace=25>Per Tap  :</mspace> {shopInputConfig.tapUpgrades[shopInputConfig.tapUpgradeLevel].tapPerIncrement} coins\n" +
         $"<mspace=25>Per Idle :</mspace> {shopInputConfig.idleUpgrades[shopInputConfig.idleUpgradeLevel].idlePerIncrement} coins";
            }
        }
    }

    [System.Serializable]
    public class StatInputConfig
    {
        public string buttonName;
        public TextMeshProUGUI statInfo;
        public TextMeshProUGUI previousScoreText;
        public MainPlayerData playerData;
    
    }

    

}
