using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using MenuComponents.SaveSystem;

public class OnlineLeaderboard : MonoBehaviour
{
    private List<PlayerData> _playerDataList;

    private float _time;
    private bool _isLoaded;

    [SerializeField] private Transform[] _leaderBoardEntries;


    public void LoadLeaderboard()
    {
        ClearLeaderboard();
        UpdateLeaderboard();

        _isLoaded = true;
    }

    public void UnLoadLeaderboard()
    {
        _isLoaded = false;
    }

    private void Update()
    {
        if (!_isLoaded) return;

        _time += Time.deltaTime;
        if (_time > 5)
        {
            UpdateLeaderboard();
            _time = 0;
        }
    }

    private void UpdateLeaderboard()
    {
        StartCoroutine(HttpsRequest.Get(UpdateLeaderboardEntries));
    }

    private void UpdateLeaderboardEntries(List<LeaderboardPlayerData> leaderboardData)
    {
        var currentPlayer = SaveManager.GetCurrentPlayerDataBestScore();

        if (currentPlayer != null)
            leaderboardData.Add(
                new LeaderboardPlayerData(
                    "currentPlayer.Name",
                    currentPlayer.Score));

        leaderboardData = leaderboardData.OrderByDescending(data => data.Score).ToList();

        var count = leaderboardData.Count;

        for (var i = 0; i < 10; i++)
        {
            if (i < count)
                FillEntries(_leaderBoardEntries[i], leaderboardData[i]);
            else
                ClearEntry(_leaderBoardEntries[i]);
        }
    }

    public void ClearLeaderboard()
    {
        foreach (var entry in _leaderBoardEntries)
        {
            ClearEntry(entry);
        }
    }

    private static void ClearEntry(Transform entry)
    {
        entry.GetChild(1).GetComponent<TextMeshProUGUI>().text =
            string.Empty;

        entry.GetChild(2).GetComponent<TextMeshProUGUI>().text =
            string.Empty;
    }


    private static void FillEntries(Transform entry, LeaderboardPlayerData playerData)
    {
        entry.GetChild(1).GetComponent<TextMeshProUGUI>().text =
            playerData.Name;

        entry.GetChild(2).GetComponent<TextMeshProUGUI>().text =
            playerData.Score.ToString();
    }
}