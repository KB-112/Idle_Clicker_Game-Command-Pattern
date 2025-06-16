using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IdleClicker

{
    public class RankCommand : IButtonCommander
    {
         List<Button> playerButtons;
         RankFunction rankFunction;
         RankConfig rankConfig;
        TapCounterConfig tapCounterConfig;
        TapCounterFunction tapCounterFunction;
      

        public RankCommand(RankFunction rankFunction,RankConfig rankConfig, TapCounterConfig tapCounterConfig, TapCounterFunction tapCounterFunction, List<Button> playerButtons )
        {
            this.playerButtons = playerButtons;
            this.rankFunction = rankFunction;
            this.rankConfig = rankConfig;
            this.tapCounterConfig = tapCounterConfig;
            this.tapCounterFunction = tapCounterFunction;

          
          
        }

        public void StoreButtonListenerCommand()
        {
            foreach (var button in playerButtons)
            {
                string currentButtonName = button.name;

                button.onClick.AddListener(() =>
                {
                    rankFunction.FetchRankFunction(rankConfig,tapCounterConfig,tapCounterFunction ,currentButtonName);
                  
                });
               // Debug.Log(" Rank Count " + rankConfig.rankList.rankEntries.Count);
            }
        }
    }
}
