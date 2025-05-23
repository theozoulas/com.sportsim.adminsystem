using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
using static UnityEditor.AssetDatabase;
#endif    

[GlobalConfig("Assets/Resources/AdminSystem/ConfigFiles/")]
public class DefaultDataEntryInputTree : GlobalConfig<DefaultDataEntryInputTree>
{
#if UNITY_EDITOR
    [Title("Default Data Entry Item Data")] [ListDrawerSettings(IsReadOnly = true, ShowFoldout = false)] [LabelText("  ")]
    public DataEntryInputData[] defaultItemData;

    [UsedImplicitly] private bool _error;

    public Dictionary<string, DataEntryInputData> DefaultItemDataDic =>
        defaultItemData.ToDictionary(cd => cd.key, cd => cd);
    
    private const string ItemsPath =
        "Packages/com.sportsim.adminsystem/Runtime/MenuComponents/ScriptableObjects/DataEntry/Input";
    

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
        var items = FindAssets("t:ScriptableObject", new[] { ItemsPath });

        defaultItemData = items.Select(a => LoadAssetAtPath<DataEntryInputData>(GUIDToAssetPath(a))).ToArray();
    }
    
#endif    
}
