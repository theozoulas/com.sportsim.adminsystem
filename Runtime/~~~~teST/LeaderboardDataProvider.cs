using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MenuComponents.SaveSystem;
using UnityEngine;

public abstract class LeaderboardDataProvider : ScriptableObject
{
   public List<PlayerData> GetLeaderboardData(PlayerData playerData = null)
   {
      var playerDataList = LoadPlayerData();

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

   protected abstract List<PlayerData> LoadPlayerData();
}
