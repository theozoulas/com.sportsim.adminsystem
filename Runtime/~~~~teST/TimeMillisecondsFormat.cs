using System;
using Sirenix.OdinInspector;

public class TimeMillisecondsFormat : ScoreFormat
{
    [ReadOnly] public string Message = "Value must be supplied in milliseconds";
    
    public override string GetValueStringFormatted(float value, bool hasBeenSet)
    {
        var timeSpan = TimeSpan.FromMilliseconds(value);

        return !hasBeenSet
            ? "00:00:00"
            : $"{timeSpan.Minutes:00}:{timeSpan.Seconds:00}:{timeSpan.Milliseconds:000}";
    }
}