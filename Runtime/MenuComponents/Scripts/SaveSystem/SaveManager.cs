using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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

        public static PlayerData currentPlayerData;
        public static SavedData StaticSavedData { get; } = new SavedData();


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
        /// Static Method <c>UpdateStaticScore</c> Update static saved score.
        /// </summary>
        /// <param name="score"></param>
        public static void UpdateStaticScore(int score)
        {
            StaticSavedData.SavedScore = score;
        }

        /// <summary>
        /// Static Method <c>SaveCurrentPlayerData</c> Save the current player data.
        /// </summary>
        public static void SaveCurrentPlayerData()
        {
            SavePlayerScore();
            SavePlayer(currentPlayerData);
        }

        /// <summary>
        /// Static Method <c>ResetStaticScore</c> Reset static save score.
        /// </summary>
        public static void ResetStaticScore()
        {
            StaticSavedData.SavedScore = 0;
        }

        /// <summary>
        /// Static Get Method <c>GetCurrentPlayerDataBestScore</c> Get current players best score.
        /// </summary>
        /// <returns>Returns current players best score as <c>PlayerData</c></returns>
        public static PlayerData GetCurrentPlayerDataBestScore()
        {
            SavePlayerScore();
            return currentPlayerData;
        }

        /// <summary>
        /// Static Method <c>SavePlayerScore</c> Save player score if larger.
        /// </summary>
        public static void SavePlayerScore()
        {
            if (StaticSavedData.SavedScore >= currentPlayerData.Score)
                currentPlayerData.Score = StaticSavedData.SavedScore;
        }

        /// <summary>
        /// Static Method <c>SavePlayer</c> Save player data to local storage.
        /// </summary>
        /// <param name="playerData"></param>
        private static void SavePlayer(PlayerData playerData)
        {
            var playersData = LoadPlayerData() ?? new List<PlayerData>();

            var formatter = new BinaryFormatter();
            var fileStream = new FileStream(SavedPath, FileMode.Create);

            if (File.Exists(SavedPath)) playersData.Add(playerData);
            else playersData.Add(playerData);

            formatter.Serialize(fileStream, playersData);
            fileStream.Close();
        }
    }
}