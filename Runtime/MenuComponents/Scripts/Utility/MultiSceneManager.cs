using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.SceneManagement.SceneManager;

namespace MenuComponents.Utility
{
    /// <summary>
    /// Class <c>MultiSceneManager</c> Used to load multiple scene at once.
    /// </summary>
    public class MultiSceneManager : MonoBehaviour
    {
        [SerializeField] private SceneReference[] menuScenes;


        private void Start()
        {
            foreach (var scene in menuScenes)
            {
                var sceneToLoad = GetSceneByPath(scene.ScenePath);

                if (!sceneToLoad.isLoaded) LoadScene(scene, LoadSceneMode.Additive);
            }
        }
    }
}