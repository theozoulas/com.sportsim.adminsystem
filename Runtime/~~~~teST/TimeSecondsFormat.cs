using System;
using Sirenix.OdinInspector;

public class TimeSecondsFormat : ScoreFormat
{
    [ReadOnly] public string Message = "Value must be supplied in seconds";

    public override string GetValueStringFormatted(float value, bool hasBeenSet)
    {
        var timeSpan = TimeSpan.FromSeconds(value);

        return !hasBeenSet
            ? "00:00:00"
            : $"{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
    }
}