using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

namespace IdleClicker
{
    public class RankFunction : MonoBehaviour
    {
       
       

        public void FetchRankFunction(RankConfig rankConfig, string buttonAvailableToPlayer)
        {
            if (rankConfig.buttonAvailableToPlayer == buttonAvailableToPlayer && rankConfig != null)
            {
                StartCoroutine(FetchLeaderboardDataCoroutine(rankConfig));
            }
           
        }

        IEnumerator FetchLeaderboardDataCoroutine( RankConfig rankConfig)
        {
            // Execute API request
            rankConfig.apiExecutor.ExecuteCommand(new GetCommand(),
                "https://6824498265ba05803399a0a2.mockapi.io/api/v1/User_Name", null);

            // Wait until data is fully loaded and available
            yield return new WaitUntil(() => rankConfig.apiHolder != null &&
                                            rankConfig.apiHolder.onSuccess != null &&
                                            rankConfig.apiHolder.onSuccess.template != null &&
                                            rankConfig.apiHolder.onSuccess.template.Count > 0);

            // Populate and sort rank data by score
            rankConfig.rankList.rankEntries = rankConfig.apiHolder.onSuccess.template
                .Select(t => new RankEntry
                {
                    playerName = t.User_Name,
                    playerScore = t.score,
                    playerId = t.id
                })
                .OrderByDescending(entry => entry.playerScore)
                .ToList();

            // Instantiate leaderboard UI entries
            for (int i = 0; i < rankConfig.rankList.rankEntries.Count; i++)
            {
                RankEntry entry = rankConfig.rankList.rankEntries[i];

                GameObject leaderboardItem = Instantiate(rankConfig.rankEntryPrefab, rankConfig.leaderboardParentTransform);
                RectTransform itemRectTransform = leaderboardItem.GetComponent<RectTransform>();
                if (itemRectTransform != null)
                {
                    itemRectTransform.anchoredPosition = new Vector2(0, -i * rankConfig.entryVerticalSpacing);
                }

                // Set player name text
                var nameTextTransform = leaderboardItem.transform.Find("Info_Text");
                if (nameTextTransform != null)
                {
                    var nameText = nameTextTransform.GetComponent<TextMeshProUGUI>();
                    if (nameText != null)
                        nameText.text = $"{i + 1}. {entry.playerName}";
                }

                // Set player score text
                var scoreTextTransform = leaderboardItem.transform.Find("Score_Text");
                if (scoreTextTransform != null)
                {
                    var scoreText = scoreTextTransform.GetComponent<TextMeshProUGUI>();
                    if (scoreText != null)
                        scoreText.text = entry.playerScore.ToString();
                }
            }
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
    }
}
