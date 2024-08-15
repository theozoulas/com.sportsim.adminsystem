using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using static UnityEngine.SceneManagement.SceneManager;

namespace MenuComponents.Utility
{
    /// <summary>
    /// Class <c>SceneChangeManager</c> Manages Scene changes.
    /// </summary>
    public class SceneChangeManager : MonoBehaviour
    {
        public static SceneChangeManager Instance { get; private set; }
        private static int ActiveSceneIndex => GetActiveScene().buildIndex;

        private Coroutine _coroutine;
        private InitializeButtonAudio _initializeButtonAudio;

        private bool _isInitializeButtonAudioNotNull;


        private void Awake()
        {
            _initializeButtonAudio = FindObjectOfType<InitializeButtonAudio>();
            _isInitializeButtonAudioNotNull = _initializeButtonAudio != null;

            if (Instance == null) Instance = this;
            else Destroy(this);
        }

        /// <summary>
        /// Method <c>ChangeToSceneForward</c> Change to next scene after button audio finished.
        /// </summary>
        public void ChangeToSceneForward()
        {
            var indexToGoTo =
                ActiveSceneIndex < sceneCountInBuildSettings - 1 ? ActiveSceneIndex + 1 : 0;

            if (_coroutine == null)
                _coroutine = StartCoroutine(WaitButtonAudioThenLoadScene(indexToGoTo));
        }

        /// <summary>
        /// Method <c>ChangeToSceneBackwards</c> Change to previous scene after button audio finished.
        /// </summary>
        [UsedImplicitly]
        public void ChangeToSceneBackwards()
        {
            var indexToGoTo =
                ActiveSceneIndex < 0 ? ActiveSceneIndex + 1 : 0;

            if (_coroutine == null)
                _coroutine = StartCoroutine(WaitButtonAudioThenLoadScene(indexToGoTo));
        }

        /// <summary>
        /// Method <c>GoToGame</c> Go to the game scene after button audio finished.
        /// </summary>
        public void GoToGame()
        {
            if (_coroutine != null) return;
            
            _coroutine = StartCoroutine(WaitButtonAudioThenLoadScene(2));
        }
        
        /// <summary>
        /// Method <c>GoToMenuSavePlayerData</c> Go to main menu and save the current players data.
        /// </summary>
        public void GoToMenuSavePlayerData()
        {
            if (_coroutine != null) return;
            
            SaveSystem.SaveManager.SaveCurrentPlayerData();
            _coroutine = StartCoroutine(WaitButtonAudioThenLoadScene(0));

        }

        /// <summary>
        /// Coroutine <c>WaitButtonAudioThenLoadScene</c> Wait for the length of the button audio click to
        /// load scene.
        /// </summary>
        /// <param name="sceneIndex"></param>
        private IEnumerator WaitButtonAudioThenLoadScene(int sceneIndex)
        {
            yield return new WaitForSeconds(ButtonClipLength());

            LoadScene(sceneIndex);

            _coroutine = null;
        }

        /// <summary>
        /// Get Method <c>ButtonClipLength</c> Get button clip length.
        /// </summary>
        /// <returns>Returns length of button audio clip as <c>float</c></returns>
        private float ButtonClipLength()
        {
            if (_isInitializeButtonAudioNotNull) return _initializeButtonAudio.AudioLength;

            return 0;
        }
    }
}