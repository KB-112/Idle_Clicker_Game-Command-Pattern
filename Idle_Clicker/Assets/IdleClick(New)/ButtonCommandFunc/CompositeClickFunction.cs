using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace IdleClicker
{
    public class CompositeClickFunction : MonoBehaviour
    {
        [Header("Health Bar Settings")]
        public Image healthBar;
        [SerializeField] private float initialFillAmount = 0f;
        public Vector2[] healthBarFillRange;           
        public Vector2Int[] clickThresholds;             
        public float[] healthBarAnimationSpeeds;
        public float animationCooldownDuration = 2f;

        [Header("Click Tracking")]
        [SerializeField] private int clickCount = 0;
        private float lastClickTime;
        private int currentThresholdIndex = 0;
        private bool isAnimating = false;

        [Header("Multiplier Settings")]
        public TextMeshProUGUI multiplierText;
        private int multiplier = 1;

        [Header("Health Bar Color")]
        public Color[] multiplierColor;

        [Header(" CharacterAnimation")]

        public Animator characterAnimator;


        private void Start()
        {
            healthBar.fillAmount = initialFillAmount;
            SetMultiplier(1);
            StartCoroutine(AnimateHealthBar());
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                HandleClick();
            }

            if (Time.time - lastClickTime > animationCooldownDuration)
            {
                isAnimating = false;
            }
        }

        private void HandleClick()
        {
            clickCount++;
            lastClickTime = Time.time;
            isAnimating = true;

            bool thresholdFound = false;

            for (int i = 0; i < clickThresholds.Length; i++)
            {
                if (clickCount >= clickThresholds[i].x && clickCount <= clickThresholds[i].y)
                {
                    currentThresholdIndex = i;
                   
                    thresholdFound = true;
                    break;
                }
            }

            if (!thresholdFound && clickCount > clickThresholds[^1].y)
            {
               

                multiplier++;
                characterAnimator.SetInteger("Mutiplier", multiplier);

                if (multiplier > 5)
                {
                    ResetMultiplier();
                }
                else
                {
                    SetMultiplier(multiplier);
                }

                clickCount = 0;
                currentThresholdIndex = 0;
            }
        }

        private void SetMultiplier(int newMultiplier)
        {
            multiplier = newMultiplier;

          
            int colorIndex = Mathf.Clamp(multiplier - 1, 0, multiplierColor.Length - 1);
            healthBar.color = multiplierColor[colorIndex];
            multiplierText.text = $"x {multiplier}";
        }


      

        private void ResetMultiplier()
        {
            clickCount = 0;
            currentThresholdIndex = 0;
           
           ResetAnimation();
            SetMultiplier(1);
        }
        private void ResetAnimation()
        {
            characterAnimator.Play("Luffy", 0, 0);
            characterAnimator.SetInteger("Mutiplier", 0);
        }
        private void StopAnimation()
        {
            characterAnimator.StopPlayback();
        }

        private IEnumerator AnimateHealthBar()
        {
            while (true)
            {
                if (isAnimating && clickCount >= clickThresholds[0].x)
                {
                    if (currentThresholdIndex < healthBarFillRange.Length)
                    {
                        var range = healthBarFillRange[currentThresholdIndex];
                        float speed = healthBarAnimationSpeeds[currentThresholdIndex];
                        healthBar.fillAmount = Mathf.PingPong(Time.time * speed, range.y - range.x) + range.x;
                    }
                }
                else
                {
                    healthBar.fillAmount = Mathf.MoveTowards(healthBar.fillAmount, 0f, 0.01f);
                    ResetMultiplier();
                    StopAnimation();
                    
                }

                yield return null;
            }
        }
    }
}
