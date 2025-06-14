using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace IdleClicker
{
    public class CoinManager : MonoBehaviour
    {
        public TextMeshProUGUI coinScoreText;
        public Button tempButton;
        public Button statButton;
        public Button closePanelButton;
        public Button shopButton;
        public int totalCoinEarnWhileTapping;
        public int totalCoinEarnWhileNotTapping;
        public int totalBalance;
       

        public float cooldownDuration = 5f;

      
        private float lastClickTime;
        private bool isNonClickEarner = false; // Text
        private bool isIdleEarningStarted = false;

        public StatHolder statHolder;
        public TextMeshProUGUI statText;
        public bool isStatOpen =false;
        public GameObject tapToPlayObj;
        public Button tapToPlay;
        bool isTapped = false;
        public GameObject totalCoinObj;
      
       
     

        private void Start()
        {
            tapToPlayObj.SetActive(true);
            totalCoinObj.SetActive(false);
            tempButton.onClick.AddListener(OnClickIncr);
            statButton.onClick.AddListener(() => StopIncr (true));
            closePanelButton.onClick.AddListener( () => StopIncr (false));
            tapToPlay.onClick.AddListener(SelfIncrState);
        }

        void SelfIncrState()
        {
            isTapped = true;
            tapToPlayObj.SetActive (false);
            totalCoinObj.SetActive (true);
         
        }

        void StopIncr(bool stat)
        {
            isStatOpen = stat;
        }
        private void Update()
        {
            //StatUpdate();
            
                UpdateUI();
            
           

            if (!isIdleEarningStarted && Time.time - lastClickTime >= cooldownDuration && !isIdleEarningStarted &&isTapped )
            {
                StartIdleEarning();
            }
        }

        private void UpdateUI()
        {
            if (!isStatOpen)
            {
                coinScoreText.text = isNonClickEarner
                ? totalCoinEarnWhileNotTapping.ToString()
                : totalCoinEarnWhileTapping.ToString();
            }
         
        }

        private void ResetTapCoins()
        {
            totalCoinEarnWhileTapping = 0;
            totalCoinEarnWhileNotTapping = 0;
        }


        private void StartIdleEarning()
        {
            isNonClickEarner = true;
            isIdleEarningStarted = true;
            ResetTapCoins();

            // Immediate idle coin for visual feedback
            totalCoinEarnWhileNotTapping++;
            TotalBalanceUpdate();
            UpdateUI();

            StartCoroutine(NonClickCoinRoutine(1f));
        }

        void OnClickIncr()
        {
          
            if (isNonClickEarner)
            {
                isNonClickEarner = false;
                isIdleEarningStarted = false;
              
                StopAllCoroutines();
            }

            totalCoinEarnWhileTapping++;
           
            lastClickTime = Time.time;

            TotalBalanceUpdate();
            UpdateUI();
        }

        IEnumerator NonClickCoinRoutine(float delay)
        {
            while (isNonClickEarner)
            {
                yield return new WaitForSeconds(delay);
                totalCoinEarnWhileNotTapping++;
                TotalBalanceUpdate();
                UpdateUI();
            }
        }

        public void TotalBalanceUpdate()
        {
            totalBalance++;
        }
    }
}
