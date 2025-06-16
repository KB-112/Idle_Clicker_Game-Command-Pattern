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
       

        public void FetchRankFunction(RankConfig rankConfig, TapCounterConfig tapCounterConfig, TapCounterFunction tapCounterFunction, string buttonAvailableToPlayer)
        {
            if (rankConfig == null)
            {
                Debug.Log("Rank COnfig null");
            }
            else
            {
                if (rankConfig.buttonAvailableToPlayer == buttonAvailableToPlayer)
                {
                    StartCoroutine(FetchLeaderboardDataCoroutine(rankConfig, tapCounterConfig, tapCounterFunction));
                }
           
            }

           
          
        }

        public IEnumerator FetchLeaderboardDataCoroutine(RankConfig rankConfig, TapCounterConfig tapCounterConfig, TapCounterFunction tapCounterFunction)
        {
            Debug.Log("Leaderboard Execution");
            rankConfig.loadingText.text = "Loading...";

            ClearLeaderboard(rankConfig.leaderboardParentTransform);

            rankConfig.apiExecutor.ExecuteCommand(new GetCommand(),
                "https://6824498265ba05803399a0a2.mockapi.io/api/v1/User_Name", null);

            yield return new WaitUntil(() =>
                rankConfig.apiHolder?.onSuccess?.template?.Count > 0);

            PopulateRankEntries(rankConfig);
            InstantiateLeaderboard(rankConfig);

            yield return null;
            RebuildUI(rankConfig);

          
            rankConfig.loadingText.text = "";
        }

        private void ClearLeaderboard(Transform leaderboardParent)
        {
            foreach (Transform child in leaderboardParent)
                Destroy(child.gameObject);
        }

        private void PopulateRankEntries(RankConfig rankConfig)
        {
            rankConfig.rankList.rankEntries = rankConfig.apiHolder.onSuccess.template
                .Select(t => new RankEntry
                {
                    playerName = t.User_Name,
                    playerScore = t.score,
                    playerId = t.id
                })
                .OrderByDescending(entry => entry.playerScore)
                .ToList();
        }

        private void InstantiateLeaderboard(RankConfig rankConfig)
        {
            for (int i = 0; i < rankConfig.rankList.rankEntries.Count; i++)
            {
                var entry = rankConfig.rankList.rankEntries[i];
                GameObject item = Instantiate(rankConfig.rankEntryPrefab, rankConfig.leaderboardParentTransform);

                var nameText = item.transform.Find("Info_Text")?.GetComponent<TextMeshProUGUI>();
                var scoreText = item.transform.Find("Score_Text")?.GetComponent<TextMeshProUGUI>();

                if (nameText != null) nameText.text = $"{i + 1}. {entry.playerName}";
                if (scoreText != null) scoreText.text = entry.playerScore.ToString();
            }
        }

        private void RebuildUI(RankConfig rankConfig)
        {
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(rankConfig.leaderboardParentTransform.GetComponent<RectTransform>());

            var scrollRect = rankConfig.leaderboardParentTransform.GetComponentInParent<ScrollRect>();
            if (scrollRect != null)
                scrollRect.verticalNormalizedPosition = 1f;
        }
    }

    [System.Serializable]
    public struct RankEntry
    {
        public string playerName;
        public int playerScore;
        public int playerId;
        public int OnTapUpgradeLevel;
        public int OnIdleUpgradeLevel;
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
        public int OnTapUpgradeLevel;
        public int OnIdleUpgradeLevel;
    }
}
