using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace IdleClicker
{
    public class NonTapCounterFunction : MonoBehaviour
    {
        private Coroutine idleCoroutine;

        private ShopConfig shopConfig;
        private NonTapCounterConfig nonTapCounterConfig;
        private MainGameInitConfig mainGameInitConfig;
        private TapCounterConfig tapCounterConfig;
        public static Action<MainGameInitConfig, TapCounterConfig, NonTapCounterConfig> idleCounterStart;


        void OnEnable()
        {
            MainGameInitFunction.onGameStarted += InitiateCounter;
            TapCounterFunction.onTapping += StopCounter;
            TapCounterFunction.startIdleCounter += InitiateCounter;
        }

        void OnDisable()
        {
            MainGameInitFunction.onGameStarted -= InitiateCounter;
           TapCounterFunction.onTapping -= StopCounter;
            TapCounterFunction.startIdleCounter -= InitiateCounter;
        }

      
        public void FetchNonTapCounterFunction(ShopConfig shopCfg, NonTapCounterConfig nonTapCfg, MainGameInitConfig gameCfg,TapCounterConfig tapCounterConfig)
        {
            shopConfig = shopCfg;
            nonTapCounterConfig = nonTapCfg;
            mainGameInitConfig = gameCfg;
            this.tapCounterConfig = tapCounterConfig;
        }

        IEnumerator NonClickCoinRoutine()
        {
            while (nonTapCounterConfig.isNonClickEarner)
            {

                int coinsEarned = shopConfig.idleUpgrades[shopConfig.idleUpgradeLevel].idlePerIncrement;
                nonTapCounterConfig.idleScore += coinsEarned;
                mainGameInitConfig.totalBalance += coinsEarned; 
                idleCounterStart?.Invoke(mainGameInitConfig, tapCounterConfig, nonTapCounterConfig);

                // Debug.Log($"Loop initiate : {shopConfig.idleUpgrades[shopConfig.idleUpgradeLevel].idlePerIncrement}");
                yield return new WaitForSeconds(1f);


               
                mainGameInitConfig.scoreText.text = nonTapCounterConfig.idleScore.ToString();
            }
        }

       
        public void InitiateCounter()
        {
            if (shopConfig == null || nonTapCounterConfig == null || mainGameInitConfig == null)
            {
                Debug.LogWarning("Configs not set! Make sure FetchNonTapCounterFunction is called before starting.");
                return;
            }
           
            nonTapCounterConfig.idleScore = 0;
           nonTapCounterConfig.isNonClickEarner = true;
            idleCoroutine = StartCoroutine(NonClickCoinRoutine());
        }

        public void StopCounter()
        {
            if (nonTapCounterConfig != null)
                nonTapCounterConfig.isNonClickEarner = false;
            Debug.Log("Idle COunter stop");
            if (idleCoroutine != null)
            {
                StopCoroutine(idleCoroutine);
                idleCoroutine = null;
            }
        }
    }


    [System.Serializable]
    public class NonTapCounterConfig
    {
        public int idleScore;
        public bool isNonClickEarner =false;


    }
}
