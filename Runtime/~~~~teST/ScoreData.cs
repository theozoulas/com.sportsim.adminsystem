using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
// ReSharper disable All

[CreateAssetMenu(fileName = "ScoreData", menuName = "ScriptableObjects/ScoreData", order = 1)]
public class ScoreData : SerializedScriptableObject
{
    [Title("Settings")]
    [TypeFilter("GetFilteredTypeList")] public ScoreFormat Format = new DefaultFormat();
    
    public bool hasMinValue;
    [ShowIf("hasMinValue")] public float minValue;

    public bool hasMaxValue;
    [ShowIf("hasMaxValue")] public float maxValue;

    public float Value { get; private set; }
    private bool _hasBeenSet;

    private void OnEnable()
    {
        ResetValue();
    }

    public IEnumerable<Type> GetFilteredTypeList()
    {
        var q = typeof(ScoreFormat).Assembly.GetTypes()
            .Where(x => !x.IsAbstract) // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
            .Where(x => typeof(ScoreFormat).IsAssignableFrom(x)); // Excludes classes not inheriting from BaseClass

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