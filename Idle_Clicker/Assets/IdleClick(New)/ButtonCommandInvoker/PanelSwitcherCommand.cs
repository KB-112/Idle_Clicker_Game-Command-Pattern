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

        public PanelSwitcherCommand(PanelSwitcherFunction panelSwitcherFunction, PanelSwitcherConfig config, List<Button> playerButtons)
        {
            this.panelSwitcherFunction = panelSwitcherFunction;
            this.config = config;
            this.playerButtons = playerButtons;

        }
        public void StoreButtonListenerCommand()
        {
            foreach (var button in playerButtons)
            {
                string currentButtonName = button.name;

                button.onClick.AddListener(() =>
                {
                    panelSwitcherFunction.UpdateDescription(config,currentButtonName);
                });
            }
        }
        
    }
}
