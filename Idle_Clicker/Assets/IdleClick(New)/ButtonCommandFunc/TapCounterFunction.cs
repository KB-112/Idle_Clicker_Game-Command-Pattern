using System;
using System.Collections;
using UnityEngine;

namespace IdleClicker
{
    public class TapCounterFunction : MonoBehaviour
    {
        public static Action onTapping;
        public static Action startIdleCounter;
        public static Action<MainGameInitConfig, TapCounterConfig, NonTapCounterConfig> updateScore;

        private float lastTapTime;
        private bool isCooldownActive = false;
        private Coroutine cooldownRoutine;

        [SerializeField] private UserData _userData;

        private TapCounterConfig _tapCounterConfig;
        private NonTapCounterConfig _nonTapCounterConfig;
        private ShopConfig shopConfig;

        void OnEnable()
        {
            MainGameInitFunction.onGameStarted += ResetTapCounter;
        }

        void OnDisable()
        {
            MainGameInitFunction.onGameStarted -= ResetTapCounter;
        }

        public void UserDataStore(UserData userData)
        {
            _userData = userData;
        }

        public void FetchTapCounterFunction(MainGameInitConfig mainGameInitConfig, TapCounterConfig tapCounterConfig, NonTapCounterConfig nonTapCounterConfig, ShopConfig shopConfig, string buttonName)
        {
            this._tapCounterConfig = tapCounterConfig;
            this._nonTapCounterConfig = nonTapCounterConfig;
            this.shopConfig = shopConfig;

            float timeSinceLastTap = Time.time - lastTapTime;

            if (timeSinceLastTap >= _tapCounterConfig.cooldownPeriod)
            {
                CoinIncrOnTapping(mainGameInitConfig);
                StartCooldownCheck();
            }

            lastTapTime = Time.time;
        }

        private void CoinIncrOnTapping(MainGameInitConfig mainGameInitConfig)
        {
            onTapping?.Invoke();

            int coinsEarned = shopConfig.tapUpgrades[shopConfig.tapUpgradeLevel].tapPerIncrement;

            _tapCounterConfig.tapScore += coinsEarned;
            _tapCounterConfig.currentScore = _tapCounterConfig.tapScore;
            mainGameInitConfig.totalBalance += coinsEarned;

            mainGameInitConfig.scoreText.text = _tapCounterConfig.tapScore.ToString();

            updateScore?.Invoke(mainGameInitConfig, _tapCounterConfig, _nonTapCounterConfig);
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

            UpdateScoreToAPI();
            startIdleCounter?.Invoke();
        }

        private void UpdateScoreToAPI()
        {
            if (_userData == null || _tapCounterConfig.apiExecutor == null)
                return;

            string jsonData = JsonUtility.ToJson(_userData);
            _tapCounterConfig.apiExecutor.ExecuteCommand(
                new PutCommand(),
                $"https://6824498265ba05803399a0a2.mockapi.io/api/v1/User_Name/{_userData.id}",
                jsonData
            );
        }

        private void ResetTapCounter()
        {
            if (_tapCounterConfig != null)
            {
                _tapCounterConfig.tapScore = 0;
                _tapCounterConfig.currentScore = 0;
            }

            if (_nonTapCounterConfig != null)
                _nonTapCounterConfig.isNonClickEarner = false;

            isCooldownActive = false;
            lastTapTime = Time.time;
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
    }
}
