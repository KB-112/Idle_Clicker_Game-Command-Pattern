using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IdleClicker
{
    [RequireComponent(typeof(BounceEffectFunction))]
    [RequireComponent(typeof(SlidingEffectFunction))]
    public class GameController : MonoBehaviour
    {
        private IButtonCommander buttonCommand;

        [Header("Player Buttons")]
        [SerializeField] private List<Button> buttonsAvailableToPlayer;

        [Header("Effect Configs")]
        [SerializeField] private List<BouncyEffectData> bounceEffectConfigs;
        [SerializeField] private List<SlideEffectConfig> slideEffectConfigs;

        private BounceEffectFunction bounceEffectFunction;
        private SlidingEffectFunction slidingEffectFunction;

        private void Awake()
        {
            bounceEffectFunction = GetComponent<BounceEffectFunction>();
            slidingEffectFunction = GetComponent<SlidingEffectFunction>();
        }

        private void Start()
        {
            InitializeGame();
        }

        private void InitializeGame()
        {
            if (buttonsAvailableToPlayer == null || buttonsAvailableToPlayer.Count == 0)
            {
                Debug.LogWarning("No buttons assigned for player interaction.");
                return;
            }

            if (bounceEffectConfigs != null && bounceEffectConfigs.Count > 0)
            {
                InitializeCommander(bounceEffectConfigs, bounceEffectFunction);
            }
            if (slideEffectConfigs != null && slideEffectConfigs.Count > 0)
            {
                InitializeCommander(slideEffectConfigs, slidingEffectFunction);
            }
            else
            {
                Debug.LogWarning("No valid config assigned.");
            }
        }

        private void InitializeCommander<TConfig, TFunction>(List<TConfig> configList, TFunction functionHandler)
        {
            if (typeof(TFunction) == typeof(BounceEffectFunction) && typeof(TConfig) == typeof(BouncyEffectData))
            {
                buttonCommand = new BounceEffectCommand(
                    configList as List<BouncyEffectData>,
                    functionHandler as BounceEffectFunction,
                    buttonsAvailableToPlayer
                );
            }
            else if (typeof(TFunction) == typeof(SlidingEffectFunction) && typeof(TConfig) == typeof(SlideEffectConfig))
            {
                buttonCommand = new SlidingCommand(
                    functionHandler as SlidingEffectFunction,
                    configList as List<SlideEffectConfig>,
                    buttonsAvailableToPlayer
                );
            }
            else
            {
                Debug.LogError("Unsupported command type combination.");
                return;
            }

            buttonCommand.StoreButtonListenerCommand();
        }
    }
}
