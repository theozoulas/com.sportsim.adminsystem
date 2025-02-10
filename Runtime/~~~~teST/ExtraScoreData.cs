using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "ExtraScoreData", menuName = "ScriptableObjects/ExtraScoreData", order = 1)]
[InlineEditor]
public class ExtraScoreData : ScoreData
{
    [InfoBox("This ID is used to reference the extra Score Data in scripts.")]
    [Required("Score ID Cannot Be Empty!")]
    [Delayed]
    [PropertyOrder(-1)]
    public string scoreID;
}
