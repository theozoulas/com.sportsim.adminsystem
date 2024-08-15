using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace MenuComponents.DynamicSystem
{
    public abstract class BaseDynamicData : ScriptableObject
    {
        /// <summary>
        /// Refresh all scripts which will call OnValidate().
        /// </summary>
        [Button(50)]
        public void Refresh()
        {
            EditorUtility.RequestScriptReload();
        } 
    }
}
