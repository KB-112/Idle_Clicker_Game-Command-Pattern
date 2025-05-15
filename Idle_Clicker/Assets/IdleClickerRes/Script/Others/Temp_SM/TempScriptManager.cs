using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;


namespace IdleClicker.External
{
    public class TempScriptManager : MonoBehaviour
    {
        [Header("Button Setup")]
        public GameObject button;
        public Vector3 scaleBtn = new Vector3(1.2f, 1.2f, 1.2f);
        public Vector3 orgscaleBtn = Vector3.one;
        public float durationScale = 0.1f;
        public Button ontapButton;

        [Header("Effects")]
        public ParticleSystem ps;

        [Header("Scoring")]
        public int score;
        public int hitpoint;
        public TextMeshProUGUI scoreText;

        [Header("Inactivity Settings")]
        public float idleResetThreshold = 2f;
        public float autoIncrementRate = 1.5f;
        private float lastTapTime = 0f;
        private float lastAutoIncTime = 0f;
        private bool isAnimating = false;

        [Header("Panel Animation")]
        public Button panelAnim;
        public Button closePanelAnim;
        public float finalDestPanel;
        public float finalDestClosePanel;
        public RectTransform panel;
        public RectTransform closePanel;
        public float panelDuration;

        [Header("Leaderboard UI")]
        public TMP_InputField nameInputField;
        public Button submitScoreButton;
        public Button fetchLeaderboardButton;
        public TextMeshProUGUI leaderboardDisplay;

        [Header("Server URLs")]
        public string submitScoreURL = "https://yourdomain.com/submit";
        public string getLeaderboardURL = "https://yourdomain.com/leaderboard";

        void Start()
        {
            ontapButton.onClick.AddListener(OnTap);
            panelAnim.onClick.AddListener(() => PanelAnim(finalDestPanel, panelDuration, false));
            closePanelAnim.onClick.AddListener(() => PanelAnim(finalDestClosePanel, panelDuration, false));

            submitScoreButton.onClick.AddListener(SendScoreToServer);
            fetchLeaderboardButton.onClick.AddListener(GetLeaderboardFromServer);

            lastTapTime = Time.time;
            lastAutoIncTime = Time.time;
        }
        private void Awake()
        {

        }
        void Update()
        {
            scoreText.text = score.ToString();

            float timeSinceTap = Time.time - lastTapTime;

            if (timeSinceTap >= idleResetThreshold && Time.time - lastAutoIncTime >= autoIncrementRate)
            {
                AutoIncreaseScore();
                lastAutoIncTime = Time.time;
            }

            if (timeSinceTap > idleResetThreshold && hitpoint > 0)
            {
                hitpoint = 0;
                Debug.Log("Hitpoint reset due to inactivity.");
            }
        }

        void PanelAnim(float finalPos, float speed, bool snap)
        {
            panel.DOAnchorPos3DY(finalPos, speed, snap);
        }

        void OnTap()
        {
            if (isAnimating) return;

            isAnimating = true;
            lastTapTime = Time.time;

            ps.Play();
            IncrementScore();

            Sequence seq = DOTween.Sequence();
            seq.Append(button.transform.DOScale(scaleBtn, durationScale).SetEase(Ease.OutQuad));
            seq.Append(button.transform.DOScale(orgscaleBtn, durationScale).SetEase(Ease.InQuad));
            seq.OnComplete(() => isAnimating = false);
        }

        void IncrementScore()
        {
            hitpoint++;

            if (hitpoint < 200)
            {
                score += 1;
            }
            else if (hitpoint > 10000)
            {
                score += 200;
            }
            else if (hitpoint > 3500)
            {
                score += 40;
            }
            else if (hitpoint > 1500)
            {
                score += 30;
            }
            else if (hitpoint > 500)
            {
                score += 20;
            }
        }

        void AutoIncreaseScore()
        {
            score++;
            Debug.Log("Auto score increased due to idle.");
        }

        void SendScoreToServer()
        {
            string playerName = nameInputField.text;
            StartCoroutine(PostScore(playerName, score));
        }

        IEnumerator PostScore(string name, int score)
        {
            string jsonData = JsonUtility.ToJson(new ScoreData { Name = name, Score = score });

            UnityWebRequest www = new UnityWebRequest(submitScoreURL, "POST");
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Score submission failed: " + www.error);
            }
            else
            {
                Debug.Log("Score submitted successfully!");
            }
        }


        void GetLeaderboardFromServer()
        {
            StartCoroutine(FetchLeaderboard());
        }

        IEnumerator FetchLeaderboard()
        {
            using (UnityWebRequest www = UnityWebRequest.Get(getLeaderboardURL))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    leaderboardDisplay.text = "Error loading leaderboard.";
                    Debug.LogError("Leaderboard request failed: " + www.error);
                }
                else
                {
                    leaderboardDisplay.text = www.downloadHandler.text;
                    Debug.Log("Leaderboard data received.");
                }
            }
        }
    }
    [System.Serializable]
    public class ScoreData
    {
        public string Name;
        public int Score;
    }
}