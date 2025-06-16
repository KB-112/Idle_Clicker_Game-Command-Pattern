using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;
namespace IdleClicker
{
    public class MainGameInitFunction : MonoBehaviour
    {
      
        public static Action onGameStarted;
       

        
        public void FetchMainGameInitFunction(MainGameInitConfig config,
                                               NonTapCounterConfig nonTapCounterConfig, string buttonAvailableToPlayer , List<Button> newButton)

        {
            
            if (config.buttonName == buttonAvailableToPlayer )
            {

                Debug.Log("Tap to play button Pressed");
                
                    config.tapToPlay.SetActive(false);
                   for( int i = 0; i < newButton.Count; i++)
                    {
                         if(newButton[i].name == config.buttonName1)
                        {
                            newButton[i].interactable = true;
                        
                        }
                    }
                       
                    config.scoreText.text = 0.ToString();
               
                    onGameStarted?.Invoke();
                
               
               

                
            }
        }
    }


    [System.Serializable]
    public class MainGameInitConfig
    {
        public string buttonName;
        public string buttonName1;
        public int tapScore;
        public int idleScore;    
        public MainPlayerData playerData;
        public GameObject tapToPlay;
        public TextMeshProUGUI scoreText;


    }
}
