using DG.Tweening.Core.Easing;
using IdleClicker.UITween;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IdleClicker
{
    public class UIAnimationController : MonoBehaviour, IButtonCommand
    {
        [Header("Animation Data")]
        [SerializeField] private UIAnimation uiAnimation;


        [Header("Buttons to Animate")]
        [SerializeField] private List<Button> buttonsToAnimate = new List<Button>();



        [Header("Panel and Animation Targets")]
        [SerializeField] private GameObject panel;
        [SerializeField] private List<RectTransform> panelElements = new List<RectTransform>();

        [Header("Particle System")]
        [SerializeField] private ParticleSystem ps;

        private CoinClickMechanism coinClickMechanism;

        private void Awake()
        {
            coinClickMechanism = new CoinClickMechanism(ps);

        }

        public void Execute(string name)
        {
            coinClickMechanism.Execute(name);

            // Find the button with matching name in buttonsToAnimate list
            Button btn = buttonsToAnimate.Find(button => button.name == name);

            if (btn != null)
            {
                HandleButtonClick(name, btn);
            }
            else
            {
                Debug.LogWarning($"[UIAnimationController] Button with name {name} not found.");
            }
        }

        private void HandleButtonClick(string buttonName, Button btn)
        {
            uiAnimation.ApplyEffect(buttonName, btn, null);


            Debug.Log($"[UIAnimationController] Button Clicked: {buttonName}");
        }

    }
}
