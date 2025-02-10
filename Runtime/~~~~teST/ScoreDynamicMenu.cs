using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities.Editor;
using UnityEngine;


public class ScoreDynamicMenu : GlobalConfig<ScoreDynamicMenu>
{
    public enum ScoreSort
    {
        Highest,
        Lowest
    }

    [EnumToggleButtons] public ScoreSort sortScoreBy;

    [Title("Main Score Data Settings")]
    [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
    public ScoreData mainScoreData;

    [Title("Extra Score Data")]
    [TypeFilter("GetFilteredTypeList")]
    public ExtraScoreData[] extraScoreData;
    
    public IEnumerable<Type> GetFilteredTypeList()
    {
        var q = typeof(ExtraScoreData).Assembly.GetTypes()
            .Where(x => !x.IsAbstract)                                          // Excludes BaseClass
            .Where(x => !x.IsGenericTypeDefinition)                             // Excludes C1<>
            .Where(x => typeof(ExtraScoreData).IsAssignableFrom(x));                 // Excludes classes not inheriting from BaseClass

        return q;
    }
}
