using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IdleClicker
{
    public class NonTapCounterCommand :IButtonCommander
    {
        private NonTapCounterFunction nonTapCounterFunction;
        private ShopConfig shopConfig;
        private NonTapCounterConfig nonTapCounterConfig;
        private MainGameInitConfig mainGameInitConfig;
        private TapCounterConfig tapCounterConfig;
        private List<Button> buttonAvailableToPlayer;


        public NonTapCounterCommand(NonTapCounterFunction nonTapCounterFunction, ShopConfig shopConfig,
                                          NonTapCounterConfig nonTapCounterConfig, MainGameInitConfig mainGameInitConfig, List<Button> buttonAvailableToPlayer)
        {
            this.nonTapCounterFunction = nonTapCounterFunction;
            this.shopConfig = shopConfig;
            this.nonTapCounterConfig = nonTapCounterConfig;
            this.mainGameInitConfig = mainGameInitConfig;
            this.buttonAvailableToPlayer = buttonAvailableToPlayer;
        }

        public void StoreButtonListenerCommand()
        {
            foreach (var button in buttonAvailableToPlayer)
            {
                string currentButtonName = button.name;

                button.onClick.AddListener(() =>
                {
                    nonTapCounterFunction.FetchNonTapCounterFunction(shopConfig, nonTapCounterConfig, mainGameInitConfig, tapCounterConfig,currentButtonName);

                });

            }
            }
        }
    }
