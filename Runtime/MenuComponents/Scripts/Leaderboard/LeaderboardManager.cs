using System.Collections.Generic;
using System.Linq;
using MenuComponents.DynamicSystem;
using MenuComponents.SaveSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MenuComponents.Leaderboard
{
    /// <summary>
    /// Class <c>LeaderboardManager</c> Manages the top 10 leaderboard.
    /// </summary>
    public class LeaderboardManager : MonoBehaviour
    {
        [SerializeField] private Transform entryPrefab;
        [SerializeField] private  GameObject leaderBoardPanel;
        [SerializeField] private string playerNameDataField;

        [SerializeField] private ColourDynamicData rankTextColour;
        [SerializeField] private ColourDynamicData entryTextColour;

        private List<PlayerData> _playerDataList;
        
        private bool _isLoaded;
        

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

            _playerDataList = GetLeaderboardData(SaveManager.GetCurrentPlayerDataBestScore());

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

            rankField.color = rankTextColour.colour;
            nameField.color = entryTextColour.colour;
            scoreField.color = entryTextColour.colour;

            rankField.text = (i + 1).ToString();
            nameField.text = playerData.GetDataFieldValueByName(playerNameDataField);
            scoreField.text = playerData.Score.ToString();
        }

        /// <summary>
        /// Get Method <c>GetLeaderboardData</c> Get the data for the leaderboard.
        /// </summary>
        /// <param name="playerData"></param>
        /// <returns>Returns a list of <c>PlayerData</c> </returns>
        private static List<PlayerData> GetLeaderboardData(PlayerData playerData = null)
        {
            var playerDataList = SaveManager.LoadPlayerData();

            if (playerDataList != null)
            {
                if (playerData != null)
                {
                    playerDataList.Add(playerData);
                }
                var sortedPlayerDataList =
                    playerDataList.OrderByDescending(data => data.Score).ToList();
                
                return sortedPlayerDataList;
            }

            playerDataList = new List<PlayerData> { playerData };

            return playerDataList;
        }
    }
}