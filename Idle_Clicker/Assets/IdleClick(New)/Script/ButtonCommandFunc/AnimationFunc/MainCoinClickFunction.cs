
using UnityEngine;

using System;


namespace IdleClicker
{
    public class MainCoinClickFunction : MonoBehaviour
    {
      
        public void MainCoinFunc(CoinFuncConfig coinFuncConfig)
        {
            coinFuncConfig.hitCount++;
           // Debug.Log($"Hit increment {coinFuncConfig.hitCount}");
            MainCoinClickThresholdManager(coinFuncConfig);
        }

        void MainCoinClickThresholdManager(CoinFuncConfig coinFuncConfig)
        {
            for (int i = 0; i < coinFuncConfig.clickThresholds.Length - 1; i++)
            {
                ResetLoopCount(coinFuncConfig);

                if (coinFuncConfig.hitCount == coinFuncConfig.clickThresholds[i].y)
                {
                    coinFuncConfig.rangeIndex++;
                }
            }
        }

        void ResetLoopCount(CoinFuncConfig coinFuncConfig)
        {
            if (coinFuncConfig.hitCount > coinFuncConfig.clickThresholds[^1].y)
            {
                coinFuncConfig.hitCount = 0;
                coinFuncConfig.loopCount++;
                coinFuncConfig.rangeIndex = 0;
            }

            if (coinFuncConfig.loopCount > coinFuncConfig.clickThresholds.Length)
            {
                coinFuncConfig.loopCount = 1;
            }
        }

       

       



    }
}


