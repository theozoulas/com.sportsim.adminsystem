using UnityEngine;

namespace MenuComponents.DataInput
{
    public abstract class Validator : MonoBehaviour
    {
        /// <summary>
        /// Abstract Method used for validation.
        /// </summary>
        /// <returns>Returns validation as a bool</returns>
        public abstract bool Validate();
    }
}
