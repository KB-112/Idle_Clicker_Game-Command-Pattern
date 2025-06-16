using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IdleClicker
{
    [RequireComponent(typeof(BounceEffectFunction))]  //1
    [RequireComponent(typeof(SlidingEffectFunction))] //2
    [RequireComponent(typeof(MainCoinClickFunction))] //3
    [RequireComponent(typeof(MultiplierBarFunction))] //4
    [RequireComponent(typeof(CoinShowerFunction))]   //5
    [RequireComponent(typeof(CharacterAnimationFunction))] //6
    [RequireComponent(typeof(StatHolderFunction))] //7
    [RequireComponent(typeof(PanelSwitcherFunction))] //8
    [RequireComponent(typeof(RankFunction))] //9
    [RequireComponent(typeof(ShopFunction))] //10
    public class GameController : MonoBehaviour
    {
        private List<IButtonCommander> buttonCommands = new List<IButtonCommander>();

        [Header("Player Buttons")]
        [SerializeField] private List<Button> buttonsAvailableToPlayer;
        [SerializeField] private List<GameObject> nonClickableButton;

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

        [SerializeField]private StatInputConfig statInputConfig;
        [SerializeField] private PanelSwitcherConfig panelSwitcherConfig;
        [SerializeField] private RankConfig rankConfig;
        [SerializeField] private ShopConfig shopConfig;

        [SerializeField] private MainGameInitConfig mainGameInitConfig;
        [SerializeField] private TapCounterConfig tapCounterConfig;
        [SerializeField] private NonTapCounterConfig nonTapCounterConfig;
        
      


        private BounceEffectFunction bounceEffectFunction;
        private SlidingEffectFunction slidingEffectFunction;
        private MainCoinClickFunction mainCoinClickFunction;
        private MultiplierBarFunction multiplierBarFunction;
        private CoinShowerFunction coinShowerFunction;
        private CharacterAnimationFunction characterAnimationFunction;
        private StatHolderFunction statHolderFunction;
        private PanelSwitcherFunction panelSwitcherFunction;
        private RankFunction rankFunction;
        private ShopFunction shopFunction;
        private MainGameInitFunction mainGameInitFunction;
        private TapCounterFunction tapCounterFunction;
        private NonTapCounterFunction nonTapCounterFunction;
       
        private void Awake()
        {
            bounceEffectFunction = GetComponent<BounceEffectFunction>();
            slidingEffectFunction = GetComponent<SlidingEffectFunction>();
            mainCoinClickFunction = GetComponent<MainCoinClickFunction>();
            multiplierBarFunction = GetComponent<MultiplierBarFunction>();
            coinShowerFunction = GetComponent<CoinShowerFunction>();
            characterAnimationFunction = GetComponent<CharacterAnimationFunction>();
            statHolderFunction = GetComponent<StatHolderFunction>();
            panelSwitcherFunction = GetComponent<PanelSwitcherFunction>();
            rankFunction = GetComponent<RankFunction>();
            shopFunction = GetComponent<ShopFunction>();
            mainGameInitFunction = GetComponent<MainGameInitFunction>();
            tapCounterFunction = GetComponent<TapCounterFunction>();
            nonTapCounterFunction = GetComponent<NonTapCounterFunction>();
          
        }


        private void Start()
        {
            IntializeContBounceEffect();
            InitializeGameCommandOnClick();

            string savedName = PlayerPrefs.GetString("User_Input");

            if (!string.IsNullOrEmpty(savedName))
            {
                InitializeFunctionValue();

              
            }
           

        }

       
        void IntializeContBounceEffect()
        {
            if (bounceEffectConfigs != null && bounceEffectConfigs.Count > 0)
            {
                var command = new ContBounceEffectCommand(bounceEffectConfigs, bounceEffectFunction, nonClickableButton);
                command.StoreButtonListenerCommand();
                buttonCommands.Add(command);
            }
        }



        void InitializeFunctionValue()
        {
            multiplierBarFunction.IntialValueofMutiplier(multiplierBarManager);
            characterAnimationFunction.InitializeLuffyAnimation(luffyAnimationConfig);
            shopFunction.Initializer(shopConfig, tapCounterFunction);
           


                StartCoroutine(rankFunction.FetchLeaderboardDataCoroutine(rankConfig, tapCounterConfig, tapCounterFunction));
            RankLoadingDuringInitialPhase();
          

        }

        void RankLoadingDuringInitialPhase()
        {
            string savedName = PlayerPrefs.GetString("User_Input");

            if (rankConfig != null && !string.IsNullOrEmpty(savedName))
            {
              //  Debug.Log("Game Controller passing Player Info");
                rankFunction.FetchRankFunction(rankConfig, tapCounterConfig,tapCounterFunction,"None");
             
                
            }
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


            if(slideEffectConfigs !=null &&shopConfig !=null )
            {
                var command = new StatHolderCommand(statInputConfig,shopConfig,statHolderFunction,mainGameInitConfig,buttonsAvailableToPlayer);
                command.StoreButtonListenerCommand();
                buttonCommands.Add(command);

            }

            if(panelSwitcherConfig !=null )
            {
                var command = new PanelSwitcherCommand(panelSwitcherFunction,panelSwitcherConfig,rankConfig,buttonsAvailableToPlayer);
                command.StoreButtonListenerCommand();
                buttonCommands.Add(command);
            }
            if(rankConfig !=null)
            {
                var command = new RankCommand(rankFunction,rankConfig ,tapCounterConfig,tapCounterFunction,buttonsAvailableToPlayer);
                command.StoreButtonListenerCommand();
                buttonCommands.Add(command);
            }
            if (shopConfig != null)
            {
                var command = new ShopCommand(shopFunction, shopConfig, buttonsAvailableToPlayer,tapCounterFunction,mainGameInitConfig);
                command.StoreButtonListenerCommand();
                buttonCommands.Add(command);
            }

            if (mainGameInitConfig!= null && tapCounterConfig !=null && nonTapCounterConfig !=null)
            {
                var command = new MainGameInitCommand(mainGameInitFunction,mainGameInitConfig,tapCounterConfig,nonTapCounterConfig,buttonsAvailableToPlayer);
                command.StoreButtonListenerCommand();
                buttonCommands.Add(command);
            }

            

            if(tapCounterConfig !=null)
            {
                var command = new TapCounterCommand(mainGameInitConfig,tapCounterFunction ,tapCounterConfig ,buttonsAvailableToPlayer,nonTapCounterConfig,shopConfig);
                command.StoreButtonListenerCommand();
                buttonCommands.Add(command);
            }
            if (nonTapCounterConfig != null)
            {
                var command = new NonTapCounterCommand(nonTapCounterFunction, shopConfig, nonTapCounterConfig, mainGameInitConfig,buttonsAvailableToPlayer);
                command.StoreButtonListenerCommand();
                buttonCommands.Add(command);
            }

           

        }
    }
}
