using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ReactionSpeedFormat : ScoreFormat
{
    public override string GetValueStringFormatted(float value, bool hasBeenSet)
    {
        return !hasBeenSet ? "n/a" : $"{value:F3}s";
    }
}