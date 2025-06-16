using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace IdleClicker
{
    public class ContBounceEffectCommand : IButtonCommander
    {

        private BounceEffectFunction bounceEffect;
        private List<BouncyEffectData> bounceEffectConfigs;
        private List<GameObject> nonClickableButton;

        public ContBounceEffectCommand(List<BouncyEffectData> bounceEffectConfigs, BounceEffectFunction bounceEffect, List<GameObject> nonClickableButton)
        {
            this.bounceEffectConfigs = bounceEffectConfigs;
            this.bounceEffect = bounceEffect;
            this.nonClickableButton = nonClickableButton;
        }
        public void StoreButtonListenerCommand()
        {
            foreach (var button in nonClickableButton)
            {
                string currentButtonName = button.name;
                RunButtonCommand(currentButtonName, nonClickableButton);
               
            }
        }

        public void RunButtonCommand(string name, List<GameObject> buttonList)
        {
         //   Debug.Log("Cont bounce effect for: " + name);

            var data = bounceEffectConfigs.FirstOrDefault(x => x.name == name);

            if (data != null && bounceEffect != null)
            {
                bounceEffect.ContiniousBounceEffect(
                    data.name,
                    buttonList,
                    data.targetTransform,                    
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
}
