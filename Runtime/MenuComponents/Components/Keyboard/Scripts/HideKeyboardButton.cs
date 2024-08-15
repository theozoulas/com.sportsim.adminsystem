using UnityEngine;
using UnityEngine.UI;

namespace MenuComponents.Components.Keyboard
{
    /// <summary>
    /// Class <c>HideKeyboardButton</c> Assigns a callback to hide the keyboard on a button.
    /// </summary>
    [RequireComponent(typeof(Button))]
    internal class HideKeyboardButton : Popup
    {
        private Button _button;


        /// <summary>
        /// Override Method <c>SetKeyboardListener</c> Overrides the keyboard listener
        /// to hide keyboard on button click.
        /// </summary>
        protected override void SetKeyboardListener()
        {
            base.SetKeyboardListener();

            if (!IsOnScreenKeyboardActive) return;

            _button = GetComponent<Button>();
            _button.onClick.RemoveListener(delegate { Keyboard.WriteSpecialKey(3); });

            _button.onClick.AddListener(delegate { Keyboard.WriteSpecialKey(3); });
        }
    }
}