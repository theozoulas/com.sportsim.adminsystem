using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;

namespace MenuComponents.SaveSystem
{
    /// <summary>
    /// Static Class <c>SaveSystem</c> System used for saving player data.
    /// </summary>
    public static class SaveManager
    {
        public static readonly string SavedPath =
            Application.persistentDataPath + "/playerData.data";

        public static readonly ScoreData StaticScoreData = ScoreDynamicMenu.Instance.mainScoreData;

        private static PlayerData _currentPlayerData;


        /// <summary>
        /// Static Get Method <c>LoadPlayerData</c> Load player data from local storage.
        /// </summary>
        /// <returns>Returns a list of <c>PlayerData</c></returns>
        public static List<PlayerData> LoadPlayerData()
        {
            if (!File.Exists(SavedPath)) return null;

            var formatter = new BinaryFormatter();

            var fileStream = new FileStream(SavedPath, FileMode.Open);

            var data = formatter.Deserialize(fileStream) as List<PlayerData>;

            fileStream.Close();

            return data;
        }

        /// <summary>
        /// Static TryGet Method <c>TryLoadPlayerData</c> Try load player data from local storage.
        /// </summary>
        /// <param name="playerData"></param>
        /// <returns>Tries to return a list of <c>PlayerData</c></returns>
        public static bool TryLoadPlayerData(out List<PlayerData> playerData)
        {
            playerData = new List<PlayerData>();

            if (!File.Exists(SavedPath)) return false;

            var formatter = new BinaryFormatter();

            var fileStream = new FileStream(SavedPath, FileMode.Open);

            playerData = formatter.Deserialize(fileStream) as List<PlayerData>;

            fileStream.Close();

            return true;
        }

        /// <summary>
        /// Static Method <c>SetCurrentPlayerData</c> Set current player data.
        /// </summary>
        /// <param name="playerData"></param>
        public static void SetCurrentPlayerData(PlayerData playerData)
        {
            _currentPlayerData = playerData;
            SavePlayer(_currentPlayerData);
        }

        /// <summary>
        /// Static Method <c>UpdateStaticScore</c> Update static saved score.
        /// </summary>
        /// <param name="score"></param>
        public static void UpdateStaticScore(float score)
        {
            if (_currentPlayerData == null) return;

            StaticScoreData.SetValue(score);
            SavePlayerScore();
            SavePlayer(_currentPlayerData);
        }

        /// <summary>
        /// Static Method <c>UpdateExtraScoreData</c> Finds and updates extra score data with an ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public static void UpdateExtraScoreData(string id, float value)
        {
            if (!TryGetExtraScoreFromId(id, out var extraScoreDataFound)) return;

            extraScoreDataFound.SetValue(value);
        }

        /// <summary>
        /// Static TryGet Method <c>UpdateExtraScoreData</c> Find a extra score data with an ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="extraScoreData"></param>
        /// <returns>Returns true if found extra data</returns>
        public static bool TryGetExtraScoreFromId(string id, out ExtraScoreData extraScoreData)
        {
            extraScoreData = ScoreDynamicMenu
                .Instance
                .extraScoreData
                .FirstOrDefault(sd => sd.scoreID == id);

            return extraScoreData != null;
        }

        /// <summary>
        /// Static Method <c>ResetStaticScore</c> Reset static save score.
        /// </summary>
        public static void ResetStaticScore()
        {
            StaticScoreData.ResetValue();
        }

        /// <summary>
        /// Static Get Method <c>GetCurrentPlayerDataBestScore</c> Get current players best score.
        /// </summary>
        /// <returns>Returns current players best score as <c>PlayerData</c></returns>
        public static PlayerData GetCurrentPlayerDataBestScore()
        {
            SavePlayerScore();
            return _currentPlayerData;
        }

        /// <summary>
        /// Static Method <c>SavePlayerScore</c> Save player score if larger.
        /// </summary>
        private static void SavePlayerScore()
        {
            if (!(StaticScoreData.Value >= _currentPlayerData.Score)) return;

            _currentPlayerData.Score = StaticScoreData.Value;
            _currentPlayerData.ScoreFormatted = StaticScoreData.GetAsStringFormatted();
        }

        /// <summary>
        /// Static Method <c>SavePlayer</c> Save player data to local storage.
        /// </summary>
        /// <param name="playerData"></param>
        private static void SavePlayer(PlayerData playerData)
        {
            var loadedPlayerData
                = TryLoadPlayerData(out var playersData);

            var formatter = new BinaryFormatter();
            var fileStream = new FileStream(SavedPath, FileMode.Create);

            if (loadedPlayerData)
            {
                var foundPlayer = UpdatePlayerInList(ref playersData, ref playerData);
                if (!foundPlayer) playersData.Add(playerData);
            }
            else playersData.Add(playerData);

            formatter.Serialize(fileStream, playersData);
            fileStream.Close();
        }

        /// <summary>
        /// Static Method <c>UpdatePlayerFromList</c> Updates Player in the List if Score is better.
        /// </summary>
        /// <param name="playerList"></param>
        /// <param name="playerData"></param>
        /// <returns>Returns true if found player</returns>
        private static bool UpdatePlayerInList(ref List<PlayerData> playerList, ref PlayerData playerData)
        {
            var currentPlayerGuid = playerData.Guid;

            var foundPlayerData
                = playerList.FirstOrDefault(pd => pd.Guid == currentPlayerGuid);

            if (foundPlayerData == null) return false;

            if (playerData.Score > foundPlayerData.Score)
            {
                foundPlayerData.Score = playerData.Score;
                foundPlayerData.ScoreFormatted = playerData.ScoreFormatted;
            }
            else playerData.ScoreFormatted = foundPlayerData.ScoreFormatted;

            return true;
        }
    }
}