using MenuComponents.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MenuComponents.DataInput
{
    /// <summary>
    /// Class <c>DataField</c> Component to store data field information
    /// </summary>
    [RequireComponent(typeof(TMP_InputField))]
    public class DataField : Validator
    {
        [SerializeField] private string dataName;
        [SerializeField] private Validation validation;

        private TMP_InputField _inputField;
        private ToggleValidationSprite _toggleValidationSprite;

        public string DataName => dataName;
        public string DataValue => _inputField.text;

        private bool _hasToggleValidationSprite;


        private void Awake()
        {
            _inputField = GetComponent<TMP_InputField>();
            _hasToggleValidationSprite = TryGetComponent(out _toggleValidationSprite);
        }

        /// <summary>
        /// Get Method <c>Validate</c> Validates the data field using <c>Validation</c> assigned.
        /// </summary>
        /// <returns></returns>
        public override bool Validate()
        {
            var valid = validation.Validate(DataValue);

            if (_hasToggleValidationSprite) _toggleValidationSprite.ToggleSprite(valid);

            return valid;
        }
    }
}