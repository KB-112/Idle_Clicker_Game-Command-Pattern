using UnityEngine;
using System;

namespace IdleClicker
{
    [CreateAssetMenu(fileName = "ScoreCont", menuName = "ScriptableObjects/ScoreCont", order = 3)]
    public class ScoreCont : ScriptableObject
    {
        [Header("Score Tracking")]
        [SerializeField] private int storeScore;
        [SerializeField] public int hitpoint;
        [SerializeField] private int incPersec;

        [Header("Multiplier Value")]
        [SerializeField] private float multiplier = 1f;

        [SerializeField, HideInInspector] private int currentSessionScore;

        public event Action<float> OnMultiplierChanged;
        public event Action<int> OnScoreChanged;

        // Public read-only access


        public void Setup(int storeScore, int hitpoint, int incPersec, float multiplier = 1f)
        {
            this.storeScore = storeScore;
            this.hitpoint = hitpoint;
            this.incPersec = incPersec;
            this.multiplier = multiplier;

            currentSessionScore = 0;
            NotifyUpdates();
        }
        public float Multiplier { get; private set; }

        public void SetMultiplier(float value)
        {
            Multiplier = Mathf.Clamp(value, 1f, 10f);
        }


        public void Initialize()
        {
            currentSessionScore = 0;
            NotifyUpdates();
        }

        public void IncrementOnTap()
        {
            incPersec++;
            storeScore += incPersec;
            currentSessionScore++;

            OnScoreChanged?.Invoke(storeScore);
        }

        public void UpdateMultiplier()
        {
            multiplier = Mathf.Clamp(hitpoint / 10f, 1f, 2f);
            OnMultiplierChanged?.Invoke(multiplier);
        }

        public void SaveScore()
        {
            PlayerPrefs.SetInt("TotalScore", storeScore);
            PlayerPrefs.SetInt("IncPerSec", incPersec);
            PlayerPrefs.SetInt("Hitpoint", hitpoint);
            PlayerPrefs.Save();
        }

        public void LoadScore()
        {
            storeScore = PlayerPrefs.GetInt("TotalScore", 0);
            incPersec = PlayerPrefs.GetInt("IncPerSec", 0);
            hitpoint = PlayerPrefs.GetInt("Hitpoint", 0);
        }

        public string GetFormattedScore() => FormatScore(storeScore);
        public string GetSessionScoreFormatted() => FormatScore(currentSessionScore);

        private void NotifyUpdates()
        {
            OnMultiplierChanged?.Invoke(multiplier);
            OnScoreChanged?.Invoke(storeScore);
        }

        private string FormatScore(int score)
        {
            if (score >= 1_000_000_000)
                return (score / 1_000_000_000f).ToString("0.##") + "B";
            else if (score >= 1_000_000)
                return (score / 1_000_000f).ToString("0.##") + "M";
            else if (score >= 1_000)
                return (score / 1_000f).ToString("0.#") + "K";
            else
                return score.ToString();
        }
    }
}
