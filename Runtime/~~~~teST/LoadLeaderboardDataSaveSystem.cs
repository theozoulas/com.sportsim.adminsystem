using System.Collections;
using System.Collections.Generic;
using MenuComponents.SaveSystem;
using UnityEngine;

public class LoadLeaderboardDataSaveSystem : LeaderboardDataProvider
{
    protected override List<PlayerData> LoadPlayerData()
    {
        return SaveManager.LoadPlayerData();
    }
}
