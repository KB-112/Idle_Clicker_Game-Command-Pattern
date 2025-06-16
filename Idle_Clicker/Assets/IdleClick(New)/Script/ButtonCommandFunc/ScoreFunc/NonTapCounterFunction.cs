using System;
using System.Collections;
using UnityEngine;

namespace IdleClicker
{
    public class NonTapCounterFunction : MonoBehaviour
    {
        private Coroutine idleCoroutine;

        private ShopConfig shopConfig;
        private NonTapCounterConfig nonTapCounterConfig;
        private MainGameInitConfig mainGameInitConfig;
        void OnEnable()
        {

            TapCounterFunction.onTapping += StopCounter;
            TapCounterFunction.startIdleCounter += InitiateCounter;
        }

        void OnDisable()
        {

            TapCounterFunction.onTapping -= StopCounter;
            TapCounterFunction.startIdleCounter -= InitiateCounter;
        }

        public void FetchNonTapCounterFunction(ShopConfig shopCfg, NonTapCounterConfig nonTapCfg, MainGameInitConfig gameCfg, TapCounterConfig tapCfg, string buttonName)
        {
            shopConfig = shopCfg;
            nonTapCounterConfig = nonTapCfg;
            mainGameInitConfig = gameCfg;
            if(nonTapCounterConfig.name == buttonName)
            {
                InitiateCounter();
            }
            

        }

        private IEnumerator NonClickCoinRoutine()
        {
            Debug.Log("Idle routine started");

            while (nonTapCounterConfig.isNonClickEarner)
            {
                int coinsEarned = shopConfig.idleUpgrades[shopConfig.idleUpgradeLevel].idlePerIncrement;

                nonTapCounterConfig.idleScore += coinsEarned;
                nonTapCounterConfig.playerData.TotalBalance += coinsEarned;

                mainGameInitConfig.scoreText.text = nonTapCounterConfig.idleScore.ToString();

                yield return new WaitForSeconds(1f);
            }

            Debug.Log("Idle routine exited");
        }


        public void InitiateCounter()
        {
            if (shopConfig == null || nonTapCounterConfig == null || mainGameInitConfig == null)
            {
                Debug.LogWarning("Configs not set! Make sure FetchNonTapCounterFunction is called before starting.");
                return;
            }

            if (idleCoroutine != null)
            {
                StopCoroutine(idleCoroutine);
            }

            Debug.Log("Idle Counter Started");

            nonTapCounterConfig.idleScore = 0;
            nonTapCounterConfig.isNonClickEarner = true;

            idleCoroutine = StartCoroutine(NonClickCoinRoutine());
        }

        public void StopCounter()
        {
            if (nonTapCounterConfig != null)
            {
                nonTapCounterConfig.isNonClickEarner = false;
                nonTapCounterConfig.idleScore = 0; // Reset
            }

            if (idleCoroutine != null)
            {
                StopCoroutine(idleCoroutine);
                idleCoroutine = null;
            }

            Debug.Log("Idle Counter Stopped");
        }

    }

    [Serializable]
    public class NonTapCounterConfig
    {
        public string name;
        public int idleScore;
        public bool isNonClickEarner = false;
        public MainPlayerData playerData;
    }
}
