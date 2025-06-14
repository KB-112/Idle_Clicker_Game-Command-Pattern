using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace IdleClicker
{
    public class TapCounterFunction : MonoBehaviour
    {
        public static Action onTapping;
        public static Action startIdleCounter;
        public static Action<MainGameInitConfig, TapCounterConfig, NonTapCounterConfig> updateScore;

        private float lastTapTime;

        // Cached configs for use in Update
       
        private TapCounterConfig _tapCounterConfig;
        private NonTapCounterConfig _nonTapCounterConfig;
        private ShopConfig shopConfig;

        private bool isCooldownActive = false;  

        public void FetchTapCounterFunction(MainGameInitConfig mainGameInitConfig, TapCounterConfig tapCounterConfig, NonTapCounterConfig nonTapCounterConfig,ShopConfig shopConfig ,string buttonName)
        {
            // Cache configs

            _tapCounterConfig = tapCounterConfig;
            _nonTapCounterConfig = nonTapCounterConfig;
            this.shopConfig = shopConfig;

            float timeSinceLastTap = Time.time - lastTapTime;

            if (timeSinceLastTap < tapCounterConfig.cooldownPeriod)
            {
                CoinIncrOnTapping(mainGameInitConfig, tapCounterConfig, nonTapCounterConfig,shopConfig);
            }

            // Always update tap time to reset cooldown
            lastTapTime = Time.time;
        }

        void Update()
        {
            if (_tapCounterConfig == null || _nonTapCounterConfig == null)
                return;

            float timeSinceLastTap = Time.time - lastTapTime;
            bool cooldownShouldBeActive = timeSinceLastTap < _tapCounterConfig.cooldownPeriod;

            // Only act when cooldown state changes
            if (cooldownShouldBeActive != isCooldownActive)
            {
                isCooldownActive = cooldownShouldBeActive;

                if (isCooldownActive)
                {
                    Debug.Log("Cooldown started.");
                    _tapCounterConfig.tapScore = 0;
                    _nonTapCounterConfig.isNonClickEarner = false;
                }
                else
                {
                    Debug.Log("Cooldown ended.");
                    _nonTapCounterConfig.isNonClickEarner = true;
                    startIdleCounter?.Invoke();
                }

             
            }
        }

        void CoinIncrOnTapping(MainGameInitConfig mainGameInitConfig, TapCounterConfig tapCounterConfig, NonTapCounterConfig nonTapCounterConfig, ShopConfig shopConfig)
        {
            onTapping?.Invoke();

            int coinsEarned = shopConfig.tapUpgrades[shopConfig.tapUpgradeLevel].tapPerIncrement;

            tapCounterConfig.tapScore += coinsEarned;
            tapCounterConfig.currentScore = tapCounterConfig.tapScore;

            mainGameInitConfig.totalBalance += coinsEarned;

            updateScore?.Invoke(mainGameInitConfig, tapCounterConfig, nonTapCounterConfig);
            mainGameInitConfig.scoreText.text = tapCounterConfig.tapScore.ToString();
        }

    }




    [System.Serializable]
    public class TapCounterConfig
    {
        public string buttonName;
        public int tapScore;
        public int currentScore;
        [Tooltip("Cooldown period in seconds between taps.")]
        public float cooldownPeriod = 1.0f;
    }
}
