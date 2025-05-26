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
                button.onClick.AddListener(() =>
                {
                    Debug.Log("Multiplier Button Clicked");
                    multiplierBarFunction.StartMultiplier(coinFuncConfig, multiplierBarManager);
                });
            }
        }
    }

}
