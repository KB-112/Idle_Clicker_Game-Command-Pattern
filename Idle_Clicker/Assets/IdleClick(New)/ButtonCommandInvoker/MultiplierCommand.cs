using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IdleClicker
{
    public class MultiplierCommand : IButtonCommander
    {
        private MultiplierBarFunction multiplierBarFunction;
        private MultiplierBarManager multiplierBarManager;
        private CoinFuncConfig coinFuncConfig;
        private List<Button> playerButtons;
        private HashSet<string> assignedButtons = new();

        public MultiplierCommand(
            CoinFuncConfig coinFuncConfig,
            MultiplierBarManager multiplierBarManager,
            MultiplierBarFunction multiplierBarFunction,
            List<Button> playerButtons)
        {
            this.coinFuncConfig = coinFuncConfig;
            this.multiplierBarManager = multiplierBarManager;
            this.multiplierBarFunction = multiplierBarFunction;
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
            Debug.Log("Running Coin Shower effect for: " + name);

            var data = multiplierBarFunction.name == name ? multiplierBarManager : null;

            if (data != null && multiplierBarFunction != null)
            {
                multiplierBarFunction.StartMultiplier(coinFuncConfig, multiplierBarManager);
            }
            else
            {
                Debug.LogWarning($"Coin Shower effect not found for: {name}");
            }
        }
    }
}
