using TMPro;
using UnityEngine;

namespace MenuComponents.Components.Keyboard
{
    /// <summary>
    /// Class <c>PopupInputField</c> Assigns a callback to show the keyboard on a input field.
    /// </summary>
    [RequireComponent(typeof(TMP_InputField))]
    internal class PopupInputField : Popup
    {
        private enum KeyboardMode
        {
            Alphanumeric,
            Numpad,
            NumpadPin
        }

        [SerializeField] private KeyboardMode keyboardMode = KeyboardMode.Alphanumeric;

        private TMP_InputField _inputField;


        /// <summary>
        /// Override Method <c>SetKeyboardListener</c> Overrides the keyboard listener
        /// to show keyboard on input field clicked.
        /// </summary>
        protected override void SetKeyboardListener()
        {
            base.SetKeyboardListener();

            if (!IsOnScreenKeyboardActive) return;

            _inputField = GetComponent<TMP_InputField>();
            _inputField.onSelect.RemoveListener(delegate { SetListenerType(); });
            _inputField.onSelect.AddListener(delegate { SetListenerType(); });
        }

        /// <summary>
        /// Set Method <c>SetListenerType</c> Sets the keyboard type,
        /// there are three types (Numpad, NumpadPin, Alphanumeric)
        /// </summary>
        private void SetListenerType()
        {
            switch (keyboardMode)
            {
                case KeyboardMode.Numpad:
                    Keyboard.SetActiveFocusNumeric(_inputField);
                    break;
                case KeyboardMode.NumpadPin:
                    Keyboard.SetActiveFocusNumericPin(_inputField);
                    break;
                case KeyboardMode.Alphanumeric:
                    Keyboard.SetActiveFocus(_inputField);
                    break;
                default:
                    Keyboard.SetActiveFocus(_inputField);
                    break;
            }
        }
    }
}