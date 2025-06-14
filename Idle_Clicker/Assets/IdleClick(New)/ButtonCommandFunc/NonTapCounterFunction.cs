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
        private TapCounterConfig tapCounterConfig;

        public static Action<MainGameInitConfig, TapCounterConfig, NonTapCounterConfig> idleCounterStart;

        void OnEnable()
        {
            MainGameInitFunction.onGameStarted += StopCounter;
            TapCounterFunction.onTapping += StopCounter;
            TapCounterFunction.startIdleCounter += InitiateCounter;
        }

        void OnDisable()
        {
            MainGameInitFunction.onGameStarted -= StopCounter;
            TapCounterFunction.onTapping -= StopCounter;
            TapCounterFunction.startIdleCounter -= InitiateCounter;
        }

        public void FetchNonTapCounterFunction(ShopConfig shopCfg, NonTapCounterConfig nonTapCfg, MainGameInitConfig gameCfg, TapCounterConfig tapCfg)
        {
            shopConfig = shopCfg;
            nonTapCounterConfig = nonTapCfg;
            mainGameInitConfig = gameCfg;
            tapCounterConfig = tapCfg;
        }

        private IEnumerator NonClickCoinRoutine()
        {
            while (nonTapCounterConfig.isNonClickEarner)
            {
                int coinsEarned = shopConfig.idleUpgrades[shopConfig.idleUpgradeLevel].idlePerIncrement;

                nonTapCounterConfig.idleScore += coinsEarned;
                mainGameInitConfig.totalBalance += coinsEarned;

                idleCounterStart?.Invoke(mainGameInitConfig, tapCounterConfig, nonTapCounterConfig);

                mainGameInitConfig.scoreText.text = nonTapCounterConfig.idleScore.ToString();

                yield return new WaitForSeconds(1f);
            }
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
                nonTapCounterConfig.isNonClickEarner = false;

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
        public int idleScore;
        public bool isNonClickEarner = false;
    }
}
