using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScoreFormat
{
    public abstract string GetValueStringFormatted(float value, bool hasBeenSet);
}
  
