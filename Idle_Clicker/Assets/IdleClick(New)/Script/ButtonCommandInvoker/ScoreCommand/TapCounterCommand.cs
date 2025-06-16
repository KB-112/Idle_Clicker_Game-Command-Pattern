using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace IdleClicker
{
    public class TapCounterCommand :IButtonCommander
    {
        private MainGameInitConfig mainGameInitConfig;
        private TapCounterConfig tapCounterConfig;
        private NonTapCounterConfig nonTapCounterConfig;
        private List<Button> buttonAvailableToPlayer;
        private TapCounterFunction tapCounterFunction;
        private ShopConfig shopConfig;
        public TapCounterCommand(MainGameInitConfig mainGameInitConfig, TapCounterFunction tapCounterFunction, TapCounterConfig tapCounterConfig, List<Button> buttonAvailableToPlayer, NonTapCounterConfig nonTapCounterConfig,ShopConfig shopConfig) 
        { 
            this.tapCounterFunction = tapCounterFunction;
            this.tapCounterConfig = tapCounterConfig;
            this.mainGameInitConfig = mainGameInitConfig;
            this.nonTapCounterConfig = nonTapCounterConfig;
            this.buttonAvailableToPlayer = buttonAvailableToPlayer;
            this.shopConfig = shopConfig;
        
        
        }



        public void StoreButtonListenerCommand()
        {
            foreach (var button in buttonAvailableToPlayer)
            {
                string currentButtonName = button.name;

                button.onClick.AddListener(() =>
                {
                    
                    tapCounterFunction.FetchTapCounterFunction(mainGameInitConfig,tapCounterConfig,nonTapCounterConfig,shopConfig,currentButtonName);
                });
            }
        }
    }
}
