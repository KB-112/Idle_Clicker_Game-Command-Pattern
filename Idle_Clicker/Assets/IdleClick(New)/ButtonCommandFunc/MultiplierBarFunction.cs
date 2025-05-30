using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace IdleClicker
{
    public class MultiplierBarFunction : MonoBehaviour
    {
        private float lastClickTime;
        private bool isAnimating;

        private CoinFuncConfig coinFuncConfig;
        private MultiplierBarManager multiplierBarManager;

        public delegate void ResetValueDelegate();

       
        public static event ResetValueDelegate OnResetValue;
        public static void TriggerResetValue()
        {
            Debug.Log("GameEvents: TriggerResetValue called");
            OnResetValue?.Invoke(); 
        }
        public void StartMultiplier(CoinFuncConfig config, MultiplierBarManager manager)
        {
            this.coinFuncConfig = config;
            this.multiplierBarManager = manager;
            Initiate();
        }

        public void IntialValueofMutiplier(MultiplierBarManager manager)
        {
            isAnimating = false;
          manager.multiplierBarImage.fillAmount = 0f;
          manager.multiplierBarImage.color = Color.white;
          

        }
        void Initiate()
        {
            lastClickTime = Time.time;
            isAnimating = true;

        }
            

      
        private void Update()
        {
            if (multiplierBarManager == null && coinFuncConfig == null)
            {
              //  Debug.Log("Multiplier Null Reference ");
                return;
            }
            else
            {
                MultiplierFunc();

            }

                
        }

        private void MultiplierFunc()
        {
           
                multiplierBarManager.multiplierBarText.text = $"x {coinFuncConfig.loopCount}";
            
          
            UpdateMultiplierBarColor(coinFuncConfig.loopCount, multiplierBarManager);
            UpdateMultiplierCoolDown();
            UpdateMultiplierBarAnimation();
        }

        private void UpdateMultiplierCoolDown()
        {
            if (!isAnimating)
                return;

            if (Time.time - lastClickTime > multiplierBarManager.animationCoolDownDuration)
            {
                isAnimating = false;
                ResetUserProgress(coinFuncConfig);
                TriggerResetValue();
            }
        }

        private void UpdateMultiplierBarAnimation()
        {
            var range = multiplierBarManager.multiplierBars[coinFuncConfig.rangeIndex];

            if (isAnimating)
            {
                float speed = range.multiplierBarAnimationSpeeds;
                multiplierBarManager.multiplierBarImage.fillAmount = Mathf.PingPong(
                    Time.time * speed,
                    range.multiplierBarFillRange.y - range.multiplierBarFillRange.x
                ) + range.multiplierBarFillRange.x;
            }
            else
            {
                multiplierBarManager.multiplierBarImage.fillAmount = Mathf.MoveTowards(
                    multiplierBarManager.multiplierBarImage.fillAmount,
                    0f,
                    0.01f
                );
            }
        }

        private void UpdateMultiplierBarColor(int newMultiplier, MultiplierBarManager manager)
        {
            if (newMultiplier <= 0 || newMultiplier > manager.multiplierBars.Count)
                return;

            manager.multiplierBarImage.color = manager.multiplierBars[newMultiplier - 1].multiplierBarColor;
        }

        public void ResetUserProgress(CoinFuncConfig coinFuncConfig)
        {
            coinFuncConfig.hitCount = 0;
            coinFuncConfig.rangeIndex = 0;
            coinFuncConfig.loopCount = 1;
        }
    }


    [System.Serializable]
    public struct MultiplierBarConfig
    {
        public Vector2 multiplierBarFillRange;
        public float multiplierBarAnimationSpeeds;
        public Color multiplierBarColor;
    }

    [System.Serializable]
    public class MultiplierBarManager
    {
        public string name;
        public List<MultiplierBarConfig> multiplierBars;

        [Header("Initializer")]
        public float initialFillAmount;
        public float healthBarAnimationDuration = 2f;
        public Image multiplierBarImage;
        public TextMeshProUGUI multiplierBarText;
        public float animationCoolDownDuration;
    }
}
