using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace IdleClicker
{
    public class SlidingCommand : IButtonCommander
    {
        private SlidingEffectFunction slidingEffectFunction;
        private List<SlideEffectConfig> slideEffectConfigs;
        private List<Button> playerButtons;
        private HashSet<string> assignedButtons = new(); 

        public SlidingCommand(SlidingEffectFunction slidingEffectFunction, List<SlideEffectConfig> slideEffectConfigs, List<Button> playerButtons)
        {
            this.slidingEffectFunction = slidingEffectFunction;
            this.slideEffectConfigs = slideEffectConfigs;
            this.playerButtons = playerButtons;
        }

        public void StoreButtonListenerCommand()
        {
            foreach (var button in playerButtons)
            {
                string currentButtonName = button.name;

                // Check if already assigned
                if (assignedButtons.Contains(currentButtonName))
                {
                    Debug.Log($"Listener already assigned for: {currentButtonName}");
                   return;
                }

                button.onClick.AddListener(() =>
                {
                    RunButtonCommand(currentButtonName, playerButtons);
                });

                assignedButtons.Add(currentButtonName); // Mark as assigned
            }
        }

        public void RunButtonCommand(string name, List<Button> buttonList)
        {
            Debug.Log("Running bounce effect for: " + name);

            var data = slideEffectConfigs.FirstOrDefault(x => x.name == name);

            if (data != null && slidingEffectFunction != null)
            {
                slidingEffectFunction.SlideEffect(
                    data.name,
                    buttonList,
                    data.targetTransform,
                    data.distanceToCover,
                    data.slidingSpeed,
                    data.snapToEnd,
                    data.isActive,
                    data.panel,
                    data.onComplete
                );
            }
            else
            {
                Debug.LogWarning($"Slide config not found for: {name}");
            }
        }
    }

    [Serializable]
    public class SlideEffectConfig
    {
        public string name;
        public RectTransform targetTransform;
        public float distanceToCover = 100f;
        public float slidingSpeed = 5f;
        public bool snapToEnd = false;
        public bool isActive;
        public GameObject panel;
        public Action onComplete;
    }
}
