using System.Collections;
using System.Collections.Generic;
using MenuComponents.DynamicSystem;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

public  class BaseDynamicManager : GlobalConfig<BaseDynamicManager>
{
    /// <summary>
    /// Refresh all scripts which will call OnValidate().
    /// </summary>
    [Button(50)]
    private void Refresh()
    {
        EditorUtility.RequestScriptReload();
    }
}
