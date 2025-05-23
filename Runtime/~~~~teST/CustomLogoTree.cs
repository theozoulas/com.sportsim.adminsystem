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
#if UNITY_EDITOR
using static UnityEditor.AssetDatabase;
#endif   

[GlobalConfig("Assets/Resources/AdminSystem/ConfigFiles/")]
public class CustomLogoTree : GlobalConfig<CustomLogoTree>
{
    [Title("Custom Logo Data")]
    [PropertyOrder(-2)]
    [ListDrawerSettings(IsReadOnly = false, ShowFoldout = false, HideAddButton = true, DraggableItems = false,
        CustomRemoveElementFunction = "DeleteAsset")]
    [LabelText("  ")]
    public List<CustomLogoData> customLogoDynamicData;
    
    [InlineEditor(InlineEditorObjectFieldModes.Hidden)]
    [BoxGroup("Create New Custom Logo Data", GroupID = "newItem")]
    public CustomLogoData newCustomLogoData;
    
    [PropertySpace(20)]
    [BoxGroup("Create New Custom Logo Data", GroupID = "newItem")]
    [LabelText("Name")]
    [OnValueChanged("ResetError")]
    public string newCustomLogoDataName;

    [UsedImplicitly] private bool _error;

    public Dictionary<string, CustomLogoData> CustomLogoDataDic =>
        customLogoDynamicData.ToDictionary(cd => cd.key, cd => cd);
    
#if UNITY_EDITOR

    [PropertySpace(20)]
    [InfoBox("Custom Logo Data Name Is Empty Or Already In Use!", InfoMessageType.Error, VisibleIf = "@_error")]
    [Button("Create", ButtonSizes.Large)]
    [BoxGroup("Create New Logo Data", GroupID = "newItem")]
    private void CreateNewMenuItemDynamicData()
    {
        _error = false;

        if (LoadAssetAtPath<CustomLogoData>(
                $"Assets/Resources/AdminSystem/LogoData/{newCustomLogoDataName}.asset") != null)
        {
            _error = true;
            return;
        }

        if (newCustomLogoDataName == string.Empty ||
            CustomLogoDataDic.ContainsKey(newCustomLogoDataName))
        {
            newCustomLogoData.key = "";
            _error = true;
            return;
        }

        newCustomLogoData.key = newCustomLogoDataName;

        if (!IsValidFolder("Assets/Resources"))
            CreateFolder("Assets", "Resources");

        if (!IsValidFolder("Assets/Resources/AdminSystem"))
            CreateFolder("Assets/Resources", "AdminSystem");

        if (!IsValidFolder("Assets/Resources/AdminSystem/LogoData"))
            CreateFolder("Assets/Resources/AdminSystem", "LogoData");

        CreateAsset(newCustomLogoData,
            $"Assets/Resources/AdminSystem/LogoData/{newCustomLogoDataName}.asset");
        SaveAssets();

        customLogoDynamicData.Add(newCustomLogoData);

        newCustomLogoData = CreateInstance<CustomLogoData>();
        newCustomLogoDataName = "New Custom Logo Data";
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
        customLogoDynamicData.Remove(asset as CustomLogoData);

        AssetDatabase.DeleteAsset(GetAssetPath(asset));
        SaveAssets();
    }

    [OnInspectorInit]
    private void InspectorInit()
    {
        customLogoDynamicData = Resources.LoadAll<CustomLogoData>("AdminSystem/LogoData").ToList();

        newCustomLogoData = CreateInstance<CustomLogoData>();
        newCustomLogoDataName = "New Custom Logo Data";
        newCustomLogoData.defaultLogo = DefaultLogoTree.Instance.defaultLogoData[0].defaultLogo;

        ResetError();
    }

    private void ResetError()
    {
        _error = false;
    }
#endif   
}
