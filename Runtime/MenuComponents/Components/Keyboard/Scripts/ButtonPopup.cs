using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MenuComponents.Components.Keyboard
{
    /// <summary>
    /// Class <c>PopupInputField</c> Assigns a callback to show the keyboard on a input field.
    /// </summary>
    [RequireComponent(typeof(Button))]
    internal class ButtonPopup : Popup
    {
        private TMP_InputField _inputField;

        private Button _button;


        /// <summary>
        /// Override Method <c>SetKeyboardListener</c> Overrides the keyboard listener
        /// to show keyboard on Button clicked.
        /// </summary>
        protected override void SetKeyboardListener()
        {
            base.SetKeyboardListener();

            if (!IsOnScreenKeyboardActive) return;

            _inputField = GetComponentInParent<TMP_InputField>();

            if (_inputField == null)
            {
                Debug.LogError($"{_button.name} Does Not Contain An InputField As Parent!");
                return;
            }
            
            _button = GetComponent<Button>();
            
            _button.onClick.RemoveListener(SetInputFieldAndShowKeyboard);
            _button.onClick.AddListener(SetInputFieldAndShowKeyboard);
        }

        /// <summary>
        /// Set Method <c>SetInputFieldAndShowKeyboard</c> Sets the keyboard's InputField and Shows it.,
        /// </summary>
        private void SetInputFieldAndShowKeyboard()
        {
            Keyboard.SetInputFieldToPopulate(_inputField);
            Keyboard.Show();
        }
    }
}