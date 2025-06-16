using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace IdleClicker
{
    public class BounceEffectCommand : IButtonCommander 
    {
        private BounceEffectFunction bounceEffect;
        private List<BouncyEffectData> bounceEffectConfigs;
        private List<Button> playerButtons;

       
        public BounceEffectCommand(List<BouncyEffectData> bounceEffectConfigs, BounceEffectFunction bounceEffect, List<Button> playerButtons)
        {
            this.bounceEffectConfigs = bounceEffectConfigs;
            this.bounceEffect = bounceEffect;
            this.playerButtons = playerButtons;
        }

        public void StoreButtonListenerCommand()
        {
            foreach (var button in playerButtons)
            {
                string currentButtonName = button.name;

                button.onClick.AddListener(() =>
                {
                    RunButtonCommand(currentButtonName, playerButtons);
                });
            }
        }

        public void RunButtonCommand(string name, List<Button> buttonList)
        {
          //  Debug.Log("Running bounce effect for: " + name);

            var data = bounceEffectConfigs.FirstOrDefault(x => x.name == name);

            if (data != null && bounceEffect != null)
            {
                bounceEffect.BouncyEffect(
                    data.name,
                    buttonList,
                    data.targetTransform,
                    data.isAnimating,
                    data.originalScale,
                    data.newScale,
                    data.scaleDuration,
                    data.onComplete
                );
            }
            else
            {
                Debug.LogWarning($"Bounce config not found for: {name}");
            }
        }
    }
    [Serializable]
    public class BouncyEffectData
    {
        public string name;
        public RectTransform targetTransform;
        public bool isAnimating;
        public Vector3 originalScale;
        public Vector3 newScale;
        public float scaleDuration;
        public Action onComplete;
    }
}

