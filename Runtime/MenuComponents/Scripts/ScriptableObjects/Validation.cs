using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Serialization;

namespace MenuComponents.ScriptableObjects
{
    /// <summary>
    /// Scriptable Object <c>Validation</c>
    /// </summary>
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Validation", order = 1)]
    public class Validation : ScriptableObject
    {
        public string regexPattern;

        /// <summary>
        /// Get Method <c>Validate</c> Validates Input.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Returns results of validation by <c>bool</c></returns>
        public bool Validate(string input)
        {
            return Regex.IsMatch(input, regexPattern);
        }
    }
}