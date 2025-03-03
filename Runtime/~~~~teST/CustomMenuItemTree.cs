using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using MenuComponents.DynamicSystem;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class CustomMenuItemTree : GlobalConfig<CustomMenuItemTree>
{
    [Title("Custom Menu Item Data")]
    [PropertyOrder(-2)]
    [ListDrawerSettings(IsReadOnly = false, ShowFoldout = false, HideAddButton = true, DraggableItems = false,
        CustomRemoveElementFunction = "DeleteAsset")]
    [LabelText("  ")]
    public List<CustomMenuItemData> customMenuItemDynamicData;
    
    [InlineEditor(InlineEditorObjectFieldModes.Hidden)]
    [BoxGroup("Create New Custom Menu Item Data", GroupID = "newItem")]
    public CustomMenuItemData newCustomMenuItemData;
    
    [PropertySpace(20)]
    [BoxGroup("Create New Custom Menu Item Data", GroupID = "newItem")]
    [LabelText("Name")]
    [OnValueChanged("ResetError")]
    public string newCustomMenuItemDataName;

    [UsedImplicitly] private bool _error;

    public Dictionary<string, CustomMenuItemData> CustomMenuItemDataDic =>
        customMenuItemDynamicData.ToDictionary(cd => cd.key, cd => cd);

    [PropertySpace(20)]
    [InfoBox("Custom Menu Item Data Name Is Empty Or Already In Use!", InfoMessageType.Error, VisibleIf = "@_error")]
    [Button("Create", ButtonSizes.Large)]
    [BoxGroup("Create New Menu Item Data", GroupID = "newItem")]
    private void CreateNewMenuItemDynamicData()
    {
        _error = false;

        if (AssetDatabase.LoadAssetAtPath<CustomMenuItemData>(
                $"Assets/Resources/AdminSystem/MenuItemData/{newCustomMenuItemDataName}.asset") != null)
        {
            _error = true;
            return;
        }

        if (newCustomMenuItemDataName == string.Empty ||
            CustomMenuItemDataDic.ContainsKey(newCustomMenuItemDataName))
        {
            newCustomMenuItemData.key = "";
            _error = true;
            return;
        }

        newCustomMenuItemData.key = newCustomMenuItemDataName;

        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            AssetDatabase.CreateFolder("Assets", "Resources");

        if (!AssetDatabase.IsValidFolder("Assets/Resources/AdminSystem"))
            AssetDatabase.CreateFolder("Assets/Resources", "AdminSystem");

        if (!AssetDatabase.IsValidFolder("Assets/Resources/AdminSystem/MenuItemData"))
            AssetDatabase.CreateFolder("Assets/Resources/AdminSystem", "MenuItemData");

        AssetDatabase.CreateAsset(newCustomMenuItemData,
            $"Assets/Resources/AdminSystem/MenuItemData/{newCustomMenuItemDataName}.asset");
        AssetDatabase.SaveAssets();

        customMenuItemDynamicData.Add(newCustomMenuItemData);

        newCustomMenuItemData = CreateInstance<CustomMenuItemData>();
        newCustomMenuItemDataName = "New Custom Menu Item Data";
    }

    /// <summary>
    /// Refresh all scripts which will call OnValidate().
    /// </summary>
    [Button(50)]
    [Title("Utility")]
    [PropertyOrder(-1)]
    [PropertySpace(SpaceAfter = 50)]
    private void Apply()
    {
        EditorUtility.RequestScriptReload();
    }

    private void DeleteAsset(Object asset)
    {
        customMenuItemDynamicData.Remove(asset as CustomMenuItemData);

        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(asset));
        AssetDatabase.SaveAssets();
    }

    [OnInspectorInit]
    private void InspectorInit()
    {
        customMenuItemDynamicData = Resources.LoadAll<CustomMenuItemData>("AdminSystem/MenuItemData").ToList();

        newCustomMenuItemData = CreateInstance<CustomMenuItemData>();
        newCustomMenuItemDataName = "New Custom Menu Item Data";
        newCustomMenuItemData.defaultSprite = DefaultMenuItemTree.Instance.defaultItemData[0].defaultSprite;

        ResetError();
    }

    private void ResetError()
    {
        _error = false;
    }
}
