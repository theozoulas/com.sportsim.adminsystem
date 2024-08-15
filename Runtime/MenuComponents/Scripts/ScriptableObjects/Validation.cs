using UnityEngine;

namespace MenuComponents.ScriptableObjects
{
    /// <summary>
    /// Scriptable Object <c>Validation</c>
    /// </summary>
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Basic Validation Object", order = 1)]   
    public class Validation : ScriptableObject
    {
        /// <summary>
        /// Get Method <c>Validate</c> Validates Input.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Returns results of validation by <c>bool</c></returns>
        public virtual bool Validate(string input)
        {
            return input != string.Empty;
        }
    }
}
