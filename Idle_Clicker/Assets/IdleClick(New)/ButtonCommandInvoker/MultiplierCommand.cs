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
          //  Debug.Log("Multiplier Command Excute Success Button Name: " + name);

            var data = multiplierBarManager.name == name ? multiplierBarManager : null;

           // Debug.Log($" multiplierBarManager.name Value : {multiplierBarManager.name}");

          //  Debug.Log($"data Value : {data}");

            if (data != null && multiplierBarFunction != null)
            {
                multiplierBarFunction.StartMultiplier(coinFuncConfig, multiplierBarManager);
            }
            else
            {
                Debug.LogWarning($"Multiplier Command Excute Failed Button Name: {name}");
            }
        }
    }
}
