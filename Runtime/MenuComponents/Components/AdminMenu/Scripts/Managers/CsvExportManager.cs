using System;
using System.IO;
using System.Linq;
using MenuComponents.SaveSystem;
using MenuComponents.Utility;
using TMPro;
using UnityEngine;

namespace MenuComponents.Components.AdminMenu
{
    /// <summary>
    /// Class <c>CsvExportManager</c> Manages exporting player data as a CSV file.
    /// </summary>
    public class CsvExportManager : MonoBehaviour
    {
        public static CsvExportManager instance;

        public TextMeshProUGUI errorLog;

        [SerializeField] private GameObject saveDialog;

        private int _numOfSaveDatafiles = 1;
        private string _desktopPath;


        private void Awake()
        {
            if (instance == null) instance = this;
            else Destroy(instance);

            saveDialog.GetComponentInChildren<TextMeshProUGUI>();
        }

        private void Start()
        {
            _desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            UpdateExistingCsvFileName();
        }

        /// <summary>
        /// Method <c>ClearData</c> Clear all player data by deleting local storage.
        /// </summary>
        public void ClearData()
        {
            if (File.Exists(SaveManager.SavedPath)) File.Delete(SaveManager.SavedPath);
        }

        /// <summary>
        /// Method <c>SaveDataToDesktop</c> Save desktop using the the desktop path plus name of the file.
        /// </summary>
        public void SaveDataToDesktop()
        {
            var savePath = _desktopPath + '\\' + FileName();

            SaveData(savePath);
        }

        /// <summary>
        /// Method <c>SaveDataWithDialog</c> Save data by opening the file browser save window.
        /// </summary>
        /// <param name="filePath"></param>
        public void SaveDataWithDialog(string filePath) => SaveData(filePath);

        /// <summary>
        /// Method <c>UpdateExistingCsvFileName</c> Updates the CVS file name by the number
        /// of CSV files on the desktop.
        /// </summary>
        private void UpdateExistingCsvFileName()
        {
            var existingSavedFiles = Directory.GetFiles(_desktopPath, "*.csv");

            foreach (var file in existingSavedFiles)
            {
                if (file.Contains(FileName())) ++_numOfSaveDatafiles;
            }
        }

        /// <summary>
        /// Method <c>SaveData</c> Save player data to CSV
        /// </summary>
        /// <param name="filePath"></param>
        private void SaveData(string filePath)
        {
            if (File.Exists(SaveManager.SavedPath))
            {
                errorLog.text = "";

                var playerData = SaveManager.LoadPlayerData();
                var file = File.Create(filePath);
                var writer = new StreamWriter(file);

                var csvTitles = 
                    playerData[0]
                        .DataFieldValues
                        .Aggregate("",
                            (current, dataFieldValue) =>
                                current + dataFieldValue.dataName.WriteCsvLine());

                writer.WriteLine($"{csvTitles}TimeRegistered,Score");

                foreach (var csvLine in playerData
                             .Select(player => new
                             {
                                 player,
                                 csvLine = player.DataFieldValues.Aggregate("",
                                     (current, dataFieldValue) =>
                                         current + dataFieldValue.dataValue.WriteCsvLine())
                             })
                             .Select(@t =>
                                 @t.csvLine + @t.player.TimeRegistered.WriteCsvLine() +
                                 @t.player.Score.ToString().WriteCsvLine()))
                {
                    writer.WriteLine(csvLine);
                }

                writer.Flush();
                writer.Close();
                saveDialog.SetActive(true);
            }
            else errorLog.text = "No Data to Export";
        }

        /// <summary>
        /// Get Method <c>FileName</c> Get the file name for the CSV file.
        /// </summary>
        /// <returns>Returns file name as <c>string</c></returns>
        private string FileName()
        {
            var version = _numOfSaveDatafiles > 1 ? "_V" + _numOfSaveDatafiles : "";
            var fileName = "SavedData_" + CurrentDate() + version;

            return fileName + ".csv";
        }

        /// <summary>
        /// Get Method <c>CurrentDate</c> Get the current date.
        /// </summary>
        /// <returns>Returns date as string</returns>
        private static string CurrentDate()
        {
            return DateTime.Today.Day +
                   "_" + DateTime.Today.Month +
                   "_" + DateTime.Today.Year;
        }
    }
}