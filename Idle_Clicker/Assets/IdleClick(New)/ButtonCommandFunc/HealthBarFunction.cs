using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace IdleClicker
{
    public class HealthBarFunction : MonoBehaviour
    {
        [SerializeField] private MultiplierBarManager multiplierBarManager;
        [SerializeField] private MainCoinClickFunction mainCoinClickFunction;

        private float lastClickTime;
        private bool isAnimating;

        private void Start()
        {
            multiplierBarManager.multiplierBarText.text = $"x {mainCoinClickFunction.loopCount}";
        }

        private void Update()
        {
            // Trigger animation on click
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log(mainCoinClickFunction.rangeCount);
                lastClickTime = Time.time;
                isAnimating = true;
                MultiplierBar(mainCoinClickFunction.loopCount);
            }

            // Stop animation after cooldown
            if (Time.time - lastClickTime > multiplierBarManager.animationCoolDownDuration)
            {
                isAnimating = false;
               mainCoinClickFunction.ResetValues();
                multiplierBarManager.multiplierBarText.text = $"x {mainCoinClickFunction.loopCount}";
            }

            AnimateHealthBar();
        }

        private void MultiplierBar(int newMultiplier)
        {
            // Safety check
            if (newMultiplier <= 0 || newMultiplier > multiplierBarManager.multiplierBars.Count)
                return;

            multiplierBarManager.multiplierBarImage.color = multiplierBarManager.multiplierBars[newMultiplier - 1].multiplierBarColor;
            multiplierBarManager.multiplierBarText.text = $"x {newMultiplier}";
        }

        private void AnimateHealthBar()
        {
            var mb = multiplierBarManager;
            var mc = mainCoinClickFunction;

            if (isAnimating)
            {
                var range = mb.multiplierBars[mc.rangeCount];
               
                float speed = range.multiplierBarAnimationSpeeds;

                mb.multiplierBarImage.fillAmount = Mathf.PingPong(
                    Time.time * speed,
                    range.multiplierBarFillRange.y - range.multiplierBarFillRange.x
                ) + range.multiplierBarFillRange.x;
            }
            else
            {
                mb.multiplierBarImage.fillAmount = Mathf.MoveTowards(
                    mb.multiplierBarImage.fillAmount,
                    0f,
                    0.01f
                );
            }
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
        public List<MultiplierBarConfig> multiplierBars;

        [Header("Initializer")]
        public float initialFillAmount;
        public float healthBarAnimationDuration = 2f;
        public Image multiplierBarImage;
        public TextMeshProUGUI multiplierBarText;
        public float animationCoolDownDuration;
    }
}
