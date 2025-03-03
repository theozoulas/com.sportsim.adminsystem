using System;
using Sirenix.OdinInspector;

[Serializable]
public class AutoSizeData
{
    [HorizontalGroup("Split")]
    [VerticalGroup("Split/Left")]
    [MinValue(1f)]
    public float minSize = 18;
    
    [VerticalGroup("Split/Right")]
    [MinValue("minSize")]
    public float maxSize = 72;
}