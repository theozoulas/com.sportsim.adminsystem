using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
#if UNITY_EDITOR
using static UnityEditor.AssetDatabase;
#endif    

[GlobalConfig("Assets/Resources/AdminSystem/ConfigFiles/")]
public class DefaultMenuItemTree : GlobalConfig<DefaultMenuItemTree>
{
#if UNITY_EDITOR
    [Title("Default Menu Item Data")] [ListDrawerSettings(IsReadOnly = true, ShowFoldout = false)] [LabelText("  ")]
    public MenuItemData[] defaultItemData;

    [UsedImplicitly] private bool _error;

    public Dictionary<string, MenuItemData> DefaultItemDataDic =>
        defaultItemData.ToDictionary(cd => cd.key, cd => cd);
    
    private const string ItemsPath =
    "Packages/com.sportsim.adminsystem/Runtime/MenuComponents/ScriptableObjects/MenuItem";

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

        defaultItemData = items.Select(a => LoadAssetAtPath<MenuItemData>(GUIDToAssetPath(a))).ToArray();
    }
#endif    
}