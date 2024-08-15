using System;
using UnityEngine;

namespace MenuComponents.Components.Keyboard
{
    /// <summary>
    /// Static Class <c>StaticManager</c> Manages Keyboard.
    /// </summary>
    public static class StaticManager 
    {
        public static OnScreenKeyboard Keyboard { get; private set; }

        public static Action onKeyboardAdded = delegate { };

        
        /// <summary>
        /// Static Method <c>ReferenceKeyboard</c> Assign static reference to the current loaded keyboard,
        /// otherwise will show a warning log if already set.
        /// </summary>
        /// <param name="keyboard"></param>
        public static void ReferenceKeyboard(OnScreenKeyboard keyboard)
        {
            if (Keyboard == null)
            {
                Keyboard = keyboard;
                onKeyboardAdded();
            }
            else Debug.LogWarning("Keyboard is Already Set!");
        }

        /// <summary>
        /// Static Method <c>ClearKeyboard</c> Clears the reference to keyboard by setting it to null.
        /// </summary>
        public static void ClearKeyboard() => Keyboard = null;
    }
}
