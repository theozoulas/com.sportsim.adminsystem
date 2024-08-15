using UnityEngine;

namespace MenuComponents.Utility
{
    /// <summary>
    /// Class <c>ScreenResolutionManager</c> Manages screen Resolution ensuring it always consistent.
    /// </summary>
    public class ScreenResolutionManager : MonoBehaviour
    {
        private bool _hasBeenReset;
    
        
        private void Awake()
        {
            ResetResolution();
        }
        
        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.D))
            {
                if (_hasBeenReset) return;
                
                ResetResolution();
                
                _hasBeenReset = true;
            }
            else _hasBeenReset = false;
        }
        
        /// <summary>
        /// Static Method <c>ResetResolution</c> Resets resolution to 1080x1920.
        /// </summary>
        private static void ResetResolution() => Screen.SetResolution(1080, 1920, true);
    }
}
