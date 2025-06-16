using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IdleClicker
{
    public class ShopCommand : IButtonCommander
    {
        private ShopFunction shopFunction;
        private ShopConfig shopConfig;
        private TapCounterFunction tapCounterFunction;
        private MainGameInitConfig mainGameInitConfig;
        private List<Button> buttonAvailableToPlayer;
      
     

        public ShopCommand(ShopFunction shopFunction, ShopConfig shopConfig, List<Button> buttonAvailableToPlayer, TapCounterFunction tapCounterFunction,MainGameInitConfig mainGameInitConfig )
        {
            this.shopFunction = shopFunction;   
            this.shopConfig = shopConfig;
            this.buttonAvailableToPlayer = buttonAvailableToPlayer;
          
            this.tapCounterFunction = tapCounterFunction;
            this.mainGameInitConfig = mainGameInitConfig;
        }
        public void StoreButtonListenerCommand()
        {
            foreach (var button in buttonAvailableToPlayer)
            {
                string currentButtonName = button.name;

                button.onClick.AddListener(() =>
                {
                    shopFunction.FetchShopDetails(shopConfig,tapCounterFunction, currentButtonName);
                                  
                });
            }
        }
    }
}
