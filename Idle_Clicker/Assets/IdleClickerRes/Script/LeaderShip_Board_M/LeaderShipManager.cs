using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

namespace IdleClicker
{
    [CreateAssetMenu(fileName = "LeaderShipManagerSO", menuName = "IdleClicker/LeaderShipManagerSO")]
    public class LeaderShipManagerSO : ScriptableObject, IButtonCommand
    {
        public string buttonName;
        private TextMeshProUGUI iconName;
        private TMP_InputField nameInputField;
        private TextMeshProUGUI leaderboardDisplay;
        private MonoBehaviour coroutineRunner; // To start coroutines

        public string submitScoreURL;
        public string getLeaderboardURL;
        public int score;

        private const float REQUEST_TIMEOUT = 10f; // seconds

        public void Initialize(
            TextMeshProUGUI iconName,
            TMP_InputField nameInputField,
            TextMeshProUGUI leaderboardDisplay,
            MonoBehaviour coroutineRunner)
        {
            this.iconName = iconName;
            this.nameInputField = nameInputField;
            this.leaderboardDisplay = leaderboardDisplay;
            this.coroutineRunner = coroutineRunner;

            CleanName();
        }

        public void Execute(string name)
        {
            if (name == buttonName)
            {
                Debug.Log($"Execute called for button: {name}");
                if (coroutineRunner != null)
                {
                    coroutineRunner.StartCoroutine(SendScoreToServer());
                }
                else
                {
                    Debug.LogError("CoroutineRunner is null! Cannot start coroutine.");
                }
            }
            else
            {
                Debug.LogWarning($"Execute ignored for button: {name} (expected {buttonName})");
            }
        }

        private void CleanName()
        {
            string cleanedName = buttonName.Replace("Button", "").Replace("_", "");
            if (iconName != null)
            {
                iconName.text = cleanedName;
                Debug.Log($"Icon name text set to: {cleanedName}");
            }
            else
            {
                Debug.LogWarning("iconName is null in CleanName!");
            }
        }

        private IEnumerator SendScoreToServer()
        {
            string playerName = nameInputField?.text;

            if (string.IsNullOrWhiteSpace(playerName))
            {
                playerName = "Unknown";
            }
            else if (playerName.Length != 6)
            {
                Debug.LogError("Player name must be exactly 6 characters long.");
                if (leaderboardDisplay != null)
                    leaderboardDisplay.text = "Name must be exactly 6 characters.";
                yield break;
            }

            Debug.Log($"Sending score for player: {playerName} with score: {score}");

            bool success = false;
            yield return PostScore(playerName, score, result => success = result);

            if (success)
            {
                Debug.Log("Score posted successfully, fetching leaderboard...");
                yield return GetLeaderboardFromServer();
            }
            else
            {
                Debug.LogError("Failed to post score.");
                if (leaderboardDisplay != null)
                    leaderboardDisplay.text = "Failed to submit score.";
            }
        }


        private IEnumerator PostScore(string name, int score, Action<bool> onComplete)
        {
            if (string.IsNullOrEmpty(submitScoreURL))
            {
                Debug.LogError("submitScoreURL is not set!");
                onComplete?.Invoke(false);
                yield break;
            }

            var scoreData = new ScoreData { Name = name, Score = score };
            string jsonData = JsonUtility.ToJson(scoreData);

            using (UnityWebRequest www = new UnityWebRequest(submitScoreURL, "POST"))
            {
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
                www.uploadHandler = new UploadHandlerRaw(bodyRaw);
                www.downloadHandler = new DownloadHandlerBuffer();
                www.SetRequestHeader("Content-Type", "application/json");

                float elapsedTime = 0f;
                www.SendWebRequest();

                while (!www.isDone)
                {
                    elapsedTime += Time.deltaTime;
                    if (elapsedTime > REQUEST_TIMEOUT)
                    {
                        Debug.LogError("PostScore timed out.");
                        onComplete?.Invoke(false);
                        yield break;
                    }
                    yield return null;
                }

                if (www.result == UnityWebRequest.Result.ConnectionError ||
                    www.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Score submission failed: " + www.error);
                    onComplete?.Invoke(false);
                }
                else
                {
                    Debug.Log("Score submitted successfully! Response: " + www.downloadHandler.text);
                    onComplete?.Invoke(true);
                }
            }
        }

        private IEnumerator GetLeaderboardFromServer()
        {
            if (string.IsNullOrEmpty(getLeaderboardURL))
            {
                Debug.LogError("getLeaderboardURL is not set!");
                if (leaderboardDisplay != null)
                    leaderboardDisplay.text = "Leaderboard URL missing.";
                yield break;
            }

            using (UnityWebRequest www = UnityWebRequest.Get(getLeaderboardURL))
            {
                float elapsedTime = 0f;
                www.SendWebRequest();

                while (!www.isDone)
                {
                    elapsedTime += Time.deltaTime;
                    if (elapsedTime > REQUEST_TIMEOUT)
                    {
                        Debug.LogError("GetLeaderboardFromServer timed out.");
                        if (leaderboardDisplay != null)
                            leaderboardDisplay.text = "Leaderboard request timed out.";
                        yield break;
                    }
                    yield return null;
                }

                if (www.result == UnityWebRequest.Result.ConnectionError ||
                    www.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Leaderboard request failed: " + www.error);
                    if (leaderboardDisplay != null)
                        leaderboardDisplay.text = "Error loading leaderboard.";
                }
                else
                {
                    string json = www.downloadHandler.text;
                    Debug.Log("Leaderboard JSON received: " + json);

                    // Parse the JSON array of leaderboard entries
                    try
                    {
                        // Unity JsonUtility can't parse arrays directly, so wrap the array into an object
                        string wrappedJson = "{\"entries\":" + json + "}";
                        LeaderboardResponse response = JsonUtility.FromJson<LeaderboardResponse>(wrappedJson);

                        if (response?.entries != null && response.entries.Length > 0)
                        {
                            string formattedText = "Leaderboard:\n";
                            for (int i = 0; i < response.entries.Length; i++)
                            {
                                var entry = response.entries[i];
                                formattedText += $"{i + 1}. {entry.Name}                -                       {entry.Score}\n";
                            }
                            leaderboardDisplay.text = formattedText;
                        }
                        else
                        {
                            leaderboardDisplay.text = "No entries found.";
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError("Failed to parse leaderboard JSON: " + ex.Message);
                        leaderboardDisplay.text = "Error parsing leaderboard data.";
                    }
                }
            }
        }

    }

    [Serializable]
    public class ScoreData
    {
        public string Name;
        public int Score;
    }
    [Serializable]
    public class LeaderboardEntry
    {
        public string Name;
        public int Score;
    }

    [Serializable]
    public class LeaderboardResponse
    {
        public LeaderboardEntry[] entries;
    }

}