using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace MenuComponents.Components.AdminMenu
{
    /// <summary>
    /// Class <c>PasswordManager</c> Manages the password for the admin menu.
    /// </summary>
    public class PasswordManager : MonoBehaviour
    {
        [SerializeField] private GameObject adminPanel;
        
        private TMP_InputField _passwordField;
        

        private void Start()
        {
            _passwordField = transform.GetComponentInParent<TMP_InputField>();
        }

        /// <summary>
        /// Method <c>ShowPassword</c> Show password in the TextMeshPro Input.
        /// </summary>
        [UsedImplicitly]
        public void ShowPassword()
        {
            _passwordField.contentType =  TMP_InputField.ContentType.Standard;
        }

        /// <summary>
        /// Method <c>HidePassword</c> Hides the password in the TextMeshPro Input with '*'.
        /// </summary>
        [UsedImplicitly]
        public void HidePassword()
        {
            _passwordField.contentType = TMP_InputField.ContentType.Pin;
        }

        /// <summary>
        /// Method <c>ResetPasswordText</c> Reset password to empty.
        /// </summary>
        private void ResetPasswordText()
        {
            _passwordField.text = "";
        }
        
        
        /// <summary>
        /// Method <c>CheckPassword</c> Check to see if password is correct then open admin panel.
        /// </summary>
        public void CheckPassword()
        {
            if(_passwordField.text == "6682")
            {
                ResetPasswordText();
                adminPanel.SetActive(true);
                _passwordField.text = "";
            }
            else
            {
                _passwordField.text = "ERROR WRONG PASSWORD!";
            }
        }
    }
}
