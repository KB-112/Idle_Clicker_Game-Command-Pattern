using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IdleClicker
{
    public class MainCoinCommand : IButtonCommander
    {
        public MainCoinClickFunction mainCoinClickFunction;
        CoinFuncConfig coinFuncConfig;
        private List<Button> playerButtons;

        public MainCoinCommand(MainCoinClickFunction mainCoinClickFunction, CoinFuncConfig coinFuncConfig, List<Button> playerButtons)
        {
            this.mainCoinClickFunction = mainCoinClickFunction;
            this.coinFuncConfig = coinFuncConfig;
            this.playerButtons = playerButtons;
        }

        public void StoreButtonListenerCommand()
        {
            foreach (var button in playerButtons)
            {
                string currentButtonName = button.name;

                button.onClick.AddListener(() =>
                {
                    Debug.Log("Main button Click");
                    mainCoinClickFunction.MainCoinFunc(coinFuncConfig); 
                });
            }
        }
    }

    [System.Serializable]
    public class CoinFuncConfig
    {
        public string buttonName;
        public Vector2Int[] clickThresholds;
        public int hitCount = 0;
        public int loopCount = 1;
        public int rangeIndex = 0;
    }
}
