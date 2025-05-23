using System;
using System.Collections.Generic;
using System.Linq;
using MenuComponents.SaveSystem;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Object = System.Object;

namespace MenuComponents.Leaderboard
{
    /// <summary>
    /// Class <c>LeaderboardManager</c> Manages the top 10 leaderboard.
    /// </summary>
    public class LeaderboardManager : MonoBehaviour
    {
        [SerializeField] private Transform entryPrefab;
        [SerializeField] private GameObject leaderBoardPanel;
        [SerializeField] private string playerNameDataField;
        

        private List<PlayerData> _playerDataList;

        private LeaderboardDataProvider _leaderboardDataProvider;

        private bool _isLoaded;

        private void Awake()
        {
            _leaderboardDataProvider = GetLeaderboardProvider();
        }

        private LeaderboardDataProvider GetLeaderboardProvider()
        {
            return GlobalConfig<LeaderboardDynamicMenu>.Instance.dataProviderType switch
            {
                LeaderboardDataProviders.SaveSystem => new LoadLeaderboardDataSaveSystem(),
                LeaderboardDataProviders.Online => new LoadLeaderboardDataOnline(),
                _ => new LoadLeaderboardDataSaveSystem()
            };
        }

        private void Start()
        {
            UpdateLeaderboard();
        }

        /// <summary>
        /// Method <c>UpdateLeaderboard</c> Update leaderboard with player data.
        /// </summary>
        private void UpdateLeaderboard()
        {
            if (_isLoaded) return;

            _playerDataList = GetLeaderboardData();

            var leaderBoardList =
                leaderBoardPanel
                    .transform
                    .GetComponentInChildren<VerticalLayoutGroup>()
                    .transform;

            if (_playerDataList != null)
            {
                for (var i = 0; i < _playerDataList.Count; i++)
                {
                    if (i < 10) FillEntries(leaderBoardList, _playerDataList[i], i);
                    else break;
                }
            }

            _isLoaded = true;
        }

        /// <summary>
        /// Method <c>FillEntries</c> Fill in leaderboard entry with passed params.
        /// </summary>
        /// <param name="leaderBoardList"></param>
        /// <param name="playerData"></param>
        /// <param name="i"></param>
        private void FillEntries(Transform leaderBoardList, PlayerData playerData, int i)
        {
            var newEntry = Instantiate(entryPrefab, leaderBoardList);

            var rankField = newEntry.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            var nameField = newEntry.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            var scoreField = newEntry.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

            rankField.color = Color.white;
            nameField.color = Color.white;
            scoreField.color = Color.white;

            rankField.text = (i + 1).ToString();
            nameField.text = playerData.GetDataFieldValueByName(playerNameDataField);
            scoreField.text = playerData.ScoreFormatted;
        }

        /// <summary>
        /// Get Method <c>GetLeaderboardData</c> Get the data for the leaderboard.
        /// </summary>
        /// <param name="playerData"></param>
        /// <returns>Returns a list of <c>PlayerData</c> </returns>
        private static List<PlayerData> GetLeaderboardData()
        {
            var playerDataList = SaveManager.LoadPlayerData();

            if (playerDataList != null)
            {
                var sortedPlayerDataList = 
                    (ScoreDynamicMenu.Instance.sortScoreBy == ScoreDynamicMenu.ScoreSort.Highest
                    ? playerDataList.OrderByDescending(data => data.Score)
                    : playerDataList.OrderBy(data => data.Score)).ToList();

                return sortedPlayerDataList;
            }

            playerDataList = new List<PlayerData>();

            return playerDataList;
        }
    }
}