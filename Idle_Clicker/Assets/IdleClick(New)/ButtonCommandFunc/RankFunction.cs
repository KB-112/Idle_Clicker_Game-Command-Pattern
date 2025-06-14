using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace IdleClicker
{
    public class RankFunction : MonoBehaviour
    {


        public void PlayerInfo(RankConfig rankConfig, TapCounterConfig tapCounterConfig, TapCounterFunction tapCounterFunction)
        {
            string currentUser = PlayerPrefs.GetString("User_Input");
            Debug.Log("Checking player info for user input: " + currentUser);

            for (int i = 0; i < rankConfig.rankList.rankEntries.Count; i++)
            {
                string entryName = rankConfig.rankList.rankEntries[i].playerName;
                Debug.Log($"Checking rank entry {i}: PlayerName = {entryName}");

                if (entryName == currentUser)
                {
                    Debug.Log("Match found. Creating UserData...");

                    UserData user = new UserData
                    {
                        User_Name = entryName,
                        Score = tapCounterConfig.currentScore,
                        id = rankConfig.rankList.rankEntries[i].playerId
                    };

                    Debug.Log($"UserData created: Name = {user.User_Name}, Score = {user.Score}, ID = {user.id}");

                    tapCounterFunction.UserDataStore(user);
                    Debug.Log("UserData stored successfully.");
                }
            }

            Debug.Log("PlayerInfo processing completed.");
        }

        public void FetchRankFunction(RankConfig rankConfig, string buttonAvailableToPlayer)
        {
            if (rankConfig.buttonAvailableToPlayer == buttonAvailableToPlayer && rankConfig != null)
            {
                StartCoroutine(FetchLeaderboardDataCoroutine(rankConfig));
            }
           
        }

        IEnumerator FetchLeaderboardDataCoroutine(RankConfig rankConfig)
        {
            // Show loading immediately
            rankConfig.loadingText.text = "Loading...";

            // Clear previous entries early for visual feedback
            foreach (Transform child in rankConfig.leaderboardParentTransform)
                Destroy(child.gameObject);

            // Execute API request
            rankConfig.apiExecutor.ExecuteCommand(new GetCommand(),
                "https://6824498265ba05803399a0a2.mockapi.io/api/v1/User_Name", null);

            // Wait until data is available
            yield return new WaitUntil(() =>
                rankConfig.apiHolder != null &&
                rankConfig.apiHolder.onSuccess != null &&
                rankConfig.apiHolder.onSuccess.template != null &&
                rankConfig.apiHolder.onSuccess.template.Count > 0
            );

            // Populate leaderboard list
            rankConfig.rankList.rankEntries = rankConfig.apiHolder.onSuccess.template
                .Select(t => new RankEntry
                {
                    playerName = t.User_Name,
                    playerScore = t.score,
                    playerId = t.id
                })
                .OrderByDescending(entry => entry.playerScore)
                .ToList();

            // Instantiate leaderboard items
            foreach (var (entry, i) in rankConfig.rankList.rankEntries.Select((e, i) => (e, i)))
            {
                GameObject item = Instantiate(rankConfig.rankEntryPrefab, rankConfig.leaderboardParentTransform);

                var nameText = item.transform.Find("Info_Text")?.GetComponent<TextMeshProUGUI>();
                if (nameText != null)
                    nameText.text = $"{i + 1}. {entry.playerName}";

                var scoreText = item.transform.Find("Score_Text")?.GetComponent<TextMeshProUGUI>();
                if (scoreText != null)
                    scoreText.text = entry.playerScore.ToString();
            }

            // Wait one frame and rebuild layout
            yield return null;
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(rankConfig.leaderboardParentTransform.GetComponent<RectTransform>());

            // Scroll to top
            var scrollRect = rankConfig.leaderboardParentTransform.GetComponentInParent<ScrollRect>();
            if (scrollRect != null)
                scrollRect.verticalNormalizedPosition = 1f;

            // Hide loading text
            rankConfig.loadingText.text = "";
        }



    }

    [System.Serializable]
    public struct RankEntry
    {
        public string playerName;
        public int playerScore;
        public int playerId;
    }

    [System.Serializable]
    public class RankList
    {
        public List<RankEntry> rankEntries;
    }

    [System.Serializable]
    public class RankConfig
    {
        public string buttonAvailableToPlayer;
        public GameObject rankEntryPrefab;
        public ApiCommandExecutor apiExecutor;
        public RankList rankList;
        public ApiHolder apiHolder;
        public Transform leaderboardParentTransform;
        public float entryVerticalSpacing = 1000f;
        public TextMeshProUGUI loadingText;
    }

    [System.Serializable]
    public class UserData
    {
        public int id;
        public string User_Name;
        public int Score;
    }
}
