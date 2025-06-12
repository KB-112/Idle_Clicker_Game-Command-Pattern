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

        public RankCommand(RankFunction rankFunction,RankConfig rankConfig, List<Button> playerButtons)
        {
            this.playerButtons = playerButtons;
            this.rankFunction = rankFunction;
            this.rankConfig = rankConfig;
        }

        public void StoreButtonListenerCommand()
        {
            foreach (var button in playerButtons)
            {
                string currentButtonName = button.name;

                button.onClick.AddListener(() =>
                {
                    rankFunction.FetchRankFunction(rankConfig,currentButtonName);
                });
            }
        }
    }
}
