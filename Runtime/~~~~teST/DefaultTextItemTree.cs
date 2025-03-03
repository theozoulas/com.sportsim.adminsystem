using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using static UnityEditor.AssetDatabase;

public class DefaultTextItemTree : GlobalConfig<DefaultTextItemTree>
{
    [Title("Default Text Item Data")] [ListDrawerSettings(IsReadOnly = true, ShowFoldout = false)] [LabelText("  ")]
    public TextItemData[] defaultItemData;

    [UsedImplicitly] private bool _error;

    public Dictionary<string, TextItemData> DefaultItemDataDic =>
        defaultItemData.ToDictionary(cd => cd.key, cd => cd);
    
    private const string ItemsPath =
        "Packages/com.sportsim.adminsystem/Runtime/MenuComponents/ScriptableObjects/TextItem";

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

        defaultItemData = items.Select(a => LoadAssetAtPath<TextItemData>(GUIDToAssetPath(a))).ToArray();
    }
}
