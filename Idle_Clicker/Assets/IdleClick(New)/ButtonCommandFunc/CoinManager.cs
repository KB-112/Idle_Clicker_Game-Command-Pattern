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
       
        void StatUpdate()
        {
            statText.text =
     $"<mspace=25>Total    :</mspace> {totalBalance} coins\n" +
     $"<mspace=25>Current  :</mspace> {(int.TryParse(coinScoreText.text, out int val) ? val : 0)} coins\n" +
     "<mspace=25>Per Tap  :</mspace> 1 coins\n" +
     "<mspace=25>Per Idle :</mspace> 1 coins";



        }

        private void Start()
        {
            tempButton.onClick.AddListener(OnClickIncr);
            statButton.onClick.AddListener(() => StopIncr (true));
            closePanelButton.onClick.AddListener( () => StopIncr (false));
        }


        void StopIncr(bool stat)
        {
            isStatOpen = stat;
        }
        private void Update()
        {
            StatUpdate();
            
                UpdateUI();
            
           

            if (!isIdleEarningStarted && Time.time - lastClickTime >= cooldownDuration && !isIdleEarningStarted )
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
