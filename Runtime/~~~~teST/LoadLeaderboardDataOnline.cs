using System.Collections;
using System.Collections.Generic;
using MenuComponents.SaveSystem;
using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/test", order = 1)]  
public class LoadLeaderboardDataOnline : LeaderboardDataProvider
{
    protected override List<PlayerData> LoadPlayerData()
    {
        return new List<PlayerData>();
    }
}
