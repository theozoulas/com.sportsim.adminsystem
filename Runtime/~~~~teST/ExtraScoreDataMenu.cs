using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class ExtraScoreDataMenu : GlobalConfig<ExtraScoreDataMenu>
{
    [Title("Extra Score Data")]
    [PropertyOrder(-2)]
    [ListDrawerSettings(IsReadOnly = false, ShowFoldout = false, HideAddButton = true, DraggableItems = false,
        CustomRemoveElementFunction = "DeleteAsset")]
    [LabelText("  ")]
    public List<ScoreData> ExtraScoreData;
    
    [InlineEditor(InlineEditorObjectFieldModes.Hidden)]
    [BoxGroup("Create New Extra Score Data", GroupID = "newItem")]
    public ScoreData newExtraScoreData;
    
    [PropertySpace(20)]
    [BoxGroup("Create New Extra Score Data", GroupID = "newItem")]
    [LabelText("Name")]
    [OnValueChanged("ResetError")]
    public string newExtraScoreDataName;

    [UsedImplicitly] private bool _error;

    public Dictionary<string, ScoreData> ExtraScoreDataDic =>
        ExtraScoreData.ToDictionary(sd => sd.key, sd => sd);

    [PropertySpace(20)]
    [InfoBox("Extra Score Data Name Is Empty Or Already In Use!", InfoMessageType.Error, VisibleIf = "@_error")]
    [Button("Create", ButtonSizes.Large)]
    [BoxGroup("Create Extra Score Data", GroupID = "newItem")]
    private void CreateNewTextItemDynamicData()
    {
        _error = false;

        if (AssetDatabase.LoadAssetAtPath<ScoreData>(
                $"Assets/Resources/AdminSystem/ExtraScoreData/{newExtraScoreDataName}.asset") != null)
        {
            _error = true;
            return;
        }

        if (newExtraScoreDataName == string.Empty ||
            ExtraScoreDataDic.ContainsKey(newExtraScoreDataName))
        {
            newExtraScoreData.key = "";
            _error = true;
            return;
        }

        newExtraScoreData.key = newExtraScoreDataName;

        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            AssetDatabase.CreateFolder("Assets", "Resources");

        if (!AssetDatabase.IsValidFolder("Assets/Resources/AdminSystem"))
            AssetDatabase.CreateFolder("Assets/Resources", "AdminSystem");

        if (!AssetDatabase.IsValidFolder("Assets/Resources/AdminSystem/ExtraScoreData"))
            AssetDatabase.CreateFolder("Assets/Resources/AdminSystem", "ExtraScoreData");

        AssetDatabase.CreateAsset(newExtraScoreData,
            $"Assets/Resources/AdminSystem/ExtraScoreData/{newExtraScoreDataName}.asset");
        AssetDatabase.SaveAssets();

        ExtraScoreData.Add(newExtraScoreData);

        newExtraScoreData = CreateInstance<ScoreData>();
        newExtraScoreDataName = "New Extra Score Data";
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
        ExtraScoreData.Remove(asset as ScoreData);

        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(asset));
        AssetDatabase.SaveAssets();
    }

    [OnInspectorInit]
    private void InspectorInit()
    {
        ExtraScoreData = Resources.LoadAll<ScoreData>("AdminSystem/ExtraScoreData").ToList();

        newExtraScoreData = CreateInstance<ScoreData>();
        newExtraScoreDataName = "New Extra Score Data";
        var textItemData = DefaultTextItemTree.Instance.defaultItemData[0];
        //newCustomTextItemData.defaultFont = textItemData.defaultFont;
        //newCustomTextItemData.groupData = textItemData.groupData;

        ResetError();
    }

    private void ResetError()
    {
        _error = false;
    }
}