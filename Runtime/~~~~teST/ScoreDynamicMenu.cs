using Sirenix.OdinInspector;
using Sirenix.Utilities;

[GlobalConfig("Assets/Resources/AdminSystem/ConfigFiles/")]
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
