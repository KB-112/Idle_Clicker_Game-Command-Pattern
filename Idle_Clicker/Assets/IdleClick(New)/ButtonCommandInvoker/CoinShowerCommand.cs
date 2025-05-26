using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IdleClicker
{
    public class CoinShowerCommand : IButtonCommander
    {
        private List<Button> playerButtons;
        private CoinShowerFunction coinShowerFunction;
        private ParticleSystemConfig particleSystemConfig;
        private HashSet<string> assignedButtons = new();
        public CoinShowerCommand(CoinShowerFunction coinShowerFunction, ParticleSystemConfig particleSystemConfig,List<Button> playerButtons)
        {
            this.playerButtons = playerButtons;
            this.coinShowerFunction = coinShowerFunction;
            this.particleSystemConfig = particleSystemConfig;
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
            Debug.Log("Running Coin Shower effect for: " + name);

            var data = particleSystemConfig.name == name ? particleSystemConfig : null;

            if (data != null && coinShowerFunction != null)
            {
                coinShowerFunction.DeployParticleSystem(particleSystemConfig);
            }
            else
            {
                Debug.LogWarning($"Coin Shower effect not found for: {name}");
            }
        }


    }
}
