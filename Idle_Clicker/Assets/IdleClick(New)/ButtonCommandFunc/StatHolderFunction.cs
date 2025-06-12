using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IdleClicker
{
    public class StatHolderFunction : MonoBehaviour
    {

        public void StatInfo(StatInputConfig statInputConfig, ShopInputConfig shopInputConfig, string buttonPressedName)
        {
            if (buttonPressedName == statInputConfig.buttonName)
            {
                statInputConfig.statInfo.text =
         $"<mspace=25>Total    :</mspace> {statInputConfig.totalBalance} coins\n" +
         $"<mspace=25>Current  :</mspace> {(int.TryParse(statInputConfig.currentScoreText.text, out int val) ? val : 0)} coins\n" +
         $"<mspace=25>Per Tap  :</mspace> {shopInputConfig.incrPerClick[0].tapPerIncrement} coins\n" +
         $"<mspace=25>Per Idle :</mspace> {shopInputConfig.incrPerSecond[0].idlePerIncrement} coins";
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

    [System.Serializable]
    public struct ShopInputTapPerIncrement
    {
       
        public int tapPerIncrement;
        public int cost;
        public int spend;
    }

    [System.Serializable]
    public struct ShopInputIdlePerIncrement
    {
        public int idlePerIncrement;
        public int cost;
        public int spend;
    }


    [System.Serializable]
    public class ShopInputConfig
    {
       public List<ShopInputTapPerIncrement> incrPerClick;
      public List<ShopInputIdlePerIncrement> incrPerSecond;
      
    }

}
