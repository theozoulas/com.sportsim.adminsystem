using UnityEngine;

namespace MenuComponents.ScriptableObjects
{
    /// <summary>
    /// Scriptable Object <c>EmailValidation</c>
    /// </summary>
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Email Validation Object", order = 1)]  
    public class EmailValidation : Validation
    {
        /// <summary>
        /// Get Method <c>Validate</c> Validates Input.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Returns results of validation by <c>bool</c></returns>
        public override bool Validate(string input)
        {
            return base.Validate(input) && input.Contains("@") && input.Contains(".");
        }
    }
}
