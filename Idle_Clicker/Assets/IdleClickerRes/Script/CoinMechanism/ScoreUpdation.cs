using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

namespace IdleClicker
{
    public class ScoreUpdation : MonoBehaviour, IButtonCommand
    {
        [SerializeField] private ScoreCont scoreCont;
        [SerializeField] private string tapButtonName = "TapButton";
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI multiplierText;
        [SerializeField] private Image multiplierBar;

        private Coroutine pulseRoutine;
        private Coroutine autoIncrementRoutine;

        private bool isAutoIncrementing = false;
        private int totalTapCount = 0;
        private const int tapsPerMultiplier = 100;
        private const float idleTimeToReset = 5f;
        private const float maxMultiplier = 10f;

        void Start()
        {
            if (scoreCont == null)
            {
                Debug.LogWarning("ScoreCont not assigned. Creating new instance.");
                scoreCont = ScriptableObject.CreateInstance<ScoreCont>();
                scoreCont.Setup(
                    PlayerPrefs.GetInt("TotalScore", 0),
                    PlayerPrefs.GetInt("Hitpoint", 0),
                    PlayerPrefs.GetInt("IncPerSec", 0),
                    1f
                );
            }

            scoreCont.Initialize();
            UpdateScoreDisplay();
            UpdateMultiplierDisplay();
            StartAutoIncrement(); // Start auto-increment on game start
        }

        void Update()
        {
            UpdateScoreDisplay();
        }

        public void Execute(string name)
        {
            if (scoreCont == null || name != tapButtonName)
                return;

            StopAutoIncrement(); // Stop auto increment on tap

            totalTapCount++;
            scoreCont.IncrementOnTap();

            // Multiplier logic
            float newMultiplier = 1f + Mathf.Floor(totalTapCount / tapsPerMultiplier);
            newMultiplier = Mathf.Min(newMultiplier, maxMultiplier);

            if (newMultiplier > scoreCont.Multiplier)
            {
                scoreCont.SetMultiplier(newMultiplier);
                if (pulseRoutine != null) StopCoroutine(pulseRoutine);
                pulseRoutine = StartCoroutine(PulseMultiplierBar(1f));
            }

            UpdateScoreDisplay();
            UpdateMultiplierDisplay();

            // Schedule reset and auto-resume
            CancelInvoke(nameof(ResetMultiplier));
            CancelInvoke(nameof(ResumeAutoIncrement));
            Invoke(nameof(ResetMultiplier), idleTimeToReset);
            Invoke(nameof(ResumeAutoIncrement), idleTimeToReset);
        }

        private void ResetMultiplier()
        {
            totalTapCount = 0;
            scoreCont.SetMultiplier(1f);
            UpdateMultiplierDisplay();
        }

        private void StartAutoIncrement()
        {
            if (!isAutoIncrementing)
            {
                isAutoIncrementing = true;
                if (autoIncrementRoutine != null)
                    StopCoroutine(autoIncrementRoutine);
                autoIncrementRoutine = StartCoroutine(AutoIncrementScore());
            }
        }

        private void StopAutoIncrement()
        {
            isAutoIncrementing = false;
            if (autoIncrementRoutine != null)
                StopCoroutine(autoIncrementRoutine);
            autoIncrementRoutine = null;
        }

        private void ResumeAutoIncrement()
        {
            StartAutoIncrement();
        }

        private IEnumerator AutoIncrementScore()
        {
            while (isAutoIncrementing)
            {
                scoreCont.IncrementOnTap();
                UpdateScoreDisplay();
                yield return new WaitForSeconds(1f);
            }
        }

        private void UpdateScoreDisplay()
        {
            if (scoreText != null && scoreCont != null)
                scoreText.text = scoreCont.GetSessionScoreFormatted();
        }

        private void UpdateMultiplierDisplay()
        {
            if (multiplierText != null)
                multiplierText.text = $"x{scoreCont.Multiplier:0.#}";

            if (multiplierBar != null && pulseRoutine == null)
                multiplierBar.fillAmount = Mathf.InverseLerp(1f, maxMultiplier, scoreCont.Multiplier);
        }

        private IEnumerator PulseMultiplierBar(float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                if (multiplierBar != null)
                {
                    float baseFill = Mathf.InverseLerp(1f, maxMultiplier, scoreCont.Multiplier);
                    float pulse = Mathf.Sin(Time.time * 8f) * 0.1f;
                    multiplierBar.fillAmount = Mathf.Clamp(baseFill + pulse, 0f, 1f);
                }
                elapsed += Time.deltaTime;
                yield return null;
            }

            if (multiplierBar != null)
                multiplierBar.fillAmount = Mathf.InverseLerp(1f, maxMultiplier, scoreCont.Multiplier);

            pulseRoutine = null;
        }

        void OnApplicationQuit() => scoreCont?.SaveScore();

        void OnApplicationPause(bool pause)
        {
            if (pause)
                scoreCont?.SaveScore();
        }

        [ContextMenu("Reset Score (Dev Only)")]
        public void ResetScore()
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("Score reset.");
        }
    }
}
