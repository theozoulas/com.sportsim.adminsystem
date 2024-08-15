using UnityEngine;

namespace MenuComponents.Components.AdminMenu
{
    /// <summary>
    /// Class <c>QuitGame</c> Used for exiting the game.
    /// </summary>
    public class QuitGame : MonoBehaviour
    {
        /// <summary>
        /// Method <c>Quit</c> Quits the game.
        /// </summary>
        public void Quit() => Application.Quit();
    }
}
