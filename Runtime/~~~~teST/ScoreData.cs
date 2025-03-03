using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
// ReSharper disable All

[CreateAssetMenu(fileName = "ScoreData", menuName = "ScriptableObjects/ScoreData", order = 1)]
[InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
public class ScoreData : SerializedScriptableObject
{
  
    [TabGroup("$key/Item", "Settings", SdfIconType.GearFill)]
    [TypeFilter("GetFilteredTypeList")] public ScoreFormat Format = new DefaultFormat();
    
    [TitleGroup("$key")] [TabGroup("$key/Item", "Settings", SdfIconType.Eyedropper)]
    public bool hasMinValue;
    
    [ShowIf("hasMinValue")][TabGroup("$key/Item", "Settings", SdfIconType.Eyedropper)] public float minValue;
    
    [TabGroup("$key/Item", "Settings", SdfIconType.Eyedropper)]
    public bool hasMaxValue;
    
    [ShowIf("hasMaxValue")][TabGroup("$key/Item", "Settings", SdfIconType.Eyedropper)]public float maxValue;

    public float Value { get; private set; }
    private bool _hasBeenSet;
    
    [HideInInlineEditors]
    public string key;

    private void OnEnable()
    {
        ResetValue();
    }

    public IEnumerable<Type> GetFilteredTypeList()
    {
        var q = typeof(ScoreFormat).Assembly.GetTypes()
            .Where(x => !x.IsAbstract) 
            .Where(x => !x.IsGenericTypeDefinition) 
            .Where(x => typeof(ScoreFormat).IsAssignableFrom(x));

        return q;
    }

    public void ResetValue()
    {
        Value = ScoreDynamicMenu.Instance.sortScoreBy == ScoreDynamicMenu.ScoreSort.Highest
            ? float.MinValue
            : float.MaxValue;

        _hasBeenSet = false;
    }

    public void SetValue(float value)
    {
        if (hasMinValue && value < minValue) return;

        if (hasMaxValue && value > maxValue) return;

        Value = value;

        _hasBeenSet = true;
    }

    public string GetAsStringFormatted()
    {
        return Format.GetValueStringFormatted(Value, _hasBeenSet);
    }
}