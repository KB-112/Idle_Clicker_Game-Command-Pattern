using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

namespace IdleClicker
{
    public class MainCoinClickFunction : MonoBehaviour
    {
        [SerializeField] CoinFuncConfig coinFuncConfig;
       public int loopCount=1;
        public int rangeCount;
        [SerializeField] private int hit;



        private void Start()
        {
            loopCount = 1;
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                hit++;
                ClickThresholdManager();
            }
           



        }
        public void ClickThresholdManager()
        {
            var c = coinFuncConfig;
            for (int i = 0; i < c.clickThresholds.Length-1; i++)
            {
                ResetLoopCount();
                if ( hit == c.clickThresholds[i].y   )
                {
                    rangeCount ++;
                    
               

                }
            }
        }

       
    public   void  ResetLoopCount( )
        {
          
            if (hit > coinFuncConfig.clickThresholds[^1].y )
            {
             //   Debug.Log(coinFucConfig.clickThresholds[^1].y);
                hit = 0;
                loopCount++;
                rangeCount = 0;

            }
            if (loopCount > coinFuncConfig.clickThresholds.Length )

            {
                loopCount = 1;
            }
          
        }


        public void ResetValues()
        {
            hit = 0;
          
            rangeCount = 0;
            loopCount = 1;
        }
       



    }

    [System.Serializable]
    public class CoinFuncConfig
    {
        public Vector2Int[] clickThresholds;
       
    }




}
