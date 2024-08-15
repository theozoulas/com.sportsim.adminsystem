using System.Linq;
using UnityEngine;

namespace MenuComponents.ScriptableObjects
{
    /// <summary>
    /// Scriptable Object <c>NumberValidation</c>
    /// </summary>
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Number Validation Object", order = 1)]  
    public class NumberValidation : Validation
    {
        /// <summary>
        /// Get Method <c>Validate</c> Validates Input.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Returns results of validation by <c>bool</c></returns>
        public override bool Validate(string input)
        {
            return base.Validate(input) && IsStringDigit(input);
        }
        
        /// <summary>
        /// Get Method <c>IsStringDigit</c> Gets whether or not the string is all digits.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Returns result as <c>bool</c></returns>
        private static bool IsStringDigit(string input)
        {
            return input
                .Where(c => !char.IsDigit(c))
                .All(c => c == '+') && input != string.Empty;
        }
    }
}