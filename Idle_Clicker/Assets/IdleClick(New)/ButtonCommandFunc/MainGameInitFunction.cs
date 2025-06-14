using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
namespace IdleClicker
{
    public class MainGameInitFunction : MonoBehaviour
    {
      
        public static Action onGameStarted;
       

        
      
        public void FetchMainGameInitFunction(MainGameInitConfig config,
                                               NonTapCounterConfig nonTapCounterConfig, string buttonAvailableToPlayer)

        {
            
            if (config.buttonName[0] == buttonAvailableToPlayer)
            {


                if (config.tapToPlay)
                {
                    config.tapToPlay.SetActive(false);
                    config.scoreText.text = 0.ToString();
                    onGameStarted?.Invoke();
                }
               
                else
                {
                    Debug.LogWarning("Tap to playbutton is not  Active in hierarchy");

                }

                
            }
        }
    }


    [System.Serializable]
    public class MainGameInitConfig
    {
        public List<string> buttonName;
        public int tapScore;
        public int idleScore;
        public int totalBalance;

        public GameObject tapToPlay;
        public TextMeshProUGUI scoreText;


    }
}
