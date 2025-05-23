using System;
using Sirenix.OdinInspector;

[Serializable]
public class FontSpacingOptions
{
    [HorizontalGroup("Split")]
    [VerticalGroup("Split/Left")]
    public float character = 0;
    
    [VerticalGroup("Split/Right")]
    public float word = 0;
    
    [HorizontalGroup("SplitBottom")]
    [VerticalGroup("SplitBottom/Left")]
    public float line = 0;
    
    [VerticalGroup("SplitBottom/Right")]
    public float paragraph = 0;
}