using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace IdleClicker
{
    public class MainGameInitCommand : IButtonCommander
    {
        private MainGameInitFunction mainGameInitFunction;
        private MainGameInitConfig mainGameInitConfig;
        private TapCounterConfig tapCounterConfig;
        private NonTapCounterConfig nonTapCounterConfig;
        public List<Button> buttonAvailableToPlayer;

        public MainGameInitCommand(MainGameInitFunction mainGameInitFunction, MainGameInitConfig mainGameInitConfig, TapCounterConfig tapCounterConfig,
                                               NonTapCounterConfig nonTapCounterConfig, List<Button> buttonAvailableToPlayer)
        {

            this.mainGameInitConfig = mainGameInitConfig;
            this.tapCounterConfig = tapCounterConfig;
            this.nonTapCounterConfig = nonTapCounterConfig;
            this.mainGameInitFunction = mainGameInitFunction;
            this.buttonAvailableToPlayer = buttonAvailableToPlayer;






        }
        public void StoreButtonListenerCommand()
        {
            foreach (var button in buttonAvailableToPlayer)
            {
                string currentButtonName = button.name;

                
                
                button.onClick.AddListener(() =>
                {
                    mainGameInitFunction.FetchMainGameInitFunction(mainGameInitConfig, nonTapCounterConfig, currentButtonName,buttonAvailableToPlayer);
                   
                });


                
            }
        }


    }
}
