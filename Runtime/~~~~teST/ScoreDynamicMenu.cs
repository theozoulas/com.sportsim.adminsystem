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
    
    [InlineEditor(ObjectFieldMode = InlineEditorObjectFieldModes.Hidden)]
    public ScoreData mainScoreData;
}
