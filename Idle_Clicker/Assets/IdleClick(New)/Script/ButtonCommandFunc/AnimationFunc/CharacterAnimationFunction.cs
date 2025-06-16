using System.Collections;
using UnityEngine;

namespace IdleClicker
{
    public class CharacterAnimationFunction : MonoBehaviour
    {
        private LuffyAnimationConfig luffyAnimationConfig;

        private void OnEnable()
        {
            MultiplierBarFunction.OnResetValue += StopLuffyAnimation;
        }

        private void OnDisable()
        {
            MultiplierBarFunction.OnResetValue -= StopLuffyAnimation;
        }

        public void InitializeLuffyAnimation(LuffyAnimationConfig config)
        {
            if (config == null || config.luffyGameObj == null) return;

            config.luffyAnimator = config.luffyGameObj.GetComponent<Animator>();
            luffyAnimationConfig = config;

            if (luffyAnimationConfig.luffyAnimator != null)
                luffyAnimationConfig.luffyAnimator.SetBool("Luffy_Anim", false);
        }

        public void TriggerLuffyAnimation(MultiplierBarManager multiplierBarManager, string buttonName)
        {
            if (multiplierBarManager == null) return;
            if(multiplierBarManager.name ==buttonName)
            {
                UpdateLuffyAnimation(multiplierBarManager);
            }
           

        }

        private void UpdateLuffyAnimation(MultiplierBarManager multiplierBarManager)
        {
            if (luffyAnimationConfig?.luffyAnimator == null) return;

            string rawMultiplier = multiplierBarManager.multiplierBarText.text.Trim().Replace("x", "");

            if (int.TryParse(rawMultiplier, out int multiplier) && multiplier > 0)
            {
                luffyAnimationConfig.luffyAnimator.SetBool("Luffy_Anim", true);
                luffyAnimationConfig.luffyAnimator.SetInteger("Multiplier", multiplier - 1);
            }
        }

        private void StopLuffyAnimation()
        {
            if (luffyAnimationConfig?.luffyAnimator != null)
                luffyAnimationConfig.luffyAnimator.SetBool("Luffy_Anim", false);
        }
    }

    [System.Serializable]
    public class LuffyAnimationConfig
    {
        public string name;
        public GameObject luffyGameObj;
        public Animator luffyAnimator;
    }
}
