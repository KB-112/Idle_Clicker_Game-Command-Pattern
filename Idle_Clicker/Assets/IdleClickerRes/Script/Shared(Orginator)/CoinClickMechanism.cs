using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace IdleClicker
{
    public class CoinClickMechanism : IButtonCommand
    {
        public int hitpoint;
        public int score;
        [Header("Inactivity Settings")]
        public float idleResetThreshold = 2f;
        public float autoIncrementRate = 1.5f;
        private float lastTapTime = 0f;
        private float lastAutoIncTime = 0f;
        private bool isAnimating = false;

        //  public TextMeshProUGUI scoreText;
        [Header("Effects")]
        public ParticleSystem ps;
        public string buttonName = "Coin_Button";

        public CoinClickMechanism(ParticleSystem ps)
        {
            this.ps = ps;

        }
        public void Execute(string name)
        {
            if (buttonName == name)
            {
                if (ps != null)
                {
                    Debug.Log("Ps Play");
                    ps.Play();
                }
                else
                {
                    Debug.LogWarning("ParticleSystem not assigned in CoinClickMechanism!");
                }
            }
        }


        void Update()
        {
            //   scoreText.text = score.ToString();

            float timeSinceTap = Time.time - lastTapTime;

            if (timeSinceTap >= idleResetThreshold && Time.time - lastAutoIncTime >= autoIncrementRate)
            {

                lastAutoIncTime = Time.time;
            }

            if (timeSinceTap > idleResetThreshold && hitpoint > 0)
            {
                hitpoint = 0;
                Debug.Log("Hitpoint reset due to inactivity.");
            }
        }

        void OnTap()
        {
            if (isAnimating) return;

            isAnimating = true;
            lastTapTime = Time.time;

            ps.Play();

        }







    }
}
