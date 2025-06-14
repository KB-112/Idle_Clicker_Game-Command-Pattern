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
       


        public NonTapCounterCommand(NonTapCounterFunction nonTapCounterFunction, ShopConfig shopConfig,
                                          NonTapCounterConfig nonTapCounterConfig, MainGameInitConfig mainGameInitConfig)
        {
            this.nonTapCounterFunction = nonTapCounterFunction;
            this.shopConfig = shopConfig;
            this.nonTapCounterConfig = nonTapCounterConfig;
            this.mainGameInitConfig = mainGameInitConfig;

        }

        public void StoreButtonListenerCommand()
        {
            nonTapCounterFunction.FetchNonTapCounterFunction(shopConfig, nonTapCounterConfig, mainGameInitConfig,tapCounterConfig);
            
        }
    }
}
