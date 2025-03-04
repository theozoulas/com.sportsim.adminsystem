using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MenuComponents.Components.Keyboard
{
    /// <summary>
    /// Static Class <c>StaticManager</c> Manages Keyboard.
    /// </summary>
    public static class StaticManager
    {
        public static IKeyboard Keyboard { get; private set; }
        public static TMP_InputField FirstInputSelected { get; private set; }

        public static Action OnKeyboardAdded = delegate { };


        /// <summary>
        /// Static Method <c>ReferenceKeyboard</c> Assign static reference to the current loaded keyboard,
        /// otherwise will show a warning log if already set.
        /// </summary>
        /// <param name="keyboard"></param>
        public static void ReferenceKeyboard(IKeyboard keyboard)
        {
            if (Keyboard == null)
            {
                Keyboard = keyboard;
                OnKeyboardAdded();
            }
            else Debug.LogWarning("Keyboard is Already Set!");
        }

        /// <summary>
        /// Static Method <c>ClearKeyboard</c> Clears the reference to keyboard by setting it to null.
        /// </summary>
        public static void ClearKeyboard() => Keyboard = null;

        /// <summary>
        /// Static Get Method <c>ClearKeyboard</c> Sets the InputField to be selelected.
        /// </summary>
        /// <param name="inputField"></param>
        public static void SetFirstInputSelected(TMP_InputField inputField) => FirstInputSelected = inputField;
    }
}

public interface IKeyboard
{
    void Show();
    void Hide();
    void SetInputFieldToPopulate(TMP_InputField inputField);
    void SetNavigationUpDown(NavigationUpDown navigationUpDown);
    Selectable TopButton { get; }
    void SetSpawnPosition(Vector3 transformPosition);
}