#if UNITY_EDITOR
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
using static UnityEditor.AssetDatabase;


[GlobalConfig("Assets/Resources/AdminSystem/ConfigFiles/")]
public class CustomDataEntryItemTree : GlobalConfig<CustomDataEntryItemTree>
{
    [Title("Custom Data Entry Item Data")]
    [PropertyOrder(-2)]
    [ListDrawerSettings(IsReadOnly = false, ShowFoldout = false, HideAddButton = true, DraggableItems = false,
        CustomRemoveElementFunction = "DeleteAsset")]
    [LabelText("  ")]
    public List<CustomDataEntryItemData> customDataEntryItemDynamicData;

    [InlineEditor(InlineEditorObjectFieldModes.Hidden)]
    [BoxGroup("Create New Custom Data Entry Item Data", GroupID = "newItem")]
    public CustomDataEntryItemData newCustomDataEntryItemData;

    [PropertySpace(20)]
    [BoxGroup("Create New Custom Data Entry Item Data", GroupID = "newItem")]
    [LabelText("Name")]
    [OnValueChanged("ResetError")]
    public string newCustomDataEntryItemDataName;

    [UsedImplicitly] private bool _error;

    public Dictionary<string, CustomDataEntryItemData> CustomDataEntryItemDataDic =>
        customDataEntryItemDynamicData.ToDictionary(cd => cd.key, cd => cd);


    [PropertySpace(20)]
    [InfoBox("Custom Data Entry Item Data Name Is Empty Or Already In Use!", InfoMessageType.Error,
        VisibleIf = "@_error")]
    [Button("Create", ButtonSizes.Large)]
    [BoxGroup("Create New Data Entry Item Data", GroupID = "newItem")]
    private void CreateNewDataEntryItemDynamicData()
    {
        _error = false;

        if (LoadAssetAtPath<CustomDataEntryItemData>(
                $"Assets/Resources/AdminSystem/DataEntryItemData/{newCustomDataEntryItemDataName}.asset") != null)
        {
            _error = true;
            return;
        }

        if (newCustomDataEntryItemDataName == string.Empty ||
            CustomDataEntryItemDataDic.ContainsKey(newCustomDataEntryItemDataName))
        {
            newCustomDataEntryItemData.key = "";
            _error = true;
            return;
        }

        newCustomDataEntryItemData.key = newCustomDataEntryItemDataName;

        if (!IsValidFolder("Assets/Resources"))
            CreateFolder("Assets", "Resources");

        if (!IsValidFolder("Assets/Resources/AdminSystem"))
            CreateFolder("Assets/Resources", "AdminSystem");

        if (!IsValidFolder("Assets/Resources/AdminSystem/DataEntryItemData"))
            CreateFolder("Assets/Resources/AdminSystem", "DataEntryItemData");

        CreateAsset(newCustomDataEntryItemData,
            $"Assets/Resources/AdminSystem/DataEntryItemData/{newCustomDataEntryItemDataName}.asset");
        SaveAssets();

        customDataEntryItemDynamicData.Add(newCustomDataEntryItemData);

        newCustomDataEntryItemData = CreateInstance<CustomDataEntryItemData>();
        newCustomDataEntryItemDataName = "New Custom Data Entry Item Data";
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
        customDataEntryItemDynamicData.Remove(asset as CustomDataEntryItemData);

        AssetDatabase.DeleteAsset(GetAssetPath(asset));
        SaveAssets();
    }

    [OnInspectorInit]
    private void InspectorInit()
    {
        customDataEntryItemDynamicData =
            Resources.LoadAll<CustomDataEntryItemData>("AdminSystem/DataEntryItemData").ToList();

        newCustomDataEntryItemData = CreateInstance<CustomDataEntryItemData>();
        newCustomDataEntryItemDataName = "New Custom Data Entry Item Data";
        newCustomDataEntryItemData.defaultSprite = DefaultDataEntryItemTree.Instance.defaultItemData[0].defaultSprite;

        ResetError();
    }

    private void ResetError()
    {
        _error = false;
    }
}

#endif