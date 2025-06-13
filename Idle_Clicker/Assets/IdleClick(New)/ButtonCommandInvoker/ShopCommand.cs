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
        private List<Button> buttonAvailableToPlayer;
        private int totalBalance;
     

        public ShopCommand(ShopFunction shopFunction, ShopConfig shopConfig, List<Button> buttonAvailableToPlayer , int toatalBalance)
        {
            this.shopFunction = shopFunction;   
            this.shopConfig = shopConfig;
            this.buttonAvailableToPlayer = buttonAvailableToPlayer;
            this.totalBalance = toatalBalance;
        }
        public void StoreButtonListenerCommand()
        {
            foreach (var button in buttonAvailableToPlayer)
            {
                string currentButtonName = button.name;

                button.onClick.AddListener(() =>
                {
                    shopFunction.FetchShopDetails(shopConfig,currentButtonName);
                                  
                });
            }
        }
    }
}
