using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IdleClicker
{
    [RequireComponent(typeof(BounceEffectFunction))]
    [RequireComponent(typeof(SlidingEffectFunction))]
    [RequireComponent(typeof(MainCoinClickFunction))]
    [RequireComponent(typeof(MultiplierBarFunction))]
    [RequireComponent(typeof(CoinShowerFunction))]
    [RequireComponent(typeof(CharacterAnimationFunction))]
    public class GameController : MonoBehaviour
    {
        private List<IButtonCommander> buttonCommands = new List<IButtonCommander>();

        [Header("Player Buttons")]
        [SerializeField] private List<Button> buttonsAvailableToPlayer;

        [Header("Effect Configs")]
        [SerializeField] private List<BouncyEffectData> bounceEffectConfigs;
        [SerializeField] private List<SlideEffectConfig> slideEffectConfigs;

        [Header("Main Coin Configs")]
        [SerializeField] private CoinFuncConfig coinFuncConfig;
        [SerializeField] private MultiplierBarManager multiplierBarManager;

        [Header("Particle System")]
        [SerializeField] private ParticleSystemConfig particleSystemConfig;

        [Header("Animation Config")]
        [SerializeField] private LuffyAnimationConfig luffyAnimationConfig;

        private BounceEffectFunction bounceEffectFunction;
        private SlidingEffectFunction slidingEffectFunction;
        private MainCoinClickFunction mainCoinClickFunction;
        private MultiplierBarFunction multiplierBarFunction;
        private CoinShowerFunction coinShowerFunction;
        private CharacterAnimationFunction characterAnimationFunction;

        private void Awake()
        {
            bounceEffectFunction = GetComponent<BounceEffectFunction>();
            slidingEffectFunction = GetComponent<SlidingEffectFunction>();
            mainCoinClickFunction = GetComponent<MainCoinClickFunction>();
            multiplierBarFunction = GetComponent<MultiplierBarFunction>();
            coinShowerFunction = GetComponent<CoinShowerFunction>();
            characterAnimationFunction = GetComponent<CharacterAnimationFunction>();
        }


        private void Start()
        {
            InitializeFunctionValue();
            InitializeGameCommandOnClick();
        }


        void InitializeFunctionValue()
        {
            multiplierBarFunction.IntialValueofMutiplier(multiplierBarManager);
            characterAnimationFunction.InitializeLuffyAnimation(luffyAnimationConfig);

        }

        private void InitializeGameCommandOnClick()
        {
            if (buttonsAvailableToPlayer == null || buttonsAvailableToPlayer.Count == 0)
            {
                Debug.LogWarning("No buttons assigned for player interaction.");
                return;
            }

            // Bounce Command
            if (bounceEffectConfigs != null && bounceEffectConfigs.Count > 0)
            {
                var command = new BounceEffectCommand(bounceEffectConfigs, bounceEffectFunction, buttonsAvailableToPlayer);
                command.StoreButtonListenerCommand();
                buttonCommands.Add(command);
            }

            // Slide Command
            if (slideEffectConfigs != null && slideEffectConfigs.Count > 0)
            {
                var command = new SlidingCommand(slidingEffectFunction, slideEffectConfigs, buttonsAvailableToPlayer);
                command.StoreButtonListenerCommand();
                buttonCommands.Add(command);
            }

            // Main Coin Command
            if (coinFuncConfig != null && mainCoinClickFunction != null)
            {
                var command = new MainCoinCommand(mainCoinClickFunction, coinFuncConfig, buttonsAvailableToPlayer);
                command.StoreButtonListenerCommand();
                buttonCommands.Add(command);
            }

            // Multiplier Command
            if (multiplierBarManager != null && multiplierBarFunction != null)
            {
                var command = new MultiplierCommand(coinFuncConfig, multiplierBarManager, multiplierBarFunction, buttonsAvailableToPlayer);
                command.StoreButtonListenerCommand();
                buttonCommands.Add(command);
            }

            // Coin Shower Command
            if (particleSystemConfig != null)
            {
                var command = new CoinShowerCommand(coinShowerFunction, particleSystemConfig, buttonsAvailableToPlayer);
                command.StoreButtonListenerCommand();
                buttonCommands.Add(command);
            }

            // Character Animation Command
            if (luffyAnimationConfig != null && multiplierBarManager != null)
            {
                var command = new CharacterAnimationCommand(characterAnimationFunction, luffyAnimationConfig, buttonsAvailableToPlayer, multiplierBarManager);
                command.StoreButtonListenerCommand();
                buttonCommands.Add(command);
            }
        }
    }
}
