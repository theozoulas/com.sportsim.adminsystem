using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using MenuComponents.DynamicSystem;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class DefaultMenuItemTree : GlobalConfig<DefaultMenuItemTree>
{
    [Title("Default Menu Item Data")] [ListDrawerSettings(IsReadOnly = true, ShowFoldout = false)] [LabelText("  ")]
    public MenuItemData[] defaultMenuItemData;

    [UsedImplicitly] private bool _error;

    public Dictionary<string, MenuItemData> DefaultMenuItemDataDic =>
        defaultMenuItemData.ToDictionary(cd => cd.key, cd => cd);

    /// <summary>
    /// Refresh all scripts which will call OnValidate().
    /// </summary>
    [Button(50)]
    [Title("Utility")]
    private void Apply()
    {
        EditorUtility.RequestScriptReload();
    }

    private readonly MenuItemData[] _defaultMenuItemDataReference =
    {
        new("Background", Color.white, true),
        new ButtonMenuItem("Button", Color.white, false),
        new ButtonMenuItem("Splash Button", Color.white, false),
        new DataEntryItem("Data Entry Input", Color.white, false),
        new("Keyboard Special Button", Color.white, true),
        new LeaderboardEntryItem("Leaderboard Entry", Color.white, false),
        new LeaderboardEntryItem("sdfsd Entry", Color.white, false),
    };

    [OnInspectorInit]
    private void InspectorInit()
    {
        if (defaultMenuItemData.Length == _defaultMenuItemDataReference.Length) return;

        defaultMenuItemData = _defaultMenuItemDataReference;
    }
}