using System;
using Sirenix.OdinInspector;

[Serializable]
public class CustomFormat : ScoreFormat
{
    public string prefix;
    public string suffix;

    public string defaultValue;

    [InfoBox("0000 - Custom" +
             "\nB - Binary" +
             "\nC - Currency" +
             "\nD - Decimal" +
             "\nE - Exponential (scientific)" +
             "\nF Fixed-point" +
             "\nG General" +
             "\nN Number" +
             "\nP Percent" +
             "\nR Round-trip" +
             "\nX Hexadecimal")]
    public string format;

    public override string GetValueStringFormatted(float value, bool hasBeenSet)
    {
        return !hasBeenSet ? defaultValue : $"{prefix}{value.ToString(format)}{suffix}";
    }
}