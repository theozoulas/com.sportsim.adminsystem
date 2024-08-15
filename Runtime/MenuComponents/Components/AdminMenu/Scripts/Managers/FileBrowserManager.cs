using System.Collections;
using System.IO;
using MenuComponents.SaveSystem;
using SimpleFileBrowser;
using UnityEngine;

namespace MenuComponents.Components.AdminMenu
{
    /// <summary>
    /// Class <c>FileBrowserManager</c> Manages the file browser window.
    /// </summary>
    public class FileBrowserManager : MonoBehaviour
    {
        private static void SetFileBrowserParent(Transform value) =>
            FileBrowser.FileBrowserParent = value;

        private void Awake()
        {
            SetFileBrowserParent(transform);
        }

        private void Start()
        {
            FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");
            FileBrowser.AddQuickLink("Users", "C:\\Users", null);
        }

        /// <summary>
        /// Method <c>OpenSaveDialog</c> Start the coroutine to open the file browser save window.
        /// </summary>
        public void OpenSaveDialog()
        {
            StartCoroutine(ShowSaveDialogCoroutine());
        }

        /// <summary>
        /// Coroutine <c>ShowSaveDialogCoroutine</c> Routine to open the file browser save window.
        /// </summary>
        private static IEnumerator ShowSaveDialogCoroutine()
        {
            FileBrowser.SetFilters(
                false,
                new FileBrowser.Filter("CSV", ".csv"));

            if (File.Exists(SaveManager.SavedPath))
            {
                CsvExportManager.instance.errorLog.text = "";

                yield return FileBrowser.WaitForSaveDialog(
                    false,
                    null,
                    "Save File");

                try
                {
                    CsvExportManager.instance.SaveDataWithDialog(FileBrowser.Result);
                }
                catch
                {
                    // ignored
                }
            }
            else CsvExportManager.instance.errorLog.text = "No Data to Export";
        }
    }
}