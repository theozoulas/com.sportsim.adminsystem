using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using static UnityEditor.AssetDatabase;
#endif    

[GlobalConfig("Assets/Resources/AdminSystem/ConfigFiles/")]
public class DefaultLogoTree : GlobalConfig<DefaultLogoTree>
{
#if UNITY_EDITOR
    [Title("Default Logo Data")] [ListDrawerSettings(IsReadOnly = true, ShowFoldout = false)] [LabelText("  ")]
    public LogoData[] defaultLogoData;

    [UsedImplicitly] private bool _error;

    public Dictionary<string, LogoData> DefaultLogoDataDic =>
        defaultLogoData.ToDictionary(cd => cd.key, cd => cd);

    private const string LogoPath =
        "Packages/com.sportsim.adminsystem/Runtime/MenuComponents/ScriptableObjects/LogoData";

    /// <summary>
    /// Refresh all scripts which will call OnValidate().
    /// </summary>
    [Button(50)]
    [Title("Utility")]
    private void Apply()
    {
        EditorUtility.RequestScriptReload();
    }

    [OnInspectorInit]
    private void InspectorInit()
    {
        var items = FindAssets("t:ScriptableObject", new[] { LogoPath });

        defaultLogoData = items.Select(a => LoadAssetAtPath<LogoData>(GUIDToAssetPath(a))).ToArray();
    }
#endif    
}