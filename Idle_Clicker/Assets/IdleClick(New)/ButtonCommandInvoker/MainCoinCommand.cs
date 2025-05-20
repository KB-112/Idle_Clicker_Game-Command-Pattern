using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdleClicker
{
    public class MainCoinCommand : IButtonCommander
    {
        public MainCoinClickFunction mainCoinClickFunction;


        public MainCoinCommand(MainCoinClickFunction mainCoinClickFunction)
        {
            this.mainCoinClickFunction = mainCoinClickFunction;
        }
        public void StoreButtonListenerCommand()
        {


        }


    }
}
