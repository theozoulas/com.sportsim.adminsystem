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

public class CustomTextItemTree : GlobalConfig<CustomTextItemTree>
{
    [Title("Custom Text Item Data")]
    [PropertyOrder(-2)]
    [ListDrawerSettings(IsReadOnly = false, ShowFoldout = false, HideAddButton = true, DraggableItems = false,
        CustomRemoveElementFunction = "DeleteAsset")]
    [LabelText("  ")]
    public List<CustomTextItemData> customTextItemDynamicData;
    
    [InlineEditor(InlineEditorObjectFieldModes.Hidden)]
    [BoxGroup("Create New Custom Text Item Data", GroupID = "newItem")]
    public CustomTextItemData newCustomTextItemData;
    
    [PropertySpace(20)]
    [BoxGroup("Create New Custom Text Item Data", GroupID = "newItem")]
    [LabelText("Name")]
    [OnValueChanged("ResetError")]
    public string newCustomTextItemDataName;

    [UsedImplicitly] private bool _error;

    public Dictionary<string, CustomTextItemData> CustomTextItemDataDic =>
        customTextItemDynamicData.ToDictionary(cd => cd.key, cd => cd);

    [PropertySpace(20)]
    [InfoBox("Custom Text Item Data Name Is Empty Or Already In Use!", InfoMessageType.Error, VisibleIf = "@_error")]
    [Button("Create", ButtonSizes.Large)]
    [BoxGroup("Create New Text Item Data", GroupID = "newItem")]
    private void CreateNewTextItemDynamicData()
    {
        _error = false;

        if (AssetDatabase.LoadAssetAtPath<CustomTextItemData>(
                $"Assets/Resources/AdminSystem/TextItemData/{newCustomTextItemDataName}.asset") != null)
        {
            _error = true;
            return;
        }

        if (newCustomTextItemDataName == string.Empty ||
            CustomTextItemDataDic.ContainsKey(newCustomTextItemDataName))
        {
            newCustomTextItemData.key = "";
            _error = true;
            return;
        }

        newCustomTextItemData.key = newCustomTextItemDataName;

        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            AssetDatabase.CreateFolder("Assets", "Resources");

        if (!AssetDatabase.IsValidFolder("Assets/Resources/AdminSystem"))
            AssetDatabase.CreateFolder("Assets/Resources", "AdminSystem");

        if (!AssetDatabase.IsValidFolder("Assets/Resources/AdminSystem/TextItemData"))
            AssetDatabase.CreateFolder("Assets/Resources/AdminSystem", "TextItemData");

        AssetDatabase.CreateAsset(newCustomTextItemData,
            $"Assets/Resources/AdminSystem/TextItemData/{newCustomTextItemDataName}.asset");
        AssetDatabase.SaveAssets();

        customTextItemDynamicData.Add(newCustomTextItemData);

        newCustomTextItemData = CreateInstance<CustomTextItemData>();
        newCustomTextItemDataName = "New Custom Text Item Data";
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
        customTextItemDynamicData.Remove(asset as CustomTextItemData);

        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(asset));
        AssetDatabase.SaveAssets();
    }

    [OnInspectorInit]
    private void InspectorInit()
    {
        customTextItemDynamicData = Resources.LoadAll<CustomTextItemData>("AdminSystem/TextItemData").ToList();

        newCustomTextItemData = CreateInstance<CustomTextItemData>();
        newCustomTextItemDataName = "New Custom Text Item Data";
        var textItemData = DefaultTextItemTree.Instance.defaultItemData[0];
        newCustomTextItemData.defaultFont = textItemData.defaultFont;
        newCustomTextItemData.groupData = textItemData.groupData;

        ResetError();
    }

    private void ResetError()
    {
        _error = false;
    }
}
