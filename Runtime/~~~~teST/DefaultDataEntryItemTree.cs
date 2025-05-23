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
public class DefaultDataEntryItemTree : GlobalConfig<DefaultDataEntryItemTree>
{
#if UNITY_EDITOR
    [ListDrawerSettings(IsReadOnly = true, ShowFoldout = false)] [LabelText("  ")]
    public DataEntryItemData[] defaultItemData;

    [UsedImplicitly] private bool _error;

    public Dictionary<string, DataEntryItemData> DefaultItemDataDic =>
        defaultItemData.ToDictionary(cd => cd.key, cd => cd);
    
    private const string ItemsPath =
        "Packages/com.sportsim.adminsystem/Runtime/MenuComponents/ScriptableObjects/DataEntry/UI";

   

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

        defaultItemData = items.Select(a => LoadAssetAtPath<DataEntryItemData>(GUIDToAssetPath(a))).ToArray();
    }
#endif    
}