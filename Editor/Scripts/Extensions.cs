using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace Editor.Scripts
{
    public static class Extensions 
    {
        public static void AddScriptableObjectsAtPath(this OdinMenuTree tree, string menuPath, IEnumerable<ScriptableObject> scriptableObjects)
        {
            foreach (var scriptableObject in scriptableObjects)
            {
                tree.AddObjectAtPath($"{menuPath}/{scriptableObject.name}", scriptableObject);
            }
        }
    }
}
