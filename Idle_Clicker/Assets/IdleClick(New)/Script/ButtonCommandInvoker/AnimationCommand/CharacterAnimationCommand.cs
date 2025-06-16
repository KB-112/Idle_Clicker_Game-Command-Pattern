using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

namespace IdleClicker
{
    public class CharacterAnimationCommand : IButtonCommander
    {
        private List<Button> playerButtons;
        private CharacterAnimationFunction characterAnimationFunction;
        private LuffyAnimationConfig luffyAnimationConfig;
        private HashSet<string> assignedButtons = new();
        private MultiplierBarManager multiplierBarManager;



        public CharacterAnimationCommand(CharacterAnimationFunction characterAnimationFunction, LuffyAnimationConfig luffyAnimationConfig, List<Button> playerButtons, MultiplierBarManager multiplierBarManager    )
        {
            this.playerButtons = playerButtons;
            this.characterAnimationFunction = characterAnimationFunction;
            this.luffyAnimationConfig = luffyAnimationConfig;
            this.multiplierBarManager = multiplierBarManager;
        }


        public void StoreButtonListenerCommand()
        {
            foreach (var button in playerButtons)
            {
                string currentButtonName = button.name;

                button.onClick.AddListener(() =>
                {

                    characterAnimationFunction.TriggerLuffyAnimation(multiplierBarManager, currentButtonName);
                    Debug.Log("Luffy Animation call");

                });

            }
        }
    }

       


    


}
