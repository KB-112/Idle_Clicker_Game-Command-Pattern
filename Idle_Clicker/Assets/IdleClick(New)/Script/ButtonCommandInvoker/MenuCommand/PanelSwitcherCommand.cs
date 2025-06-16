using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IdleClicker
{
    public class PanelSwitcherCommand : IButtonCommander
    {
        private List<Button> playerButtons;
        PanelSwitcherConfig config;
        PanelSwitcherFunction panelSwitcherFunction;
        private RankConfig rankConfig;

        public PanelSwitcherCommand(PanelSwitcherFunction panelSwitcherFunction, PanelSwitcherConfig config,RankConfig rankConfig, List<Button> playerButtons)
        {
            this.panelSwitcherFunction = panelSwitcherFunction;
            this.config = config;
            this.playerButtons = playerButtons;
            this.rankConfig = rankConfig;

        }
        public void StoreButtonListenerCommand()
        {
            foreach (var button in playerButtons)
            {
                string currentButtonName = button.name;

                button.onClick.AddListener(() =>
                {
                    panelSwitcherFunction.UpdateDescription(config,rankConfig,currentButtonName);
                });
            }
        }
        
    }
}
