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

                // Check if already assigned
                if (assignedButtons.Contains(currentButtonName))
                {
                    Debug.Log($"Listener already assigned for: {currentButtonName}");
                    return;
                }

                button.onClick.AddListener(() =>
                {
                    RunButtonCommand(currentButtonName, playerButtons);
                });

                assignedButtons.Add(currentButtonName); // Mark as assigned
            }
        }

        public void RunButtonCommand(string name, List<Button> buttonList)
        {
            Debug.Log("Character Animation effect for: " + name);

            var data = luffyAnimationConfig.name == name ? luffyAnimationConfig.name : null;

            if (data != null && characterAnimationFunction!= null)
            {
                characterAnimationFunction.TriggerLuffyAnimation(multiplierBarManager);
            }
            else
            {
                Debug.LogWarning($"Character Animation effect  not found for: {name}");
            }
        }


    }


}
