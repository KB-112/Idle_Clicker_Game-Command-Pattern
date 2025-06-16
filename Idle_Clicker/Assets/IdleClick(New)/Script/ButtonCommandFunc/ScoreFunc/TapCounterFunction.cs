using System;
using System.Collections;
using UnityEngine;

namespace IdleClicker
{
    public class TapCounterFunction : MonoBehaviour
    {
        public static Action onTapping;
        public static Action startIdleCounter;

        private Coroutine cooldownRoutine;
        private bool isCooldownActive;

        private TapCounterConfig _tapCounterConfig;
        private NonTapCounterConfig _nonTapCounterConfig;
        private ShopConfig _shopConfig;
        private MainGameInitConfig _mainGameInitConfig;

        private void OnEnable()
        {
            Vibration.Init();
            MainGameInitFunction.onGameStarted += ResetTapCounter;
        }

        private void OnDisable()
        {
            MainGameInitFunction.onGameStarted -= ResetTapCounter;
        }

        public void FetchTapCounterFunction(MainGameInitConfig initCfg, TapCounterConfig tapCfg, NonTapCounterConfig idleCfg, ShopConfig shopCfg, string buttonName)
        {
            _mainGameInitConfig = initCfg;
            _tapCounterConfig = tapCfg;
            _nonTapCounterConfig = idleCfg;
            _shopConfig = shopCfg;

            if (_tapCounterConfig.buttonName == buttonName)
            {
                HandleTap(); 
                StartCooldownCheck();
            }
        }

        private void HandleTap()
        {
            if (_nonTapCounterConfig.isNonClickEarner)
            {
                Debug.Log("Switching from idle to tap. Resetting scores.");
                _nonTapCounterConfig.isNonClickEarner = false;
                _nonTapCounterConfig.idleScore = 0;

                _tapCounterConfig.tapScore = 0;
                _tapCounterConfig.currentScore = 0;
            }

            onTapping?.Invoke();

            int coinsEarned = _shopConfig.tapUpgrades[_shopConfig.tapUpgradeLevel].tapPerIncrement;

            _tapCounterConfig.tapScore += coinsEarned;
            _tapCounterConfig.currentScore = _tapCounterConfig.tapScore;
            _tapCounterConfig.mainPlayerData.TotalBalance += coinsEarned;
            Vibration.VibratePop();
            _mainGameInitConfig.scoreText.text = _tapCounterConfig.tapScore.ToString();
        }

        private void StartCooldownCheck()
        {
            if (cooldownRoutine != null)
                StopCoroutine(cooldownRoutine);

            cooldownRoutine = StartCoroutine(CooldownCheck());
        }

        private IEnumerator CooldownCheck()
        {
            isCooldownActive = true;
            _nonTapCounterConfig.isNonClickEarner = false;

            yield return new WaitForSeconds(_tapCounterConfig.cooldownPeriod);

            isCooldownActive = false;
            _nonTapCounterConfig.isNonClickEarner = true;

            HighScore(); 
            UpdateScoreToAPI();

            Debug.Log("Invoking startIdleCounter event");
            startIdleCounter?.Invoke();
        }

        private void UpdateScoreToAPI()
        {
            if (_tapCounterConfig.mainPlayerData == null || _tapCounterConfig.apiExecutor == null)
                return;

            string jsonData = JsonUtility.ToJson(_tapCounterConfig.mainPlayerData);

            Debug.Log($"Sending updated score to API: {_tapCounterConfig.mainPlayerData.Score}");

            _tapCounterConfig.apiExecutor.ExecuteCommand(
                new PutCommand(),
                $"https://6824498265ba05803399a0a2.mockapi.io/api/v1/User_Name/{_tapCounterConfig.mainPlayerData.id}",
                jsonData
            );
        }

        private void ResetTapCounter()
        {
            if(_tapCounterConfig !=null)
            {
                Debug.Log("ResetTapCounter called");

                _tapCounterConfig.tapScore = 0;
                _tapCounterConfig.currentScore = 0;

                _nonTapCounterConfig.isNonClickEarner = false;
                _nonTapCounterConfig.idleScore = 0;

                isCooldownActive = false;
            }
           
        }

        public void HighScore()
        {
            if (_tapCounterConfig != null)
            {


                if (_tapCounterConfig.currentScore > _tapCounterConfig.mainPlayerData.Score)
                {
                    Debug.Log($"New High Score: {_tapCounterConfig.currentScore}");
                    _tapCounterConfig.mainPlayerData.Score = _tapCounterConfig.currentScore;
                }
                else
                {
                    Debug.Log($"No new high score. Current: {_tapCounterConfig.currentScore}, High: {_tapCounterConfig.mainPlayerData.Score}");
                }
            }
        }
    }

    [Serializable]
    public class TapCounterConfig
    {
        public string buttonName;
        public int tapScore;
        public int currentScore;

        [Tooltip("Cooldown period in seconds between taps.")]
        public float cooldownPeriod = 1.0f;

        public ApiCommandExecutor apiExecutor;
        public MainPlayerData mainPlayerData;
    }
}
