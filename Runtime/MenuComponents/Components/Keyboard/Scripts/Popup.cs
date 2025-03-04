using UnityEngine;

namespace MenuComponents.Components.Keyboard
{
    /// <summary>
    /// Class <c>Popup</c> Base class for keyboard popup.
    /// </summary>
    internal abstract class Popup : MonoBehaviour
    {
        protected static IKeyboard Keyboard => StaticManager.Keyboard;
        protected static bool IsOnScreenKeyboardActive => Keyboard != null;

        private bool _initialized;

        
        private void Awake()
        {
            StaticManager.OnKeyboardAdded += SetKeyboardListener;
        }

        private void OnDestroy()
        {
            StaticManager.OnKeyboardAdded -= SetKeyboardListener;
        }

        private void OnEnable()
        {
            if(!_initialized) SetKeyboardListener();
        }

        /// <summary>
        /// Virtual Method <c>SetKeyboardListener</c> Set the keyboard listener
        /// if no keyboard then display warning log. 
        /// </summary>
        protected virtual void SetKeyboardListener()
        {
            if (!IsOnScreenKeyboardActive && _initialized)
                Debug.LogWarning("No Keyboard Found!");
            else _initialized = true;
        }
    }
}

