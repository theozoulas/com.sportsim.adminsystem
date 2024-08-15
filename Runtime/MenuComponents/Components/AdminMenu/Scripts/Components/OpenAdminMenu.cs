using UnityEngine;

namespace MenuComponents.Components.AdminMenu
{
    /// <summary>
    /// Class <c>OpenAdminMenu</c> Used to open admin menu.
    /// </summary>
    public class OpenAdminMenu : MonoBehaviour
    {
        [SerializeField] private GameObject pauseOverlay;

        private int _inputDetected;
    

        /// <summary>
        /// Method <c>ShowAdminPanel</c> Shows the admin menu if button clicked more or equal to 6 times.
        /// </summary>
        /// <param name="panel"></param>
        public void ShowAdminPanel(GameObject panel)
        {
            _inputDetected++;

            if (_inputDetected <= 6) return;

            pauseOverlay.SetActive(true);
            panel.SetActive(true);
            gameObject.SetActive(false);

            _inputDetected = 0;
        }
    }
}